using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerSpace.Audio
{
    public class AudioManager : MonoBehaviour
    {
        Model model = null;
        AudioInScene audioInScene = new AudioInScene();
        AudioSource audioSource = null;

        public void ChangeAudio(string newState)
        {
            audioInScene.ChangeState(newState);
            switch (audioInScene)
            {
                case AudioInScene.Menu:
                    audioSource.clip = model.menuAudio;
                    break;
                case AudioInScene.Hall:
                    audioSource.clip = model.hallAudio;
                    break;
                case AudioInScene.Game:
                    audioSource.clip = model.gameAudio;
                    break;
                default:
                    break;
            }
            audioSource.Play();
        }

        private void Awake()
        {
            audioInScene = AudioInScene.None;
            audioSource = GetComponent<AudioSource>();
            model = GetComponent<Model>();
        }
    }
}

