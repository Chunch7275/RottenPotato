using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Name of the scene to load (must match exactly)")]
    public string sceneName;

    // Call this from the button's OnClick() event
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name not set on SceneLoader.");
        }
    }
}
