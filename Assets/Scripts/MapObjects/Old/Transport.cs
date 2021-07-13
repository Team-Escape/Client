using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
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