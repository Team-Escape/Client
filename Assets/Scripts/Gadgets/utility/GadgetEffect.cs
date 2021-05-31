using UnityEngine;
using Gadget.Effecter;
using PlayerSpace.Game;

public abstract class GadgetEffect : MonoBehaviour
{
    protected Model model;
    protected Camera cam;
    protected GameObject owner;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected IEffecter effecter;
    protected void Awake()
    {
        effecter = GetComponent<IEffecter>();
        model = effecter.GetPlayer().GetComponent<Model>();
        owner = effecter.GetPlayer();
        cam = effecter.GetCamera();
    }
    protected void OnEnable()
    {
        CallWhenUse();
    }
    protected abstract void CallWhenUse();
    public Sprite GetSprite()
    {
        return sprite;
    }
}
