using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isDestroy;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
}
