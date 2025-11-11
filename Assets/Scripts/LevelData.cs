using UnityEngine;

public class LevelData : MonoBehaviour
{
    [SerializeField] private int _levelNumber;
    [SerializeField] private string _levelName;

    public int GetLevelNumber() => _levelNumber;
    public string LevelName() => _levelName;
}
