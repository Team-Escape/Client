using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class Slot : IitemHandler
    {

        private ItemData itemData ;
        private ItemSystem itemModel;
        // todo: change dependents on only ItemSystemManager not IGameItemControl
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
            //itemHash = itemModel.GetItem(id,playerModel);
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
    /*
    public class Slot_scriptVersion: ISlot
    {
        public IGameItemControl GameItemControl { get; set; }
        static ItemSystemManager itemModel;
        // todo: new item model with script and load (singleton)
        public Slot_scriptVersion(ItemSystemManager itemsys)
        {
            itemModel = itemsys;
        }

        public void SetGameItem(int id, Model playerModel)
        {
            GameItemControl = itemModel.GetItem(id);
            GameItemControl.Init(playerModel);
        }

        public void Use()
        {
            GameItemControl.Use();
            GameItemControl = null;
        }
        public void EffectBy(int itemID){
            
        }
        public Sprite GetCurrentSprite(){
            return GameItemControl.GetSprite();
        }
        public bool isEmpty(){
            return GameItemControl==null;
        }
    }*/
}