using UnityEngine;
namespace PlayerSpace.Gameplayer
{
    public interface IItemView
    {
        string name{get;}
        IGameItemControl itemControl{get;}
        Sprite sprite{get;}
    }
}