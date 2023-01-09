using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePlatform : MonoBehaviour
{
    private Rigidbody2D rb;

    public Vector2 landingPoint;
    public Vector2 offset;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
    
    public void SetLandingPoint(Vector2 point)
    {
        landingPoint = point;
        offset = (Vector2)transform.position - point;
    }
}
