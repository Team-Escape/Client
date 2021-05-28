using System.Collections.Generic;
using PlayerSpace.Game;
namespace Gadget.effecter
{
    public interface IEffecter
    {
        void UseHeal(int healMax, int healperTime, float delay);
        void UseInvisible(float during);
        void UseReduction(float during, float smallerScale, float stay, float limitScale);
        Model GetModel();

    }
}