using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Animations;
using UnityEngine;

namespace _Scripts.Players
{
    [CreateAssetMenu(menuName = "Player/Player Config", fileName = "DefaultConfig")]
    public class PlayerConfig : ScriptableObject
    {
        private Dictionary<string, float> _originalStats;

        [Header("Locomotion Settings")] 
        public float accelerationTime;

        [Header("Player Stats")] 
        public float speed;
        public float strength;
        public float invincibilityTime;

        [Header("Misc Settings")] 
        public RuntimeAnimatorController playerAnimator; 
        public LayerMask terrainLayer;
        


        public void OnStart()
        {
            _originalStats = new Dictionary<string, float>
            {
                { nameof(speed), speed },
                { nameof(strength), strength },
            };
        }

        public void ApplyBuff(string statToBuff, float newValue)
        {
            var fieldInfo = GetType().GetField(statToBuff, BindingFlags.Instance | BindingFlags.Public | BindingFlags
                .NonPublic);
            if (fieldInfo == null)
            {
                Debug.LogWarning($"Stat '{statToBuff}' does not exist in PlayerConfig.");
                return;
            }

            if (fieldInfo.FieldType != typeof(float))
            {
                Debug.LogWarning($"Stat '{statToBuff}' is not a float field.");
                return;
            }

            if (!_originalStats.ContainsKey(statToBuff))
            {
                _originalStats[statToBuff] = (float)fieldInfo.GetValue(this);
            }

            fieldInfo.SetValue(this, newValue);
        }

        public void RevertBuff()
        {
            foreach (var stat in _originalStats)
            {
                var fieldInfo = GetType().GetField(stat.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (fieldInfo == null || fieldInfo.FieldType != typeof(float)) continue;
                fieldInfo.SetValue(this, stat.Value);
            }
            _originalStats.Clear(); 
        }

    }
}
