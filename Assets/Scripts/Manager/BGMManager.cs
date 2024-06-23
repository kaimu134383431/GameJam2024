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
            audioSource.Play(); // �V�[���̊J�n����BGM���Đ�
        }
    }

    // BGM�̍Đ����J�n
    public void PlayBGM()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // BGM�̍Đ����~
    public void StopBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // BGM�̈ꎞ��~
    public void PauseBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    // BGM�̍ĊJ
    public void ResumeBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }
}
