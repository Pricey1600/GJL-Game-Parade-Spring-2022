using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyScript : MonoBehaviour
{
    private bool firstCollision, successfulDelivery;

    [SerializeField] private float timerLength, timer;
    [SerializeField] private ParentSpawner parentSpawner;

    private void Start()
    {
        parentSpawner = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ParentSpawner>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Parent")
        {
            //successful hit
            Debug.Log("Successful Devilvery");
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.transform.parent = collision.gameObject.transform;
            //set new position and rotation here
            gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z +1f);
            successfulDelivery = true;
            //trigger new spawn event
            parentSpawner.SpawnParent();
            //send message to parent script for animation triggering, etc
        }
        else
        {
            if (!firstCollision)
            {
                timer = timerLength;
                firstCollision = true;
            }
            
        }
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0 && !successfulDelivery)
            {
                //not successful
                //explode baby.
                Debug.Log("Failed Delivery");
                parentSpawner.SpawnParent();
                Destroy(gameObject);
            }
        }
    }
}
