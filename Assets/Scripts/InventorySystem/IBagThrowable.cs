using System;

namespace InventorySystem
{
    public interface IBagThrowable
    {
        void PutInBag();
        void RemoveFromBag();
        string GetID();
    }
}