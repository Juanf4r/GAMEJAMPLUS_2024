using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_Velocity : MonoBehaviour
{
    public static PU_Velocity Instance;

    public void moreVelocity(float speed)
    {
        speed = speed + 100;
    }
}
