using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class Model : MonoBehaviour
    {
        public Rigidbody2D rb;
        PlayerState CurrentPlayerState { get; set; }
        FrontState CurrentFrontState { get; set; }
        GroundState CurrentGroundState { get; set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            CurrentPlayerState = new PlayerState();
            CurrentFrontState = new FrontState();
            CurrentGroundState = new GroundState();
        }
    }

}