using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private int m_MaxHP = 100;
    public int MaxHP => m_MaxHP;

    protected float _currentHP = 100;
    public float CurrentHP => _currentHP;
}