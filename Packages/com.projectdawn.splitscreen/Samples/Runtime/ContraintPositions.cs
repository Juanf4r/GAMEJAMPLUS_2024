using UnityEngine;
using Unity.Mathematics;

namespace ProjectDawn.SplitScreen
{
    public class ContraintPositions : MonoBehaviour, ISplitScreenTranslatingPosition
    {
        public Bounds Bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(1, 1, 1));

        public float3 OnSplitScreenTranslatingPosition(int index, float3 position)
        {
            position = math.clamp(position, Bounds.min, Bounds.max);
            return position;
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawBounds(Bounds, new Color32(255, 255, 255, 128));
        }
    }
}
