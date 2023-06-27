using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_R : MonoBehaviour
{
    public float lifeTime;
    public float distance;
    public LayerMask WhatisSolid;
    public float dmg;
    public GameObject SmallExp;
    public LayerMask WhatisEnemy;
    public float SplashRadius;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDmg();
    }
    //DamageFunc 
    public void CheckDmg()
    {
        //HitInfo
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, WhatisSolid);
        if (hitInfo.collider != null)
        {
            //CheckEnemy
            if (hitInfo.collider.CompareTag("Blue"))
            {
                //DamageRegistered
                hitInfo.collider.GetComponent<Health>().TakeDmg(dmg);
            }
            //CheckEnemy
            if (hitInfo.collider.CompareTag("Object"))
            {
                //DamageRegistered
                DestroyProjectile();
            }

            //ProjectileGone
            DestroyProjectile();
        }
    }
    //DestroyFunc
    public void DestroyProjectile()
    {

        Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.position, SplashRadius, WhatisEnemy);
        foreach (Collider2D collider in Colliders)
        {
            Health UnitHP = collider.GetComponent<Health>();
            UnitHP.TakeDmg(dmg);
        }
        Destroy(gameObject);
        Instantiate(SmallExp, transform.position, Quaternion.identity);
    }
}
