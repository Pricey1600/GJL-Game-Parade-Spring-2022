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

    private GameObject newParent, newBystander;
    private string newType;

    private GameObject oldParent;

    [SerializeField] private int bystandersToSpawn;

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
        newBystander = Instantiate(parentPrefab, Place().position, Quaternion.identity);
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
        newParent = Instantiate(parentPrefab, Place().position, Quaternion.identity);
        GiveRole(true);
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

    private Transform Place()
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
        }
        else if(newType == "sitting")
        {
            //find sitting area from list
            int sitSpotInt = Random.Range(0, sittingLocations.Count-1);
            spawnArea = sittingLocations[sitSpotInt];
        }
        return spawnArea;
    }

    private void GiveRole(bool expecting)
    {
        
        if (expecting)
        {
            newParent.GetComponent<ParentScript>().type = newType;
            newParent.GetComponent<ParentScript>().isExpecting = expecting;
            newParent.gameObject.tag = "Parent";
        }
        else
        {
            newBystander.GetComponent<ParentScript>().type = newType;
            newBystander.GetComponent<ParentScript>().isExpecting = expecting;
            newBystander.gameObject.tag = "Bystander";
        }
        
    }

    private void DeleteOldParent()
    {
        Destroy(oldParent);
    }
}
