using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyPatrol : MonoBehaviour, IKillable
{
    public float moveSpeed = 3;
    public float rotateSpeed = .6f;
    public List<Vector3> patrolPoints = new List<Vector3>();

    GameObject Player = null;
    [NonSerialized] public Vector3 originalPosition = Vector3.zero;

    public int Action = 0;
    [NonSerialized] public float FireRate = .5f;

    int nextPoint = 0;
    float timeStep = 0;

    [NonSerialized] public Quaternion toRotation = Quaternion.identity;
    [NonSerialized] public Quaternion prevRotation = Quaternion.identity;
    [NonSerialized] public Vector3 direction;

    bool isAttacking = false;
    bool isPatrolling = false;
    bool isSearching = false;
    bool isTurning = false;

    [NonSerialized] public Vector3 lastSeenPos = Vector3.zero;

    GameObject cone = null;

    [NonSerialized] public Vector3 tempVector;
    [NonSerialized] public float angle;
    [NonSerialized] public Quaternion rot;

    private NavMeshAgent agent;

    public void Killed()
    {
        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        cone = this.transform.GetChild(0).gameObject;
        agent = GetComponent<NavMeshAgent>();

        this.originalPosition = this.transform.position;
        this.Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.originalPosition.y, this.transform.position.z);

        if (Action != 0) isPatrolling = false;
        if (Action != 1) isAttacking = false;
        if (Action != 2) isSearching = false;
            
        agent.isStopped = false;

        switch (Action)
        {
            case 0:
                Patrol();
                break;
            case 1:
                Attack();
                break;
            case 2:
                if (!isSearching) this.lastSeenPos = Player.transform.position;
                isSearching = true;
                Search();
                break;
            default:
                this.Action = 0;
                break;
        }
    }

    public void SetAction(int num)
    {
        this.Action = num;
    }

    public void SetPlayerPos(Vector3 pos)
    {
        this.lastSeenPos = pos;
    }

    void Patrol()
    {
        if (Vector3.Distance(this.transform.position, patrolPoints[nextPoint]) <= 0.1)
        {
            if (this.nextPoint + 1 < patrolPoints.Count)
                this.nextPoint++;
            else this.nextPoint = 0;

            isPatrolling = false;
        }

        if (!isPatrolling)
        {
            direction = patrolPoints[nextPoint] - this.transform.position;

            prevRotation = this.transform.rotation;
            toRotation = Quaternion.LookRotation(direction);

            this.timeStep = 0;
            this.isTurning = true;
            isPatrolling = true;
        }

        if (isTurning)
        {
            agent.isStopped = true;
            this.transform.rotation = Quaternion.Slerp(prevRotation, toRotation, timeStep);
            if (timeStep == 1) isTurning = false;
        }
        else
        {
            agent.destination = patrolPoints[nextPoint];
        }
            //this.transform.position = Vector3.MoveTowards(this.transform.position, patrolPoints[nextPoint], moveSpeed * Time.fixedDeltaTime);
        

        this.timeStep += Time.fixedDeltaTime * rotateSpeed;
        if (this.timeStep > 1) this.timeStep = 1;
    }

    void Attack()
    {
        if (Player != null)
        {

            Vector3 posPlayer = Player.transform.position;
            //this.transform.LookAt(posPlayer);

            agent.destination = posPlayer;
            if (agent.remainingDistance <= agent.stoppingDistance + 5)
                agent.isStopped = true;
            //this.transform.position = Vector3.MoveTowards(this.transform.position, posPlayer, moveSpeed * Time.fixedDeltaTime);

            if (!isAttacking)
            {
                isAttacking = true;
                Invoke("Attacking", FireRate);
            }

        }
        else
        {
            Debug.Log("Player not in sight");
            isAttacking = false;
        }
    }

    void Attacking()
    {
        if (isAttacking)
        {
            Invoke("Attacking", FireRate);
        }
    }

    void Search()
    {
        agent.destination = lastSeenPos;

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    this.Action = 0;
                }
            }
        }

        //this.transform.position = Vector3.MoveTowards(this.transform.position, lastSeenPos, moveSpeed * Time.fixedDeltaTime);
    }

}
