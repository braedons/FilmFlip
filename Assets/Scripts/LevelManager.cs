using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    private struct PointInTime {
        public Vector3 pos;
        public Vector3 localScale;

        public PointInTime(Vector3 pos, Vector3 localScale) {
            this.pos = pos;
            this.localScale = localScale;
        }
    }

    public GameObject[] reversableObjects;
    private Stack<PointInTime>[] pointsInTime;
    private bool skipRewindPoint = false;

    private bool isRewinding = false;

    public Image rewindSymbol;
    public GameObject completedSymbol;

    // Controls
    private KeyCode resetButton = KeyCode.R;

    void Start() {
        pointsInTime = new Stack<PointInTime>[reversableObjects.Length];
        for (int i = 0; i < pointsInTime.Length; i++) {
            pointsInTime[i] = new Stack<PointInTime>();
        }

        Time.timeScale = 1f;
    }

    private void Update() {
        if (Input.GetKeyDown(resetButton))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void FixedUpdate() {
        if (isRewinding) {
            if (pointsInTime[0].Count > 0) {
                for (int i = 0; i < pointsInTime.Length; i++) {
                    PointInTime point = pointsInTime[i].Pop();
                    reversableObjects[i].GetComponent<Rigidbody2D>().MovePosition(point.pos);
                    // reversableObjects[i].transform.localScale = point.localScale;                
                }
            }
            else {
                // Time.timeScale = 0f;
                // Rewind(false);
            }
        }
        else {
            if (!skipRewindPoint) {
                for (int i = 0; i < pointsInTime.Length; i++) {
                    pointsInTime[i].Push(new PointInTime(reversableObjects[i].transform.position,
                            reversableObjects[i].transform.localScale));
                }
            }

            skipRewindPoint = !skipRewindPoint;
        }
    }

    private void Rewind(bool rewind) {
        rewindSymbol.enabled = rewind;
        for (int i = 0; i < pointsInTime.Length; i++) {
            reversableObjects[i].GetComponent<Rigidbody2D>().isKinematic = rewind;  

            try {
                reversableObjects[i].GetComponent<MovingPlatform>().shouldMovePlayer = rewind;
            }
            catch {}
        }
        isRewinding = rewind;
    }

    public void ReachedGoal(string goalTag) {
        if (goalTag == "End") {
            GameObject.FindGameObjectWithTag("Start").GetComponent<Animator>().SetTrigger("Open");
            Rewind(true);
        }
        else {
            Time.timeScale = 0f;
            rewindSymbol.enabled = false;
            completedSymbol.SetActive(true);

        }
    }

    public bool IsRewinding() {
        return isRewinding;
    }

    public void NextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
