using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BattleController : MonoBehaviour
{
    public event Action<bool> ShootingPossibility;
    public event Action<bool> PistolButtonActivated;
    public event Action<bool> GunButtonActivated;

    [SerializeField] private float m_ShootCooldown;

    [SerializeField] private Button m_ShootEnemyButton;
    [SerializeField] private Button m_ChoosePistolButton;
    [SerializeField] private Button m_ChooseGunButton;

    private Player _player;
    public Player Player => _player;

    private Enemy _enemy;
    public Enemy Enemy => _enemy;

    private Inventory _inventory;
    private LevelController _levelController;

    private Item _currentAmmo;
    private ItemType _currentItemType;
    private ShootType _currentShootType;

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

        _currentShootType = ShootType.Head;
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

        yield return new WaitForSeconds(m_ShootCooldown);

        HitPlayer();

        yield return new WaitForSeconds(m_ShootCooldown);

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
}