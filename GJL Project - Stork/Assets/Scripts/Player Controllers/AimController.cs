using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class AimController : MonoBehaviour
{
    public bool isAiming, hasBaby;
    [SerializeField] private float aiming_V0, aiming_a, aiming_g, aiming_h, aiming_Vx, aiming_Vy, aiming_R;
    [SerializeField] private CinemachineVirtualCamera aimVC;
    [SerializeField] private CinemachineFreeLook followVC;
    private CinemachinePOV aimPOV;
    [SerializeField] private Transform aimLocator, van, launcher, aimLocatorSprite;
    [SerializeField] private List<GameObject> babyPrefabs;
    [SerializeField] private LayerMask aimColliderMask;

    private AudioSource vanAS;
    [SerializeField] private List<AudioClip> launchSFX;
    [SerializeField] private AudioClip NoBabySFX;

    private Quaternion camAnglesQ;
    private Quaternion aimLocatorSpriteAnglesQ;

    private void OnEnable()
    {
        BabyScript.OnSuccess += ReloadBaby;
        BabyScript.OnFailure += ReloadBaby;
    }

    private void OnDisable()
    {
        BabyScript.OnSuccess -= ReloadBaby;
        BabyScript.OnFailure -= ReloadBaby;
    }
    private void Start()
    {
        aimPOV = aimVC.GetCinemachineComponent<CinemachinePOV>();
        vanAS = gameObject.GetComponent<AudioSource>();
        aimVC.gameObject.SetActive(false);
        Vector3 aimLocatorSpriteAngles = new Vector3(-90, 0, 0);
        aimLocatorSpriteAnglesQ.eulerAngles = aimLocatorSpriteAngles;
        ReloadBaby();
    }

    private void FixedUpdate()
    {
        //if(aimLocator.position.x > van.position.x)
        //{
        //    var thirdPersonFollow = aimVC.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        //    thirdPersonFollow.CameraSide = 0f;
        //}
        //else
        //{
        //    var thirdPersonFollow = aimVC.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        //    thirdPersonFollow.CameraSide = 1f;
        //}
        if (isAiming)
        {
            AlignLauncher();
            FindAimPos();
            locatorRaycast();

            Time.timeScale = 0.3f;
            aimLocatorSprite.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            aimLocatorSprite.gameObject.SetActive(false);
            //aimPOV.m_HorizontalAxis.Value = followVC.m_XAxis.Value;
            //aimPOV.gameObject.transform.position = followVC.transform.position;
            
        }
    }

    public void Launch(InputAction.CallbackContext context)
    {
        if (context.started && isAiming && hasBaby)
        {
            int babyInt = Random.Range(0, babyPrefabs.Count);
            var babyToLaunch = Instantiate(babyPrefabs[babyInt], launcher.position, launcher.rotation);
            babyToLaunch.GetComponent<Rigidbody>().velocity = (launcher.forward * (aiming_V0 + 0.5f));
            hasBaby = false;
            vanAS.PlayOneShot(launchSFX[Random.Range(0, launchSFX.Count)], 0.7f);
        }
        else if (!hasBaby && context.started)
        {
            vanAS.PlayOneShot(NoBabySFX);
        }
        
    }

    private void AlignLauncher()
    {
        Vector3 camAngles = new Vector3(aimPOV.m_VerticalAxis.Value - 30f, aimPOV.m_HorizontalAxis.Value, 0);
        
        camAnglesQ.eulerAngles = camAngles;
        launcher.rotation= camAnglesQ;
    }

    private void FindAimPos()
    {
        float angleDegrees = launcher.rotation.eulerAngles.x;
        //Debug.Log("Angle(degree): " + angleDegrees);
        if (angleDegrees < 0)
        {
            angleDegrees = -angleDegrees;
        }
        float angleRadian = -angleDegrees * Mathf.PI/180;
        
        aiming_a = angleRadian;
        

        aiming_h = launcher.position.y;

        aiming_Vx = aiming_V0 * Mathf.Cos(aiming_a);
        aiming_Vy = aiming_V0 * Mathf.Sin(aiming_a);

        aiming_R = aiming_Vx * (aiming_Vy + Mathf.Sqrt((aiming_Vy*aiming_Vy) + 2 * aiming_g * aiming_h)) / aiming_g;

        aimLocator.localPosition = new Vector3(0, 0.1f, aiming_R);
    }

    private void locatorRaycast()
    {
        if(Physics.Raycast(aimLocator.transform.position, Vector3.down, out RaycastHit hit, 100f, aimColliderMask))
        {
            aimLocatorSprite.position = new Vector3(hit.point.x, hit.point.y +0.1f, hit.point.z);
            aimLocatorSprite.rotation = aimLocatorSpriteAnglesQ;
            Debug.DrawRay(aimLocator.position, aimLocator.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
        }
    }
    public void Aim(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() == true)
        {
            isAiming = true;
            aimVC.gameObject.SetActive(true);
        }
        else
        {
            isAiming = false;
            aimVC.gameObject.SetActive(false);
        }
    }

    private void ReloadBaby()
    {
        hasBaby = true;
    }
}
