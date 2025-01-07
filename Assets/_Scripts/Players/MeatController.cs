using UnityEngine;

namespace _Scripts.Players
{
    public class MeatController : MonoBehaviour
    {
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
        }
    }
}
