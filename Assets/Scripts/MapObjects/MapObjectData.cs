using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectData : MonoBehaviour
{
    public int id = 0;
    public Transform endpoint;
    public Transform entrance;
    public Transform exit;
    public Transform nextCollider;
    public PolygonCollider2D polygonCollider2D;
}
