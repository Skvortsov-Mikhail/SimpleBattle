using System;
using Zenject;

public class PlayerStats : Stats
{
    public event Action<float> HPUpdated;
    public event Action<int> HelmetArmorUpdated;
    public event Action<int> BodyArmorUpdated;

    private int _helmetArmor = 0;
    public int HelmetArmor => _helmetArmor;

    private int _bodyArmor = 0;
    public int BodyArmor => _bodyArmor;

    private Player _player;
    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }

    private void Start()
    {
        _player.PlayerHPUpdated += OnHPUdated;
        _player.HelmetArmorUpdated += OnHelmetArmorUpdated;
        _player.BodyArmorUpdated += OnBodyArmorUpdated;
    }

    private void OnDestroy()
    {
        _player.PlayerHPUpdated -= OnHPUdated;
        _player.HelmetArmorUpdated -= OnHelmetArmorUpdated;
        _player.BodyArmorUpdated -= OnBodyArmorUpdated;
    }

    private void OnHPUdated(float value)
    {
        _currentHP = value;

        HPUpdated?.Invoke(value);
    }

    private void OnHelmetArmorUpdated(int value)
    {
        _helmetArmor = value;

        HelmetArmorUpdated?.Invoke(value);
    }

    private void OnBodyArmorUpdated(int value)
    {
        _bodyArmor = value;

        BodyArmorUpdated?.Invoke(value);
    }
}