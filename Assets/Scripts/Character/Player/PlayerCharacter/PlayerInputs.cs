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

    /// <summary>
    /// Met à jour les valeurs des inputs du joueur à chaque frame.
    /// </summary>
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

    #region Getter Methods
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
    #endregion
}
