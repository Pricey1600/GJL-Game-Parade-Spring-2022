using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParentSpawner : MonoBehaviour
{
    [SerializeField] private GameObject parentPrefab;
    [SerializeField] private List<Transform> sittingLocations;
    [SerializeField] private List<Transform> spawnAreas;
    [SerializeField] private List<GameObject> bystanders;
    private int currentArea;

    private Transform spawnArea;
    private Vector3 spawnPos;

    private GameObject newParent, newBystander;
    private string newType;

    private GameObject oldParent;

    [SerializeField] private int bystandersToSpawn;

    [SerializeField] private Transform objMarker;
    [SerializeField] private float objMarkerOffsetY;

    private void Start()
    {
        for(int i=0; i< bystandersToSpawn; i++)
        {
            SpawnBystander();
        }
        SpawnParent();
    }

    public void SpawnBystander()
    {
        Assign(false);
        newBystander = Instantiate(parentPrefab, Place(), Quaternion.identity);
        Debug.Log("Bystander Spawned");
        bystanders.Add(newBystander);
        GiveRole(false);
    }
    public void SpawnParent()
    {
        if(newParent != null)
        {
            oldParent = newParent;
        }
        Assign(true);
        newParent = Instantiate(parentPrefab, Place(), Quaternion.identity);
        GiveRole(true);
        AssignObjective(newParent);
        DeleteOldParent();
    }

    private void Assign(bool isParent)
    {
        if (isParent)
        {
            int typeInt = Random.Range(0, 3);
            if (typeInt == 0)
            {
                newType = "standing";
            }
            else if (typeInt == 1)
            {
                newType = "walking";
            }
            else
            {
                newType = "sitting";
            }
        }
        else
        {
            int typeInt = Random.Range(0, 2);
            if (typeInt == 0)
            {
                newType = "standing";
            }
            else if (typeInt == 1)
            {
                newType = "walking";
            }
        }
        
        

    }

    private Vector3 Place()
    {
        if(newType == "standing" || newType == "walking")
        {
            //find next area to spawn in
            currentArea++;
            if(currentArea > spawnAreas.Count-1)
            {
                currentArea = 0;
            }
            spawnArea = spawnAreas[currentArea];
            Debug.Log(spawnArea);
            spawnPos = spawnArea.GetComponent<SpawnArea>().GetSpawnPos();
        }
        else if(newType == "sitting")
        {
            //find sitting area from list
            int sitSpotInt = Random.Range(0, sittingLocations.Count-1);
            spawnPos = sittingLocations[sitSpotInt].position;
        }
        return spawnPos;
    }

    private void GiveRole(bool expecting)
    {
        
        if (expecting)
        {
            newParent.GetComponent<ParentScript>().type = newType;
            newParent.GetComponent<ParentScript>().isExpecting = expecting;
            newParent.gameObject.tag = "Parent";
            newParent.GetComponent<ParentScript>().SetUp();
        }
        else
        {
            newBystander.GetComponent<ParentScript>().type = newType;
            newBystander.GetComponent<ParentScript>().isExpecting = expecting;
            newBystander.gameObject.tag = "Bystander";
            newBystander.GetComponent<ParentScript>().SetUp();
        }
        
    }

    private void AssignObjective(GameObject target)
    {
        objMarker.transform.parent = target.transform;
        objMarker.localPosition = new Vector3(0, objMarkerOffsetY, 0);
    }

    private void DeleteOldParent()
    {
        if(oldParent != null)
        {
            oldParent.gameObject.GetComponent<ParentScript>().DestroyCountdown();
        }
        
    }
}
