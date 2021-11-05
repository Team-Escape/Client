using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class UIHintControl : MonoBehaviour
{
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
        SetComponent();
    }

    void SetComponent()
    {
        if (GetComponent<Image>())
        {
            image = GetComponent<Image>();
        }
        else if (GetComponentInChildren<Image>())
        {
            image = GetComponentInChildren<Image>();
        }
    }
}
