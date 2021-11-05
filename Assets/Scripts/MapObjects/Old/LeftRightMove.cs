using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMove : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float distance;
    public bool goRight;
    public float delay;
    public float startDelay;
    float time;
    float startTime;
    bool counting;
    private Vector2 nowSpot;
    bool isRight;
    [SerializeField] Transform originSpot = null;

    // Start is called before the first frame update
    void Start()
    {
        if (goRight) isRight = true;
        else isRight = false;
        nowSpot = transform.position;
        rb = GetComponent<Rigidbody2D>();
        time = 0;
        startTime = 0;
        counting = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (startTime < startDelay)
            startTime += Time.deltaTime;
        if (startTime >= startDelay)
        {
            time += Time.deltaTime;
            nowSpot = transform.position;
            //Debug.Log(nowSpot.x - originSpot.position.x);
            if (goRight)
            {
                if (nowSpot.x - originSpot.position.x >= distance)
                {
                    isRight = false;
                    if (counting == true)
                    {
                        time = 0f;
                        counting = false;
                    }
                }
                else if (nowSpot.x - originSpot.position.x <= 0)
                {
                    isRight = true;
                    if (counting == true)
                    {
                        time = 0f;
                        counting = false;
                    }
                }
            }
            else
            {
                if (nowSpot.x >= originSpot.position.x)
                {
                    isRight = false;
                    if (counting == true)
                    {
                        time = 0f;
                        counting = false;
                    }
                }
                else if (originSpot.position.x - nowSpot.x >= distance)
                {
                    isRight = true;
                    if (counting == true)
                    {
                        time = 0f;
                        counting = false;
                    }
                }
            }
            if (time >= delay)
            {
                counting = true;
                if (isRight == true)
                {
                    rb.velocity = new Vector2(speed, 0);
                }
                else
                    rb.velocity = new Vector2(-speed, 0);
            }
            else rb.velocity = new Vector2(0, 0);
        }
    }
}
