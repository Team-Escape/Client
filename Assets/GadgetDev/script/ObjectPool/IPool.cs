using UnityEngine;
namespace ObjectPool{
    public interface IPool
    {
        
        void PutObject(GameObject obj);
    }
}