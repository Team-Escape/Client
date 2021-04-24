using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PressAnyButton : MonoBehaviour
{
    [SerializeField] string nextScene = "MenuScene";
    public System.Action<string> SceneCallbackAction { get; set; }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
                SceneCallbackAction(nextScene);
        }
    }
}
