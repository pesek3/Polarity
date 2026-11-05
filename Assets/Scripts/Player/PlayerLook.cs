using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam; 
    public float xRotation = 0f;

    public float xSensitivity = 10f;
    public float ySensitivity = 30f;
    // Start is called before the first frame update

    public void ProcessLook(Vector2 input) 
    {
        float mouseX = input.x * xSensitivity * Time.deltaTime; // otaceni kamery podle inputu z input manageru
        float mouseY = input.y * ySensitivity * Time.deltaTime; // otaceni kamery podle inputu z input manageru

        xRotation -= mouseY; 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // omezeni otaceni kamery nahoru a dolu

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX); // otaceni hrace podle inputu z input manageru
    }
}
