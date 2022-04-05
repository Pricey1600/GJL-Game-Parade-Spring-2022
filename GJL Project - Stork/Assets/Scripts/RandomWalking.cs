using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomWalking : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float walkRadius = 1f;
    [SerializeField] private float maxSpeed, minSpeed;

    private bool isStanding;
    private float standingTimer;
    private Animator AC;

    private void Start()
    {
        transform.position = RandomNavmeshLocation();
        agent.speed = Random.Range(minSpeed, maxSpeed);
        AC = gameObject.GetComponentInChildren<Animator>();
    }
    public Vector3 RandomNavmeshLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private void Update()
    {
        if (!agent.hasPath && agent.isActiveAndEnabled && !isStanding)
        {
            //choose whether to find a new path or to stand for a bit
            int newTypeInt = Random.Range(0, 2);
            if(newTypeInt == 0)
            {
                agent.SetDestination(RandomNavmeshLocation());
                AC.SetBool("isStanding", false);
            }
            else
            {
                ChangeToStanding();
            }
            
        }
        if(standingTimer > 0)
        {
            standingTimer -= Time.deltaTime;
            if(standingTimer <= 0)
            {
                isStanding = false;
            }
        }
    }

    private void ChangeToStanding()
    {
        int duration = Random.Range(3, 20);
        standingTimer = duration;
        isStanding = true;
        AC.SetBool("isStanding", true);
        agent.SetDestination(transform.position);

    }

    public void ToggleAgent()
    {
        if (agent.enabled == true)
        {
            agent.enabled = false;
        }
        else
        {
            agent.enabled = true;
        }
    }
}
