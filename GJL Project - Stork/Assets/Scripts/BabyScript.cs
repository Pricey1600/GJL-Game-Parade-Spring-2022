using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyScript : MonoBehaviour
{
    private bool firstCollision, successfulDelivery;

    [SerializeField] private float timerLength, timer;
    [SerializeField] private ParentSpawner parentSpawner;

    private AudioSource babyAS;
    [SerializeField] private AudioClip babyCry, babySuccess;
    [SerializeField] private List<AudioClip> babyLaugh; 

    public delegate void ScoreAction();
    public static event ScoreAction OnSuccess;
    public static event ScoreAction OnFailure;

    Quaternion newRotQ;

    private void Start()
    {
        parentSpawner = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ParentSpawner>();
        babyAS = gameObject.GetComponent<AudioSource>();
        AudioClip chosenLaugh = babyLaugh[Random.Range(0, babyLaugh.Count)];
        babyAS.PlayOneShot(chosenLaugh);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Parent")
        {
            //successful hit
            OnSuccess?.Invoke();
            //Debug.Log("Successful Devilvery");
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.transform.parent = collision.gameObject.transform;
            //set new position and rotation here
            gameObject.transform.localPosition = new Vector3(0.049f, 0.485f, 0.452f);
            Vector3 newRot = new Vector3(181.7f, 83.8f, -207.5f);
            newRotQ.eulerAngles = newRot;
            gameObject.transform.localRotation = newRotQ;
            successfulDelivery = true;
            //trigger new spawn event
            parentSpawner.SpawnParent();
            //send message to parent script for animation triggering, etc
            babyAS.PlayOneShot(babySuccess);
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
                //Debug.Log("Failed Delivery");
                parentSpawner.SpawnParent();
                OnFailure?.Invoke();
                StartCoroutine("BabyFail");
            }
        }
    }

    IEnumerator BabyFail()
    {
        babyAS.PlayOneShot(babyCry);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
