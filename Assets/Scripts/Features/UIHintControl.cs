using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class UIHintControl : MonoBehaviour
{
    public bool doDectectPlayer = false;

    public Sprite keyboard = null;
    public Sprite joystick = null;
    Image image;

    public void SetCurrentControllerImage(int mode)
    {
        switch (mode)
        {
            case 0:
                image.sprite = joystick;
                break;
            case 1:
                image.sprite = keyboard;
                break;
        }
    }

    private void Awake()
    {
        if (GetComponent<Image>())
        {
            image = GetComponent<Image>();
        }
        else if (GetComponentInChildren<Image>())
        {
            image = GetComponentInChildren<Image>();
        }
        image.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (doDectectPlayer == false) return;
        if (other.tag == "Player")
        {
            image.enabled = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (doDectectPlayer == false) return;
        if (other.tag == "Player")
        {
            image.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            image.enabled = false;
        }
    }
}
