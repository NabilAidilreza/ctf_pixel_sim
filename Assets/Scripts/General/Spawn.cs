using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private float T = 1f;

    // Update is called once per frame
    void Update()
    {
        if (T <= 0f)
        {
            transform.position = transform.parent.position + Random.insideUnitSphere * 5f;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            T = 1f;
        }
        else
        {
            T -= Time.deltaTime;
        }
    }
}
