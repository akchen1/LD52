using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    [SerializeField] Rope rope;
    public SpriteRenderer SpriteRenderer;
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Destroy(this.gameObject);
        if (collision.gameObject.tag != "Player") return;
        PlayerLauncher player = collision.gameObject.GetComponent<PlayerLauncher>();
        if (player.state == PlayerLauncher.PlayerState.InAir)
            rope.OnRopeCut(this);
    }
}
