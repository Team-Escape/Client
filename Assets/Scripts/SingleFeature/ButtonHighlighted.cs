using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlighted : MonoBehaviour//, IPointerEnterHandler//, ISelectHandler, IDeselectHandler
{
    System.Action<Button> highlightAction = null;
    Button button = null;
    Animator animator = null;

    public void Selected()
    {
        animator.SetTrigger("Selected");
    }
    public void Deselected()
    {
        animator.SetTrigger("Normal");
    }
    public void Pressed()
    {
        animator.SetTrigger("Pressed");
        this.AbleToDo(0.1f, () => button.onClick.Invoke());
    }

    private void OnMouseOver()
    {
        highlightAction(button);
    }

    private void OnMouseDown()
    {
        Pressed();
    }

    public void Init(System.Action<Button> callback)
    {
        highlightAction = callback;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        animator = GetComponent<Animator>();
        GetComponent<BoxCollider2D>().enabled = true;
    }

    IEnumerator WaitForPlay(string name, System.Action callback)
    {
        yield return new WaitForSeconds(0.5f);
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

        animator.Play(name, 0, 0);
        yield return new WaitForSeconds(length);
        callback();
    }
}

// public void OnSelect(BaseEventData eventData)
// {
//     if (selectionAction == null) return;
//     selectionAction(GetComponent<Button>());
// }
// public void OnDeselect(BaseEventData eventData)
// {
//     if (selectionAction == null) return;
//     deselectAction(GetComponent<Button>());
// }