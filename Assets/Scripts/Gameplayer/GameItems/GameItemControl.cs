using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class GameItemControl
    {
        internal Model model = null;

        public virtual void Init(Model model)
        {
            this.model = model;
        }
    }
}