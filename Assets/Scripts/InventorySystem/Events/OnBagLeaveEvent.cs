using GameEventBus.Events;

namespace InventorySystem
{
    public class OnBagLeaveEvent : EventBase
    {
        private IBagThrowable item;
        public IBagThrowable Item => item;
        
        public OnBagLeaveEvent(IBagThrowable item) {
            this.item = item;
        }
    }
}