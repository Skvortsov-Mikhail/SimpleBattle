using System;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public event Action CharacterDied;

    protected float _maxHP;
    public float MaxHP => _maxHP;
    protected float _currentHP;
    public float CurrentHP => _currentHP;

    protected virtual void Awake()
    {
        Init();
    }

    public virtual void AddHP(float hp)
    {
        if (hp <= 0)
        {
            Debug.LogWarning("You are trying to add incorrect HP");
            return;
        }

        _currentHP = Mathf.Clamp(_currentHP + hp, 0, _maxHP);
    }

    protected virtual void CheckDeath()
    {
        if (_currentHP <= 0)
        {
            CharacterDied?.Invoke();
        }
    }

    protected abstract void Init();
}