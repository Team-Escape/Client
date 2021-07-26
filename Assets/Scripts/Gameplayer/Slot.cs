using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class Slot : ISlot
    {
        public GameItemControl GameItemControl { get; set; }
        ItemModel itemModel;

        public Slot()
        {
            itemModel = ItemModel.instance;
            GameItemControl = new GameItemControl();
        }

        public void SetGameItem(int id, Model playerModel)
        {
            GameItemControl = itemModel.GetItem(id);
            GameItemControl.Init(playerModel);
        }

        public void Use()
        {
            GameItemControl.Use();
        }
    }
}