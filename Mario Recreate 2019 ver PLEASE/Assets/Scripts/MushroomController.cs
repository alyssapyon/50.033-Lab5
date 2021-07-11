using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    int direction = 1;
    public float speed = 4f;
    bool shouldMove = true;
    bool collected = false;

    Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
        originalScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (shouldMove)
        {
            rigidBody.velocity = new Vector2(speed * direction, rigidBody.velocity.y);
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("Ground") || !col.gameObject.CompareTag("Obstacles"))
        {
            direction = direction * (-1);
        }

        if (col.gameObject.CompareTag("Player"))
        {
            collected = true;
            shouldMove = false;

            StartCoroutine(consume());

            
        }
    }

    //void OnBecameInvisible()
    //{
    //    Destroy(gameObject);
    //}

    IEnumerator consume()
    {
        Debug.Log("consume starts");

        int steps = 5;

        for (int i = 0; i < steps; i++)
        {
            this.transform.localScale = new Vector3(
                (float)(this.transform.localScale.x * 1.1), 
                (float)(this.transform.localScale.y * 1.1), 
                this.transform.localScale.z
                );


            yield return null;
        }
        this.transform.localScale = originalScale;
        this.transform.position = new Vector3(this.transform.position.x, -7.0f, 0);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield break;
    }
}
