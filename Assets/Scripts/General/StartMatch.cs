using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMatch : MonoBehaviour
{
    public List<GameObject> BlueUnitTypes = new List<GameObject>();
    public List<GameObject> RedUnitTypes = new List<GameObject>();
    int index;

    public GameObject Blue_Assault;
    public GameObject Blue_Support;
    public GameObject Blue_Medic;

    public GameObject Red_Assault;
    public GameObject Red_Support;
    public GameObject Red_Medic;

    public Transform SpawnBlue;
    public Transform SpawnRed;

    private float B = 0.5f;
    private float R = 0.5f;

    public float teamsize;

    public GameObject UNITS;
    // Start is called before the first frame update
    void Start()
    {
        BlueUnitTypes.Add(Blue_Assault);
        BlueUnitTypes.Add(Blue_Support);
        BlueUnitTypes.Add(Blue_Medic);

        RedUnitTypes.Add(Red_Assault);
        RedUnitTypes.Add(Red_Support);
        RedUnitTypes.Add(Red_Medic);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("CTF");
        }
        if (GameObject.FindObjectsOfType<Blue>().Length < teamsize)
        {
            if(B <= 0f)
            {
                index = Random.Range(0, BlueUnitTypes.Count);
                GameObject curr_Unit = BlueUnitTypes[index];
                GameObject UNIT = Instantiate(curr_Unit, new Vector3(SpawnBlue.position.x + Random.Range(-5f, 5f), SpawnBlue.position.y + Random.Range(-30f, 30f), 0), Quaternion.identity);
                UNIT.transform.parent = UNITS.transform;
                B = 0.5f;
            }
            else
            {
                B -= Time.deltaTime;
            }
        }
        if (GameObject.FindObjectsOfType<Red>().Length < teamsize)
        {
            if (R <= 0f)
            {
                index = Random.Range(0, RedUnitTypes.Count);
                GameObject curr_Unit = RedUnitTypes[index];
                GameObject UNIT = Instantiate(curr_Unit, new Vector3(SpawnRed.position.x + Random.Range(-5f, 5f), SpawnRed.position.y + Random.Range(-30f, 30f), 0), Quaternion.identity);
                UNIT.transform.parent = UNITS.transform;
                R = 0.5f;
            }
            else
            {
                R -= Time.deltaTime;
            }
        }
    }
}
