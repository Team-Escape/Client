using UnityEngine;
using Gadget.utility;
using Gadget.slot;
using Gadget.effecter;
    [RequireComponent (typeof (Effecter))]
    public class InteractWithGadget : MonoBehaviour {
        
        [SerializeField]GameObject slotObj;
        Slot slot;
        Effecter effecter;
        private void Start() {
            slot = slotObj.GetComponent<Slot>();
            effecter = GetComponent<Effecter>();
        }
        private void Update() {
            //todo:call when input to use the gadget
            if(!slot.IsEmpty()){
                if(Input.GetKeyDown(KeyCode.P)){
                    UseGadget();
                }
                else if(Input.GetKeyDown(KeyCode.O)){
                    UseGadget();
                }
            }
        }
        public void UseGadget(){
            IGadget gadget = slot.Tackout();
            gadget.Use(effecter);
        }
        
        public bool PickUp(IGadget gadget){
            if(slot==null)return false;
            if(slot.Store(gadget)){
                return true;
            }
            return false;
        }
        
        
    }