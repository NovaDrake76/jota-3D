using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    [Header("Sensitivity Settings")]
    public float xSensitivity = 1f;
    public float ySensitivity = 1f;

    // Sensitivity adjustment factor, use this to scale sensitivity in game settings
    private float sensitivityFactor = 0.5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * sensitivityFactor;
        float mouseY = input.y * sensitivityFactor;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

    public void AdjustSensitivity(float newSensitivity)
    {
        sensitivityFactor = newSensitivity;
    }
}
