using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public AudioSource enemyAudioSource;
	public GameConstants gameConstants;
	private int moveRight;
	private float originalX;
	private Vector2 velocity;
	private Rigidbody2D enemyBody;
	private float heightScale;
	private float halfHeight;


	void Start()
	{
		enemyBody = GetComponent<Rigidbody2D>();

		// get the starting position
		originalX = transform.position.x;

		// randomise initial direction
		moveRight = Random.Range(0, 2) == 0 ? -1 : 1;

		// compute initial velocity
		ComputeVelocity();

		// get height of enemy
		heightScale = this.transform.localScale.y;
		halfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

		//Debug.Log(halfHeight.ToString());

		// subscribe
		GameManager.OnPlayerDeath += EnemyRejoice;
	}

	void ComputeVelocity()
	{
		velocity = new Vector2((moveRight) * gameConstants.maxOffset / gameConstants.enemyPatroltime, 0);
	}

	void MoveEnemy()
	{
		enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
	}

	void Update()
	{
		if (Mathf.Abs(enemyBody.position.x - originalX) < gameConstants.maxOffset)
		{// move gomba
			MoveEnemy();
		}
		else if ((enemyBody.position.x - originalX) < (-1 *gameConstants.maxOffset))
		{
			// change direction
			moveRight = 1;
			ComputeVelocity();
			MoveEnemy();
		} 
		else
        {
			moveRight = -1;
			ComputeVelocity();
			MoveEnemy();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.gameObject.tag == "Obstacles")
		{
			moveRight *= 1;
		}


		// check if it collides with Mario
		if (other.gameObject.tag == "Player")
		{
			// check if collides on top
			float yoffset = (other.transform.position.y - this.transform.position.y);
			if (yoffset > 0.75f)
			{
				KillSelf();
			}
			else
			{
				// hurt player
				CentralManager.centralManagerInstance.damagePlayer();
			}
		}


	}

	void KillSelf()
	{
		// enemy dies
		CentralManager.centralManagerInstance.increaseScore();
		StartCoroutine(flatten());
		Debug.Log("Kill sequence ends");
	}

	IEnumerator flatten()
	{
		Debug.Log("Flatten starts");

		enemyAudioSource.PlayOneShot(enemyAudioSource.clip);


		int steps = 5;
		float stepper = heightScale / (float)steps;

		for (int i = 0; i < steps; i++)
		{
			this.transform.localScale = new Vector3(this.transform.localScale.x, (this.transform.localScale.y - stepper), this.transform.localScale.z);

            //make sure enemy is still above ground

            this.transform.position = new Vector3(
                this.transform.position.x,
                gameConstants.groundSurface + (GetComponent<SpriteRenderer>().bounds.extents.y),
                0
                );
            //this.transform.position = new Vector3(
            //	this.transform.position.x,
            //	gameConstants.groundSurface + (halfHeight * (i/5)),
            //	0
            //	);

            yield return null;
		}
		Debug.Log("Flatten ends");
		this.transform.localScale = new Vector3(this.transform.localScale.x, (heightScale), this.transform.localScale.z); ;
		this.gameObject.SetActive(false);
		Debug.Log("Enemy returned to pool");
		yield break;
	}

	// animation when player is dead
	void EnemyRejoice()
	{
		//Debug.Log("Enemy killed Mario");
		// do whatever you want here, animate etc
		// ...
		GetComponent<Animator>().SetBool("playerIsDead", true);
		velocity = Vector3.zero;
		GameManager.OnPlayerDeath -= EnemyRejoice;

	}

    private void OnDestroy()
    {
		GameManager.OnPlayerDeath -= EnemyRejoice;

	}



}