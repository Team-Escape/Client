
using PlayerSpace.Game;
using UnityEngine;
using System.Collections.Generic;
namespace Gadget.Effector
{
    public interface IEffector
    {

        Camera GetCamera();


        void UseGadget(int gid);
        GameObject GetPlayer();
        Sprite GetSprite(int gid);
        GadgetEffect[] GetGadgetEffects();
    }

}