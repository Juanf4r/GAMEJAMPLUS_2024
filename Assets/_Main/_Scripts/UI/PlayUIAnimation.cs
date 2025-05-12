using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.UI
{
    public class PlayUIAnimation : MonoBehaviour
    {
        private Animator animator;

        void Start()
        {
            animator.GetComponent<Animator>();
        }

        public void PlayAnimation()
        {
            animator.SetTrigger("Pressed");
        }
    }
}


