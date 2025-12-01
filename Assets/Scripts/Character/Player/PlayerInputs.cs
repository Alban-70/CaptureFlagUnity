using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    // Private fields
    private float horizontal;
    private float vertical;
    private float mouseX;
    private bool isRunning;
    private bool jumpPressed;
    private bool attackPressed;
    private bool bowHold;
    private bool bowShoot;
    private bool switchSword;
    private bool switchBow;

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");

        isRunning = Input.GetKey(KeyCode.LeftShift);
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        attackPressed = Input.GetKeyDown(KeyCode.Mouse0);
        bowHold = Input.GetKey(KeyCode.Mouse1);
        bowShoot = Input.GetKeyDown(KeyCode.Mouse0);
        switchSword = Input.GetKeyDown(KeyCode.R);
        switchBow = Input.GetKeyDown(KeyCode.T);
    }

    // Getter functions
    public float GetHorizontal() { return horizontal; }
    public float GetVertical() { return vertical; }
    public float GetMouseX() { return mouseX; }
    public bool IsRunPressed() { return isRunning; }
    public bool IsJumpPressed() { return jumpPressed; }
    public bool IsAttackPressed() { return attackPressed; }
    public bool IsBowHold() { return bowHold; }
    public bool IsBowShoot() { return bowShoot; }
    public bool IsSwitchSword() { return switchSword; }
    public bool IsSwitchBow() { return switchBow; }
}
