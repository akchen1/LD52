using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Platform : MonoBehaviour
{
	private Collider2D platformCollider;

    private void Start()
    {
		platformCollider = GetComponent<Collider2D>();
    }

    public Vector2 GetClosestEdge(Vector2 point)
	{

		Vector2 p = platformCollider.ClosestPoint(point);
		Vector2 direction = (p - point).normalized;
		RaycastHit2D hit = Physics2D.Raycast(point, direction, direction.magnitude, 1 << 3);
		return hit.normal;
	}
}
