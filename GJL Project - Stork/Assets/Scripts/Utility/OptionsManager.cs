using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider cameraSensXSlider, cameraSensYSlider;
    [SerializeField] private TMP_Text cameraSensXText, cameraSensYText;
    [SerializeField] private float defaultFollowCamSpeedX = 150f, minFollowCamSpeedX, maxFollowCamSpeedX, defaultFollowCamSpeedY = 1f, minFollowCamSpeedY, maxFollowCamSpeedY;
    private CinemachineFreeLook followCam;
    private CinemachineVirtualCamera aimCam;
    private CinemachinePOV aimPOV;

    private void OnEnable()
    {
        GameManager.OnPause += setSliderValue;
        
    }
    private void OnDisable()
    {
        GameManager.OnPause -= setSliderValue;
    }
    private void Awake()
    {
        followCam = GameObject.FindGameObjectWithTag("FollowCam").GetComponent<CinemachineFreeLook>();
        aimCam = GameObject.FindGameObjectWithTag("AimCam").GetComponent<CinemachineVirtualCamera>();
        aimPOV = aimCam.GetCinemachineComponent<CinemachinePOV>();

        cameraSensXSlider.minValue = minFollowCamSpeedX;
        cameraSensXSlider.maxValue = maxFollowCamSpeedX;

        cameraSensYSlider.minValue = minFollowCamSpeedY;
        cameraSensYSlider.maxValue = maxFollowCamSpeedY;

        if (PlayerPrefs.GetFloat("followCamSpeedX") <= 0)
        {
            PlayerPrefs.SetFloat("followCamSpeedX", defaultFollowCamSpeedX);
        }
        if (PlayerPrefs.GetFloat("followCamSpeedY") <= 0)
        {
            PlayerPrefs.SetFloat("followCamSpeedY", defaultFollowCamSpeedY / 50);
        }
        //setFollowCamSpeedX(PlayerPrefs.GetFloat("followCamSpeedX"));
        //setFollowCamSpeedY(PlayerPrefs.GetFloat("followCamSpeedY")*50);
        setSliderValue();



    }

    public void setFollowCamSpeedX(float speed)
    {
        PlayerPrefs.SetFloat("followCamSpeedX", speed);
        followCam.m_XAxis.m_MaxSpeed = speed;
        aimPOV.m_HorizontalAxis.m_MaxSpeed = speed;
        cameraSensXText.text = speed.ToString();
        //Debug.Log("X:" + PlayerPrefs.GetFloat("followCamSpeedY"));
    }
    public void setFollowCamSpeedY(float speed)
    {
        PlayerPrefs.SetFloat("followCamSpeedY", (speed / 50));
        followCam.m_YAxis.m_MaxSpeed = (speed / 50);
        aimPOV.m_VerticalAxis.m_MaxSpeed = speed;
        cameraSensYText.text = speed.ToString();
        //Debug.Log("Y: " + PlayerPrefs.GetFloat("followCamSpeedY"));
    }

    private void setSliderValue()
    {
        //Debug.LogWarning("setSliderValue Called");
        cameraSensXSlider.value = PlayerPrefs.GetFloat("followCamSpeedX");
        cameraSensYSlider.value = PlayerPrefs.GetFloat("followCamSpeedY")*50;
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
