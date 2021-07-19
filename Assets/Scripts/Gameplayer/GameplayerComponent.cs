using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class GameplayerComponent
    {
        internal View view;
        internal Model model;
        internal Animator anim { get { return model.anim; } }
        internal Rigidbody2D rb { get { return model.rb; } }
        internal GroundState CurrentGroundState { get { return model.CurrentGroundState; } set { model.CurrentGroundState = value; } }
        internal FrontState CurrentFrontState { get { return model.CurrentFrontState; } set { model.CurrentFrontState = value; } }
    }
}