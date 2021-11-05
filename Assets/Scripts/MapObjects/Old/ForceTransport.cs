using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTransport : MonoBehaviour
{
    public float destinationX;
    public float destinationY;
    public float leftX;
    public float downY;
    public float rightX;
    public float upY;
    public bool TPWhenOutOfRange;
    // Start is called before the first frame update
    void Start()
    {
        TPWhenOutOfRange=true;
    }

    // Update is called once per frame
    void Update()
    {
        {
            if (TPWhenOutOfRange)
            {
                if (transform.localPosition.x <= leftX
                    || transform.localPosition.x >= rightX
                    || transform.localPosition.y <= downY
                    || transform.localPosition.y >= upY)
                {
                    transform.localPosition = new Vector2(destinationX, destinationY);
                }
            }
            else
            {
                if (transform.localPosition.x <= rightX
                    && transform.localPosition.x >= leftX
                    && transform.localPosition.y <= upY
                    && transform.localPosition.y >= downY)
                {
                    transform.localPosition = new Vector2(destinationX, destinationY);
                }
            }
        }
    }
}
