using System.Linq;
using UnityEngine;

public class SavingController : MonoBehaviour
{
    [SerializeField] private bool m_SaveOnQuit;

    private ISaveable[] _saveables;

    private void Awake()
    {
        _saveables = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>().ToArray();
    }

    private void OnApplicationQuit()
    {
        if (m_SaveOnQuit == true)
        {
            SaveAllData();
        }
    }

    public void ResetAllData()
    {
        foreach (var saveable in _saveables)
        {
            if (saveable != null)
            {
                saveable.ResetData();
            }
        }
    }

    public void SaveAllData()
    {
        foreach (var saveable in _saveables)
        {
            if (saveable != null)
            {
                saveable.SaveData();
            }
        }
    }
}