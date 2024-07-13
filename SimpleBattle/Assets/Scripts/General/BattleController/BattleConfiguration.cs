using UnityEngine;

[CreateAssetMenu]
public class BattleConfiguration : ScriptableObject
{
    [SerializeField] public float ShootCooldown;
    [SerializeField] public ShootType StartShootType;
}