

namespace PlayerSpace.Gameplayer
{
    public class ShrinkingPotion : GameItemControl_Mono
    {
        public override void Use()
        {
            RoleExtension.ConditionFunc condition = ShrinkCondition;
            AbleToDoCondition(condition, Shirnk, () => AbleToDo(5f, ResetScale));
        }

        public void Shirnk()
        {
            model.characterSize *= 0.9f;
        }

        public void ResetScale()
        {
            model.characterSize = 1f;
            Recycle();
        }

        public bool ShrinkCondition()
        {
            if (model.characterSize > 0.5f)
                return true;
            return false;
        }
    }
}