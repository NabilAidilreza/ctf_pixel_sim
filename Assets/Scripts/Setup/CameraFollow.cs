using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera RTSCAMERA;
    private Func<Vector3> GetCameraFollowPosFunc;
    private Func<float> GetCameraZoomFunc;

    public void SetUp(Func<Vector3> GetCameraFollowPosFunc, Func<float> GetCameraZoomFunc)
    {
        this.GetCameraFollowPosFunc = GetCameraFollowPosFunc;
        this.GetCameraZoomFunc = GetCameraZoomFunc;
    }
    private void Start()
    {
        RTSCAMERA = transform.GetComponent<Camera>();
    }
    public void SetCameraFollowPos(Vector3 cameraFollowPosition)
    {
        SetGetCameraFollowPosFunc(() => cameraFollowPosition);
    }
    public void SetCameraZoom(float cameraZoom)
    {
        SetGetCameraZoomFunc(() => cameraZoom);
    }
    public void SetGetCameraFollowPosFunc(Func<Vector3> GetCameraFollowPosFunc)
    {
        this.GetCameraFollowPosFunc = GetCameraFollowPosFunc;
    }
    public void SetGetCameraZoomFunc(Func<float> GetCameraZoomFunc)
    {
        this.GetCameraZoomFunc = GetCameraZoomFunc;
    }
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleZoom();
    }
    private void HandleMovement()
    {
        Vector3 cameraFollowPosition = GetCameraFollowPosFunc();
        cameraFollowPosition.z = transform.position.z;

        Vector3 cameraMoveDir = (cameraFollowPosition - transform.position).normalized;
        float distance = Vector3.Distance(cameraFollowPosition, transform.position);
        float cameraMoveSpeed = 5f;

        if (distance > 0)
        {
            Vector3 newCameraPos = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;

            float distanceAfterMove = Vector3.Distance(newCameraPos, cameraFollowPosition);
            if (distanceAfterMove > distance)
            {
                newCameraPos = cameraFollowPosition;
            }
            transform.position = newCameraPos;
        }
        // Camera Boundary //
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -110f, 100f),
            Mathf.Clamp(transform.position.y, -155f, 160f),
            transform.position.z);
    }
    private void HandleZoom()
    {
        float cameraZoom = GetCameraZoomFunc();
        float cameraZoomDiff = cameraZoom - RTSCAMERA.orthographicSize;
        float cameraZoomSpeed = 1f;
        RTSCAMERA.orthographicSize += cameraZoomDiff * cameraZoomSpeed * Time.deltaTime;

        if (cameraZoomDiff > 0)
        {
            if (RTSCAMERA.orthographicSize > cameraZoom)
            {
                RTSCAMERA.orthographicSize = cameraZoom;
            }
            else
            {
                if (RTSCAMERA.orthographicSize < cameraZoom)
                {
                    RTSCAMERA.orthographicSize = cameraZoom;
                }
            }
        }
    }
}
