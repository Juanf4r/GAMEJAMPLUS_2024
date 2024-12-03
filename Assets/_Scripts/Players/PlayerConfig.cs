using UnityEngine;

namespace _Scripts.Players
{
    [CreateAssetMenu(menuName = "Player/Player Config", fileName = "DefaultConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Locomotion Settings")]
        public float speed;
        public float accelerationTime; 

        [Header("Misc Settings")]
        public float stunnedTime;
        public LayerMask terrainLayer;
    }
}
