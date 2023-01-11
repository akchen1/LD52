using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.GetComponent<SpriteRenderer>().bounds.size.x);
        Debug.Log(gameObject.GetComponent<SpriteRenderer>().bounds.size.y);
    }


}
