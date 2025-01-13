using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Players
{
    public class CameraObstructionChecker : MonoBehaviour
    {
        [SerializeField] private Transform player; 
        [SerializeField] private LayerMask obstacleLayer; 
        [SerializeField] private float transparentAlpha = 0.3f;
        [SerializeField] private float checkRadius = 0.2f; 

        private List<Renderer> obstructedObjects = new List<Renderer>(); 
        private List<Renderer> previousObstructedObjects = new List<Renderer>(); 

        private void LateUpdate()
        {
            CheckObstructions();
        }

        private void CheckObstructions()
        {
            foreach (var renderer in previousObstructedObjects)
            {
                if (renderer)
                    SetObjectAlpha(renderer, 1f); 
            }

            previousObstructedObjects.Clear();
            previousObstructedObjects.AddRange(obstructedObjects);

            obstructedObjects.Clear();

            var directionToPlayer = player.position - transform.position;
            var hits = Physics.SphereCastAll(transform.position, checkRadius, directionToPlayer.normalized, directionToPlayer.magnitude, obstacleLayer);

            foreach (var hit in hits)
            {
                Debug.Log("OBSTRUCTION CHECKER");

                var renderer = hit.collider.GetComponent<Renderer>();
                if (renderer)
                {
                    SetObjectAlpha(renderer, transparentAlpha);
                    obstructedObjects.Add(renderer);
                }
            }
        }

        private void SetObjectAlpha(Renderer renderer, float alpha)
        {
        }
    }
}
