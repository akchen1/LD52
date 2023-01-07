using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Member : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;

    public Level level;
    public MemberConfig conf;
    
    private Vector3 wanderTarget;
    private Rigidbody2D rbody;

    void Start()
    {
        level = FindObjectOfType<Level>();
        conf = FindObjectOfType<MemberConfig>();

        position = transform.position;
        velocity = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
        rbody = GetComponent<Rigidbody2D>();
    }
    //private void OnDrawGizmos()
    //{
    //    Vector3 ahead = position + velocity.normalized * conf.collisionRadius;
    //    Vector3 ahead2 = position + velocity.normalized * conf.collisionRadius * 0.5f;
    //    Gizmos.DrawLine(transform.position, ahead);
    //    Gizmos.DrawLine(transform.position, ahead2);
        

    //}
    private void Update()
    {
        acceleration = Combine();
        acceleration = Vector3.ClampMagnitude(acceleration, conf.maxAcceleration);
        velocity = velocity + acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, conf.maxVelocity);
        position = position + velocity * Time.deltaTime;
        //WrapAround(ref position, -level.bounds, level.bounds);
        transform.position = position;
        //rbody.MovePosition(position);
        //rbody.velocity = velocity;
    }

    protected Vector3 Wander()
    {
        float jitter = conf.wanderJitter * Time.deltaTime;
        wanderTarget += new Vector3(RandomBinomial() * jitter, RandomBinomial() * jitter, 0);
        wanderTarget = wanderTarget.normalized;
        wanderTarget *= conf.wanderRadius;
        Vector3 targetInLocalSpace = wanderTarget + new Vector3(conf.wanderDistance, conf.wanderDistance, 0);
        Vector3 targetInWorldSpace = transform.TransformPoint(targetInLocalSpace);
        targetInWorldSpace -= this.position;
        return targetInWorldSpace.normalized;
    }

    private Vector3 Cohesion()
    {
        Vector3 cohesionVector = new Vector3();
        int countMembers = 0;
        List<Member> neighbors = level.GetNeighbors(this, conf.cohesionRadius);
        if (neighbors.Count == 0) return cohesionVector;
        foreach (Member member in neighbors)
        {
            if (isInFOV(member.position))
            {
                cohesionVector += member.position;
                countMembers++;
            }
        }
        if (countMembers == 0) return cohesionVector;

        cohesionVector /= countMembers;
        cohesionVector = cohesionVector - this.position;
        cohesionVector = Vector3.Normalize(cohesionVector);
        return cohesionVector;
    }

    private Vector3 Alignment()
    {
        Vector3 alignVector = new Vector3();
        List<Member> members = level.GetNeighbors(this, conf.alignmentRadius);
        if (members.Count == 0) return alignVector;
        foreach (Member member in members)
        {
            if (isInFOV(member.position))
            {
                alignVector += member.velocity;
            }
        }
        return alignVector.normalized;

    }

    private Vector3 Separation()
    {
        Vector3 separateVector = new Vector3();
        List<Member> members = level.GetNeighbors(this, conf.separationRadius);
        if (members.Count == 0) return separateVector;
        foreach (Member member in members)
        {
            if (isInFOV(member.position))
            {
                Vector3 movingTowards = this.position - member.position;
                if (movingTowards.magnitude > 0)
                {
                    separateVector += movingTowards.normalized / movingTowards.magnitude;
                }
            }
        }
        return separateVector.normalized;
    }

    private Vector3 Avoidance()
    {
        Vector3 avoidVector = new Vector3();
        List<Enemy> enemyList = level.GetEnemies(this, conf.avoidanceRadius);
        if (enemyList.Count == 0) return avoidVector;
        foreach (Enemy enemy in enemyList)
        {
            avoidVector += RunAway(enemy.transform.position);
        }
        return avoidVector.normalized;
    }

    private Vector3 RunAway(Vector3 target)
    {
        Vector3 neededVelocity = (position - target).normalized * conf.maxVelocity;
        return neededVelocity - velocity;
    }

    private Vector3 Avoid(Vector3 target)
    {
        Vector3 avoidVelocity = (target - position - target).normalized * conf.maxVelocity;
        return avoidVelocity;
    }

    private Vector3 CollisionAvoidance()
    {
        Vector3 collisionVector = new Vector3();
        LayerMask wallMask = LayerMask.NameToLayer("wall");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(position, conf.collisionRadius, Vector2.zero, 0);
        hits = hits.Where(x => x.collider.tag == "Wall").ToArray();
        if (hits.Length == 0) return collisionVector;
        
        RaycastHit2D closest = hits.OrderBy(x => Vector3.Distance(x.point, position)).ToArray()[0];
        Debug.Log(closest.collider.name);
        collisionVector += ((Vector3)closest.point - closest.transform.position).normalized * conf.maxVelocity - velocity;
        //foreach (RaycastHit2D collider in hits)
        //{
        //    if (collider.collider != null && collider.collider.tag == "Wall")
        //    {
        //        Debug.Log(name + " hit " + collider.collider.name);

        //        collisionVector += ((Vector3)collider.point - collider.transform.position).normalized * conf.maxVelocity - velocity;
        //    }
        //}
        //Debug.Log(collisionVector.normalized);
        return collisionVector.normalized;
    }
    private Vector3 CollisionAvoidance1()
    {
        Vector3 collisionVector = new Vector3();
        LayerMask wallMask = LayerMask.NameToLayer("wall");
        Vector3 ahead = position + velocity.normalized * conf.collisionRadius;
        Vector3 ahead2 = position + velocity.normalized * conf.collisionRadius * 0.5f;

        RaycastHit2D[] hit = Physics2D.RaycastAll(position, velocity.normalized, conf.collisionRadius);
        //RaycastHit2D[] hit2 = Physics2D.RaycastAll(position, ahead2);

        Debug.DrawLine(position, ahead);
        Debug.DrawLine(position, ahead2);

        foreach (RaycastHit2D h in hit)
        {
            if (h.collider.tag == "Wall")
            {
                Debug.Log("hit " + h.collider.name);
                collisionVector += ((Vector3)h.point - h.collider.transform.position) - ahead;
                //Debug.Log(collisionVector.normalized * conf.maxVelocity);
                //break;
            }
        }

        //if (hit.collider != null && hit.collider.tag == "Wall")
        //{
        //    collisionVector = ahead - hit.collider.transform.position;
        //    collisionVector = collisionVector.normalized;
        //} else if (hit2.collider != null && hit2.collider.tag == "Wall")
        //{
        //    collisionVector = ahead - hit2.collider.transform.position;
        //    collisionVector = collisionVector.normalized;
        //}

        //foreach (RaycastHit2D collider in hits)
        //{
        //    if (collider.collider != null && collider.collider.tag == "Wall")
        //    {
        //        Debug.Log(name + " hit " + collider.collider.name);

        //        collisionVector += Avoid(collider.point);
        //    }
        //}
        //Debug.Log(collisionVector.normalized);
        return collisionVector.normalized * conf.maxVelocity;
    }
    virtual protected Vector3 Combine()
    {
        Vector3 finalVec = conf.cohesionPriority * Cohesion() + conf.wanderPriority * Wander() 
            + conf.alignmentPriority * Alignment() + conf.separationPriority * Separation() + conf.avoidancePriority * Avoidance() + conf.collisionPriority * CollisionAvoidance();
        return finalVec;
    }

    private void WrapAround(ref Vector3 vector, float min, float max)
    {
        vector.x = WrapAroundFloat(vector.x, min, max);
        vector.y = WrapAroundFloat(vector.y, min, max);
        vector.z = WrapAroundFloat(vector.z, min, max);

    }

    private float WrapAroundFloat(float value, float min, float max)
    {
        if (value > max)
        {
            value = min;
        } else if (value < min)
        {
            value = max;
        }
        return value;
    }

    private float RandomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    private bool isInFOV(Vector3 vec)
    {
        return Vector3.Angle(this.velocity, vec - this.position) <= conf.maxFOV;
    }
}
