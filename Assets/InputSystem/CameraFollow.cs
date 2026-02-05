using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform target;
    public float smoothSpeed = 0.8f;
    

    public Vector3 offset;
    void LateUpdate(){
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position,desiredPos,smoothSpeed);
        transform.position = smoothPos;

    }
    
}
