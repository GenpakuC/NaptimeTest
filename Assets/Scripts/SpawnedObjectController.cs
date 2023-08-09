using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObjectController : MonoBehaviour
{
    // Start is called before the first frame update
    float randomTimeToRotate = 1; //random time after which object will rotate
    float timer = 0f; //count time to next rotation
    float shootTimer = 0f; //count time to next shoot
    BulletPool bulletPool; //bullet pool to use

    [SerializeField] int lifeCount = 3; // life count for that object

    [SerializeField] float respawnTime = 0f;//count time to respawn after hp loss
    [SerializeField] bool respawn = false;

    void Start()
    {
        randomTimeToRotate = Random.Range(0f, 1f);
    }
    public void SetBulletPool(BulletPool pool)
    {
        bulletPool = pool;
    }
    public void GotHit()
    {
        lifeCount -= 1;
        if(lifeCount <= 0)
        {
            //destory object if it doesn't have any hp left
            GameObject.FindObjectOfType<GameController>().DestoryObject(gameObject);
            return;
        }
        respawn = true;
        respawnTime = 2f;
        // hide object on scene
        transform.GetChild(0).gameObject.SetActive(false);
      //  GetComponent<Renderer>().enabled = false;
      //  GetComponent<Collider>().enabled = false;
    }
    void Rotate()
    {
        transform.Rotate(0, Random.Range(0,360), 0); //sets object rotation to random value between 0 and 360
        randomTimeToRotate = Random.Range(0f, 1f); //sets random time after which object will change rotation
    }
    // Update is called once per frame
    void Update()
    {
        if (respawn)
        {
            if(respawnTime <= 0)
            {
                respawn = false;
                //show object
                transform.GetChild(0).gameObject.SetActive(true);
              //  GetComponent<Renderer>().enabled = true;
              //  GetComponent<Collider>().enabled = true;
            }
            else
            {
                respawnTime -= Time.deltaTime;
            }

            return;
        }
        if (timer >= randomTimeToRotate)
        {
            timer = 0f;
            Rotate();
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (shootTimer >= 1)
        {
            shootTimer = 0f;
            Vector3 spawnPos = transform.position + transform.forward;
            Quaternion spawnRot = transform.rotation;
            bulletPool.SpawnBullet(spawnPos, spawnRot); //spawn bullet from bulletpool instead of instantiating it to use less resources 
        }
        else
        {
            shootTimer += Time.deltaTime;
        }
      
       
    }
}
