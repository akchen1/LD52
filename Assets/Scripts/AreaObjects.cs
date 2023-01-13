using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AreaObjects")]
public class AreaObjects : ScriptableObject
{
    public List<GameObject> objects = new List<GameObject>();
    public List<Vector3> positions = new List<Vector3>();
    public List<Vector3> scales = new List<Vector3>();
}
