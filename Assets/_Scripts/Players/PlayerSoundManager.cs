using System;
using _ScriptableObjects.Scripts;
using UnityEngine;

namespace _Scripts.Players
{
    public class PlayerSoundManager : MonoBehaviour
    {
        public AudioClip[] footstepClips;
        public AudioClip[] pickupClips;
        public AudioClip[] powerUpClips;
        public AudioClip[] attackClips;
        public AudioClip[] onDamagedClips;
        public void PlayFootStep(){
            SoundFXChannel.PlaySoundFxClip(footstepClips,transform.position, .5f);
        }

        public void PlayPickup()
        {
            SoundFXChannel.PlaySoundFxClip(pickupClips,transform.position, .5f);
        }

        public void PlayPowerUp(PowerUpSo powerUp)
        {
            var sound = powerUp.buffType switch
            {
                PowerUpType.Movement => powerUpClips[0],
                PowerUpType.Teleport => powerUpClips[1],
                PowerUpType.Strength => powerUpClips[2],
                _ => throw new ArgumentOutOfRangeException()
            };
            
            SoundFXChannel.PlaySoundFxClip(sound, transform.position,.5f);
        }

        public void PlayPunch()
        {
            SoundFXChannel.PlaySoundFxClip(attackClips,transform.position,.5f);
        }

        public void PlayOnHit(bool strong)
        {
            var audioClip = strong ? onDamagedClips[1] : onDamagedClips[0];
            SoundFXChannel.PlaySoundFxClip(audioClip,transform.position,.5f);
        }
    }
}
