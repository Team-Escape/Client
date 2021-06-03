using UnityEngine;
using Gadget.Effector;
using ObjectPool;
namespace Gadget.Utility
{
    public class GadgetBase : MonoBehaviour, IGadget, IPoolObject
    {

        [SerializeField] protected Sprite icon;
        [SerializeField] protected int ID;
        [SerializeField] protected int pid;
        protected GameObject owner = null;
        protected int type = 0;
        public int GetObjType()
        {
            return type;
        }
        public void InitGadget(int id, bool isEffectOwner)
        {
            ID = id;
        }
        public void Use(GameObject owner)
        {
            //write the action when using this gadget
            this.owner = owner;
            owner.GetComponentInChildren<IEffector>().UseGadget(ID);
            //getEffect.UseGadget(ID);
            GadgetPool.PutObject(gameObject);
        }

        public Sprite GetIcon()
        {
            return icon;
        }

        // implement Ipoolobject
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
            return pid;
        }
    }
}