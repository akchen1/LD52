using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineController : MonoBehaviour
{
	[SerializeField] private Animator anim;
	[SerializeField] private float attackLength;
	[SerializeField] private float attackCooldown;

	private float timer;
	private bool isAttacking;

	private void Start()
	{
		if (anim == null)
		{
			anim = GetComponent<Animator>();
		}

		timer = 0f;
		isAttacking = false;
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		if (!isAttacking)
		{
			if (timer >= attackCooldown)
			{
				timer = 0f;
				isAttacking = true;
				anim.SetBool("IsAttacking", true);
			}
		}
		else
		{
			if (timer >= attackLength)
			{
				timer = 0f;
				isAttacking = false;
				anim.SetBool("IsAttacking", false);
			}
		}
	}
}
