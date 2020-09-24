using GameEventBus.Events;

namespace InventorySystem
{
    public class OnBagEnterEvent : EventBase
    {
        private IBagThrowable item;
        public IBagThrowable Item => item;
        
        public OnBagEnterEvent(IBagThrowable item) {
            this.item = item;
        }
    }
}