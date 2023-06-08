using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using  NPCAI;
using Shooting_range;

public class NPCMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshAgent agent;
    public Transform target;
    public NPCWeaponController weapon;
    public enum States { Patrol, Attack, Follow }
    public States currentState;

    public Transform[] wayPoints;

    public int currentWayPoint;
    public bool inSight;
    public Vector3 directionToPlayer;
    public float shootDistance;
    public float maxFollowDist;

    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        //agent.Warp(wayPoints[0].position);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStates();
        CheckForPlayer();
    }
    private void OnEnable()
    {
        agent.SetDestination(wayPoints[currentWayPoint + 1].position);
    }
    private void UpdateStates()
    {
        switch (currentState)
        {
            case States.Patrol:
                Patrol();
                break;
            case States.Attack:
                Attack();
                break;
            case States.Follow:
                Follow();
                break;
        }
    }
    private void Patrol()
    {
        if (agent.destination != wayPoints[currentWayPoint].position)
        {
            agent.destination = wayPoints[currentWayPoint].position;
        }
        if (HasReached())
        {
            currentWayPoint = (currentWayPoint + 1) % wayPoints.Length;
        }
        if (inSight && directionToPlayer.magnitude <= maxFollowDist)
        {
            currentState = States.Follow;
        }
    }
    public void LookAtTarget()
    {
        Vector3 lookDirection = directionToPlayer;
        lookDirection.y = 0f;

        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
    }


    private void Attack()
        {
            if (!inSight || directionToPlayer.magnitude > shootDistance)
            {
                currentState = States.Follow;
            }
            LookAtTarget();
            weapon.Shoot(directionToPlayer);
        }

        private void Follow()
        {
            if (directionToPlayer.magnitude <= shootDistance && inSight)
            {
                agent.ResetPath();
                currentState = States.Attack;
            }
            else
            {
                if (target != null && directionToPlayer.magnitude <= maxFollowDist)
                {
                    agent.SetDestination(target.position);
                }
                else
                {
                currentState = States.Patrol;
                }
            }
        }
        private bool HasReached()
        {
        //Debug.Log(agent.hasPath);
        //Debug.Log(!agent.pathPending);
        //Debug.Log(agent.remainingDistance <= agent.stoppingDistance);
        //Debug.Log(agent.hasPath && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
        return (agent.hasPath && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
        }
        private void CheckForPlayer()
        {
            directionToPlayer = target.position - transform.position;
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hitInfo))
            {
                inSight = hitInfo.transform.CompareTag("Player");
                Debug.DrawRay(transform.position, directionToPlayer.normalized);
                //if(inSight)
                //Debug.Log("player sighted");
            }
        }

}

