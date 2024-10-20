using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBull : Ghost
{
    Vector3 origin_point;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;
    private int currentPointIndex = 0;

    [Header("Chase Settings")]
    public float ChaseDelay = 3f;
    public float maxChaseSpeed = 10f;
    public float chaseAcceleration = 2f;
    public Vector3 offsetChase = new Vector3(0, 2.2f, 0);

    public bool disable_on_return;
    public float return_time = 30f;
    float return_timer;

    Vector2 chaseDir;

    bool lockedIn;

    public enum State { patrol, prepare, chase }
    public State initial_state;
    State curState = State.patrol;

    protected override void OnEnable()
    {
        origin_point = transform.position;
        base.OnEnable();
    }

    protected override void Initialize()
    {
        base.Initialize();

        curState = initial_state;

        if (curState == State.prepare)
        {
            StartCoroutine(ChaseAfterDelay());
        }
        else
            lockedIn = false;
    }

    protected void ReturnToOrigin()
    {
        transform.position = origin_point;
        Initialize();
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
            case State.prepare: Prepare(); break;
            case State.chase: ChasePlayer(); break;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPointIndex];
        var dir = targetPoint.position - transform.position;
        dir.Normalize();
        rigid2d.velocity = dir * moveSpeed;

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    void Prepare()
    {

        chaseDir = (player.position - transform.position) + offsetChase;
        chaseDir.Normalize();
        ghostDir = chaseDir.x;

    }

    private IEnumerator ChaseAfterDelay()
    {
        Debug.Log("Ghost has detected the player. Waiting to start chase...");
        lockedIn = true;
        curState = State.prepare;
        rigid2d.velocity = Vector2.zero;

        yield return new WaitForSeconds(ChaseDelay);

        return_timer = 0;

        curState = State.chase;
    }

    private void ChasePlayer()
    {
        return_timer += Time.deltaTime;
        if (return_timer >= return_time)
        {
            if (disable_on_return)
                gameObject.SetActive(false);
            else
                ReturnToOrigin();
        }

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
            StartCoroutine(ChaseAfterDelay());
        }
    }
}
