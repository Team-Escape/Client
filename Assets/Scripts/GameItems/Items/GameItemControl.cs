using System;
using UnityEngine;
namespace PlayerSpace.Gameplayer
{
    public class GameItemControl : MonoBehaviour, IGameItemControl
    {
        public Sprite sprite = null;
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
}