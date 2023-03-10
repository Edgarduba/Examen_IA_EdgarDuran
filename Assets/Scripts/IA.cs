using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA : MonoBehaviour
{
    enum State {Patrolling, Chasing, Attack, Wait}

    State currentState;
    private NavMeshAgent agente;

    public Transform[] destinationPoints;
    int destinationIndex;

    [SerializeField] Transform player;
    [SerializeField] float rangovision;
    [SerializeField] float rangoataque;

    [SerializeField] float esperar;
    float elapsedTime;

    void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentState = State.Patrolling;
        destinationIndex = 0;
    }

    
    void Update()
    {
        switch(currentState)
        {
            case State.Patrolling:
                Patrol();
            break;

            case State.Wait: 
                Waiting();
            break;

            case State.Chasing:
                Chase();
            break;

            case State.Attack:
                Debug.Log("Attack");
                currentState = State.Patrolling;
            break;

            default:
                currentState = State.Patrolling;
            break;
        }
    }

    void Patrol()
    {
        agente.destination = destinationPoints[destinationIndex].position;

        if(Vector3.Distance(transform.position, destinationPoints[destinationIndex].position)< 1)
        {
            if(destinationIndex == (destinationPoints.Length - 1))
            {
                destinationIndex = 0;
                currentState = State.Wait;
            }
            else
            {
                destinationIndex ++;
                currentState = State.Wait;
            }
        }

        if(Vector3.Distance(transform.position, player.position) < rangovision)
        {
            currentState = State.Chasing;
        }
    }

    void Chase()
    {
        agente.destination = player.position;
        if(Vector3.Distance(transform.position,player.position)> rangovision)
        {
            currentState = State.Patrolling;
        }
        if(Vector3.Distance(transform.position,player.position)< rangoataque)
        {
            currentState = State.Attack;
        }
    }

    void Waiting()
    {
        
        elapsedTime += Time.deltaTime;
        

        if(elapsedTime >= esperar)
        {
            currentState = State.Patrolling;
            elapsedTime = 0;
        }
    }

    private void OnDrawGizmos() 
    {
        foreach(Transform point in destinationPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(point.position, 1);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangovision);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoataque);
    }
}
