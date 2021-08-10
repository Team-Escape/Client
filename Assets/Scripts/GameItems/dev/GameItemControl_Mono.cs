using System;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    
    public class GameItemControl_Mono : MonoBehaviour,IGameItemControl
    {
        protected Sprite sprite = null;
        internal Model model = null;
        protected ItemModel itemModel;
        internal Action<float, Action> AbleToDo { get { return model.AbleToDo; } }
        internal Action<RoleExtension.ConditionFunc, Action, Action> AbleToDoCondition { get { return model.AbleToDoCondition; } }
        public void Init(Model model)
        {
            this.model = model;
            itemModel = ItemModel.instance;
        }
        public virtual void Use() { }
        public Sprite GetSprite()
        {
            return sprite; 
        }
        internal void Recycle(){
            itemModel.Recycle(this);
        }
    }
    
        
}