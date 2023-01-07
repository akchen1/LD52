using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum BirdState { Idle, Flee, Wander }
public class BirdAIController : MonoBehaviour
{
    // The target object that the enemy should avoid
    public Transform target;

    // The maximum distance that the enemy will try to stay away from the target
    public float avoidanceDistance = 5f;

    // The distance at which the enemy will start avoiding the target
    public float triggerDistance = 10f;

    // The agent component used for pathfinding
    private IAstarAI agent;

    private Seeker seeker;

    // The radius around the enemy's starting position in which it will wander
    public int wanderRadius = 10;
    public int fleePathDistance = 5;

    public float wanderWaitTime = 2f;
    private float wanderTimer;

    public BirdState birdState;

    private bool stateChanged;

    void Start()
    {
        // Get the agent component
        agent = GetComponent<IAstarAI>();
        seeker = GetComponent<Seeker>();
        agent.destination = transform.position;
        wanderTimer = wanderWaitTime;
    }


    void Update()
    {
        if (birdState == BirdState.Idle) return;
        CalculateBirdState();
        CalculatePath();
    }

    private void CalculateBirdState()
    {
        BirdState newBirdState = InTriggerDistance() ? BirdState.Flee : BirdState.Wander;
        stateChanged = newBirdState != birdState;
        birdState = newBirdState;
    }

    private bool InTriggerDistance()
    {
        return Vector3.Distance(transform.position, target.position) < triggerDistance;
    }


    private Path GetPath()
    {
        RandomPath path;
        if (birdState == BirdState.Flee)
        {
            path = FleePath.Construct(transform.position, target.position, fleePathDistance * 1000);
            path.aimStrength = 1;
        } else
        {
            path = RandomPath.Construct(transform.position, (int)wanderRadius * 1000);
            path.spread = 4000;

        }
        return path;
    }

    private void CalculatePath()
    {

        if ((!agent.pathPending && (agent.reachedEndOfPath || !agent.hasPath)) || stateChanged)
        {
            stateChanged = false;
            Path path = GetPath();
            if (path == null) return;
            agent.SetPath(path);
        }

    }

    

}