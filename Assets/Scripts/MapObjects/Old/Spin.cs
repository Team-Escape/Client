using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    public float spinSpeed;
    [SerializeField]
    public bool clockwise;
    [SerializeField]
    public float angle;

    // Update is called once per frame
    void Update()
    {
        if (angle > 360 || angle<-360)
        {
            angle = 0;
        }
        if (clockwise)
            angle += spinSpeed;
        else
            angle -= spinSpeed;
        transform.rotation = Quaternion.Euler(0,0,angle);
    }
}
