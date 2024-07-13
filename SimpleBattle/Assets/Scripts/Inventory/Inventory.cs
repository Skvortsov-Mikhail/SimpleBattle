using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Inventory : MonoBehaviour, ISaveable
{
    public const string InventorySavingDataName = "Inventory";

    [SerializeField] private PopupPanel m_PopupPanel;

    [SerializeField] private Slot m_PlayerHelmetSlot;
    public Slot PlayerHelmetSlot => m_PlayerHelmetSlot;
    [SerializeField] private Slot m_PlayerBodySlot;
    public Slot PlayerBodySlot => m_PlayerBodySlot;

    private Slot[] _slots;
    private Object[] _items;

    private SlotData[] _slotData;

    private DiContainer _diContainer;
    private Player _player;

    [Inject]
    public void Construct(DiContainer diContainer, Player player)
    {
        _diContainer = diContainer;
        _player = player;
    }

    private void Awake()
    {
        _items = Resources.LoadAll("Prefabs/Items", typeof(Item));

        _slots = GetComponentsInChildren<Slot>();

        if (FileHandler.HasFile(InventorySavingDataName) == true)
        {
            LoadData();

            return;
        }

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

        SaveData();
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

        SaveData();
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

    #region Saving

    [System.Serializable]
    private class SlotData
    {
        public Slot Slot;
        public ItemType ItemType;
        public int Amount;
    }

    public void SaveData()
    {
        UpdateSlotData();

        Saver<SlotData[]>.Save(InventorySavingDataName, _slotData);
    }

    public void LoadData()
    {
        Saver<SlotData[]>.TryLoad(InventorySavingDataName, ref _slotData);

        InitSlotData();
    }

    public void ResetData()
    {
        FileHandler.Reset(InventorySavingDataName);
    }

    private void UpdateSlotData()
    {
        _slotData = new SlotData[_slots.Length + 2];

        int helmetSlotIndex = _slots.Length;
        int bodySlotIndex = _slots.Length + 1;

        for (int i = 0; i < _slots.Length; i++)
        {
            _slotData[i] = new SlotData();

            _slotData[i].Slot = _slots[i];

            if (_slots[i].IsEmpty == false)
            {
                _slotData[i].ItemType = _slots[i].Item.Config.ItemType;
                _slotData[i].Amount = _slots[i].ItemAmount;
            }
        }

        _slotData[helmetSlotIndex] = new SlotData();
        _slotData[helmetSlotIndex].Slot = m_PlayerHelmetSlot;

        if (m_PlayerHelmetSlot.Item != null)
        {
            _slotData[helmetSlotIndex].ItemType = m_PlayerHelmetSlot.Item.Config.ItemType;
            _slotData[helmetSlotIndex].Amount = 1;
        }

        _slotData[bodySlotIndex] = new SlotData();
        _slotData[bodySlotIndex].Slot = m_PlayerBodySlot;

        if (m_PlayerBodySlot.Item != null)
        {
            _slotData[bodySlotIndex].ItemType = m_PlayerBodySlot.Item.Config.ItemType;
            _slotData[bodySlotIndex].Amount = 1;
        }
    }

    private void InitSlotData()
    {
        if (_slotData == null) return;

        for (int i = 0; i < _slotData.Length; i++)
        {
            if (_slotData[i].Amount != 0)
            {
                int index = -1;

                for (int j = 0; j < _items.Length; j++)
                {
                    if (_slotData[i].ItemType == _items[j].GetComponent<Item>().Config.ItemType)
                    {
                        index = j;
                        break;
                    }
                }

                var item = _diContainer.InstantiatePrefab(_items[index], _slotData[i].Slot.transform);
                var itemComponent = item.GetComponent<Item>();

                if (_slotData[i].Slot.TryFillSlot(itemComponent, _slotData[i].Amount) == true)
                {
                    itemComponent.transform.SetParent(_slotData[i].Slot.transform);
                    itemComponent.transform.localPosition = Vector3.zero;
                    itemComponent.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

                    itemComponent.GetComponent<ItemEventHandler>().enabled = i >= _slots.Length ? false : true;
                }
            }
        }

        CheckArmor();
    }

    private void CheckArmor()
    {
        if (m_PlayerHelmetSlot.Item != null)
        {
            _player.ChangeHelmetArmor(m_PlayerHelmetSlot.Item.Config.PropertyValue);
        }

        if (m_PlayerBodySlot.Item != null)
        {
            _player.ChangeBodyArmor(m_PlayerBodySlot.Item.Config.PropertyValue);
        }
    }

    #endregion
}