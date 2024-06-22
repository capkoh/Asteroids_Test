namespace Game.Cache
{
    public interface ICacheItem
    {
        bool IsAlive { get; }
        void Update();
    }
}
