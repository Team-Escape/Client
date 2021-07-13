using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP : MonoBehaviour
{
    [SerializeField] Transform endPos = null;
    private void OnTriggerStay2D(Collider2D other)
    {
        other.transform.position = endPos.position;
    }
}
