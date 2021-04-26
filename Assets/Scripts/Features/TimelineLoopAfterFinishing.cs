using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineLoopAfterFinishing : MonoBehaviour
{
    [SerializeField] TimelineAsset[] timelineAssets = new TimelineAsset[2];
    PlayableDirector director = null;
    int currentIndex = 0;
    bool isStarted = false;

    public void Init()
    {
        currentIndex = 0;
        RebindAndPlay(timelineAssets[currentIndex]);
        isStarted = true;
    }
    private void Update()
    {
        if (!isStarted) return;

        currentIndex = 0;
        if (director.state != PlayState.Playing)
        {
            if (currentIndex > 0) return;
            currentIndex++;
            RebindAndPlay(timelineAssets[currentIndex]);
        }
    }
    void RebindAndPlay(TimelineAsset _asset)
    {
        director.playableAsset = _asset;
        director.RebuildGraph();
        director.time = 0f;
        director.Play();
    }
    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }
}