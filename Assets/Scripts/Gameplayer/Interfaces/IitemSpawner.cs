namespace PlayerSpace.Gameplayer
{
    public interface IitemSpawner
    {
        ItemData TackItem();
        bool IsEmpty();
    }
    [System.Serializable]
    public class ItemData{
        public ItemAction action;
        public ItemEffect effect;
        public bool isAffectOwner;
        public ItemData(ItemAction action,ItemEffect effect,bool isAffectOwner){
            this.action = action;
            this.effect = effect;
            this.isAffectOwner = isAffectOwner;
        }
        public ItemData(){
            action = ItemAction.NULL;
            effect = ItemEffect.NULL;
        }
    }
    public enum ItemAction{
        NULL = -1,
        EAT,
        THROW
    }
    public enum ItemEffect{
        NULL = -1,
        HEALTH,
        JUMP_LOWER,
        FASS_TO_ASS,
        REDUCTION
    }

}