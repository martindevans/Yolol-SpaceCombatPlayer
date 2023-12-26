namespace Assets.Scripts.Events
{
    public interface IDebugDestroyNotificationReceiver
    {
        public int ID { get; }

        public void DestroyEvent(DebugDestroy destroy);
    }
}