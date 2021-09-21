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

    // patrolling path points
    public Transform[] wayPoints;
    public int currentWayPointsIndex;
    public float enemySpeed = 1.0f; // 1m/s

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

        FollowPatrollingPath();
    }

    public void HandleChasingState()
    {
        
        if (!CanSeePlayer())
        {
            ChangeState(EnemyState.Patrolling);
        }

        this.transform.position = MyMoveTowards(this.transform.position, playerGO.transform.position, enemySpeed*Time.deltaTime*1.5f);
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

    private void FollowPatrollingPath()
    {
        Vector3 target = wayPoints[currentWayPointsIndex].position;
        //Vector3.Distance(this.transform.position, target) < 0.1f
        //if (this.transform.position==target)
        if(Vector3.Distance(this.transform.position, target) < 0.1f)
        {
            currentWayPointsIndex = CalculateNextWayPointIndex();
            target = wayPoints[currentWayPointsIndex].transform.position;
        }

        //this.transform.position = Vector3.MoveTowards(this.transform.position, target,enemySpeed*Time.deltaTime);
        this.transform.position = MyMoveTowards(this.transform.position, target, enemySpeed * Time.deltaTime);

    }

    private Vector3 MyMoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
    {
        Vector3 current2Target = target - current;

        //-------- Start of Changes related to Rotation -------------
        //this.transform.LookAt(target); //Abrupt1: this is a too abrupt rotation; see also Abrupt2 below

        Quaternion qtargetrotation = Quaternion.LookRotation(current2Target);
        //this.transform.rotation = qtargetrotation;  //Abrupt2: this is a too abrupt rotation; not very beleivable; to try, uncomment this and comment out the line below

        // This is the smoothest rotation; more beleivable
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, qtargetrotation, Time.deltaTime);
        //-------- End of Changes related to Rotation -------------

        Vector3 movement = current + current2Target.normalized * maxDistanceDelta;
        return movement;

    }



    private int CalculateNextWayPointIndex()
    {
        return ((currentWayPointsIndex + 1) % wayPoints.Length);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        

        for (int i = 0; i < wayPoints.Length; i++)
        {
            Gizmos.DrawLine(wayPoints[i].position, wayPoints[(i + 1) % wayPoints.Length].position);
        }
    }
}
