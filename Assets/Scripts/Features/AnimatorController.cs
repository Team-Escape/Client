using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] float whenToPlay = -1f;
    [SerializeField] string autoTriggerParameter = "first";

    Animator animator = null;

    public void SetTrigger(string name)
    {
        animator.SetTrigger(name);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (whenToPlay >= 0)
        {
            StartCoroutine(AutoPlayAfterSecs());
        }
    }

    IEnumerator AutoPlayAfterSecs()
    {
        yield return new WaitForSeconds(whenToPlay);
        SetTrigger(autoTriggerParameter);
    }

}
