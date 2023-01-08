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
        foreach (RopeSegment segment in segments)
        {
            if (segment == cutSegment)
            {
                Destroy(segment.gameObject);
            } else
            {

                StartCoroutine(FadeRope(segment));
            }
        }
    }

    private IEnumerator FadeRope(RopeSegment segment)
    {
        Destroy(segment.GetComponent<Joint2D>());
        for (float t = 2f; t >= 0f; t -= Time.deltaTime)
        {
            float normalizedTime = t / 2f;
            Color color = segment.SpriteRenderer.color;
            color.a = normalizedTime;
            segment.SpriteRenderer.color = color;
            yield return 0;
        }
        Destroy(segment.gameObject);
    }
}
