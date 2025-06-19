using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook freeLook;
    public float rotataionSpeed;

    Vector2 lookInput;




    void Update()
    {
        if(lookInput != Vector2.zero)
        {
            freeLook.m_XAxis.Value += lookInput.x * rotataionSpeed;
            freeLook.m_YAxis.Value += lookInput.y * rotataionSpeed;
        }

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
}
