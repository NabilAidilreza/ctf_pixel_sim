using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureBar : MonoBehaviour
{
    public bool IsBlue;
    private float bar;
    private float progress;
    Vector3 localscale;
    public int scale;
    private Transform start;
    // Start is called before the first frame update
    void Start()
    {
        localscale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float BLUEPROGRESS = GetComponentInParent<Flag_Status>().blueCapture;
        float REDPROGRESS = GetComponentInParent<Flag_Status>().redCapture;
        float CapTime = GetComponentInParent<Flag_Status>().CaptureTime;
        if (IsBlue)
        {
            
            
            progress = BLUEPROGRESS;
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            
            progress = REDPROGRESS;
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        if(((BLUEPROGRESS >= CapTime && REDPROGRESS <= 0.01) || (REDPROGRESS >= CapTime && BLUEPROGRESS <= 0.01)))
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        localscale.x = progress / scale;
        transform.localScale = localscale;
    }

}
