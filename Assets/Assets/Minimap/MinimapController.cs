using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minimap
{
    public class MinimapController : MonoBehaviour
    {
        [Header("Minimap Settings")]
        [SerializeField] private RectTransform minimapRect; 
        [SerializeField] private Sprite minimapImage;
        [SerializeField] private float mapWidth = 32.999903f; 
        [SerializeField] private float mapDepth = 32.989066f; 
        [SerializeField] private Vector2 mapOffset = new Vector2(-3.015903f, -2.916576f); 

        [Header("Icon Settings")]
        [SerializeField] private GameObject iconPrefab; 

        [Header("Minimap Elements")]
        [SerializeField] private List<MinimapElementData> elements = new List<MinimapElementData>(); 

        private void Start()
        {
            this.GetComponent<Image>().sprite = this.minimapImage;
            foreach (var element in elements)
            {
                if (element.TargetTransform != null && element.IconSprite != null)
                {
                    AddMinimapElement(element.IconSprite, element.TargetTransform);
                }
            }
        }
        
        public void AddMinimapElement(Sprite icon, Transform targetTransform)
        {
            var iconInstance = Instantiate(iconPrefab, minimapRect);
            var iconImage = iconInstance.GetComponent<Image>();
            if (iconImage != null)
            {
                iconImage.sprite = icon;
            }

            var minimapElement = iconInstance.AddComponent<MinimapElement>();
            minimapElement.Initialize(targetTransform, minimapRect, mapWidth, mapDepth, mapOffset);
        }
        
        public void AddElementAtRuntime(MinimapElementData elementData)
        {
            if (elementData.TargetTransform != null && elementData.IconSprite != null)
            {
                elements.Add(elementData);
                AddMinimapElement(elementData.IconSprite, elementData.TargetTransform);
            }
        }
    }
    
    [System.Serializable]
    public class MinimapElementData
    {
        public Transform TargetTransform; 
        public Sprite IconSprite; 
    }

    public class MinimapElement : MonoBehaviour
    {
        private Transform targetTransform; 
        private RectTransform minimapRect; 
        private float mapWidth; 
        private float mapDepth; 
        private Vector2 mapOffset; 
        
        public void Initialize(Transform target, RectTransform minimap, float width, float depth, Vector2 offset)
        {
            targetTransform = target;
            minimapRect = minimap;
            mapWidth = width;
            mapDepth = depth;
            mapOffset = offset;
        }

        private void Update()
        {
            if (targetTransform == null) return;

            // Convert world position to minimap position
            var worldPos = targetTransform.position;
            var normalizedX = (worldPos.x - mapOffset.x) / mapWidth;
            var normalizedZ = (worldPos.z - mapOffset.y) / mapDepth;

            var minimapX = normalizedX * minimapRect.sizeDelta.x;
            var minimapY = normalizedZ * minimapRect.sizeDelta.y;

            // Update the icon position on the minimap
            ((RectTransform)transform).anchoredPosition = new Vector2(minimapX, minimapY);
        }
    }
}
