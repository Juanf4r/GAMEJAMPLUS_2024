using UnityEngine;

namespace _Scripts.Players
{
    public class PlayerSoundManager : MonoBehaviour
    {
        public AudioClip[] footstepClips;
        public AudioClip[] pickupClips;

        
        public void PlayFootStep(){
            SoundFXChannel.PlaySoundFxClip(footstepClips,transform.position, .5f);
        }

        public void PlayPickup()
        {
            SoundFXChannel.PlaySoundFxClip(pickupClips,transform.position, .5f);
        }
        
    }
}
