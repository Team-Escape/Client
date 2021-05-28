namespace ObjectPool{
    public interface IPoolObject
    {
        void Recycle();
        void Init();
        int GetID();

    }
}