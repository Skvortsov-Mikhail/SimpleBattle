using UnityEngine;
using Zenject;

public class ItemUsageController : MonoBehaviour
{
    [SerializeField] private Slot m_PlayerHelmetSlot;
    [SerializeField] private Slot m_PlayerBodySlot;

    private Item _currentItem;
    private Inventory _inventory;

    private BattleController _battleController;
    [Inject]
    public void Construct(BattleController battleController, Inventory inventory)
    {
        _battleController = battleController;
        _inventory = inventory;
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

            case ItemType.HelmetArmor:
                HelmetArmorAction();
                break;

            case ItemType.BodyArmor:
                BodyArmorAction();
                break;

            case ItemType.AidKit:
                AidKitAction();
                break;
        }
    }
    private void AmmoAction()
    {
        _currentItem.ParentSlot.FillSlotToMaxAmount();
    }

    private void HelmetArmorAction()
    {
        _battleController.Player.ChangeHelmetArmor(_currentItem.Config.PropertyValue);

        if (m_PlayerHelmetSlot.Item != null)
        {
            var previousHelmet = m_PlayerHelmetSlot.Item;
            var newSlot = _inventory.GetFirstEmptySlot();

            if (newSlot.TryFillSlot(previousHelmet, 1) == true)
            {
                previousHelmet.transform.SetParent(newSlot.transform);
                previousHelmet.transform.localPosition = Vector3.zero;
                previousHelmet.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

                previousHelmet.GetComponent<ItemEventHandler>().enabled = true;
            }
        }

        if (m_PlayerHelmetSlot.TryFillSlot(_currentItem, 1) == true)
        {
            _currentItem.transform.SetParent(m_PlayerHelmetSlot.transform);
            _currentItem.transform.localPosition = Vector3.zero;
            _currentItem.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

            _currentItem.GetComponent<ItemEventHandler>().enabled = false;
        }
    }

    private void BodyArmorAction()
    {
        _battleController.Player.ChangeBodyArmor(_currentItem.Config.PropertyValue);

        if (m_PlayerBodySlot.Item != null)
        {
            var previousBody = m_PlayerBodySlot.Item;
            var newSlot = _inventory.GetFirstEmptySlot();

            if (newSlot.TryFillSlot(previousBody, 1) == true)
            {
                previousBody.transform.SetParent(newSlot.transform);
                previousBody.transform.localPosition = Vector3.zero;
                previousBody.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

                previousBody.GetComponent<ItemEventHandler>().enabled = true;
            }
        }

        if (m_PlayerBodySlot.TryFillSlot(_currentItem, 1) == true)
        {
            _currentItem.transform.SetParent(m_PlayerBodySlot.transform);
            _currentItem.transform.localPosition = Vector3.zero;
            _currentItem.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

            _currentItem.GetComponent<ItemEventHandler>().enabled = false;
        }
    }

    private void AidKitAction()
    {
        _battleController.Player.AddHP(_currentItem.Config.PropertyValue);

        _currentItem.ParentSlot.TrySpendItem(_currentItem.Config.RequiredAmountToUse);
    }
}