using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixerGroup group;
    
    private PauseMenu pauseMenu;
    
    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = group;
        }
    }

    private void Start() {
        Play("Theme");
    }

    private void Update() {
        // Check if paused
        // if (Input.GetMouseButtonDown(0) && Cursor.visible) {
        //     Play("Click");
        // }
    }

    public void Play(String name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound: " + s + " not found");
            return;
        }
        s.source.Play();
    }
}
