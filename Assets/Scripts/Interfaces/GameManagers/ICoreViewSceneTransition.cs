using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ICoreViewSceneTransition
{
    void UpdateLoadingUI(bool state);
    void UpdateMaskUI(bool state);
    void PlayMaskAnimation(string name);
    void MaskIn(System.Action callback);
    void MaskInWithLoading(System.Action callback, System.Action loadingCallback);
    void MaskOut();
    void MaskOutWithLoading(System.Action loadingcallback);
}
