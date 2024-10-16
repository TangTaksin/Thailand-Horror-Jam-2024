using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBull : Ghost
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;
    private int currentPointIndex = 0;

    [Header("Chase Settings")]
    public float ChaseDelay = 3f;
    public float maxChaseSpeed = 10f;
    public float chaseAcceleration = 2f;
    Vector2 chaseDir;

    bool lockedIn;

    public enum State { patrol, prepare, chase}
    State curState = State.patrol;

    protected override void Initialize()
    {
        base.Initialize();

        curState = State.patrol;
    }

    protected override void Update()
    {
        base.Update();

        StateCheck();
    }

    void StateCheck()
    {
        if (player == null) return;

        switch (curState)
        {
            case State.patrol: Patrol(); break;
            case State.prepare: break;
            case State.chase: ChasePlayer(); break;
        }
    }

    private void Patrol()
    {
        Transform targetPoint = patrolPoints[currentPointIndex];
        var dir = targetPoint.position - transform.position;
        dir.Normalize();
        rigid2d.velocity = dir * moveSpeed;

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    private IEnumerator ChaseAfterDelay()
    {
        Debug.Log("Ghost has detected the player. Waiting to start chase...");
        curState = State.prepare;
        rigid2d.velocity = Vector2.zero;
        yield return new WaitForSeconds(ChaseDelay);

        chaseDir = player.position - transform.position;
        chaseDir.Normalize();

        curState = State.chase;
    }

    private void ChasePlayer()
    {
        if (Mathf.Abs(rigid2d.velocity.x) < maxChaseSpeed)
        {
            rigid2d.velocity += chaseDir * chaseAcceleration * Time.deltaTime;
        }
    }

    protected override void OnPlayerDetected(bool value)
    {
        print("OnPlayerDetected");
        if (value && !lockedIn)
        {
            lockedIn = true;
            StartCoroutine(ChaseAfterDelay());
        }
    }
}
