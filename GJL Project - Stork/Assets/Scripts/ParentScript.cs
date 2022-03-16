using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ParentScript : MonoBehaviour
{
    public string type;
    public bool isExpecting;

    private void Start()
    {
        if (type == "standing")
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            gameObject.GetComponent<RandomWalking>().enabled = false;
        }
        else if (type == "walking")
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            gameObject.GetComponent<RandomWalking>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            gameObject.GetComponent<RandomWalking>().enabled = false;
        }
    }
}
