using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject menu;
    private KeyCode menuButton = KeyCode.Escape;

    void Start() {
    }

    void Update() {
        if (Input.GetKeyDown(menuButton))
            Activate(!menu.activeSelf);
    }

    public void Activate(bool active) {
        menu.SetActive(active);
        Time.timeScale = active? 0f : 1f;
    }

    public void Menu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit() {
        Application.Quit();
    }
}
