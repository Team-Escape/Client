using UnityEngine;
using Gadget.effecter;
using ObjectPool;
namespace Gadget.utility{
    public abstract class PotionBase :GadgetBase{
        
        //[SerializeField]protected float speedOffset,flyDivide,maxSpeed;
        [SerializeField]protected GameObject potion;
        private void Awake() {
            base.type = 1;
        }
        public void Throw(Vector2 direction,Transform owner){
            
            GameObject potion = GadgetPool.GetObject(this.potion.GetComponent<IPoolObject>().GetID());
            //float speed =Mathf.Min(maxSpeed,ownerTransform.parent.GetComponent<Rigidbody2D>().velocity.magnitude/flyDivide+speedOffset);
            //potion.transform.position = ownerTransform.position;
            potion.GetComponent<PotionObj>().Setting(owner,CallWhenTrigger,direction);
            CallWhenThrow();
        }   
        
        protected abstract void CallWhenThrow();
        public abstract void CallWhenTrigger(IEffecter effecter);
            
        
        
        
    }
}