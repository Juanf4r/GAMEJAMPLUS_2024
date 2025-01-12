using UnityEngine;

namespace _Scripts.Players
{
    public class PlayerSoundManager : MonoBehaviour
    {
        public AudioClip[] footstepClips; 
        
        public void PlayFootStep(){
            SoundFXChannel.PlaySoundFxClip(footstepClips,transform.position, .5f);
        }
    }
}
