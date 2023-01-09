using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Platform : MonoBehaviour
{
	public Collider2D outerCollider;
	public Collider2D innerCollider;
	void Start()
	{
		//outerCollider.isTrigger = true;
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
