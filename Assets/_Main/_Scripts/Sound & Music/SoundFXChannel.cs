using Settings;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(menuName = "Managers/Sound Manager", fileName = "Sound Manager")]
    public class SoundFXChannel : ScriptableObject
    {
        private static SoundFXChannel _instance;
        public static SoundFXChannel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<SoundFXChannel>($"Sound Manager");
                }

                return _instance;
            }
        }
        public AudioSource soundObject;

        private const float PitchChangeMultiplier = 0.2f;

        private float GlobalSFXVolume
        {
            get
            {
                var config = ConfigManager.LoadConfig();
                return config.settings.sfxvolume;
            }
        }

        public static void PlaySoundFxClip(AudioClip clip, Vector3 soundPosition, float volume, bool loop = false)
        {
            var randPitch = Random.Range(volume, volume + PitchChangeMultiplier);
            var finalVolume = randPitch * Instance.GlobalSFXVolume; // Apply global volume multiplier

            var audio = Instantiate(Instance.soundObject, soundPosition, Quaternion.identity);
            audio.clip = clip;
            audio.volume = finalVolume;
            if (loop) audio.loop = true;
            audio.Play();
        }
        
        public static void PlaySoundFxClip(AudioClip[] clips, Vector3 soundPosition, float volume, bool loop = false)
        {
            var randClip = Random.Range(0, clips.Length);
            var randPitch = Random.Range(volume, volume + PitchChangeMultiplier);
            var finalVolume = randPitch * Instance.GlobalSFXVolume; // Apply global volume multiplier

            var audio = Instantiate(Instance.soundObject, soundPosition, Quaternion.identity);
            audio.clip = clips[randClip];
            audio.volume = finalVolume;
            if (loop) audio.loop = true;
            audio.Play();
        }
    }
}