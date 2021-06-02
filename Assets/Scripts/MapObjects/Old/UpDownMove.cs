/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownMove : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float distance;
    public bool goUp;
    public float delay;
    public float startDelay;
    [SerializeField] Transform originSpot = null;
    private Vector2 nowSpot;
    bool isUp;
    float time;
    bool counting;
    float startTime;
    
    // Start is called before the first frame update
    void Start()
    {
        if (goUp) isUp = true;
        else isUp = false;
        nowSpot = transform.position;
        rb = GetComponent<Rigidbody2D>();
        time = 0;
        startTime = 0;
        counting = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(startTime < startDelay)
            startTime += Time.deltaTime;
        if (startTime >= startDelay)
        {
            time += Time.deltaTime;
            nowSpot = transform.position;
            if (goUp)
            {
                if ((nowSpot.y - originSpot.position.y) > distance)
                {
                    isUp = false;
                    if (counting == true)
                    {
                        time = 0f;
                        counting = false;
                    }

                }
                else if (nowSpot.y - originSpot.position.y < 0)
                {
                    isUp = true;
                    if (counting == true)
                    {
                        time = 0f;
                        counting = false;
                    }

                }
            }
            else
            {
                if (nowSpot.y > originSpot.position.y)
                {
                    isUp = false;
                    if (counting == true)
                    {
                        time = 0f;
                        counting = false;
                    }

                }
                else if (originSpot.position.y - nowSpot.y > distance)
                {
                    isUp = true;
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
                if (isUp == true)
                {
                    rb.velocity = new Vector2(0, speed);
                }
                else
                    rb.velocity = new Vector2(0, -speed);
            }
            else rb.velocity = new Vector2(0, 0);
        }
    }
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownMove : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float distance;
    public bool goUp;
    public float delay;
    public float startDelay;
    float time;
    float startTime;
    bool counting;
    private Vector2 nowSpot;
    bool isUp;
    [SerializeField] Transform originSpot = null;

    // Start is called before the first frame update
    void Start()
    {
        if (goUp) isUp = true;
        else isUp = false;
        nowSpot = transform.position;
        rb = GetComponent<Rigidbody2D>();
        time = 0;
        startTime = 0;
        counting = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(nowSpot.y - originSpot.position.y);
        if (startTime < startDelay)
            startTime += Time.deltaTime;
        if (startTime >= startDelay)
        {
            time += Time.deltaTime;
            nowSpot = transform.position;
            //Debug.Log(nowSpot.x - originSpot.position.x);
            if (goUp)
            {
                if (nowSpot.y - originSpot.position.y >= distance)
                {
                    isUp = false;
                    if (counting == true)
                    {
                        time = 0f;
                        counting = false;
                    }
                }
                else if (nowSpot.y - originSpot.position.y <= 0)
                {
                    isUp = true;
                    if (counting == true)
                    {
                        time = 0f;
                        counting = false;
                    }
                }
            }
            else
            {
                if (nowSpot.y >= originSpot.position.y)
                {
                    isUp = false;
                    if (counting == true)
                    {
                        time = 0f;
                        counting = false;
                    }
                }
                else if (originSpot.position.y - nowSpot.y >= distance)
                {
                    isUp = true;
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
                if (isUp == true)
                {
                    rb.velocity = new Vector2(0, speed);
                }
                else
                    rb.velocity = new Vector2(0,-speed);
            }
            else rb.velocity = new Vector2(0, 0);
        }
    }
}
