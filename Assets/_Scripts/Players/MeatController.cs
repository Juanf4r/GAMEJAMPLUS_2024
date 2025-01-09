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
                GameManager.Instance.contadorJugador1++;
                if (GameManager.Instance.contadorJugador1 >= 3)
                {
                    GameManager.Instance.GanarRondaJugador1();
                }
            }
            else
            {
                GameManager.Instance.contadorJugador2++;
                if (GameManager.Instance.contadorJugador2 >= 3)
                {
                    GameManager.Instance.GanarRondaJugador2();
                }
            }
            GameManager.Instance.LocalizarCarne();
        }
    }
}
