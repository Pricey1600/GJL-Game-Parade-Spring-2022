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
    private void Start()
    {
        followCam = GameObject.FindGameObjectWithTag("FollowCam").GetComponent<CinemachineFreeLook>();
        aimCam = GameObject.FindGameObjectWithTag("AimCam").GetComponent<CinemachineVirtualCamera>();
        aimPOV = aimCam.GetCinemachineComponent<CinemachinePOV>();
        if (PlayerPrefs.GetFloat("followCamSpeedX") < minFollowCamSpeedX)
        {
            PlayerPrefs.SetFloat("followCamSpeedX", defaultFollowCamSpeedX);
        }
        if (PlayerPrefs.GetFloat("followCamSpeedY") < minFollowCamSpeedY/50)
        {
            PlayerPrefs.SetFloat("followCamSpeedY", defaultFollowCamSpeedY/50);
        }
        setFollowCamSpeedX(PlayerPrefs.GetFloat("followCamSpeedX"));
        setFollowCamSpeedY(PlayerPrefs.GetFloat("followCamSpeedY")*50);

        cameraSensXSlider.minValue = minFollowCamSpeedX;
        cameraSensXSlider.maxValue = maxFollowCamSpeedX;

        cameraSensYSlider.minValue = minFollowCamSpeedY;
        cameraSensYSlider.maxValue = maxFollowCamSpeedY;

    }

    public void setFollowCamSpeedX(float speed)
    {
        PlayerPrefs.SetFloat("followCamSpeedX", speed);
        followCam.m_XAxis.m_MaxSpeed = speed;
        aimPOV.m_HorizontalAxis.m_MaxSpeed = speed;
        cameraSensXText.text = speed.ToString();
    }
    public void setFollowCamSpeedY(float speed)
    {
        PlayerPrefs.SetFloat("followCamSpeedY", (speed / 50));
        followCam.m_YAxis.m_MaxSpeed = (speed / 50);
        aimPOV.m_VerticalAxis.m_MaxSpeed = speed;
        cameraSensYText.text = speed.ToString();
    }

    private void setSliderValue()
    {
        cameraSensXSlider.value = PlayerPrefs.GetFloat("followCamSpeedX");
        cameraSensYSlider.value = PlayerPrefs.GetFloat("followCamSpeedY")*50;
    }
}
