using UnityEngine;
namespace PlayerSpace.Gameplayer
{
    public abstract class ItemBase:MonoBehaviour,ItemEffecActor
    {
        [SerializeField]Sprite sprite;
        [SerializeField]ItemEffect effect;
        [SerializeField]protected bool isenable;
        protected Model model;
        public ItemEffect GetId(){
            return effect;
        }
        public void Trigger(Model model){
            this.model = model;
            if(isenable) return;
            isenable = true;
            CallWhenUse();
        }
        public Sprite GetSprite(){
            return sprite;
        }
        protected abstract void CallWhenUse();
    }
}