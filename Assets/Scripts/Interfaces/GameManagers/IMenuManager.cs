using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerSpace.Menu
{
    interface IMenuManager
    {
        void Init(System.Action<string, bool> callback, System.Action audioCallback);
        void AnimationEventCallback();
        void Play();
        void Settings();
        void Exit();
        void Confirm();
    }
}