using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PolygonColliderToEdgeColliderEditor : EditorWindow
{
	[MenuItem("Tools/Convert PolygonColliders to EdgeColliders")]
	static void ConvertPolygonCollidersToEdgeColliders()
	{
		// Get all selected GameObjects
		GameObject[] gameObjects = Selection.gameObjects;

		// Prompt the user to enter the expansion distance
		float sizeIncrease = 0.25f;
		// For each selected GameObject, convert its PolygonColliders to EdgeColliders
		foreach (GameObject gameObject in gameObjects)
		{
			gameObject.tag = "InnerWall";
			gameObject.layer = 3;
			// Get the inner collider component
			PolygonCollider2D innerTempCollider = gameObject.GetComponent<PolygonCollider2D>();

			// Create a new game object for the outer collider
			GameObject outerObject = new GameObject("Platform");

			outerObject.AddComponent<Platform>();
			outerObject.transform.position = gameObject.transform.position;
			outerObject.transform.SetParent(gameObject.transform.parent);
			gameObject.transform.SetParent(outerObject.transform);
			outerObject.tag = "Wall";
			outerObject.layer = 3;

			// Add an outer collider component to the new game object
			PolygonCollider2D outerTempCollider = outerObject.AddComponent<PolygonCollider2D>();

			// Set the outer collider's shape to match the inner collider's shape, but larger
			Vector2[] outerPoints = new Vector2[innerTempCollider.points.Length];
			for (int i = 0; i < innerTempCollider.points.Length; i++)
			{
				Vector2 point = innerTempCollider.points[i];
				float length = Mathf.Sqrt(point.x * point.x + point.y * point.y) + sizeIncrease;
				float angle = Mathf.Atan2(point.y, point.x);
				outerPoints[i] = new Vector2(length * Mathf.Cos(angle), length * Mathf.Sin(angle));
			}
			outerTempCollider.points = outerPoints;

			List<Vector2> points = outerTempCollider.points.ToList();
			EdgeCollider2D edge = outerTempCollider.gameObject.GetComponent<EdgeCollider2D>();
			if (edge == null)
			{
				edge = outerTempCollider.gameObject.AddComponent<EdgeCollider2D>();
			}
			points.Add(points[0]);
			edge.points = points.ToArray();
			DestroyImmediate(outerTempCollider);
			//outerObject.GetComponent<Platform>().platformCollider = edge;

			points = innerTempCollider.points.ToList();
			edge = innerTempCollider.gameObject.GetComponent<EdgeCollider2D>();
			if (edge == null)
			{
				edge = innerTempCollider.gameObject.AddComponent<EdgeCollider2D>();
			}
			points.Add(points[0]);
			edge.points = points.ToArray();
			//DestroyImmediate(innerTempCollider);

		}
	}
}