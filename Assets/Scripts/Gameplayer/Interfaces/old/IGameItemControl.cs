using UnityEngine;
namespace PlayerSpace.Gameplayer
{
    public interface IGameItemControl
    {
        Sprite GetSprite ();
        void Init(Model model);
        void Use();
    }
}