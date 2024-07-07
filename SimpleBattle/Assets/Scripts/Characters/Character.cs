using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public event Action CharacterDied;

    [SerializeField] protected Stats _stats;
    public Stats Stats => _stats;

    protected int _maxHP;
    protected float _currentHP;

    protected virtual void Start()
    {
        Init();
    }

    public virtual void AddHP(float hp)
    {
        if (hp <= 0)
        {
            Debug.LogWarning("You try to add incorrect HP");
            return;
        }

        _currentHP = Mathf.Clamp(_currentHP + hp, 0, _maxHP);
    }

    protected void CheckDeath()
    {
        if (_currentHP <= 0)
        {
            CharacterDied?.Invoke();
        }
    }

    protected virtual void Init()
    {
        _maxHP = _stats.MaxHP;
        _currentHP = _maxHP;
    }
}