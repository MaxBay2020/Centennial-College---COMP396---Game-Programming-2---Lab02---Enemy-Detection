using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking
    }

    public float distanceToChase = 5.0f;
    public float closeEnoughDistance = 2.0f;

    public EnemyState currentState = EnemyState.Patrolling;

    public bool canSeePlayer;

    [SerializeField]
    private GameObject playerGO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleFSM();
    }

    public void HandleFSM()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                HandlePatrollingState();
                break;
            case EnemyState.Chasing:
                HandleChasingState();
                break;
            case EnemyState.Attacking:
                HandleAttachkingState();
                break;
            default:
                break;
        }

    }

    public void HandlePatrollingState()
    {
        Debug.Log(currentState);
        if (CanSeePlayer())
        {
            ChangeState(EnemyState.Chasing);
        }
    }

    public void HandleChasingState()
    {
        Debug.Log(currentState);
        if (!CanSeePlayer())
        {
            ChangeState(EnemyState.Patrolling);
        }
    }

    public void HandleAttachkingState()
    {
        Debug.Log(currentState);
    }

    public void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }

    public bool CanSeePlayer()
    {
        // 1. the distance between the enemy and player is less than 2.0f
        Vector3 enemy2Player_heading = playerGO.transform.position - this.transform.position;
        float distance = Vector3.Magnitude(enemy2Player_heading);

        // 2. the cos angle should be greater than 0;
        float cosAngle_enemy2player = Vector3.Dot(enemy2Player_heading, this.transform.forward);

        canSeePlayer = distance<= distanceToChase && cosAngle_enemy2player>0;

        return canSeePlayer;
    }
}
