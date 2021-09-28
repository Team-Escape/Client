using UnityEngine;
namespace PlayerSpace.Gameplayer
{
    
    public interface ItemSystem
    {
        //return a hash of item
        //ItemEffect GetItem(int id,Model playerModel);
        //use a item with the hash
        void UseItem(ItemData itemdata);
        //use a item with the hash
        void EffectBy(ItemObj itemdata);
        //get sprite from item
        Sprite GetItemSprite(ItemData itemHash);
        void SetPlayerModel(Model playerModel);
    }
}