using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class VideoController : MonoBehaviour
{
    VideoPlayer vp;
    public RawImage rawImage;
    public TMP_Text text;
    public VideoClipContainer[] clips;
    public System.Action callback;
    public Image leftHintImage;

    public Image controllerHintImage;
    public Image joystickHintImage;

    bool isStarted = false;

    int n = 0;

    private void Awake()
    {
        vp = GetComponent<VideoPlayer>();
        ChangePage(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChangePage(-1);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangePage(1);
        }
    }

    public void ChangePage(int newIndex)
    {
        if (isStarted) return;

        if (n + newIndex >= clips.Length)
        {
            isStarted = true;
            callback();
            return;
        }
        if (n + newIndex < 0)
        {
            return;
        }

        switch (n + newIndex)
        {
            case 0:
                n += newIndex;
                rawImage.gameObject.SetActive(false);
                controllerHintImage.gameObject.SetActive(true);
                joystickHintImage.gameObject.SetActive(true);
                leftHintImage.gameObject.SetActive(false);
                break;
            default:
                rawImage.gameObject.SetActive(true);
                controllerHintImage.gameObject.SetActive(false);
                joystickHintImage.gameObject.SetActive(false);
                leftHintImage.gameObject.SetActive(true);
                vp.clip = clips[n].clip;
                vp.Play();
                text.text = clips[n].msg;
                break;
        }
    }
}

[System.Serializable]
public class VideoClipContainer
{
    public VideoClip clip;
    public string msg;
}