using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;

        public void UpdatePercentage(float progress)
        {
            if (progressBar != null)
            {
                progressBar.value = progress;
            }
            else
            {
                Debug.LogError("Progress Bar is not Set");
            }
        }
    }
}