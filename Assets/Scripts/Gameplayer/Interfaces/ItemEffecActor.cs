using UnityEngine;
namespace PlayerSpace.Gameplayer
{
    public interface ItemEffecActor
    {
        ItemEffect GetId();
        void Trigger(Model model);
        Sprite GetSprite();
    }
}