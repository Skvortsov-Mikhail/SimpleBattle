using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour
{
    [SerializeField] private PopupPanel m_PopupPanel;

    private Slot[] _slots;
    private Object[] _items;

    private DiContainer _diContainer;
    [Inject]
    public void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }

    private void Start()
    {
        _items = Resources.LoadAll("Prefabs/Items", typeof(Item));

        _slots = GetComponentsInChildren<Slot>();

        GetStartPack();
    }

    public void ShowPopupWindow(Item item)
    {
        m_PopupPanel.gameObject.SetActive(true);
        m_PopupPanel.ResreshPopup(item);
    }

    public void AddRandomItem()
    {
        int randomItem = Random.Range(0, _items.Length);

        var slot = GetFirstEmptySlot();

        var newItem = _diContainer.InstantiatePrefab(_items[randomItem], slot.transform);

        var item = newItem.GetComponent<Item>();

        item.SetParentSlot(slot);

        if (slot.TryFillSlot(item, 1) == false)
        {
            Debug.LogError("Cannot fill slot");
        }
    }

    public Slot GetFirstEmptySlot()
    {
        if (HasEmptySlot() == false)
        {
            Debug.LogError("No empty slots!");

            return null;
        }

        int index = 0;

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsEmpty == true)
            {
                index = i;
                break;
            }
        }

        return _slots[index];
    }

    public Item GetFirstAmmo(ItemType type)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].IsEmpty == true) continue;

            if (_slots[i].Item.Config.ItemType != type) continue;

            return _slots[i].Item;
        }

        Debug.LogWarning("No Ammo this type");

        return null;
    }

    private void GetStartPack()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            var slot = GetFirstEmptySlot();

            var newItem = _diContainer.InstantiatePrefab(_items[i], slot.transform);

            var item = newItem.GetComponent<Item>();

            item.SetParentSlot(slot);

            if (slot.TryFillSlot(item, item.Config.MaxAmountInSlot) == false)
            {
                Debug.LogError("Cannot fill slot");
            }
        }
    }

    private bool HasEmptySlot()
    {
        bool hasEmptySlot = false;

        foreach (var slot in _slots)
        {
            if (slot.IsEmpty == true)
            {
                hasEmptySlot = true;
                break;
            }
        }

        return hasEmptySlot;
    }
}