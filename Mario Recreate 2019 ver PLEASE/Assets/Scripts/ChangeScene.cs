using System.Collections;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
	//public AudioSource changeSceneSound;
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			//changeSceneSound.PlayOneShot(changeSceneSound.clip);
			StartCoroutine(LoadYourAsyncScene("MarioLevel2"));
		}
	}

	IEnumerator LoadYourAsyncScene(string sceneName)
	{
		//yield return new WaitUntil(() => !changeSceneSound.isPlaying);
		yield return null;
		CentralManager.centralManagerInstance.changeScene();
	}
}