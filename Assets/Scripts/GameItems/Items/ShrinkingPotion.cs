using System;
using System.Collections;
using UnityEngine;

namespace PlayerSpace.Gameplayer
{
    public class ShrinkingPotion : GameItemControl
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
        }

        public bool ShrinkCondition()
        {
            if (model.characterSize > 0.5f)
                return true;
            return false;
        }
    }
}