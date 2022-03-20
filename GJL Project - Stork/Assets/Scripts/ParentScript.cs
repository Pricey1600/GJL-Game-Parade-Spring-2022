using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ParentScript : MonoBehaviour
{
    public string type;
    public bool isExpecting, sittingBystander;

    private float timer;
    [SerializeField] private float timerLength;

    private Animator AC;
    private NavMeshAgent navAgent;

    private AudioSource parentAS;

    private void OnEnable()
    {
            BabyScript.OnSuccess += successReaction;
            BabyScript.OnFailure += failReaction;
        
    }
    private void OnDisable()
    {


            BabyScript.OnSuccess -= successReaction;
            BabyScript.OnFailure -= failReaction;

        
    }

    private void Start()
    {
        if (sittingBystander)
        {
            type = "sitting";
            SetUp();
        }
    }
    public void SetUp()
    {
        AC = gameObject.GetComponentInChildren<Animator>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        parentAS = gameObject.GetComponent<AudioSource>();
        if (type == "standing")
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            gameObject.GetComponent<RandomWalking>().enabled = false;
            AC.SetBool("isStanding", true);
        }
        else if (type == "walking")
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            gameObject.GetComponent<RandomWalking>().enabled = true;
            AC.SetBool("isStanding", false);
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            gameObject.GetComponent<RandomWalking>().enabled = false;
            AC.SetBool("isSitting", true);
            //set sitting animations here
        }
    }

    public void DestroyCountdown()
    {
        timer = timerLength;
        gameObject.GetComponent<RandomWalking>().enabled = false;
        navAgent.SetDestination(transform.position);
        gameObject.GetComponent<Collider>().enabled = false;
        AC.SetTrigger("Success");

    }

    private void successReaction()
    {
        if (isExpecting)
        {
            BabyScript.OnSuccess -= successReaction;
            BabyScript.OnFailure -= failReaction;
            
            if (Random.Range(0, 20) == 1)
            {
                parentAS.Play();
            }
        }
        
    }
    private void failReaction()
    {
        if (isExpecting)
        {
            BabyScript.OnSuccess -= successReaction;
            BabyScript.OnFailure -= failReaction;
        }
        
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
