using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Object = System.Object;
using UI;

namespace Core{
    public class BootStrapLoader : MonoBehaviour
    {
        [Header("Loading Screen")]
        [SerializeField] private GameObject loadingScreen;
        private Loader loader;

        [Header("Resources to load")]
        [SerializeField] private List<ResourceData> resourceList = new();
        private float totalProgress;

        private async void Start(){
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(true);
            } 
            else 
            {
                Debug.LogError("Missing loadingScreen object");
            }
            loader = loadingScreen?.GetComponent<Loader>();
            try
            {
                var totalResources = resourceList.Count;
                var progressBar = 1f / totalResources;
                foreach (var resource in resourceList)
                {
                    await LoadResourceAsync(resource, progressBar);
                }
            } 
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async Task LoadResourceAsync(ResourceData resource, float progressPerResource)
        {
            Debug.Log($"Loading resource {resource.nameIndentifier}");
            if (!resource.isScene)
            {
                var handle = resource.asset.LoadAssetAsync<Object>();
                while (!handle.IsDone)
                {
                    var currentProgress = totalProgress + handle.PercentComplete * progressPerResource;
                    UpdateProgressUI(currentProgress);
                    await Task.Yield();
                }
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log($"Loaded resource {resource.nameIndentifier}");
                    var instanceProperty = handle.Result.GetType().GetProperty("Instance");
                    if (instanceProperty != null && instanceProperty.CanWrite)
                    {
                        instanceProperty.SetValue(null, handle.Result);
                        if (resource.needsOnStart)
                        {
                            CallOnStartMethod(handle.Result);
                        }
                    }
                } 
                else Debug.LogError($"Failde to load resource {resource.nameIndentifier}");
            } 
            else 
            {
                var sceneHandle = Addressables.LoadSceneAsync(resource.asset, LoadSceneMode.Single);
                while (!sceneHandle.IsDone)
                {
                    var currentProgress = totalProgress = sceneHandle.PercentComplete * progressPerResource;
                    UpdateProgressUI(currentProgress);
                    await Task.Yield();
                }
                if (sceneHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log($"Loaded Scene: {resource.nameIndentifier}");
                    //GameManager.Instance.SetCurrentSceneHandle(sceneHandle);
                } 
                else
                {
                    Debug.LogError($"Failed to load Scene: {resource.nameIndentifier}");
                }  
            }
            totalProgress += progressPerResource;
            UpdateProgressUI(totalProgress);
        }

        private void UpdateProgressUI(float progress)
        {
            loader.UpdatePercentage(progress);
        }

        private void CallOnStartMethod(object loadedObject)
        {
            var onStartMethod = loadedObject.GetType().GetMethod("OnStart");
            if (onStartMethod != null)
            {
                Debug.Log($"Calling OnStart for {loadedObject.GetType().Name}");
                onStartMethod.Invoke(loadedObject, null);
            } 
            else
            {
                Debug.LogWarning($"OnStart method not found in {loadedObject.GetType().Name}");
            } 
        }
    }

    [Serializable]
    public class ResourceData{
        public string nameIndentifier;
        public AssetReference asset;
        public bool isScene;
        public bool needsOnStart;
    }
}
