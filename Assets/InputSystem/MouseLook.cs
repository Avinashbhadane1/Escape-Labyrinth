using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 0.15f;
    public Transform playerBody;

    float xRotation = 0f;

    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * sensitivity;

        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseDelta.x);
    }
}
