using System;
using UnityEngine;

public class Enemy : Character
{
    public event Action<float> EnemyHPUpdated;

    [SerializeField] private EnemyConfiguration _enemyConfiguration;

    private float _damage;
    public float Damage => _damage;

    public void ApplyDamage(float damage)
    {
        _currentHP = Mathf.Clamp(_currentHP - damage, 0, _maxHP);

        EnemyHPUpdated?.Invoke(_currentHP);

        CheckDeath();
    }

    public override void AddHP(float hp)
    {
        base.AddHP(hp);

        EnemyHPUpdated?.Invoke(_currentHP);
    }

    protected override void Init()
    {
        _maxHP = _enemyConfiguration.MaxHP;
        _currentHP = _maxHP;

        _damage = _enemyConfiguration.Damage;
    }
}