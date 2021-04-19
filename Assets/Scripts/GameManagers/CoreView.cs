using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LoadingTransitionContainer
{
    public GameObject GetSelf { get { return selfObject; } }
    [SerializeField] GameObject selfObject = null;
}
[System.Serializable]
public class MaskTransitionContainer
{
    public GameObject GetSelf { get { return selfObject; } }
    [SerializeField] GameObject selfObject = null;
    public Animator GetTransition { get { return transition; } }
    [SerializeField] Animator transition = null;
}

public class CoreView : MonoBehaviour, ICoreViewSceneTransition
{
    [SerializeField] LoadingTransitionContainer loadingContainer = null;
    [SerializeField] MaskTransitionContainer maskContainer = null;
    bool isPlaying = false;

    public void UpdateLoadingUI(bool state)
    {
        loadingContainer.GetSelf.SetActive(state);
    }

    public void UpdateMaskUI(bool state)
    {
        maskContainer.GetSelf.SetActive(state);
    }

    public void PlayMaskAnimation(string name)
    {
        maskContainer.GetTransition.Play(name);
    }

    public void MaskIn(System.Action callback)
    {
        if (isPlaying) return;

        string name = "MaskIn";
        UpdateMaskUI(true);
        PlayMaskAnimation(name);
        StartCoroutine(WaitForPlay(name, callback));
    }
    public void MaskInWithLoading(System.Action callback, System.Action loadingCallback)
    {
        if (isPlaying) return;

        string name = "MaskIn";
        UpdateMaskUI(true);
        PlayMaskAnimation(name);
        StartCoroutine(WaitForPlay(name, callback));
        StartCoroutine(WaitForPlay(name, loadingCallback));
    }

    public void MaskOut()
    {
        if (isPlaying) return;

        string name = "MaskOut";
        PlayMaskAnimation(name);
        StartCoroutine(WaitForPlay(name, () => UpdateMaskUI(false)));
    }

    public void MaskOutWithLoading(System.Action callback)
    {
        callback();
        if (isPlaying) return;

        string name = "MaskOut";
        PlayMaskAnimation(name);
        StartCoroutine(WaitForPlay(name, () => UpdateMaskUI(false)));
    }

    IEnumerator WaitForPlay(string name, System.Action callback)
    {
        isPlaying = true;
        AnimationClip[] clips = maskContainer.GetTransition.runtimeAnimatorController.animationClips;
        float length = 0;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == name)
            {
                length = clip.length;
                break;
            }
        }
        yield return new WaitForSeconds(length);
        isPlaying = false;
        if (callback != null) callback();
    }
}
