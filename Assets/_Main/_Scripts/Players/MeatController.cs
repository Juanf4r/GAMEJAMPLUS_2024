using UnityEngine;
using Core;

namespace _Scripts.Players
{
    public class MeatController : MonoBehaviour
    {
        public AudioClip[] eatClips;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (other.gameObject.GetComponent<PlayerManager>().isPlayerOne)
            {
                GameManager.Instance.CheckPlayer1Meat();
            }
            else
            {
                GameManager.Instance.CheckPlayer2Meat();
            }
            SoundFXChannel.PlaySoundFxClip(eatClips, transform.position, .6f);
            GameManager.Instance.LocateMeat();
        }
    }   
}
