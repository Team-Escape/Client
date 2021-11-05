using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public void PlaySF(GameObject go, AudioClip clip)
    {
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(WaitForAudioPlay(go, audioSource, clip));
    }

    IEnumerator WaitForAudioPlay(GameObject go, AudioSource audioSource, AudioClip audioClip)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(audioSource);
    }
}
