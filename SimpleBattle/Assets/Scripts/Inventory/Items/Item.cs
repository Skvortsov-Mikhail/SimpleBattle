using System;
using UnityEngine;
using Zenject;

public class Item : MonoBehaviour
{
    public event Action ParentSlotChanged;

    [SerializeField] private ItemConfiguration m_Config;
    public ItemConfiguration Config => m_Config;

    private Slot _parentSlot;
    public Slot ParentSlot => _parentSlot;

    private Inventory _inventory;
    [Inject]
    public void Construct(Inventory inventory)
    {
        _inventory = inventory;
    }

    public void ShowPopupWindow()
    {
        _inventory.ShowPopupWindow(this);
    }

    public void SetParentSlot(Slot slot)
    {
        _parentSlot = slot;

        ParentSlotChanged?.Invoke();
    }

    public void RemoveFromInventory()
    {
        _parentSlot.UnfillSlot();

        Destroy(gameObject);
    }
}