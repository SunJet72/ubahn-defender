using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class SceneButton : MonoBehaviour
{
     [Tooltip("Name of the scene to load. Must be in Build Settings.")]
    public string sceneName;

    // This must be public, return void, and take no parameters
    public void LoadSceneByName()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("SceneButton: sceneName is empty!");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }
}
