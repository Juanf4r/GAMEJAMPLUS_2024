using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Transparency : MonoBehaviour
{
    [SerializeField] private Transform pointRay;
    [SerializeField] private Transform transMainCam;
    [SerializeField] private Material materialTransparent;
    private Dictionary<GameObject, List<Material>> originalMaterials = new Dictionary<GameObject, List<Material>>();
    public  List<GameObject> currentlyTransparent = new List<GameObject>();

    public void Update()
    {
        HandleTransparency();
    }

    private void HandleTransparency()
    {

        Vector3 direction = pointRay.position - transMainCam.position;
        float distance = direction.magnitude;
        RaycastHit[] hits = Physics.RaycastAll(transMainCam.position, direction, distance);
        HashSet<GameObject> hitObjects = new HashSet<GameObject>();

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Transparent"))
            {
                GameObject obj = hit.collider.gameObject;
                hitObjects.Add(obj);
                MakeTransparent(obj);
            }
        }
        for (int i = currentlyTransparent.Count - 1; i >= 0; i--)
        {
            GameObject obj = currentlyTransparent[i];
            if (!hitObjects.Contains(obj))
            {
                RestoreOriginalMaterials(obj);
                currentlyTransparent.RemoveAt(i);
            }
        }
    }

    private void MakeTransparent(GameObject obj)
    {
        if (!originalMaterials.ContainsKey(obj))
        {
            SaveOriginalMaterials(obj);
            SetTransparentMaterials(obj);
        }
        if (!currentlyTransparent.Contains(obj))
        {
            currentlyTransparent.Add(obj);
        }
    }


    private void SaveOriginalMaterials(GameObject obj)
    {
        List<Material> materials = new List<Material>();
        MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();

        //Debug.Log($"Saving materials for: {obj.name} - Renderers found: {renderers.Length}");

        foreach (MeshRenderer renderer in renderers)
        {
            materials.AddRange(renderer.materials);
        }

        originalMaterials[obj] = materials;
    }


    private void SetTransparentMaterials(GameObject obj)
    {
        MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            Material[] transparentMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                transparentMaterials[i] = materialTransparent;
            }
            renderer.materials = transparentMaterials;
        }
    }

    private void RestoreOriginalMaterials(GameObject obj)
    {
        if (originalMaterials.ContainsKey(obj))
        {
            MeshRenderer[] renderers = obj.GetComponentsInChildren<MeshRenderer>();
            List<Material> materials = originalMaterials[obj];
            int index = 0;

            foreach (MeshRenderer renderer in renderers)
            {
                Material[] restoredMaterials = new Material[renderer.materials.Length];
                for (int i = 0; i < renderer.materials.Length; i++)
                {
                    if (index < materials.Count)
                    {
                        restoredMaterials[i] = materials[index];
                        index++;
                    }
                }
                renderer.materials = restoredMaterials;
            }

            originalMaterials.Remove(obj);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Transparent"))
        {
            //Debug.Log("Trigger Enter: " + other.gameObject.name);

            MakeTransparent(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Transparent"))
        {
            RestoreOriginalMaterials(other.gameObject);
            currentlyTransparent.Remove(other.gameObject);
        }
    }
}
