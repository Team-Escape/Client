using System.Collections.Generic;
using UnityEngine;
namespace PlayerSpace.Gameplayer
{
    [CreateAssetMenu (menuName="item/container")]
    public class GameItemContainer:ScriptableObject{
          
        public List<GameItemEntity> items;
        
    }
    [System.Serializable]
    public struct GameItemEntity{
        
        public string name;
        public GameItemControl_Script itemControl;
        public Sprite sprite;
    } 
}