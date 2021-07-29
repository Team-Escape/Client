using System;
using UnityEngine;
using System.Collections.Generic;
namespace PlayerSpace.Gameplayer
{
    [CreateAssetMenu (menuName="item/GameItemControl")]
    public class GameItemControl : ScriptableObject
    {
        protected Sprite sprite = null;
        internal Model model = null;
        internal Action<float, Action> AbleToDo { get { return model.AbleToDo; } }
        internal Action<RoleExtension.ConditionFunc, Action, Action> AbleToDoCondition { get { return model.AbleToDoCondition; } }
        public void Init(Model model)
        {
            this.model = model;
        }
        public virtual void Use() { }
        public Sprite GetSprite
        {
            get { return sprite; }
        }
    }
    public interface IGameItemControl
    {
        Sprite GetSprite { get; }
        void Init(Model model);
        void Use();
    }
    [CreateAssetMenu (menuName="item/container")]
    public class GameItemContainer:ScriptableObject{
        [System.Serializable]
        public struct GameItemEntity{
            int id;
            public string name;
            public GameItemControl itemControl;
            public Sprite sprite;
            
            
        }   
        public List<GameItemEntity> items;
        
    }
        
}