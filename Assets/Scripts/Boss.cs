using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float[] fireballSpeed;
    public float[] distance;
    public Transform[] fireballs;

    private void Update()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            fireballs[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * fireballSpeed[i]) * distance[i], Mathf.Sin(Time.time * fireballSpeed[i]) * distance[i], 0);
        }
    }
}
