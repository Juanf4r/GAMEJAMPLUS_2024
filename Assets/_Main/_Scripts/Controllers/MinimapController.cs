using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Minimap
{
    public class MinimapController : MonoBehaviour
    {
        public static MinimapController instance;
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


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }
        private void Start()
        {
            this.GetComponent<Image>().sprite = this.minimapImage;
            foreach (var element in elements)
            {
                if (element.TargetTransform != null && element.IconSprite != null)
                {
                    AddMinimapElement(element);
                }
            }
        }
        
        public void AddMinimapElement(MinimapElementData elementData)
        {
            if (elementData.TargetTransform == null || elementData.IconSprite == null) 
                return;

            var iconInstance = Instantiate(iconPrefab, minimapRect);
            var iconImage = iconInstance.GetComponent<Image>();
            iconImage.sprite = elementData.IconSprite;
            iconImage.preserveAspect = elementData.PreserveAspect;

            RectTransform iconRect = iconInstance.GetComponent<RectTransform>();
    
            Vector2 finalSize = elementData.BaseSize;
    
            if (elementData.ScaleWithMap)
            {
                float mapScale = Mathf.Min(mapWidth, mapDepth) * elementData.ScaleFactor;
                finalSize = new Vector2(mapScale, mapScale);
            }
    
            if (elementData.PreserveAspect && elementData.IconSprite != null)
            {
                float aspect = elementData.IconSprite.rect.height / elementData.IconSprite.rect.width;
                finalSize.y = finalSize.x * aspect;
            }
    
            iconRect.sizeDelta = finalSize;

            var minimapElement = iconInstance.AddComponent<MinimapElement>();
            minimapElement.Initialize(elementData.TargetTransform, minimapRect, mapWidth, mapDepth, mapOffset);
        }
        
        public void AddElementAtRuntime(MinimapElementData elementData)
        {
            elements.Add(elementData);
            AddMinimapElement(elementData);
        }
    }
    
    [System.Serializable]
    public class MinimapElementData
    {
        public Transform TargetTransform;
        public Sprite IconSprite;
    
        [Header("Icon Settings")]
        [Tooltip("Base size in pixels")]
        public Vector2 BaseSize = new Vector2(20, 20);
    
        [Tooltip("If true, size will scale with map dimensions")]
        public bool ScaleWithMap = false;
    
        [Tooltip("Size multiplier when scaling with map"), Range(0.01f, 0.2f)]
        public float ScaleFactor = 0.05f;
    
        [Tooltip("Maintain sprite aspect ratio")]
        public bool PreserveAspect = true;
        
        
        
    }

    public class MinimapElement : MonoBehaviour
    {
        private Transform targetTransform;
        private RectTransform minimapRect;
        private RectTransform rectTransform; 
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
            rectTransform = GetComponent<RectTransform>(); 

        }

        private void Update()
        {
            if (targetTransform == null) return;

            var worldPos = targetTransform.position;
            var normalizedX = (worldPos.x - mapOffset.x) / mapWidth;
            var normalizedZ = (worldPos.z - mapOffset.y) / mapDepth;

            var minimapX = normalizedX * minimapRect.sizeDelta.x;
            var minimapY = normalizedZ * minimapRect.sizeDelta.y;

            rectTransform.anchoredPosition = new Vector2(minimapX, minimapY);
        }
    }
}
