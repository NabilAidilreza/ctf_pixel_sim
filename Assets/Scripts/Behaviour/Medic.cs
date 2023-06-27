using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Passive Heal To Nearby Allies //
public class Medic : MonoBehaviour
{
    public float healValue;
    public float healRate;
    public float healRadius;
    public LayerMask WhatIsTargets;
    public GameObject MedicBag;
    public bool IsBlue;
    private float MedicBagDropCoolDown;
    private float RESET;
    // Start is called before the first frame update
    void Start()
    {
        MedicBagDropCoolDown = 5f;
        RESET = MedicBagDropCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (MedicBagDropCoolDown < 0)
        {
            MedicBagDropCoolDown = RESET;
        }
        else
        {
            MedicBagDropCoolDown -= Time.deltaTime;
        }
        if (healRate <= 0)
        {
            Collider2D[] targetsToHeal = Physics2D.OverlapCircleAll(this.transform.position, healRadius, WhatIsTargets);
            for (int i = 0; i < targetsToHeal.Length; i++)
            {
                targetsToHeal[i].GetComponent<Health>().Heal(healValue);
            }
            healRate = 0.1f;
        }
        else
        {
            healRate -= Time.deltaTime;
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
