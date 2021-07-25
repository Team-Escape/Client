using System;
using UnityEngine;
namespace PlayerSpace.Gameplayer
{
    public class GameItemControl
    {
        internal Model model = null;
        internal Action<float, Action> AbleToDo { get { return model.AbleToDo; } }
        public virtual void Init(Model model)
        {
            this.model = model;
        }
        public virtual void Use() { }
    }
}