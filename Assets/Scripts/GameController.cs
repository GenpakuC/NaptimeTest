using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject objToSpawnPref; //prefab for object to spawn
    [SerializeField] Dropdown objToSpawnDD;
    [SerializeField] GameObject menuBGToHide;
    [SerializeField] BulletPool bulletPool; //pool with bullets
    [SerializeField] Text objAliveTxt; // shows how many objects are alive
    [SerializeField] Text lastObjNameTxt; // shows last object name
    [SerializeField] GameObject endGameUI;
    int objectsToSpawn; //how manyobjects will be spawned
    //min/max position to spawn your object
    [SerializeField] Vector3 gridMin; 
    [SerializeField] Vector3 gridMax;

    // Start is called before the first frame update

    [SerializeField] List<GameObject> spawnedObjects = new List<GameObject>(); // all spawned objects

    List<Vector3> spawnPlaces = new List<Vector3>();

    [SerializeField] bool globalShootTimer = false;
    float shootTimer = 0f; //count time to next shoot
    void Start()
    {
        endGameUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        objAliveTxt.text = "Objects alive: " + spawnedObjects.Count;
        if(spawnedObjects.Count == 1) // if only one object alive end game
        {
            EndGame();
        }

        if (globalShootTimer) //if global shoot is activated then shoot all bullets at once
        {
            if (shootTimer >= 1)
            {
                shootTimer = 0f;
                StartCoroutine(Shoot());
            }
            else
            {
                shootTimer += Time.deltaTime;
            }       
        }

    }
    IEnumerator Shoot()
    {
        for (int x = 0; x < spawnedObjects.Count; x++)
        {
            if (spawnedObjects[x].transform.GetChild(0).gameObject.activeInHierarchy)
            {
                spawnedObjects[x].GetComponent<SpawnedObjectController>().Shoot();
            }
        }
        yield return null;
    }
    void EndGame()
    {
        lastObjNameTxt.text = spawnedObjects[0].name + " won!";
        endGameUI.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
    public void DestoryObject(GameObject obj)
    {
        spawnedObjects.Remove(obj);
        Destroy(obj);
    }
    public void StartGame()
    {
      menuBGToHide.SetActive(false);    
      objectsToSpawn = int.Parse(objToSpawnDD.options[objToSpawnDD.value].text);
      GenerateSpawnPlaces(); //calculate position for all objects to spawn to make it easier for them to spawn at once
      GenerateAllObjects(); //spawn all object in generated before positions
     // PrepareBullets(); // prepare first bullets to remove lagspike from spawning all of them at once
      objAliveTxt.gameObject.SetActive(true);
    }
    void PrepareBullets()
    {
        bulletPool.PreparePoolForBullets(objectsToSpawn);
    }

    void GenerateSpawnPlaces()
    {
        spawnPlaces.Clear();
        for(int x=0;x<objectsToSpawn; x++)
        {
            spawnPlaces.Add(GenerateSpawnPlace());
        }
    }

    void GenerateAllObjects()
    {
        spawnedObjects.Clear();
        for (int x=0;x<spawnPlaces.Count;x++)
        {
            GameObject spawnedObj = Instantiate(objToSpawnPref, spawnPlaces[x], objToSpawnPref.transform.rotation);
            spawnedObj.GetComponent<SpawnedObjectController>().SetObjectData(bulletPool,globalShootTimer);
            spawnedObj.name = "Object " + x;
            spawnedObjects.Add(spawnedObj);
        }
    }


    Vector3 GenerateSpawnPlace()
    {
        Vector3 spawnPlace = new Vector3(Random.Range(gridMin.x, gridMax.x), Random.Range(gridMin.y, gridMax.y), Random.Range(gridMin.z, gridMax.z)); //take random place to spawn

        for(int x = 0; x < spawnPlaces.Count; x++) //check if something else is to close for this example I'm checking if something is closer than 3;
        {
            if (Vector3.Distance(spawnPlace, spawnPlaces[x]) <= 3)
            {
                //if generated place is to close then generate new one
                spawnPlace = GenerateSpawnPlace();
                break;
            }
        }
        return spawnPlace;
    }


}
