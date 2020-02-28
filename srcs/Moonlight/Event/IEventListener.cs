namespace Moonlight.Event
{
    public abstract class EventListener<T> : IEventListener where T : IEventNotification
    {
        public void Handle(object notification)
        {
            Handle((T)notification);
        }

        protected abstract void Handle(T notification);
    }

    public interface IEventListener
    {
        void Handle(object notification);
    }
}