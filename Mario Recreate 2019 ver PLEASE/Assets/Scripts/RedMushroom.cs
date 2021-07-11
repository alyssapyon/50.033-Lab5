using System.Collections;
using UnityEngine;

// JUMP BOOST

public class RedMushroom : MonoBehaviour, ConsumableInterface
{
	public Texture t;
	public void consumedBy(GameObject player)
	{
		// give player jump boost
		player.GetComponent<PlayerController>().jumpBoost += 21;
		StartCoroutine(removeEffect(player));
	}

	IEnumerator removeEffect(GameObject player)
	{
		yield return new WaitForSeconds(5.0f);
		player.GetComponent<PlayerController>().jumpBoost -= 21;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			// update UI
			CentralManager.centralManagerInstance.addPowerup(t, 0, this);
			GetComponent<Collider2D>().enabled = false;
		}
	}

	private static RedMushroom _instance;
	public static RedMushroom Instance
	{
		get { return _instance; }
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		_instance = this;
		DontDestroyOnLoad(this.gameObject); // only works on root gameObjects
	}


}