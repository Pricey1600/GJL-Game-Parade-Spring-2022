using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParentSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> parentPrefabs;
    [SerializeField] private List<Material> humanMaterials;
    [SerializeField] private List<Transform> sittingLocations;
    [SerializeField] private List<Transform> spawnAreas;
    [SerializeField] private List<GameObject> bystanders;
    private int currentArea;

    private Transform spawnArea;
    private Vector3 spawnPos;

    private GameObject newParent, newBystander;
    private string newType;
    private Quaternion newRotation;

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
        newBystander = Instantiate(parentPrefabs[GetPrefab()], Place(), newRotation);
        newBystander.GetComponentInChildren<Renderer>().material = humanMaterials[Random.Range(0, humanMaterials.Count)];
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
        newParent = Instantiate(parentPrefabs[GetPrefab()], Place(), newRotation);
        newParent.GetComponentInChildren<Renderer>().material = humanMaterials[Random.Range(0, humanMaterials.Count)];
        GiveRole(true);
        AssignObjective(newParent);
        DeleteOldParent();
    }

    private int GetPrefab()
    {
        int prefabIndex = Random.Range(0, parentPrefabs.Count);
        return prefabIndex;
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
            newRotation = Quaternion.identity;
        }
        else if(newType == "sitting")
        {
            //find sitting area from list
            int sitSpotInt = Random.Range(0, sittingLocations.Count);
            spawnPos = sittingLocations[sitSpotInt].position;
            newRotation = sittingLocations[sitSpotInt].rotation;
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
