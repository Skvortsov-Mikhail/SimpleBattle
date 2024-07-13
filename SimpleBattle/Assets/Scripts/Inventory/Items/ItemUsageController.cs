using UnityEngine;
using Zenject;

public class ItemUsageController : MonoBehaviour
{
    private Slot _playerHelmetSlot;
    private Slot _playerBodySlot;

    private Item _currentItem;
    private Inventory _inventory;

    private Player _player;
    [Inject]
    public void Construct(Player player, Inventory inventory)
    {
        _player = player;
        _inventory = inventory;
    }

    private void Start()
    {
        _playerHelmetSlot = _inventory.PlayerHelmetSlot;
        _playerBodySlot = _inventory.PlayerBodySlot;
    }

    public void UseItem(Item item)
    {
        _currentItem = item;

        switch (_currentItem.Config.ItemType)
        {
            case ItemType.PistolAmmo:
            case ItemType.GunAmmo:
                AmmoAction();
                break;

            case ItemType.LightHelmetArmor:
            case ItemType.HeavyHelmetArmor:
                HelmetArmorAction();
                break;

            case ItemType.LightBodyArmor:
            case ItemType.HeavyBodyArmor:
                BodyArmorAction();
                break;

            case ItemType.AidKit:
                AidKitAction();
                break;
        }

        _inventory.SaveData();
    }

    private void AmmoAction()
    {
        _currentItem.ParentSlot.FillSlotToMaxAmount();
    }

    private void HelmetArmorAction()
    {
        _player.ChangeHelmetArmor(_currentItem.Config.PropertyValue);

        ChangeArmor(_playerHelmetSlot);
    }

    private void BodyArmorAction()
    {
        _player.ChangeBodyArmor(_currentItem.Config.PropertyValue);

        ChangeArmor(_playerBodySlot);
    }

    private void AidKitAction()
    {
        _player.AddHP(_currentItem.Config.PropertyValue);

        _currentItem.ParentSlot.TrySpendItem(_currentItem.Config.RequiredAmountToUse);
    }

    private void ChangeArmor(Slot slot)
    {
        if (slot.Item != null)
        {
            var previousItem = slot.Item;
            var newSlot = _inventory.GetFirstEmptySlot();

            if (newSlot.TryFillSlot(previousItem, 1) == true)
            {
                previousItem.transform.SetParent(newSlot.transform);
                previousItem.transform.localPosition = Vector3.zero;
                previousItem.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

                previousItem.GetComponent<ItemEventHandler>().enabled = true;
            }
        }

        if (slot.TryFillSlot(_currentItem, 1) == true)
        {
            _currentItem.transform.SetParent(slot.transform);
            _currentItem.transform.localPosition = Vector3.zero;
            _currentItem.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

            _currentItem.GetComponent<ItemEventHandler>().enabled = false;
        }
    }
}