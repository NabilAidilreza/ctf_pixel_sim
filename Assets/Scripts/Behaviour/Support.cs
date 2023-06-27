using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support : MonoBehaviour
{
    // Set Claymore mine //
    public bool IsBlue;
    public float ran;
    private float T;
    // Start is called before the first frame update
    void Start()
    {
        ran = Random.Range(0, 3);
        T = Random.Range(5f, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        if(ran == 0)
        {
            Base_Behaviour bb = GetComponent<Base_Behaviour>();
            bb.SpreadValue = 5;
            bb.CoolDownDuration = 0.05f;
        }
        else
        {
            Base_Behaviour bb = GetComponent<Base_Behaviour>();
            bb.SpreadValue = 10;
            bb.CoolDownDuration = 0.1f;
        }
        if(T <= 0)
        {
            T = Random.Range(5f, 8f);
            ran = Random.Range(0, 3);
        }
        else
        {
            T -= Time.deltaTime;
        }
        // Grouping Behaviour //
        float separateSpeed = 1;
        float separateRadius = 1f;
        Vector2 sum = Vector2.zero;
        float count = 0f;
        // Sphere For Check Ally
        var hits = Physics2D.OverlapCircleAll(transform.position, separateRadius);
        foreach (var hit in hits)
        {
            if (IsBlue)
            {
                if (hit.GetComponent<Blue>() != null && hit.transform != transform)
                {
                    Vector2 difference = transform.position - hit.transform.position;
                    difference = difference.normalized / Mathf.Abs(difference.magnitude);
                    sum += difference;
                    count++;
                }
            }
            else
            {
                if (hit.GetComponent<Red>() != null && hit.transform != transform)
                {
                    Vector2 difference = transform.position - hit.transform.position;
                    difference = difference.normalized / Mathf.Abs(difference.magnitude);
                    sum += difference;
                    count++;
                }
            }
        }
        if (count > 0)
        {
            sum /= count;
            sum = sum.normalized * separateSpeed;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)sum, separateSpeed * Time.deltaTime);
        }
    }
}