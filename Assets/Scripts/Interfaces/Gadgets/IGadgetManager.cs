using UnityEngine;
namespace Gadget.GadgetManager
{
    public interface IGadgetManager
    {
        Sprite GetSprite(int ID);
        void Use(int ID);
    }
}