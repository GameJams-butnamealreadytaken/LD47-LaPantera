using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject PlayerTarget;

    void Start()
    {
    }

    void Update()
    {
        if (!PlayerTarget)
            return;

        Vector3 vCameraLoc = PlayerTarget.transform.position;
        vCameraLoc.y += 6.0f;
        vCameraLoc.z -= 6.0f;

        transform.position = vCameraLoc;
        transform.rotation = Quaternion.AngleAxis(35.0f, Vector3.right);
    }
}
