using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BattleController : MonoBehaviour, ISaveable
{
    public const string BattleControllerCurrentShootTypeSavingDataName = "BattleControllerCurrentShootTypeData";
    public const string BattleControllerCurrentItemTypeSavingDataName = "BattleControllerCurrentItemTypeData";

    public event Action<bool> ShootingPossibility;
    public event Action<bool> PistolButtonActivated;
    public event Action<bool> GunButtonActivated;

    [SerializeField] private BattleConfiguration m_BattleConfiguration;

    [Header("Buttons")]
    [SerializeField] private Button m_ShootEnemyButton;
    [SerializeField] private Button m_ChoosePistolButton;
    [SerializeField] private Button m_ChooseGunButton;

    private Player _player;
    private Enemy _enemy;
    private Inventory _inventory;
    private LevelController _levelController;

    private Item _currentAmmo;
    private ItemType _currentItemType;
    private ShootType _currentShootType;
    private float _shootCooldown;

    [Inject]
    public void Construct(Player player, Enemy enemy, Inventory inventory, LevelController levelController)
    {
        _player = player;
        _enemy = enemy;
        _inventory = inventory;
        _levelController = levelController;
    }

    private void Start()
    {
        _player.CharacterDied += OnPlayerDied;
        _enemy.CharacterDied += OnEnemyDied;

        m_ShootEnemyButton.onClick.AddListener(HitEnemy);
        m_ChoosePistolButton.onClick.AddListener(ChoosePistol);
        m_ChooseGunButton.onClick.AddListener(ChooseGun);

        ShootingPossibility?.Invoke(false);

        _shootCooldown = m_BattleConfiguration.ShootCooldown;
        _currentShootType = m_BattleConfiguration.StartShootType;

        if (FileHandler.HasFile(BattleControllerCurrentShootTypeSavingDataName) == true ||
            FileHandler.HasFile(BattleControllerCurrentItemTypeSavingDataName) == true)
        {
            LoadData();
        }
    }

    private void OnDestroy()
    {
        m_ShootEnemyButton.onClick.RemoveAllListeners();
        m_ChoosePistolButton.onClick.RemoveAllListeners();
        m_ChooseGunButton.onClick.RemoveAllListeners();
    }

    private void HitPlayer()
    {
        _player.ApplyDamage(_enemy.Damage, _currentShootType);

        _currentShootType = _currentShootType == ShootType.Head ? ShootType.Body : ShootType.Head;

        SaveData();
    }

    private void HitEnemy()
    {
        SwitchAmmo(_currentItemType);

        if (_currentAmmo.ParentSlot.TrySpendItem(_currentAmmo.Config.RequiredAmountToUse) == false)
        {
            Debug.LogWarning("Not enough ammo!");

            return;
        }

        _enemy.ApplyDamage(_currentAmmo.Config.PropertyValue);

        StartCoroutine(WaitingEnemyTurnCoroutine(_currentItemType));
    }

    private void OnPlayerDied()
    {
        _levelController.PlayerDied();
    }

    private void OnEnemyDied()
    {
        _levelController.EnemyDied();
    }

    private void ChoosePistol()
    {
        PistolButtonActivated?.Invoke(false);
        GunButtonActivated?.Invoke(true);

        SwitchAmmo(ItemType.PistolAmmo);

        ShootingPossibility?.Invoke(true);
    }

    private void ChooseGun()
    {
        PistolButtonActivated?.Invoke(true);
        GunButtonActivated?.Invoke(false);

        SwitchAmmo(ItemType.GunAmmo);

        ShootingPossibility?.Invoke(true);
    }

    private void SwitchAmmo(ItemType type)
    {
        _currentItemType = type;
        _currentAmmo = _inventory.GetFirstAmmo(type);

        SaveData();
    }

    private IEnumerator WaitingEnemyTurnCoroutine(ItemType type)
    {
        ShootingPossibility?.Invoke(false);

        switch (type)
        {
            case ItemType.PistolAmmo:
                GunButtonActivated?.Invoke(false);
                break;

            case ItemType.GunAmmo:
                PistolButtonActivated?.Invoke(false);
                break;
        }

        yield return new WaitForSeconds(_shootCooldown);

        HitPlayer();

        _inventory.SaveData();

        yield return new WaitForSeconds(_shootCooldown);

        ShootingPossibility?.Invoke(true);

        switch (type)
        {
            case ItemType.PistolAmmo:
                GunButtonActivated?.Invoke(true);
                break;

            case ItemType.GunAmmo:
                PistolButtonActivated?.Invoke(true);
                break;
        }

        StopCoroutine(WaitingEnemyTurnCoroutine(type));
    }

    #region Saving

    public void SaveData()
    {
        Saver<ShootType>.Save(BattleControllerCurrentShootTypeSavingDataName, _currentShootType);
        Saver<ItemType>.Save(BattleControllerCurrentItemTypeSavingDataName, _currentItemType);
    }

    public void LoadData()
    {
        Saver<ShootType>.TryLoad(BattleControllerCurrentShootTypeSavingDataName, ref _currentShootType);
        Saver<ItemType>.TryLoad(BattleControllerCurrentItemTypeSavingDataName, ref _currentItemType);

        SetLoadedAmmo();
    }

    public void ResetData()
    {
        FileHandler.Reset(BattleControllerCurrentShootTypeSavingDataName);
        FileHandler.Reset(BattleControllerCurrentItemTypeSavingDataName);
    }

    private void SetLoadedAmmo()
    {
        switch (_currentItemType)
        {
            case ItemType.PistolAmmo:
                ChoosePistol();
                m_ChoosePistolButton.interactable = false;
                break;
            case ItemType.GunAmmo:
                ChooseGun();
                m_ChooseGunButton.interactable = false;
                break;
            default:
                break;
        }
    }

    #endregion
}