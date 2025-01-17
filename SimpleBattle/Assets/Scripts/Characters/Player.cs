using System;
using UnityEngine;

public class Player : Character, ISaveable
{
    public const string PlayerHPSavingDataName = "PlayerHP";

    public event Action<float> PlayerHPUpdated;
    public event Action<int> HelmetArmorUpdated;
    public event Action<int> BodyArmorUpdated;

    [SerializeField] private PlayerConfiguration _playerConfiguration;

    private int _helmetArmor = 0;
    public int HelmetArmor => _helmetArmor;

    private int _bodyArmor = 0;
    public int BodyArmor => _bodyArmor;

    protected override void Awake()
    {
        base.Awake();

        if (FileHandler.HasFile(PlayerHPSavingDataName) == true)
        {
            LoadData();
        }
    }

    public void ApplyDamage(float damage, ShootType type)
    {
        float modifiedDamage = damage;

        switch (type)
        {
            case ShootType.Head:
                modifiedDamage = Mathf.Clamp(damage - _helmetArmor, 0, float.MaxValue);
                break;

            case ShootType.Body:
                modifiedDamage = Mathf.Clamp(damage - _bodyArmor, 0, float.MaxValue);
                break;
        }

        _currentHP = Mathf.Clamp(_currentHP - modifiedDamage, 0, _maxHP);

        SaveData();

        PlayerHPUpdated?.Invoke(_currentHP);

        CheckDeath();
    }

    public override void AddHP(float hp)
    {
        base.AddHP(hp);

        SaveData();

        PlayerHPUpdated?.Invoke(_currentHP);
    }

    public void ChangeHelmetArmor(int newValue)
    {
        _helmetArmor = Mathf.Clamp(newValue, 0, int.MaxValue);

        HelmetArmorUpdated?.Invoke(_helmetArmor);
    }

    public void ChangeBodyArmor(int newValue)
    {
        _bodyArmor = Mathf.Clamp(newValue, 0, int.MaxValue);

        BodyArmorUpdated?.Invoke(_bodyArmor);
    }

    protected override void Init()
    {
        _maxHP = _playerConfiguration.MaxHP;
        _currentHP = _maxHP;

        _helmetArmor = _playerConfiguration.DefaultHelmetArmor;
        _bodyArmor = _playerConfiguration.DefaultBodyArmor;
    }

    #region Saving

    public void SaveData()
    {
        Saver<float>.Save(PlayerHPSavingDataName, _currentHP);
    }

    public void LoadData()
    {
        Saver<float>.TryLoad(PlayerHPSavingDataName, ref _currentHP);
    }

    public void ResetData()
    {
        FileHandler.Reset(PlayerHPSavingDataName);
    }

    #endregion
}