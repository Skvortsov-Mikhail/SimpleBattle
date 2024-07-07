using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject m_GameOverPanel;
    private Button _confirmButton;

    private Inventory _inventory;
    private Enemy _enemy;

    [Inject]
    public void Construct(Inventory inventory, Enemy enemy)
    {
        _inventory = inventory;
        _enemy = enemy;
    }

    private void Start()
    {
        _confirmButton = m_GameOverPanel.GetComponent<Button>();
        _confirmButton.onClick.AddListener(ReloadScene);

        m_GameOverPanel.SetActive(false);
    }
    private void OnDestroy()
    {
        _confirmButton.onClick.RemoveAllListeners();
    }

    public void EnemyDied()
    {
        _inventory.AddRandomItem();
        _enemy.AddHP(_enemy.Stats.MaxHP);
    }

    public void PlayerDied()
    {
        m_GameOverPanel.SetActive(true);
    }

    private void ReloadScene()
    {
        m_GameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}