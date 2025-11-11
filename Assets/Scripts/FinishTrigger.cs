using UnityEngine;

public enum FinishAction
{
    LoadSceneByIndex,
    LoadNextScene
}
[RequireComponent(typeof(SceneLoader))]
public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private FinishAction _finishAction;

    private int _playerCount;
    private int _playersInTrigger;
    private bool _cupInTrigger;

    private void Awake()
    {
        _playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playersInTrigger++;
            CheckCompletion();
        }
        else if (other.CompareTag("Cup"))
        {
            _cupInTrigger = true;
            CheckCompletion();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _playersInTrigger--;
        else if (other.CompareTag("Cup"))
            _cupInTrigger = false;
    }

    private void CheckCompletion() 
    {
        if (_playersInTrigger == _playerCount && _cupInTrigger)
            LevelExit();
    }

    private void EnumHandler()
    {
        switch (_finishAction)
        {
            case FinishAction.LoadSceneByIndex:
                _sceneLoader.LoadSceneByIndex();
                break;
            case FinishAction.LoadNextScene:
                _sceneLoader.LoadNextScene();
                break;
            default:
                Debug.LogWarning("Неизвестное действие для FinishTrigger!");
                break;
        }
    }

    private void LevelExit()
    {
        Debug.Log("Level completed!");

        if (_sceneLoader == null)
        {
            Debug.LogError("SceneLoader.Instance == null! Убедись, что объект SceneLoader есть в сцене.");
            return;
        }

        EnumHandler();
    }
}