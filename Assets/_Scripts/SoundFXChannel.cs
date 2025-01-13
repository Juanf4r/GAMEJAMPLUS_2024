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

        private const float VolumeChangeMultiplier = 0.1f;
        private const float PitchChangeMultiplier = 0.25f;

        public static void PlaySoundFxClip(AudioClip clip, Vector3 soundPosition, float volume, bool loop = false)
        {
            var randVolume = Random.Range(volume - VolumeChangeMultiplier,  volume + VolumeChangeMultiplier);
            var randPitch = Random.Range(volume - PitchChangeMultiplier,  volume + PitchChangeMultiplier);

            var audio = Instantiate(Instance.soundObject, soundPosition, Quaternion.identity);
            audio.clip = clip;
            audio.volume = randVolume;
            audio.volume = randPitch;
            if (loop) audio.loop = true;
            audio.Play();
        }
        
        public static void PlaySoundFxClip(AudioClip[] clips, Vector3 soundPosition, float volume, bool loop = false)
        {
            var randClip = Random.Range(0, clips.Length);
            var randVolume = Random.Range(volume - VolumeChangeMultiplier,  volume + VolumeChangeMultiplier);
            var randPitch = Random.Range(volume - PitchChangeMultiplier,  volume + PitchChangeMultiplier);

            var audio = Instantiate(Instance.soundObject, soundPosition, Quaternion.identity);
            audio.clip = clips[randClip];
            audio.volume = randVolume;
            audio.volume = randPitch;
            if (loop) audio.loop = true;
            audio.Play();
        }
    }
}