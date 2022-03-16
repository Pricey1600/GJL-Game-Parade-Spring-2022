using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPivotCentering : MonoBehaviour
{
    public Transform launcher;
    private Quaternion newRotationQ;

    // Update is called once per frame
    void Update()
    {
        Vector3 newRotation = new Vector3(0, launcher.localRotation.eulerAngles.y, 0);
        //Debug.Log("Launcher Y: " + launcher.rotation.eulerAngles.y);
        newRotationQ.eulerAngles = newRotation;
        transform.localRotation = newRotationQ;
    }
}
