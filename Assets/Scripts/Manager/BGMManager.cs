using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing from this game object");
        }
        else
        {
            audioSource.Play(); // シーンの開始時にBGMを再生
        }
    }

    // BGMの再生を開始
    public void PlayBGM()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // BGMの再生を停止
    public void StopBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // BGMの一時停止
    public void PauseBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    // BGMの再開
    public void ResumeBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }
}
