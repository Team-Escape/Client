
using PlayerSpace.Game;
using UnityEngine;
namespace Gadget.Effector
{
    public interface IEffector
    {

        Camera GetCamera();


        void UseGadget(int gid);
        GameObject GetPlayer();
        Sprite GetSprite(int gid);
    }

}