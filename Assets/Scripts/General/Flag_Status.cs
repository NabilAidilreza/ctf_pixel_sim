using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag_Status : MonoBehaviour
{
    [Header("Name Of The Red Team(Player Must Have This Tag)")]
    public string redTeam = "Red";
    [Header("Name Of The Blue Team(Player Must Have This Tag)")]
    public string blueTeam = "Blue";
    //Sprites To Change The Capture Point To
    public GameObject BlueCapColor;
    public GameObject RedCapColor;
    [Header("Time It Takes To Capture The Point")]
    public float CaptureTime = 30;
    [Header("Radius In Which The Point Is Capturable")]
    public float CapRadius = 7;
    //Time the team has held the capture point
    public float redCapture;
    public float blueCapture;
    //Empty Collider Array
    Collider2D[] colliders;
    //Radius to hold the statuses of the teams
    public bool redCaptureStatus;
    public bool blueCaptureStatus;
    public string InPossession = "";
    void Start()
    {
        //Both of the teams capture status at the start are false
        blueCaptureStatus = false;
        //greyCaptureStatus = false;
        redCaptureStatus = false;
        //Both of the teams capture times are 0 at the start
        redCapture = 0;
        blueCapture = 0;
        //greyCapture = 0;
    }
    void Update()
    {
        //Get the colliders inside the origin of this gameobject and the radius(CapRadius)
        colliders = Physics2D.OverlapCircleAll(transform.position, CapRadius);
        if(colliders.Length <= 0)
        {
            if(InPossession == "RED")
            {
                if(blueCapture > 0)
                {
                    blueCapture -= Time.deltaTime;
                }
                redCapture += Time.deltaTime * 0.5f;
            }
            if(InPossession == "BLUE")
            {
                if(redCapture > 0)
                {
                    redCapture -= Time.deltaTime;
                }
                blueCapture += Time.deltaTime * 0.5f;
            }
        }
        //If there is a collider detected within the radius(CapRadius)
        if (colliders.Length > 0 && colliders != null)
        {
            //Then loop through the colliders
            for (int i = 0; i < colliders.Length; i++)
            {
                //Check if the Team Counts are not equal to each other
                if (TeamCount(blueTeam, colliders) != TeamCount(redTeam, colliders) || TeamCount(redTeam, colliders) != TeamCount(blueTeam, colliders))
                {
                    //If the collider within the circle is of the blue team
                    if (colliders[i].tag == blueTeam)
                    {
                        //And the teams capture time is less than the time it takes to capture
                        if (blueCapture < CaptureTime)
                        {
                            //Add to the teams capture time
                            blueCapture += Time.deltaTime;
                            //If the enemy teams capture time is greater than 0
                            if (redCapture > 0)
                            {
                                //Subtract the enemy teams capture time
                                redCapture -= Time.deltaTime;
                            }
                        }
                        //Else the teams capture time is greater than the capture time
                        else
                        {
                            //Set the capture status of the team to true and the enemys team to false
                            InPossession = "BLUE";
                            blueCaptureStatus = true;
                            redCaptureStatus = false;
                        }
                    }
                    //Check if the Team Counts are not equal to each other
                    else if (colliders[i].tag == redTeam)
                    {
                        //If the collider within the circle is of the red team
                        if (redCapture < CaptureTime)
                        {
                            //Add to the teams capture time
                            redCapture += Time.deltaTime;
                            //If the enemy teams capture time is greater than 0
                            if (blueCapture > 0)
                            {
                                //Subtract the enemy teams capture time
                                blueCapture -= Time.deltaTime;
                            }
                        }
                        //Else the teams capture time is greater than the capture time
                        else
                        {
                            //Set the capture status of the team to true and the enemys team to false
                            InPossession = "RED";
                            redCaptureStatus = true;
                            blueCaptureStatus = false;
                        }
                    }
                }
            }
        }
        //If the teams capture status is equal to true then set the color of our capture point accordingly
        if (redCaptureStatus == true)
        {
            RedCapColor.transform.position = new Vector3(RedCapColor.transform.position.x, RedCapColor.transform.position.y, -1);
            BlueCapColor.transform.position = new Vector3(BlueCapColor.transform.position.x, BlueCapColor.transform.position.y, 1);
        }
        if (blueCaptureStatus == true)
        {
            BlueCapColor.transform.position = new Vector3(BlueCapColor.transform.position.x, BlueCapColor.transform.position.y, -1);
            RedCapColor.transform.position = new Vector3(RedCapColor.transform.position.x, RedCapColor.transform.position.y, 1);
        }
    }
    //Uses Team Tag and Collider Array To Find The Team Count
    int TeamCount(string tag, Collider2D[] colliders)
    {
        //Set the count to 0
        int count = 0;
        //Loop through the colliders array
        for (int i = 0; i < colliders.Length; i++)
        {
            //If the collider's tag within the array equal the team tag
            if (colliders[i].tag == tag)
            {
                //Add to the count
                count += 1;
            }
        }
        //Then return the complete count of the tag within the array
        return count;
    }
}

