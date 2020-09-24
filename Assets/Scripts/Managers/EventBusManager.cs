using GameEventBus;

namespace Managers
{
    public static class EventBusManager
    {
        private static EventBus bus = null;
        public static EventBus Bus
        {
            get { return bus ?? (bus = new EventBus()); }
        }
    }
}
