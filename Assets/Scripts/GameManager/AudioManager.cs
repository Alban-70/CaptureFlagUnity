using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClipsVolume;

    public AudioSource audioSource;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }

    public void PlayDebutJeu()
    {
        audioSource.PlayOneShot(audioClipsVolume[0]);
    }

    public void PlayMort()
    {
        audioSource.PlayOneShot(audioClipsVolume[1]);
    }
}
