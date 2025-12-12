using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public KeyCode moveForwardKey = KeyCode.Z;
    public KeyCode moveBackwardKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.Q;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode bowShootKey = KeyCode.Mouse0;
    public KeyCode bowKey = KeyCode.Mouse1;
    public KeyCode switchSwordKey = KeyCode.R;
    public KeyCode switchBowKey = KeyCode.T;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode pauseKey = KeyCode.Escape;
    public KeyCode getItems = KeyCode.E;

    // Private fields
    private float horizontal;
    private float vertical;
    private float mouseX;
    private bool isRunning;
    private bool isStoppingGame;
    private bool jumpPressed;
    private bool attackPressed;
    private bool bowHold;
    private bool bowShoot;
    private bool switchSword;
    private bool switchBow;
    private bool isGettingItems;

    /// <summary>
    /// Met à jour les valeurs des inputs du joueur à chaque frame.
    /// </summary>
    void Update()
    {
        isStoppingGame = Input.GetKeyDown(KeyCode.Escape);
        
        if (Time.timeScale == 0f)
        {
            ResetInputs();
            return;
        }

        horizontal = 0f;
        vertical = 0f;

        if (Input.GetKey(moveForwardKey)) vertical += 1f;
        if (Input.GetKey(moveBackwardKey)) vertical -= 1f;
        if (Input.GetKey(moveRightKey)) horizontal += 1f;
        if (Input.GetKey(moveLeftKey)) horizontal -= 1f;

        mouseX = Input.GetAxis("Mouse X"); 

        isRunning = Input.GetKey(runKey);
        jumpPressed = Input.GetKeyDown(jumpKey);
        attackPressed = Input.GetKeyDown(attackKey);
        bowHold = Input.GetKey(bowKey);
        bowShoot = Input.GetKeyDown(bowShootKey);
        switchSword = Input.GetKeyDown(switchSwordKey);
        switchBow = Input.GetKeyDown(switchBowKey);
        isGettingItems = Input.GetKeyDown(getItems);
    }

    private void ResetInputs()
    {
        jumpPressed = false;
        attackPressed = false;
        bowHold = false;
        bowShoot = false;
        switchSword = false;
        switchBow = false;
        isRunning = false;
    }

    #region Getter Methods

    public KeyCode GetJumpKey() => jumpKey;
    public KeyCode GetAttackKey() => attackKey;
    public KeyCode GetBowShootKey() => bowShootKey;
    public KeyCode GetBowKey() => bowKey;
    public KeyCode GetSwitchSwordKey() => switchSwordKey;
    public KeyCode GetSwitchBowKey() => switchBowKey;
    public KeyCode GetRunKey() => runKey;
    public KeyCode GetPauseKey() => pauseKey;
    public KeyCode GetMoveForwardKey() => moveForwardKey;
    public KeyCode GetMoveBackwardKey() => moveBackwardKey;
    public KeyCode GetMoveLeftKey() => moveLeftKey;
    public KeyCode GetMoveRightKey() => moveRightKey;
    public KeyCode GetItemsKey() => getItems;


    /// <summary>
    /// Retourne la valeur de l'axe horizontal.
    /// </summary>
    public float GetHorizontal() { return horizontal; }

    /// <summary>
    /// Retourne la valeur de l'axe vertical.
    /// </summary>
    public float GetVertical() { return vertical; }

    /// <summary>
    /// Retourne la valeur de l'axe horizontal de la souris.
    /// </summary>
    public float GetMouseX() { return mouseX; }

    /// <summary>
    /// Indique si le joueur est en train de courir (Shift).
    /// </summary>
    public bool IsRunPressed() { return isRunning; }

    /// <summary>
    /// Indique si le joueur a mis le jeu en pause (Echap).
    /// </summary>
    public bool IsStoppingGame() { return isStoppingGame; }

    /// <summary>
    /// Indique si le joueur a appuyé sur la touche de saut (Space).
    /// </summary>
    public bool IsJumpPressed() { return jumpPressed; }

    /// <summary>
    /// Indique si le joueur a appuyé sur le bouton d'attaque (Mouse0).
    /// </summary>
    public bool IsAttackPressed() { return attackPressed; }

    /// <summary>
    /// Indique si le joueur maintient le bouton de l'arc (Mouse1).
    /// </summary>
    public bool IsBowHold() { return bowHold; }

    /// <summary>
    /// Indique si le joueur a tiré une flèche (Mouse0).
    /// </summary>
    public bool IsBowShoot() { return bowShoot; }

    /// <summary>
    /// Indique si le joueur a demandé à changer vers l'épée (R).
    /// </summary>
    public bool IsSwitchSword() { return switchSword; }

    /// <summary>
    /// Indique si le joueur a demandé à changer vers l'arc (T).
    /// </summary>
    public bool IsSwitchBow() { return switchBow; }

    public bool IsGettingItems() { return isGettingItems; }
    #endregion
}
