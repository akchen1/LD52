using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberConfig : MonoBehaviour
{
    public float maxFOV = 180;
    public float maxAcceleration;
    public float maxVelocity;

    // Wander variables
    public float wanderJitter;
    public float wanderRadius;
    public float wanderDistance;
    public float wanderPriority;

    // Cohesion Variables
    public float cohesionRadius;
    public float cohesionPriority;

    // Alignment variables
    public float alignmentRadius;
    public float alignmentPriority;

    // Separation Variables;
    public float separationRadius;
    public float separationPriority;

    // Avoidance Variables
    public float avoidanceRadius;
    public float avoidancePriority;

    // Collision Avoidance Variables
    public float collisionRadius;
    public float collisionPriority;
}
