using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class AnimationCore : MonoBehaviour
{
    [SerializeField] string animationName = "";

    [SerializeField] bool isLooping = true;
    [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(isLooping))]
    [SerializeField] float min = 0.5f, max = 5f;
    [SerializeField] bool isDelay = false;
    [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And, nameof(isDelay))]
    [SerializeField] float delayDuration = 5;


    Animator animator = null;
    bool ableToPlay = false;

    public System.Action AnimationCallback { get; set; }


    public void AnimationEvent()
    {
        AnimationCallback();
    }

    public IEnumerator WaitForFinish()
    {
        ableToPlay = false;
        StopAllCoroutines();
        CancelInvoke();
        while (AnimatorIsPlaying() == false)
        {
            yield return null;
        }
        ableToPlay = false;
        animator.enabled = false;
    }

    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Empty");
    }

    private void SetAbleToPlay()
    {
        ableToPlay = true;
    }

    private void Update()
    {
        if (ableToPlay == false) return;

        ableToPlay = false;
        CancelInvoke();
        StopAllCoroutines();

        if (isLooping)
        {
            float delay = Random.Range(min, 5f);
            delay = (delay > max) ? max : delay;
            StartCoroutine(WaitForPlay(delay));
        }
    }

    IEnumerator WaitForPlay(string name, System.Action callback)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float length = 0;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == name)
            {
                length = clip.length;
                break;
            }
        }

        animator.Play(animationName, 0, 0);
        yield return new WaitForSeconds(length);
        callback();
    }

    IEnumerator WaitForPlay(float sec)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float length = 0;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                length = clip.length;
                break;
            }
        }

        animator.Play(animationName, 0, 0);

        Invoke("SetAbleToPlay", length + sec);
        yield return new WaitForSeconds(length + sec);
        ableToPlay = true;
    }

    private void OnEnable()
    {
        if (animator == null) animator = GetComponent<Animator>();
        animator.enabled = true;

        if (isLooping) Invoke("SetAbleToPlay", 0.1f + (isDelay ? delayDuration : 0));
        else StartCoroutine(WaitForPlay(0.1f + (isDelay ? delayDuration : 0)));
    }

    private void OnDisable()
    {
        ableToPlay = false;
    }
}
#endif