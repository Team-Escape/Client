using UnityEngine;
using Gadget.Effecter;
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

        public void Use(GameObject owner)
        {
            //write the action when using this gadget
            this.owner = owner;

            //getEffect.UseGadget(ID);
            GadgetPool.PutObject(gameObject);
        }
        public void SetID(int id)
        {
            ID = id;
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