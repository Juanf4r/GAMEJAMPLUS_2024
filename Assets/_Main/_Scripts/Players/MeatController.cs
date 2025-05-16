using UnityEngine;
using Core;

namespace _Scripts.Players
{
    public class MeatController : MonoBehaviour
    {
        #region Singleton
        public static MeatController Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion
        public AudioClip[] eatClips;
        [SerializeField] private RuntimeAnimatorController meatAnimatorController;
        public RuntimeAnimatorController meatGoldAnimatorController;
        [SerializeField] private Animator meatAnimator;

        void OnEnable()
        {
            SetAnimatorController(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (other.gameObject.GetComponent<PlayerManager>().isPlayerOne)
            {
                GameManager.Instance.CheckPlayer1Meat();
                other.gameObject.GetComponent<PlayerManager>().playerInput = Vector2.zero;
            }
            else
            {
                GameManager.Instance.CheckPlayer2Meat();
                other.gameObject.GetComponent<PlayerManager>().playerInput = Vector2.zero;
            }
            SoundFXChannel.PlaySoundFxClip(eatClips, transform.position, .6f);
            GameManager.Instance.LocateMeat();
        }
        public void SetAnimatorController(bool isGold)
        {
            if (isGold)
            {
                meatAnimator.runtimeAnimatorController = meatGoldAnimatorController;
                meatAnimator.Play(0);
            }
            else
            {
                meatAnimator.runtimeAnimatorController = meatAnimatorController;
                meatAnimator.Play(0);
            }
        }
    }
}
