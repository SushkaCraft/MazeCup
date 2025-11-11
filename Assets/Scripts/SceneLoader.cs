using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int _sceneIndex;

    public void LoadSceneByIndex() => SceneManager.LoadScene(_sceneIndex);

    public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void AppQuit() => Application.Quit();
    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);
        else
            Debug.LogWarning("Next scene index is out of bounds.");
    }
}