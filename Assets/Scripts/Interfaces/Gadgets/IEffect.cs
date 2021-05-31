
using PlayerSpace.Game;
using UnityEngine;
namespace Gadget.Effecter
{
    public interface IEffecter
    {

        Camera GetCamera();
        //void UseReduction(float during,float smallerScale,float stay,float limitScale);
        Model GetModel();
        bool SetEffect(int id);
        void DistoryEffect(int id);
        void UseGadget(int gid);
        GameObject GetPlayer();
        Sprite GetSprite(int gid);
    }

}