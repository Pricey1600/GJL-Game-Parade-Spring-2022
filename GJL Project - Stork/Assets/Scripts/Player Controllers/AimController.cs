using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class AimController : MonoBehaviour
{
    public bool isAiming;
    [SerializeField] private float launchPower;
    [SerializeField] private CinemachineVirtualCamera aimVC;
    [SerializeField] private Transform aimLocator, van, launcher;
    [SerializeField] private GameObject crosshair, babyPrefab;
    [SerializeField] private LayerMask aimColliderMask;

    private void Update()
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
            FindAimPos();
            AlignLauncher();
            crosshair.SetActive(true);
        }
        else
        {
            crosshair.SetActive(false);
        }
    }

    public void Launch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var babyToLaunch = Instantiate(babyPrefab, launcher.position, launcher.rotation);
            babyToLaunch.GetComponent<Rigidbody>().velocity = launcher.forward * launchPower;
        }
        
    }

    private void AlignLauncher()
    {
        launcher.LookAt(aimLocator);
    }

    private void FindAimPos()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            aimLocator.position = raycastHit.point;

        }
        else
        {
            aimLocator.position = ray.origin + (ray.direction * 100);
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
}
