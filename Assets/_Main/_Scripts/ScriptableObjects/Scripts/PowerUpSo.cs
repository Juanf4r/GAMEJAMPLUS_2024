using UnityEditor.Animations;
using UnityEngine;

namespace _ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "Power Ups/New Power Up", fileName = "PowerUp")]
    public class PowerUpSo : ScriptableObject
    {
        public PowerUpType buffType;
        public Sprite buffSprite;
        public Sprite alphaTexture;
        public Animator powerUpAnimator;
        
        [Header("Buff Stats")]
        public float speed; 
        public float duration; 
        public float strength; 
    }

    public enum PowerUpType
    {
        Teleport, 
        Movement, 
        Strength,
    }
}
