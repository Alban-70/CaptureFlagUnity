using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Serialized Fields
    [Header("Animations")]
    [SerializeField] private Animator anim; // L'Animator pour gérer toutes les animations du joueur
    [SerializeField] private PlayerInputs inputs; // Script qui récupère les inputs du joueur
    [SerializeField] private PlayerMovement movement; // Référence au script de mouvement pour contrôler le joueur
    [SerializeField] private Transform arrowSocket;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject arrowPreview;
    [SerializeField] private Quaternion arrowModelOffset = Quaternion.identity;

    #endregion

    #region Private State
    private MeshRenderer mrHead, mrStick;
    private Vector3 positionHold = new Vector3(0.1562f, -0.0224f, 0.0272f);
    private Quaternion rotationHold = Quaternion.Euler(77.569f, -275.068f, -362.837f);
    private Vector3 arrowVector;
    private float speedLaunchArrow = 20f;
    private float attackCount = 1; // Pour suivre les combos d'attaques au sol
    private bool continueAirAttack = false; // Faire la deuxième partie de l'attaque
    private bool airAttackRequested = false; // Marque qu'une attaque aérienne a été demandée
    private bool isHoldingBow = false; // Est-ce que le joueur tient son arc
    private enum WeaponType { Sword, Bow } // Types d'armes disponibles
    private WeaponType currentWeapon = WeaponType.Sword; // L'arme actuellement équipée
    #endregion

    #region Bow Holding & Weapon Switching

    void Awake()
    {
        mrHead = arrow.transform.GetChild(0).GetComponent<MeshRenderer>();
        mrStick = arrow.transform.GetChild(1).GetComponent<MeshRenderer>();
    }

    void Update()
    {
        // On vérifie si le joueur change d'arme 
        HandleWeaponSwitch();
        // On vérifie si le jouer attaque
        HandleAttack();
    }

    private void HandleWeaponSwitch()
    {
        // Si le joueur appuie sur le bouton de switch, on change d'arme
        if (inputs.IsSwitchSword()) SwitchToSword();
        if (inputs.IsSwitchBow()) SwitchToBow();
    }

    private void HandleAttack()
    {
        // Gestion des attaques selon l'arme équipée
        if (currentWeapon == WeaponType.Bow)
        {
            arrow.transform.SetParent(arrowSocket);
            // Si le joueur tient l'arc
            if (inputs.IsBowHold())
            {
                mrHead.enabled = true;
                mrStick.enabled = true;
                arrow.transform.SetLocalPositionAndRotation(positionHold, rotationHold);
                EnterBowHold(); // Met le joueur en position de tir
                if (inputs.IsBowShoot())
                {
                    arrowPreview.SetActive(false);
                    TryToAttackWhileHolding(); // Tire la flèche
                }
            }
            else
            {
                mrHead.enabled = false;
                mrStick.enabled = false;
                ExitBowHold(); // On ne tient plus l'arc
            }

        }
        else        // Si l'arme équipée est l'épée
        {
            if (inputs.IsAttackPressed())
            {
                if (movement.IsGrounded()) 
                    TryToAttackSwordInGround(); // Attaque au sol
                else 
                    TryToAttackSwordInAir(); // Attaque en l'air
            }
        }
    }

    private void SwitchToSword()
    {
        anim.SetLayerWeight(1, 1f); // Se met sur le Layer WeaponLayer
        // Si on a déjà l'épée, on ne fait rien
        if (currentWeapon == WeaponType.Sword) return;

        currentWeapon = WeaponType.Sword;
        anim.SetTrigger("SwitchToSword");
        anim.SetLayerWeight(0, 1f); // Se met de nouveau sur le Layer par défaut
    }

    private void SwitchToBow()
    {
        anim.SetLayerWeight(1, 1f); // Se met sur le Layer WeaponLayer
        // Si on a déjà l'arc, on ne fait rien
        if (currentWeapon == WeaponType.Bow) return;

        currentWeapon = WeaponType.Bow;
        anim.SetTrigger("SwitchToBow");
        anim.SetLayerWeight(0, 1f); // Se met de nouveau sur le Layer par défaut
    }

    // Met le joueur en posture de tir
    public void EnterBowHold()
    {
        if (movement == null) return;

        isHoldingBow = true;
        anim.SetBool("isHolding", true);
        movement.SetBowHold(true); // Informe le script de mouvement que le joueur tient l'arc
    }

    // Quitte la posture de tir
    public void ExitBowHold()
    {
        if (movement == null) return;

        isHoldingBow = false;
        anim.SetBool("isHolding", false);
        
        movement.SetBowHold(false);
    }

    #endregion

    #region Sword Attacks
    // Attaque au sol (épée)
    public void TryToAttackSwordInGround()
    {
        anim.SetTrigger("SwordAttack");
        anim.SetFloat("attackCount", attackCount);
        attackCount++;
        if (attackCount >= 3) attackCount = 1;
    }

    // Attaque en l'air (épée)
    public void TryToAttackSwordInAir()
    {
        anim.SetTrigger("SwordAttackInAir");
        anim.applyRootMotion = false;
        airAttackRequested = true;

        // Communication avec le script de mouvement
        movement.airAttackRequested = true;
        movement.EnableMovementAndJump(false);
    }
    #endregion

    #region Bow Attacks
    // Tir depuis la posture de tir
    public void TryToAttackWhileHolding()
    {
        anim.SetTrigger("ShotWhileHolding");
    }
    #endregion

    #region Air Attack Methods
    public void ResetContinueAttackInAir()
    {
        continueAirAttack = false;
        movement.EnableMovementAndJump(true);
        anim.applyRootMotion = true;
    }

    public void PrepareAirAttackFall() => airAttackRequested = true;

    public void LaunchArrow()
    {
        if (arrow == null || arrowSocket == null) return;

        GameObject newArrow = Instantiate(arrow, arrowSocket.position, arrowSocket.rotation);
        Rigidbody arrowRb = newArrow.GetComponent<Rigidbody>();
        if (arrowRb == null) return;
        
        Vector3 direction = arrowSocket.right;
        newArrow.transform.rotation = Quaternion.LookRotation(direction) * arrowModelOffset;

        newArrow.transform.SetParent(null);
        arrowRb.isKinematic = false;
        arrowRb.linearVelocity = direction * speedLaunchArrow;
    }
    #endregion
}