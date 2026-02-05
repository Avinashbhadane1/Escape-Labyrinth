using UnityEngine;

public class LimitCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject player;
    void LateUpdate(){
        transform.position = new Vector3(player.transform.position.x,40,player.transform.position.z);
    }
}
