using UnityEngine;
using Gadget.Effector;
using ObjectPool;
namespace Gadget.Utility
{
    public class PotionBase : MonoBehaviour, IGadget, IPoolObject
    {

        //[SerializeField]protected float speedOffset,flyDivide,maxSpeed;
        [SerializeField] protected GameObject potion;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected int PID;
        [SerializeField] protected int ID;
        protected int type = 1;
        protected bool isEffectOwner = false;
        public int GetObjType()
        {
            return type;
        }
        public void Use(GameObject owner)
        {
            //write the action when using this gadget

            Throw(new Vector2(1, 0), owner.transform);
            GadgetPool.PutObject(gameObject);
        }
        public void Throw(Vector2 direction, Transform owner)
        {

            GameObject potion = GadgetPool.GetObject(this.potion.GetComponent<IPoolObject>().GetPID());
            //float speed =Mathf.Min(maxSpeed,ownerTransform.parent.GetComponent<Rigidbody2D>().velocity.magnitude/flyDivide+speedOffset);
            potion.transform.position = owner.position;
            //potion.GetComponent<PotionObj>().Setting(owner, ID, isEffectOwner);
            //CallWhenThrow();
        }

        public void InitGadget(int id, bool isEffectOwner)
        {
            ID = id;
            this.isEffectOwner = isEffectOwner;
        }


        public Sprite GetIcon()
        {
            return icon;
        }
        public void Recycle()
        {
            gameObject.SetActive(false);
        }
        public void Init()
        {
            gameObject.SetActive(true);
        }
        public int GetID()
        {
            return ID;
        }

        public int GetPID()
        {
            return PID;
        }
    }
}