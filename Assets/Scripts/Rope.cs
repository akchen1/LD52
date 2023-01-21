using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private Rigidbody2D platfrom;
    [SerializeField] private RopeSegment[] segments;

    private bool ropeCut;

    public void OnRopeCut(RopeSegment cutSegment)
    {
        if (ropeCut) return;
        ropeCut = true;
        platfrom.gravityScale = 5;
        platfrom.GetComponent<Joint2D>().enabled = false;
        foreach (RopeSegment segment in segments)
        {
            if (segment == cutSegment)
            {
                Destroy(segment.gameObject);
            } else
            {

                StartCoroutine(FadeRope(segment.gameObject));
            }
        }
        if (platfrom.GetComponent<RopePlatform>().isDestroy)
        {
            StartCoroutine(FadeRope(platfrom.gameObject));
        }
    }

    private IEnumerator FadeRope(GameObject segment)
    {
        Destroy(segment.GetComponent<Joint2D>());
        SpriteRenderer spriteRenderer = segment.GetComponent<SpriteRenderer>();
        for (float t = 2f; t >= 0f; t -= Time.deltaTime)
        {
            float normalizedTime = t / 2f;
            Color color = spriteRenderer.color;
            color.a = normalizedTime;
            spriteRenderer.color = color;
            yield return 0;
        }
        Destroy(segment.gameObject);
    }
}
