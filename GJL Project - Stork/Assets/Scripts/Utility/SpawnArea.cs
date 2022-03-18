using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] float areaSize, maxDist;
    private NavMeshHit myNavHit;

    [SerializeField] private GameObject spawnLocatorObj;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, areaSize);
    }

    public Vector3 GetSpawnPos()
    {
        while (true)
        {
            Vector3 randomPoint = Random.insideUnitSphere * areaSize + transform.position;
            //Instantiate(spawnLocatorObj, randomPoint, Quaternion.identity);
            if (NavMesh.SamplePosition(randomPoint, out myNavHit, maxDist, 1))
            {
                //Instantiate(spawnLocatorObj, myNavHit.position, Quaternion.identity);
                return myNavHit.position;
            }
        }
        
        
        
    }

}
