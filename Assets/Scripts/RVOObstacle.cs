using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;
using System.Linq;
public class RVOObstacle : MonoBehaviour
{
    void Start()
    {
        Pathfinding.RVO.Simulator sim = (FindObjectOfType(typeof(RVOSimulator)) as RVOSimulator).GetSimulator();
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        Vector3[] vert = collider.points.Select(x => (Vector3)x).ToArray();
        sim.AddObstacle(vert, 0);
    }
}
