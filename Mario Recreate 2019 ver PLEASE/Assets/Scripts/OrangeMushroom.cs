using System.Collections;
using UnityEngine;

// SPEED BOOST
public class OrangeMushroom : MonoBehaviour, ConsumableInterface
{
	public Texture t;
	public void consumedBy(GameObject player)
	{
		// give player jump boost
		player.GetComponent<PlayerController>().speedBoost *= 10;
		StartCoroutine(removeEffect(player));
	}

	IEnumerator removeEffect(GameObject player)
	{
		yield return new WaitForSeconds(5.0f);
		player.GetComponent<PlayerController>().speedBoost /= 10;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			// update UI
			CentralManager.centralManagerInstance.addPowerup(t, 1, this);
			GetComponent<Collider2D>().enabled = false;
		}
	}
	private static OrangeMushroom _instance;
	public static OrangeMushroom Instance
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