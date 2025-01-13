using UnityEngine;

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
                GameManager.Instance.GanarRondaJugador1();
            }
            else
            {
                GameManager.Instance.GanarRondaJugador2();
            }
            SoundFXChannel.PlaySoundFxClip(eatClips, transform.position, .5f);
            GameManager.Instance.LocalizarCarne();
        }
    }   
}
