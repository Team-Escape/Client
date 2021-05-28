using UnityEngine;
using Gadget.utility;
using UnityEngine.UI;
namespace Gadget.slot{
    public class Slot : MonoBehaviour {

        [SerializeField]Component gadgetView;
        [SerializeField]IGadget gadget;
        [SerializeField]Image icon;
        private void Update() {

            if(this.gadget != null) gadgetView = (Component)gadget;
        }
        public bool Store(IGadget gadget){
            
            if(this.gadget == null){
                this.gadget = gadget;
                icon.sprite = gadget.GetIcon();
                return true;
            }
            else{
                Debug.LogWarning("slot full");
                return false;
            }
        }
        public IGadget Tackout(){
            if(gadget==null) return null;
            IGadget output;
            output = gadget;
            gadget = null;
            icon.sprite = null;
            return output;
        }
        public bool IsEmpty(){
            return gadget == null;
        }
    }
}