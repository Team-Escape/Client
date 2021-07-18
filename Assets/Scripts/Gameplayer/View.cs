using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class View : MonoBehaviour
    {
        Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
    }
}