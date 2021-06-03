using UnityEngine;
using Gadget.Effector;
using PlayerSpace.Game;
public abstract class GadgetEffect : MonoBehaviour
{
    protected Model model;
    protected Camera camera;
    protected GameObject owner;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected IEffector effecter;
    [SerializeField] protected int id;
    protected void Awake()
    {

        effecter = GetComponent<IEffector>();
        model = effecter.GetPlayer().GetComponent<Model>();
        owner = effecter.GetPlayer();
        camera = effecter.GetCamera();
        enabled = false;
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
    public int GetId()
    {
        return id;
    }
}
