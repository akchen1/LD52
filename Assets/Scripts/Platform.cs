using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Platform : MonoBehaviour
{
    [SerializeField] Collider2D outerCollider;
    [SerializeField] Collider2D innerCollider;
    void Start()
    {
        outerCollider = GenerateEdgeCollider(outerCollider as PolygonCollider2D);
        if (innerCollider != null)
            innerCollider = GenerateEdgeCollider(innerCollider as PolygonCollider2D);
        //outerCollider.isTrigger = true;
    }

    private EdgeCollider2D GenerateEdgeCollider(PolygonCollider2D collider)
    {
        List<Vector2> points = collider.points.ToList();
        EdgeCollider2D edge = collider.gameObject.GetComponent<EdgeCollider2D>();
        if (edge == null)
        {
            edge = collider.gameObject.AddComponent<EdgeCollider2D>();
        }
        points.Add(points[0]);
        edge.points = points.ToArray();
        Destroy(collider);
        return edge;
    }

    public void ChangePlatformType(bool isTrigger)
    {
        outerCollider.isTrigger = isTrigger;
    }
}
