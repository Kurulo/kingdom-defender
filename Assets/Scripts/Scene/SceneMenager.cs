using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMenager : MonoBehaviour {
    public void ReloadScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Debug.Log("Loose Game !!!");
    }
}
