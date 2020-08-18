using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelector : MonoBehaviour {

    public void Select(string name) {
        SceneManager.LoadScene(name);
    }

    public void Quit() {
        Application.Quit();
    }
}
