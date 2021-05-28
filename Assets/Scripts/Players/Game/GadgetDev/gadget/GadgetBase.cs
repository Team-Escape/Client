using UnityEngine;
using Gadget.effecter;
using ObjectPool;
namespace Gadget.utility{
    public abstract class GadgetBase : MonoBehaviour,IGadget,IPoolObject {

        [SerializeField]protected Sprite icon;
        [SerializeField]protected int ID;
        protected int type=0;
        public int GetObjType(){
            return type;
        }
        public void Use(IEffecter getEffect){
            //write the action when using this gadget
            CallWhenUse(getEffect);
            GadgetPool.PutObject(gameObject);
        }
        protected abstract void CallWhenUse(IEffecter getEffect);
        public Sprite GetIcon(){
            return icon;
        }
        // implement Ipoolobject
        public void Recycle(){
          //  gameObject.SetActive(false);
        }
        public void Init(){
            gameObject.SetActive(true);
        }
        public int GetID(){
            return ID;
        }
        
    }
}