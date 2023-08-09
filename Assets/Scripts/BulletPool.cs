using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab; //prefab for bullet object
    [SerializeField] List<GameObject> bullets = new List<GameObject>(); //list of all spawned bullets
    public void SpawnBullet(Vector3 pos,Quaternion rot)
    {
        StartCoroutine(Spawn(pos,rot));
    }

    IEnumerator Spawn(Vector3 pos,Quaternion rot)
    {
        GameObject bulletToUse = null;
        //check if you can reuse bullet spawned before
        for (int x = 0; x < bullets.Count; x++)
        {
            if (!bullets[x].activeInHierarchy)
            {
                bullets[x].GetComponent<BulletController>().Clear();
                bullets[x].transform.position = pos;
                bullets[x].transform.rotation = rot;
                bullets[x].SetActive(true);
                bulletToUse = bullets[x];
                break;
            }
        }

        if (bulletToUse == null)
        {
            //if you don't have any not used bullets spawn a new one
            bulletToUse = Instantiate(bulletPrefab, pos, rot);
            bullets.Add(bulletToUse);
        }
        //push bullet forward the direction your object were facing
        bulletToUse.GetComponent<Rigidbody>().AddForce(bulletToUse.transform.forward * 30, ForceMode.VelocityChange);
        yield return null;
    }

    public void PreparePoolForBullets(int bulletsCount)
    {
        StartCoroutine(PreparePool(bulletsCount));
        /*
        for (int x=0;x<bulletsCount;x++)
        {
            GameObject bulletToUse = Instantiate(bulletPrefab);
            bulletToUse.SetActive(false);
            bullets.Add(bulletToUse);
        }*/
    }
    IEnumerator PreparePool(int bulletsCount)
    {
        for (int x = 0; x < bulletsCount; x++)
        {
            GameObject bulletToUse = Instantiate(bulletPrefab);
            bulletToUse.SetActive(false);
            bullets.Add(bulletToUse);
        }
        yield return null;
    }
}
