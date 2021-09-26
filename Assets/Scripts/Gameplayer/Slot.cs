using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class Slot : IitemHandler
    {

        private ItemData itemData ;
        private ItemSystem itemModel;
        
        public Slot(ItemSystem itemSystem)
        {
            itemModel = itemSystem;
            itemData = null;
        }
        public void EffectBy(ItemData itemData){
            itemModel.EffectBy(itemData);
        }
        public void SetGameItem(ItemData itemData)
        {
            this.itemData = itemData;
            
        }

        public void Use()
        {
            itemModel.UseItem(itemData);
            itemData = null;
        }
        
        public Sprite GetCurrentSprite(){
            return itemModel.GetItemSprite(itemData);
        }
        public bool isEmpty(){
            return itemData==null;
        }
    }
    
}