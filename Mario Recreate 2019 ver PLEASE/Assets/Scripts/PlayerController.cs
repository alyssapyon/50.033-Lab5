using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameConstants gameConstants;

    private Rigidbody2D marioBody;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    private bool pressLeft;
    private bool pressRight;
    private bool releaseLeft;
    private bool releaseRight;

    //public Transform enemyLocation;
    //public Text scoreText;
    //private int score = 0;
    //private bool jumpedOverEnemy = false;

    private bool timeToRestart = false;
    private float wait;

    private Animator marioAnimator;

    // private AudioSource marioAudio;
    AudioSource AudioSourceJump;
    AudioSource AudioSourceSkid;

    public AudioClip audioClipJump;
    public AudioClip audioClipSkid;

    public UnityEngine.Audio.AudioMixerGroup mixerGroup;

    private bool isLanding = false;
    public ParticleSystem dustCloud;

    public int speedBoost = 1;
    public int jumpBoost = 0;


    private static PlayerController _instance;
    public static PlayerController Instance
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


    // Start is called before the first frame update
    void Start()
    {

        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();

        // marioAudio = GetComponent<AudioSource>();
        AudioSourceJump = gameObject.AddComponent<AudioSource>();
        AudioSourceJump.loop = false;
        AudioSourceJump.playOnAwake = false;
        AudioSourceJump.volume = 1.0f;
        AudioSourceJump.clip = audioClipJump;
        AudioSourceJump.outputAudioMixerGroup = mixerGroup;

        AudioSourceSkid = gameObject.AddComponent<AudioSource>();
        AudioSourceSkid.loop = false;
        AudioSourceSkid.playOnAwake = false;
        AudioSourceSkid.volume = 1.0f;
        AudioSourceSkid.clip = audioClipSkid;
        AudioSourceSkid.outputAudioMixerGroup = mixerGroup;

        // subscribe
        GameManager.OnPlayerDeath += PlayerDiesSequence;


    }

    private void Update()
    {

        pressLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("a");
        pressRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d");
        releaseLeft = Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp("a");
        releaseRight = Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp("d");

        // toggle state
        if (pressLeft && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;

            // check velocity
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        if (pressRight && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;

            // check velocity
            if (Mathf.Abs(marioBody.velocity.x) > 1.0)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        if (Input.GetKeyDown("z"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z, this.gameObject);
        }

        if (Input.GetKeyDown("x"))
        {
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X, this.gameObject);
        }

        if (isLanding && onGroundState)
        {
            dustCloud.Play();
            isLanding = false;
        }

        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        //if (!onGroundState
        //    && marioBody.position.y > enemyLocation.position.y
        //    && Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
        //{
        //    jumpedOverEnemy = true;
        //}

        if (timeToRestart)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            marioBody.velocity = Vector2.zero;

            wait += Time.deltaTime;

            if (wait > 1.0)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
        }

    }

    // FixedUpdate may be called once per frame. See documentation for details.

    void FixedUpdate()
    {
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < gameConstants.playerMaxSpeed * speedBoost)
                marioBody.AddForce(movement * gameConstants.playerDefaultForce);
        }

        if (releaseLeft || releaseRight)
        {
            // stop
            marioBody.velocity = Vector2.zero;
        }

        if (Input.GetKeyDown("space") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * (gameConstants.playerMaxJumpSpeed + jumpBoost), ForceMode2D.Impulse);
            onGroundState = false;
            isLanding = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }

    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles"))
        {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);

            //if (jumpedOverEnemy)
            //{
            //    score++;
            //    jumpedOverEnemy = false;
            //}
            //scoreText.text = "Score: " + score.ToString();
        };
    }

    // called when mario collides with enemy - Gomba
    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        Debug.Log("Collided with Enemy!");

    //        timeToRestart = true;
    //    }
    //}

    void PlayJumpSound()
    {
        AudioSourceJump.PlayOneShot(AudioSourceJump.clip);
    }

    void PlaySkidSound()
    {
        AudioSourceSkid.PlayOneShot(AudioSourceSkid.clip);
    }

    void PlayerDiesSequence()
    {
        // Mario dies
        Debug.Log("Mario dies");
        // do whatever you want here, animate etc
        marioAnimator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        marioBody.AddForce(Vector3.up * 30, ForceMode2D.Impulse);
        marioBody.gravityScale = 30;
        StartCoroutine(dead());
    }

    IEnumerator dead()
    {
        yield return new WaitForSeconds(1.0f);
        marioBody.bodyType = RigidbodyType2D.Static;
    }



}
