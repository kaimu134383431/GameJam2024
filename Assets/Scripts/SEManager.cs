using UnityEngine;
using System.Collections.Generic;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f; // デフォルトの音量は1（最大音量）
    }

    public List<Sound> sounds;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySE(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            audioSource.volume = sound.volume;
            audioSource.PlayOneShot(sound.clip);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + name);
        }
    }
}
