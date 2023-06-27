using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
public class Base_Behaviour : MonoBehaviour
{
    public bool IsBlue;
    public Rigidbody2D Projectile;
    public float EngageRange;
    public float StopRange;
    public Transform ShootPoint;
    public float CoolDownDuration;
    private float CoolDown;
    private Transform target;
    public float offset;
    private int i;
    private float projectileSpeed = 50f;
    private Animator Soldier_Anim;

    public float SpreadValue;

    private float distance = 50f;
    private string Name = "";
    AIDestinationSetter AI;
    AIPath path;

    private float TimeToBChange;
    private float ran;

    public float ammoAmount;
    private float resetAmount;
    public float reloadTime;
    private float RT;
    private List<GameObject> Flags = new List<GameObject>();
    public string STATE;
    // Start is called before the first frame update
    void Start()
    {
        RT = reloadTime;
        resetAmount = ammoAmount;
        GameObject[] FLAGS = GameObject.FindGameObjectsWithTag("Flag");
        foreach(GameObject flag in FLAGS)
        {
            Flags.Add(flag);
        }
        Soldier_Anim = gameObject.GetComponent<Animator>();
        CoolDown = CoolDownDuration;
        AI = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
        Physics2D.queriesStartInColliders = false;
        if (IsBlue)
        {
            Name = "Red";
        }
        else
        {
            Name = "Blue";
        }
        ran = Random.Range(0, 2);
        TimeToBChange = Random.Range(5f, 10f);
        if (ran == 0)
        {
            STATE = "ATTACK";
        }
        else
        {
            STATE = "CAPTURE";
        }
    }

    // GENERAL LOGIC //
    void Update()
    {
        target = FindClosestObject();
        int FlagIndex = FindClosestFlag();
        bool FullAttack = CheckFlagDominance();
        if(TimeToBChange <= 0f)
        {
            ran = Random.Range(0, 2);
            if (ran == 0)
            {
                STATE = "ATTACK";
            }
            else
            {
                STATE = "CAPTURE";
            }
            TimeToBChange = Random.Range(5f, 15f);
        }
        else
        {
            TimeToBChange -= Time.deltaTime;
        }
        if(STATE == "CAPTURE")
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance);
            if(hitInfo.collider != null && hitInfo.collider.CompareTag(Name))
            {
                Shoot(target);
                AI.target = null;
            }
            else
            {
                AI.target = Flags[FlagIndex].gameObject.transform.GetChild(0).transform;
                AI.enabled = true;
                path.enabled = true;
            }
        }
        else if(STATE == "ATTACK")
        {
            if(target != null)
            {
                AI.target = target;
                Attack(target);
            }
        }
        else
        {

        }
        //// Attack Unit
        //if(ran == 0)
        //{
        //    if (target == null)
        //    {

        //    }
        //    else
        //    {
        //        AI.target = target;
        //        Attack(target);
        //    }
        //}
        //else if(ran == 1)
        //{
        //    if (target == null)
        //    {

        //    }
        //    else
        //    {
        //        // Full Assault if all flags are captured
        //        if(FullAttack == true)
        //        {
        //            AI.target = target;
        //            Attack(target);
        //        }
        //        else
        //        {
        //            // Capture flag
        //            AI.enabled = true;
        //            path.enabled = true;
        //            AI.target = Flags[FlagIndex].gameObject.transform.GetChild(0).transform;
        //            //Shoot(target);
        //        }
        //    }
        //}
        
    }
    // SHOOT LOGIC //
    public void Attack(Transform target) // Move and Shoot //
    {
        // Attack enemy if in range and if closest flag is captured // 
        Vector3 relativeTarget = (target.GetChild(0).transform.position - transform.position).normalized;
        Quaternion toQuaternion = Quaternion.FromToRotation(Vector3.up, relativeTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, toQuaternion, 5f * Time.deltaTime);
        float Range = Vector2.Distance(transform.position, target.transform.position);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,transform.up,distance);
        if (Range < EngageRange && hitInfo.collider != null && hitInfo.collider.CompareTag(Name))
        {
            if(ammoAmount > 0)
            {
                //Debug.DrawLine(transform.position, hitInfo.point, Color.red);
                // Juggle and Shoot //
                AI.enabled = false;
                path.enabled = false;
                if (Range <= StopRange + 1)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.position, -1.5f * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.position, 2f * Time.deltaTime);
                }
                Soldier_Anim.SetBool("Shooting", true);
                // Shoot Bullet //
                if (CoolDown <= 0)
                {
                    Rigidbody2D projectileInstance;
                    float W = Random.Range(-SpreadValue, SpreadValue);
                    ShootPoint.localRotation = Quaternion.Euler(0f, 0f, W);
                    projectileInstance = Instantiate(Projectile, ShootPoint.position, ShootPoint.rotation) as Rigidbody2D;
                    projectileInstance.GetComponent<Rigidbody2D>().velocity = ShootPoint.transform.up * projectileSpeed;
                    ammoAmount -= 1;
                    CoolDown = CoolDownDuration;
                }
                else
                {
                    CoolDown -= Time.deltaTime;
                }
            }
            else
            {
                // Reloading //
                if(RT <= 0)
                {
                    ammoAmount = resetAmount;
                    RT = reloadTime;
                }
                else
                {
                    RT -= Time.deltaTime;
                }

            }
        }
        else
        {
            //Debug.DrawLine(transform.position, transform.position + transform.up * distance, Color.green);
            AI.enabled = true;
            path.enabled = true;
            //transform.position = Vector2.MoveTowards(transform.position, target.position, 5f * Time.deltaTime);
            Soldier_Anim.SetBool("Shooting", false);
        }
    }
    public void Shoot(Transform target) // Hold and Shoot //
    {
        // Attack enemy if in range and if closest flag is captured // 
        Vector3 relativeTarget = (target.GetChild(0).transform.position - transform.position).normalized;
        Quaternion toQuaternion = Quaternion.FromToRotation(Vector3.up, relativeTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, toQuaternion, 5f * Time.deltaTime);
        float Range = Vector2.Distance(transform.position, target.transform.position);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance);
        if (Range < EngageRange && hitInfo.collider != null && hitInfo.collider.CompareTag(Name))
        {
            if (ammoAmount > 0)
            {
                Debug.DrawLine(transform.position, hitInfo.point, Color.red);
                // Juggle and Shoot //
                if (Range < StopRange)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.position, -1.5f * Time.deltaTime);
                }
                Soldier_Anim.SetBool("Shooting", true);
                // Shoot Bullet //
                if (CoolDown <= 0)
                {
                    Rigidbody2D projectileInstance;
                    float W = Random.Range(-SpreadValue, SpreadValue);
                    ShootPoint.localRotation = Quaternion.Euler(0f, 0f, W);
                    projectileInstance = Instantiate(Projectile, ShootPoint.position, ShootPoint.rotation) as Rigidbody2D;
                    projectileInstance.GetComponent<Rigidbody2D>().velocity = ShootPoint.transform.up * projectileSpeed;
                    ammoAmount -= 1;
                    CoolDown = CoolDownDuration;
                }
                else
                {
                    CoolDown -= Time.deltaTime;
                }
            }
            else
            {
                // Reloading //
                if (RT <= 0)
                {
                    ammoAmount = resetAmount;
                    RT = reloadTime;
                }
                else
                {
                    RT -= Time.deltaTime;
                }

            }
        }
        else
        {
            Soldier_Anim.SetBool("Shooting", false);
        }
    }
    // FLAG LOGIC //
    public int FindClosestFlag()
    {
        
        if (IsBlue == true)
        {
            int index = -1;
            float Minimum = Mathf.Infinity;
            for (int i = 0; i < Flags.Count; i++)
            {
                float distance = (Flags[i].transform.position - this.transform.position).sqrMagnitude;
                if (distance < Minimum && Flags[i].GetComponent<Flag_Status>().InPossession != "BLUE")
                {
                    Minimum = distance;
                    index = i;
                }
            }
            //Debug.Log(index);
            if (index == -1)
            {
                return 0;
            }
            else
            {
                return index;
            }
            
            
        }
        else if(IsBlue == false)
        {
            int index = -1;
            float Minimum = Mathf.Infinity;
            for (int i = 0; i < Flags.Count; i++)
            {
                float distance = (Flags[i].transform.position - this.transform.position).sqrMagnitude;
                if (distance < Minimum && Flags[i].GetComponent<Flag_Status>().InPossession != "RED")
                {
                    Minimum = distance;
                    index = i;
                }
            }
            //Debug.Log(index);
            if (index == -1)
            {
                return 0;
            }
            else
            {
                return index;
            }
        }
        else
        {
            return 0;
        }
    }
    public bool CheckFlagDominance()
    {
        bool FullAttack = true;
        if (IsBlue)
        {
            for (int i = 0; i < Flags.Count; i++)
            {
                Flag_Status Curr_Flag = Flags[i].gameObject.GetComponent<Flag_Status>();
                if (Curr_Flag.InPossession != "BLUE")
                {
                    FullAttack = false;
                }
                else
                {
                    FullAttack = true;
                }
            }
            return FullAttack;
        }
        else
        {
            for (int i = 0; i < Flags.Count; i++)
            {
                Flag_Status Curr_Flag = Flags[i].gameObject.GetComponent<Flag_Status>();
                if (Curr_Flag.InPossession != "RED")
                {
                    FullAttack = false;
                }
                else
                {
                    FullAttack = true;
                }
            }
            return FullAttack;
        }
    }
    public Transform FindClosestObject()
    {
        if (IsBlue == true)
        {
            float DtoC = Mathf.Infinity;
            Red closestEnemy = null;
            Red[] lall = GameObject.FindObjectsOfType<Red>();
            foreach (Red curr in lall)
            {
                float DtoE = (curr.transform.position - this.transform.position).sqrMagnitude;
                if (DtoE < DtoC)
                {
                    DtoC = DtoE;
                    closestEnemy = curr;
                }
            }
            if (closestEnemy == null)
            {
                return null;
            }
            return closestEnemy.GetComponent<Transform>();
        }
        else
        {
            float DtoC = Mathf.Infinity;
            Blue closestPlayer = null;
            Blue[] lall = GameObject.FindObjectsOfType<Blue>();
            foreach (Blue curr in lall)
            {
                float DtoE = (curr.transform.position - this.transform.position).sqrMagnitude;
                if (DtoE < DtoC)
                {
                    DtoC = DtoE;
                    closestPlayer = curr;
                }
            }
            if (closestPlayer == null)
            {
                return null;
            }
            return closestPlayer.GetComponent<Transform>();
        }
    }
}