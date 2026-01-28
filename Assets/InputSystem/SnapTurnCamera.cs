using UnityEngine;

public class SnapTurnCamera : MonoBehaviour
{
    public Transform player;
    public float snapAngle = 90f;
    public float turnCooldown = 0.25f;

    private bool canTurn = true;

    void Update()
    {
        if (!canTurn) return;

        if (Input.GetKeyDown(KeyCode.D))
            SnapTurn(1);

        if (Input.GetKeyDown(KeyCode.A))
            SnapTurn(-1);
    }

    void SnapTurn(int direction)
    {
        float y = player.eulerAngles.y + snapAngle * direction;
        y = Mathf.Round(y / snapAngle) * snapAngle;

        player.rotation = Quaternion.Euler(0, y, 0);

        canTurn = false;
        Invoke(nameof(ResetTurn), turnCooldown);
    }

    void ResetTurn()
    {
        canTurn = true;
    }
}
