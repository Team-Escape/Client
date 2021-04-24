using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour
{
    [SerializeField] float delay = 0f;
    [SerializeField] Playable[] playables = null;
    [SerializeField] TimelineAsset[] timelineAssets = null;

    PlayableDirector director = null;
    int currentPlayable = 0;

    public void PlayNextCut()
    {
        currentPlayable++;
        director.playableAsset = timelineAssets[currentPlayable];
        director.Play();
    }

    public void Init()
    {
        this.AbleToDo(delay, () => director.Play());
        StartCoroutine(WaitForDirecotrFinished(currentPlayable));
    }

    IEnumerator WaitForDirecotrFinished(int index)
    {
        while (!playables[index].IsDone())
        {
            yield return null;
        }
        PlayNextCut();
    }

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }
}
