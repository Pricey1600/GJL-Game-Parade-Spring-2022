using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ParentScript : MonoBehaviour
{
    public string type;
    public bool isExpecting;

    private float timer;
    [SerializeField] private float timerLength;

    public void SetUp()
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

            //set sitting animations here
        }
    }

    public void DestroyCountdown()
    {
        timer = timerLength;
        //play success animation here
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
