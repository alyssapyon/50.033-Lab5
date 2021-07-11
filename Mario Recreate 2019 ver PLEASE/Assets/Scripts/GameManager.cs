using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;
	public static GameManager Instance
    {
		get { return _instance; }
    }

	public Text score;
	//public GameObject panel, restartButton;
	public delegate void gameEvent();
	public static event gameEvent OnPlayerDeath;
	public static event gameEvent OnEnemyDeath;
	
	private int playerScore = 0;



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




    public void increaseScore()
	{
		playerScore += 1;
		score.text = "SCORE: " + playerScore.ToString();

		OnEnemyDeath();
	}

	public void damagePlayer()
	{
		OnPlayerDeath();


	}
}