using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using ProjectDawn.SplitScreen;

public class ConvexTest : MonoBehaviour
{
    public Transform[] Points;

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        var points = new NativeArray<float2>(Points.Length, Allocator.Temp);
        for (int i = 0; i < points.Length; ++i)
            points[i] = new float2(Points[i].transform.position.x, Points[i].transform.position.y);

        bool isConvex = IsConvex(points);

        ProjectDawn.SplitScreen.Gizmos.DrawConvexPolygon(points, 1, 0, isConvex ? Color.green : Color.red);
        points.Dispose();
    }

    bool IsConvex(in NativeArray<float2> points)
    {
        float sign;
        {
            float2 p0 = points[0];
            float2 p1 = points[1];
            float2 p2 = points[2];

            float2 l0 = p1 - p0;
            float2 l1 = p2 - p1;

            float crossProduct = l0.x * l1.y - l0.y * l1.x;

            sign = math.sign(crossProduct);
        }

        for (int i = 1; i < points.Length; ++i)
        {
            float2 p0 = points[i];
            float2 p1 = points[(i + 1) % points.Length];
            float2 p2 = points[(i + 2) % points.Length];

            float2 l0 = p1 - p0;
            float2 l1 = p2 - p1;

            float crossProduct = l0.x * l1.y - l0.y * l1.x;

            if (sign != math.sign(crossProduct))
                return false;
        }

        return true;
    }
}
