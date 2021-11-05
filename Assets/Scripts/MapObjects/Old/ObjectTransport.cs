using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransport : MonoBehaviour
{
    [SerializeField] GameObject startPlace = null;
    [SerializeField] GameObject endPlace = null; 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == endPlace)
        {
            transform.position = startPlace.transform.position;
        }
    }
}