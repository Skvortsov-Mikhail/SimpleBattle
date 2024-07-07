using System;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float m_Damage;
    public float Damage => m_Damage;

    public event Action<float> EnemyHPUpdated;

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
}