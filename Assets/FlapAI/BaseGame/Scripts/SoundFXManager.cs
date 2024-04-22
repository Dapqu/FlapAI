using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance { get; private set;}

    [SerializeField] private System.Boolean isAI;
    [SerializeField] private AudioSource soundFXObject;
    private AudioSource currentAudioSource;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        if (isAI) {
            AudioListener.volume = 0f;
        }
        else {
            AudioListener.volume = 1f;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform transform, float volume) {
        // Spawn in gameObject
        currentAudioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);

        // Assign the audioClip
        currentAudioSource.clip = audioClip;

        // Assign volume
        currentAudioSource.volume = volume;

        // Play sound
        currentAudioSource.Play();

        // Get length of sound FX clip
        float clipLength  = currentAudioSource.clip.length;

        // Destroy gameObject
        Destroy(currentAudioSource.gameObject, clipLength);
    }


}
