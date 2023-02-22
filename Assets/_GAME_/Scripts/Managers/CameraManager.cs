using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    #region Public
    [FoldoutGroup("Cameras")]
    public CinemachineVirtualCamera menuCam;
    [FoldoutGroup("Cameras")]
    public CinemachineVirtualCamera gameplayCam;
    [FoldoutGroup("Cameras")]
    public CinemachineVirtualCamera successCam;
    [FoldoutGroup("Cameras")]
    public CinemachineVirtualCamera failCam;
    #endregion

    #region Local
    private Transform _targetTransform;
    private float _shakeTimer;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    #endregion

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime / Time.timeScale;
            if (_shakeTimer <= 0f)
            {
                _cinemachineBasicMultiChannelPerlin = gameplayCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.LevelLoaded += SelectMenuCam;
        GameEvents.LevelStarted += SelectGameplayCam;
        GameEvents.LevelFinished += OnLevelFinished;
    }

    private void OnDisable()
    {
        GameEvents.LevelLoaded -= SelectMenuCam;
        GameEvents.LevelStarted -= SelectGameplayCam;
        GameEvents.LevelFinished -= OnLevelFinished;
    }

    public void SetCameraTarget(Transform target)
    {
        _targetTransform = target;

        menuCam.Follow = _targetTransform;
        menuCam.LookAt = _targetTransform;

        gameplayCam.Follow = _targetTransform;
        gameplayCam.LookAt = _targetTransform;

        successCam.Follow = _targetTransform;
        successCam.LookAt = _targetTransform;

        failCam.Follow = _targetTransform;
        failCam.LookAt = _targetTransform;

        //levelEndCam.Follow = _targetTransform.transform;
        //levelEndCam.LookAt = _targetTransform.transform;
    }

    public void Init(Transform target)
    {
        SetCameraTarget(target);

        SelectMenuCam();
    }

    public void SelectMenuCam()
    {
        menuCam.Priority = 11;
        gameplayCam.Priority = 10;
        failCam.Priority = 10;
        successCam.Priority = 10;
        //levelEndCam.Priority = 10;
    }

    public void SelectGameplayCam()
    {
        menuCam.Priority = 10;
        gameplayCam.Priority = 11;
        failCam.Priority = 10;
        successCam.Priority = 10;
        //levelEndCam.Priority = 10;
    }

    public void SelectFailCam()
    {
        menuCam.Priority = 10;
        gameplayCam.Priority = 10;
        failCam.Priority = 11;
        successCam.Priority = 10;
        //levelEndCam.Priority = 10;
    }

    public void SelectSuccessCam()
    {
        menuCam.Priority = 10;
        gameplayCam.Priority = 10;
        failCam.Priority = 10;
        successCam.Priority = 11;
        //levelEndCam.Priority = 10;
    }


    //public void SelectLevelEndCam()
    //{
    //    levelEndCam.Priority = 11;
    //    gameplayCam.Priority = 10;
    //    failCam.Priority = 10;
    //    successCam.Priority = 10;
    //}

    //public void LevelEndCamLockZ(Vector3 _playerPos)
    //{
    //    levelEndCam.GetComponent<LockCameraPosition>().ZPosition = _playerPos.z ;
    //    levelEndCam.GetComponent<LockCameraPosition>().LockZ = true;
    //}

    private void ShakeCamera(ShakeCameraData eventData)
    {
        _cinemachineBasicMultiChannelPerlin = gameplayCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = eventData.Intensity;
        _shakeTimer = eventData.Time;
    }

    void OnLevelFinished(bool successed)
    {
        if (successed)
        {
            SelectSuccessCam();
        }
        else
        {
            SelectFailCam();
        }
    }
}
