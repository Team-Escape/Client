using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class GameplayerComponent
    {
        internal View view;
        internal Model model;
        internal Transform transform { get { return model.transform; } }
        internal Animator anim { get { return model.anim; } }
        internal Rigidbody2D rb { get { return model.rb; } }
        internal PlayerState CurrentPlayerState { get { return model.CurrentPlayerState; } set { model.CurrentPlayerState = value; } }
        internal GroundState CurrentGroundState { get { return model.CurrentGroundState; } set { model.CurrentGroundState = value; } }
        internal FrontState CurrentFrontState { get { return model.CurrentFrontState; } set { model.CurrentFrontState = value; } }
        internal System.Action<float, System.Action> AbleToDo { get { return model.AbleToDo; } }
    }
}