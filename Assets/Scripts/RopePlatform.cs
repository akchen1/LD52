using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePlatform : MonoBehaviour
{
    private Rigidbody2D rb;
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
}
