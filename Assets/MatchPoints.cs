using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPoints : MonoBehaviour
{
    public Slider BLUESLIDER;
    public Slider REDSLIDER;
    public List<GameObject> Flags;
    public float BluePoints;
    public float RedPoints;
    private float timeToAdd = 2f;
    private float RESET;
    public float MaxPoints;
    public string MatchStatus = "";
    public GameObject Status_UI;
    public GameObject Blue_Text_UI;
    public GameObject Red_Text_UI;
    public GameObject Indicator;
    // Start is called before the first frame update
    void Start()
    {
        MatchStatus = "First To " + MaxPoints.ToString() + " Wins";
        Flags = new List<GameObject>();
        GameObject[] TEMP = GameObject.FindGameObjectsWithTag("Flag");
        foreach(GameObject flag in TEMP)
        {
            Flags.Add(flag);
        }
        BLUESLIDER.maxValue = MaxPoints;
        REDSLIDER.maxValue = MaxPoints;
        RESET = timeToAdd;
        BluePoints = 0;
        RedPoints = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMatchStatus();
        CheckMatchEnd();
        BLUESLIDER.value = BluePoints;
        REDSLIDER.value = RedPoints;
        if (timeToAdd < 0)
        {
            foreach (GameObject flag in Flags)
            {
                if (flag.GetComponent<Flag_Status>().InPossession == "BLUE")
                {
                    BluePoints += 1;
                }
                else
                {
                    if (flag.GetComponent<Flag_Status>().InPossession == "RED")
                    {
                        RedPoints += 1;
                    }
                    else
                    {

                    }
                }
            }
            timeToAdd = RESET;
        }
        else
        {
            timeToAdd -= Time.deltaTime;
        }
    }
    void UpdateMatchStatus()
    {
        Status_UI.GetComponentInChildren<TextMesh>().text = MatchStatus;
        Blue_Text_UI.GetComponentInChildren<TextMesh>().text = BluePoints.ToString();
        Red_Text_UI.GetComponentInChildren<TextMesh>().text = RedPoints.ToString();
    }
    void CheckMatchEnd()
    {
        if(BluePoints >= MaxPoints)
        {
            BluePoints = MaxPoints;
            MatchStatus = "BLUE TEAM WINS";
            Indicator.GetComponent<Image>().color = Color.blue;
            Time.timeScale = 0;
        }
        if (RedPoints >= MaxPoints)
        {
            RedPoints = MaxPoints;
            MatchStatus = "RED TEAM WINS";
            Indicator.GetComponent<Image>().color = Color.red;
            Time.timeScale = 0;
        }
    }
}
