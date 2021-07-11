using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SpawnManager : MonoBehaviour
{
    public GameObject mario;

    public int greenEnemyCount = 3;
    public int gombaEnemyCount = 0;


    public GameConstants gameConstants;

    // Start is called before the first frame update

    void Start()
    {
        for (int j = 0; j < greenEnemyCount; j++)
            spawnFromPooler(ObjectType.greenEnemy);

        for (int k = 0; k < gombaEnemyCount; k++)
            spawnFromPooler(ObjectType.gombaEnemy);

        // subscribe
        GameManager.OnEnemyDeath += spawnNewEnemy;
    }


    void spawnFromPooler(ObjectType i)
    {
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(i);

        if (item != null)
        {
            //set position
            // item.transform.localScale = new Vector3(1, 1, 1);

            float Xvalue = Random.Range(-9.5f, 9.5f);
            while (System.Math.Abs(Xvalue - mario.transform.position.x) < 2.0f)
            {
                Xvalue = Random.Range(-9.5f, 9.5f);
            }

            //item.transform.position = new Vector3(Xvalue, groundDistance + item.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            item.transform.position = new Vector3(Xvalue, gameConstants.groundDistance, 0);
            item.SetActive(true);
        }
        else
        {
            Debug.Log("not enough items in the pool!");
        }
    }

    public void spawnNewEnemy()
    {

        ObjectType i = Random.Range(0, 2) == 0 ? ObjectType.gombaEnemy : ObjectType.greenEnemy;
        spawnFromPooler(i);

    }

    private void OnDestroy()
    {
        GameManager.OnPlayerDeath -= spawnNewEnemy;

    }

}
