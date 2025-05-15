using System;
using _ScriptableObjects.Scripts;
using UnityEngine;

namespace _Scripts.Players
{
    public class PlayUISound : MonoBehaviour
    {
        public AudioClip buttonSound;

        public void PlaySound()
        {
            SoundFXChannel.PlaySoundFxClip(buttonSound, transform.position, .5f, false);
        }
    }
}

