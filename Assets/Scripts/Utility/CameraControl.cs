 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("ÊÂ¼þ¼àÌý")]
    public VoidEventSO afterSceneLoadEvent;
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;
    public VoidEventSO cameraShakeEvent;
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }
    private void OnEnable()
    {
        if (cameraShakeEvent != null)
        { cameraShakeEvent.OnEventRaised += OnCameraShake; }
        afterSceneLoadEvent.OnEventRaised += OnafterSceneLoadEvent;
    }

    private void OnafterSceneLoadEvent()
    {
       GetCameraBounds();
    }

    private void OnDisable()
    {
        if (cameraShakeEvent != null)
            cameraShakeEvent.OnEventRaised -= OnCameraShake;
        afterSceneLoadEvent.OnEventRaised -= OnafterSceneLoadEvent;
    }

    private void OnCameraShake()
    {
        impulseSource.GenerateImpulse();
    }

    //private void Start()
    //{
    //    GetCameraBounds();
    //}
    private void GetCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null )
        {
            return;
        }

        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache(); 
    }
}
