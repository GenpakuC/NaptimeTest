using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //controlls bullet behaviour
    public void Clear()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.transform.parent != null)
        {
            if (collision.gameObject.transform.parent.GetComponent<SpawnedObjectController>())
            {
                collision.gameObject.transform.parent.GetComponent<SpawnedObjectController>().GotHit();
            }
        }
        
        gameObject.SetActive(false);
    }
}
