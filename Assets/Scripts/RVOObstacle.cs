using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;
using System.Linq;
public class RVOObstacle : MonoBehaviour
{
    CircleCollider2D CircleCollider2D;
    public bool isActive;
    public Transform guide;
    public Transform player;
    public GameObject thisGround;
    void Start()
    {
        //Pathfinding.RVO.Simulator sim = (FindObjectOfType(typeof(RVOSimulator)) as RVOSimulator).GetSimulator();
        //PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        //Vector3[] vert = collider.points.Select(x => (Vector3)x).ToArray();
        //sim.AddObstacle(vert, 0);
        CircleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (!isActive) return;
        Vector2 center = CircleCollider2D.bounds.center;
        //transform.up = direction;
        //Vector3 direction = guide.position - (Vector3)center;
        Vector3 direction = guide.right;
        RaycastHit2D[] grounds = Physics2D.RaycastAll(guide.position, direction, 10f, 1 << 3);
        //Debug.DrawRay(trans)
        Debug.DrawRay(guide.position, guide.right);
        foreach (RaycastHit2D ground in grounds)
        {
            if (ground.collider != null && ground.collider.gameObject == thisGround)
            {
                Debug.Log(ground.collider.name);
                Debug.Log(ground.point);
                Debug.DrawLine(guide.position, ground.point);
                player.transform.position = ground.point;
                //transform.position = ground.point - offset;
                Vector3 factor = guide.position - (Vector3)ground.point;
                Debug.DrawRay(player.transform.position, ground.normal, Color.red, 10);
                guide.RotateAround(center, Vector3.forward, .01f * factor.magnitude);
                return;
            }

        }
    }
}
