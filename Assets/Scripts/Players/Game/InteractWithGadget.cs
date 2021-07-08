using UnityEngine;
using Gadget.Utility;
using Gadget.slot;
using Gadget.Effector;

public class InteractWithGadget : MonoBehaviour
{
    [SerializeField] GameObject slotObj;

    IEffector Effecter;
    Slot slot;

    private void Start()
    {
        slot = slotObj.GetComponent<Slot>();
        Effecter = GetComponent<IEffector>();
    }
    private void Update()
    {
        //todo:call when input to use the gadget
        if (!slot.IsEmpty())
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                UseGadget();
            }

        }
    }
    public void UseGadget()
    {
        IGadget gadget = slot.Tackout();
        if(gadget!=null)
            gadget.Use(Effecter.GetPlayer());

    }

    public bool PickUp(IGadget gadget)
    {

        if (slot == null)
        {
            return true;
        }

        if (slot.Store(gadget))
        {
            slot.SetSprite(Effecter.GetSprite(gadget.GetID()));
            return true;
        }
        return false;
    }


}