using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelCounter;
    [SerializeField] private TMP_Text _gameTime;

    private int _levelNumber;
    private float _timer;

    private LevelData _levelData;
    private void Start()
    {
        _levelData = GameObject.FindFirstObjectByType<LevelData>();

        _levelNumber = _levelData.GetLevelNumber();

        LevelNumberDisplay();
    }

    private void LevelNumberDisplay()
    {
        _levelCounter.text = $"Уровень: {_levelNumber}";

    }
}
