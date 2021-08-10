using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class Slot : IitemHandler
    {

        private int itemHash ;
        static ItemSystemManager itemModel;
        // todo: change dependents on only ItemSystemManager not IGameItemControl
        public Slot(ItemSystemManager itemSystem)
        {
            itemModel = itemSystem;
            itemHash = -1;
        }

        public void SetGameItem(int id, Model playerModel)
        {
            itemHash = itemModel.GetItem(id,playerModel);
        }

        public void Use()
        {
            itemModel.UseItem(itemHash);
            itemHash = -1;
        }
        public void EffectBy(int itemID){
            
        }
        public Sprite GetCurrentSprite(){
            return itemModel.GetItemSprite(itemHash);
        }
        public bool isEmpty(){
            return itemHash==-1;
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