using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Platform : MonoBehaviour
{
    [SerializeField] Collider2D outerCollider;
    [SerializeField] Collider2D innerCollider;

    [SerializeField] private bool extrude;

    private Vector2 offset;
    void Start()
    {
        offset = outerCollider.offset;
        outerCollider = GenerateEdgeCollider(outerCollider as PolygonCollider2D);

        if (innerCollider != null)
            innerCollider = GenerateEdgeCollider(innerCollider as PolygonCollider2D);

        //if (extrude)
        //{
        //    EdgeCollider2D newCollider = gameObject.AddComponent<EdgeCollider2D>();
        //    ExtrudeCollider(outerCollider as EdgeCollider2D, innerCollider as EdgeCollider2D);
        //}

    }

    private void ExtrudeCollider(EdgeCollider2D collider, EdgeCollider2D newCollider)
    {
        //newCollider.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        // Get the current points of the collider
        Vector2[] originalPoints = collider.points;

        // Create an array to store the scaled points
        Vector2[] scaledPoints = new Vector2[originalPoints.Length];

        for (int i = 0; i < originalPoints.Length; i++)
        {
            //Vector2 offset = originalPoints[i] - center;
            scaledPoints[i] = new Vector2(originalPoints[i].x * 1.5f, originalPoints[i].y * 1.5f);
        }

        // Set the collider's points to the scaled points
        newCollider.offset = offset;
        newCollider.points = scaledPoints;
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
        edge.offset = offset;
        return edge;
    }

    public void ChangePlatformType(bool isTrigger)
    {
        outerCollider.isTrigger = isTrigger;
    }

    public Vector2 GetClosestEdge(Vector2 point)
    {
        
        Vector2 p = outerCollider.ClosestPoint(point);
        Vector2 direction = (p - point).normalized;
        RaycastHit2D hit = Physics2D.Raycast(point, direction, direction.magnitude, 1 << 3);
        Debug.DrawRay(point, direction);
        return hit.normal;
    }
}
