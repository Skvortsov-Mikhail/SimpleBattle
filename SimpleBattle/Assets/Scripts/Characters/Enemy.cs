using System;
using UnityEngine;

public class Enemy : Character, ISaveable
{
    public const string EnemyHPSavingDataName = "EnemyHP";

    public event Action<float> EnemyHPUpdated;

    [SerializeField] private EnemyConfiguration _enemyConfiguration;

    private float _damage;
    public float Damage => _damage;

    protected override void Awake()
    {
        base.Awake();

        if (FileHandler.HasFile(EnemyHPSavingDataName) == true)
        {
            LoadData();
        }
    }

    public void ApplyDamage(float damage)
    {
        _currentHP = Mathf.Clamp(_currentHP - damage, 0, _maxHP);

        SaveData();

        EnemyHPUpdated?.Invoke(_currentHP);

        CheckDeath();
    }

    public override void AddHP(float hp)
    {
        base.AddHP(hp);

        SaveData();

        EnemyHPUpdated?.Invoke(_currentHP);
    }

    protected override void Init()
    {
        _maxHP = _enemyConfiguration.MaxHP;
        _currentHP = _maxHP;

        _damage = _enemyConfiguration.Damage;
    }

    #region Saving

    public void SaveData()
    {
        Saver<float>.Save(EnemyHPSavingDataName, _currentHP);
    }

    public void LoadData()
    {
        Saver<float>.TryLoad(EnemyHPSavingDataName, ref _currentHP);
    }

    public void ResetData()
    {
        FileHandler.Reset(EnemyHPSavingDataName);
    }
    #endregion
}