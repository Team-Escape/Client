using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagerSpace.Audio
{
    enum AudioInScene
    {
        None,
        Menu,
        Hall,
        Game,
    }
    public class Model : MonoBehaviour
    {
        public AudioClip menuAudio;
        public AudioClip hallAudio;
        public AudioClip gameAudio;
    }
}