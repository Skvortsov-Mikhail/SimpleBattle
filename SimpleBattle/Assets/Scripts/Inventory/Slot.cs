using UnityEngine;

public class Slot : MonoBehaviour
{
    private bool _isEmpty = true;
    public bool IsEmpty => _isEmpty;

    private Item _item;
    public Item Item => _item;

    private int _itemAmount = 0;
    public int ItemAmount => _itemAmount;

    public bool TryFillSlot(Item item, int amount)
    {
        if (_itemAmount + amount > item.Config.MaxAmountInSlot) return false;

        if (_item != null && _item.Config.ItemType != item.Config.ItemType) return false;

        if (item.ParentSlot != null)
        {
            item.ParentSlot.UnfillSlot();
        }

        if (_item != null && _item.Config.ItemType == item.Config.ItemType)
        {
            Destroy(_item.gameObject);
        }

        _item = item;
        _itemAmount += amount;

        item.SetParentSlot(this);

        _isEmpty = false;

        return true;
    }

    public void UnfillSlot()
    {
        _item = null;
        _itemAmount = 0;

        _isEmpty = true;
    }

    public void FillSlotToMaxAmount()
    {
        if (_item == null) return;

        _itemAmount = _item.Config.MaxAmountInSlot;

        _item.SetParentSlot(this);  // TODO crutch to resreshUI
    }

    public bool TrySpendItem(int amount)
    {
        if (_item == null || _itemAmount < amount) return false;

        _itemAmount -= amount;

        _item.SetParentSlot(this);  // TODO crutch to resreshUI

        if (_itemAmount <= 0)
        {
            _item.RemoveFromInventory();
        }

        return true;
    }
}