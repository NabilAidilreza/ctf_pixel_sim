using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Assault : MonoBehaviour
{
    // Rocket Launcher //
    public bool IsBlue;
    public Rigidbody2D Projectile;
    public Transform ShootPoint;
    public Transform Target;
    public GameObject Launcher;
    private GameObject L;
    private float RAN;
    private float projectileSpeed = 25f;
    // Start is called before the first frame update
    void Start()
    {
        RAN = Random.Range(5f,15f);
    }

    // Update is called once per frame
    void Update()
    {
        if (RAN <= 0f)
        {
            Rigidbody2D projectileInstance;
            projectileInstance = Instantiate(Projectile, ShootPoint.position, ShootPoint.rotation) as Rigidbody2D;
            projectileInstance.GetComponent<Rigidbody2D>().velocity = ShootPoint.transform.up * projectileSpeed;
            L = Instantiate(Launcher, Target.position, Target.rotation);
            StartCoroutine("Destroy");
            RAN = Random.Range(5f, 15f);
        }
        else
        {
            RAN -= Time.deltaTime;
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
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(L);
    }
}