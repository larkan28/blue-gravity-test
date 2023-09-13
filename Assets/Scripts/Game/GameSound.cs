using UnityEngine;

public class GameSound : Singleton<GameSound>
{
    private AudioSource m_audioSource;

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
            return;

        if (m_audioSource == null)
            m_audioSource = gameObject.AddComponent<AudioSource>();

        m_audioSource.PlayOneShot(clip, volume);
    }
}
