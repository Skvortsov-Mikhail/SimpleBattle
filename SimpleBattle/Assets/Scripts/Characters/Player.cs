using System;
using UnityEngine;

public class Player : Character
{
    public event Action<float> PlayerHPUpdated;
    public event Action<int> HelmetArmorUpdated;
    public event Action<int> BodyArmorUpdated;

    private int _helmetArmor = 0;
    private int _bodyArmor = 0;

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

        PlayerHPUpdated?.Invoke(_currentHP);

        CheckDeath();
    }

    public override void AddHP(float hp)
    {
        base.AddHP(hp);

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
}