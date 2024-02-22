

namespace ShopOnlineCore.Utils
{
    public class EventHandlerCore
    {
        public static event EventHandler<EventParams> ProcessCompleted;

        public void RealeaseEvent(string @event, object[] args)
        {
            ProcessCompleted?.Invoke(this, new EventParams() { @event = @event, args = args });
        }

    }

    public class EventParams : EventArgs
    {
        public string @event { get; set; }
        public object[] args { get; set; }
    }
}
