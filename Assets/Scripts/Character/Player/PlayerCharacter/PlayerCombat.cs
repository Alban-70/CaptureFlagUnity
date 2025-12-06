using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Serialized Fields
    [Header("Animations")]
    [SerializeField] private Animator anim; // Animator pour gérer toutes les animations du joueur
    [SerializeField] private PlayerInputs inputs; // Script qui récupère les inputs du joueur
    [SerializeField] private PlayerMovement movement; // Référence au script de mouvement
    [SerializeField] private Transform arrowSocket; // Point de spawn des flèches
    [SerializeField] private GameObject arrow; // Prefab de la flèche
    [SerializeField] private GameObject arrowPreview; // Modèle de prévisualisation de la flèche
    [SerializeField] private Quaternion arrowModelOffset = Quaternion.identity; // Décalage de rotation pour le modèle
    #endregion

    #region Private State
    private MeshRenderer mrHead, mrStick; // Composants MeshRenderer du modèle de flèche
    private Vector3 positionHold = new Vector3(0.1562f, -0.0224f, 0.0272f); // Position de prévisualisation
    private Quaternion rotationHold = Quaternion.Euler(77.569f, -275.068f, -362.837f); // Rotation de prévisualisation
    private Vector3 arrowVector; // Direction ou vecteur utilisé pour le tir de la flèche
    private float speedLaunchArrow = 35f; // Vitesse de lancement des flèches
    private float attackCount = 1; // Pour suivre les combos d'attaques au sol
    private bool continueAirAttack = false; // Indique si la deuxième partie de l'attaque aérienne peut être déclenchée
    private bool airAttackRequested = false; // Marque qu'une attaque aérienne a été demandée
    private bool isHoldingBow = false; // Indique si le joueur tient son arc
    private enum WeaponType { Sword, Bow } // Types d'armes disponibles
    private WeaponType currentWeapon = WeaponType.Sword; // Arme actuellement équipée
    #endregion

    #region Bow Holding & Weapon Switching
    /// <summary>
    /// Initialise les composants et désactive la prévisualisation de la flèche.
    /// </summary>
    void Awake()
    {
        // Récupère les MeshRenderer de la prévisualisation de flèche
        mrHead = arrowPreview.transform.GetChild(0).GetComponent<MeshRenderer>();
        mrStick = arrowPreview.transform.GetChild(1).GetComponent<MeshRenderer>();
        arrowPreview.SetActive(false); // Masque la flèche de prévisualisation au départ
    }

    /// <summary>
    /// Appelé chaque frame pour gérer le changement d'arme et les attaques.
    /// </summary>
    void Update()
    {
        HandleWeaponSwitch(); // Vérifie si le joueur change d'arme
        HandleAttack();       // Vérifie si le joueur attaque
    }

    /// <summary>
    /// Vérifie les inputs pour changer d'arme et appelle la méthode correspondante.
    /// </summary>
    private void HandleWeaponSwitch()
    {
        if (inputs.IsSwitchSword()) SwitchToSword(); // Si input pour switcher épée
        if (inputs.IsSwitchBow()) SwitchToBow();     // Si input pour switcher arc
    }

    /// <summary>
    /// Gère les attaques du joueur selon l'arme équipée.
    /// </summary>
    private void HandleAttack()
    {
        if (currentWeapon == WeaponType.Bow)
        {
            if (inputs.IsBowHold()) // Si le joueur maintient le bouton pour l'arc
            {
                arrowPreview.SetActive(true); // Affiche le modèle de flèche
                arrowPreview.transform.SetParent(arrowSocket); // Le parent est le socket
                mrHead.enabled = true;
                mrStick.enabled = true;
                arrowPreview.transform.SetLocalPositionAndRotation(positionHold, rotationHold); // Position correcte
                EnterBowHold(); 
                
                if (inputs.IsBowShoot()) // Si le joueur tire
                {
                    arrowPreview.SetActive(false); // Masque le modèle
                    TryToAttackWhileHolding(); // Instancie et tire la flèche
                }
            }
            else
            {
                // Si le joueur relâche le bouton d'arc
                arrowPreview.SetActive(false);
                arrowPreview.transform.SetParent(arrowSocket);
                mrHead.enabled = false;
                mrStick.enabled = false;
                ExitBowHold(); // Quitte la posture de tir
            }
        }
        else // Si l'arme équipée est l'épée
        {
            if (inputs.IsAttackPressed()) // Vérifie si le joueur attaque
            {
                if (movement.IsGrounded()) 
                    TryToAttackSwordInGround(); // Attaque au sol
                else 
                    TryToAttackSwordInAir(); // Attaque en l'air
            }
        }
    }

    /// <summary>
    /// Change l'arme équipée en épée.
    /// </summary>
    private void SwitchToSword()
    {
        anim.SetLayerWeight(1, 1f); // Active le layer WeaponLayer
        if (currentWeapon == WeaponType.Sword) return; // Rien à faire si déjà épée

        currentWeapon = WeaponType.Sword;
        anim.SetTrigger("SwitchToSword"); // Lance l'animation
        anim.SetLayerWeight(0, 1f); // Remet le layer par défaut
    }

    /// <summary>
    /// Change l'arme équipée en arc.
    /// </summary>
    private void SwitchToBow()
    {
        anim.SetLayerWeight(1, 1f);
        if (currentWeapon == WeaponType.Bow) return;

        currentWeapon = WeaponType.Bow;
        anim.SetTrigger("SwitchToBow");
        anim.SetLayerWeight(0, 1f);
    }

    /// <summary>
    /// Met le joueur en posture de tir à l'arc.
    /// </summary>
    public void EnterBowHold()
    {
        if (movement == null) return;

        isHoldingBow = true; 
        anim.SetBool("isHolding", true); // Active l'animation de posture
        movement.SetBowHold(true);       // Informe le script de mouvement
    }

    /// <summary>
    /// Quitte la posture de tir à l'arc.
    /// </summary>
    public void ExitBowHold()
    {
        if (movement == null) return;

        isHoldingBow = false;
        anim.SetBool("isHolding", false); 
        movement.SetBowHold(false);
    }
    #endregion

    #region Sword Attacks
    /// <summary>
    /// Effectue une attaque au sol avec l'épée.
    /// </summary>
    public void TryToAttackSwordInGround()
    {
        anim.SetTrigger("SwordAttack"); // Lance l'animation
        anim.SetFloat("attackCount", attackCount); // Définit la valeur pour le combo
        attackCount++;
        if (attackCount >= 3) attackCount = 1; // Reset du combo après 3 attaques
    }

    /// <summary>
    /// Effectue une attaque en l'air avec l'épée.
    /// </summary>
    public void TryToAttackSwordInAir()
    {
        anim.SetTrigger("SwordAttackInAir");
        anim.applyRootMotion = false; // Ne pas appliquer le root motion pendant l'attaque
        airAttackRequested = true; // Permet l'attaque en chute
        movement.airAttackRequested = true; 
        movement.EnableMovementAndJump(false); // Bloque le mouvement pendant l'attaque
    }
    #endregion

    #region Bow Attacks
    /// <summary>
    /// Tire une flèche depuis la posture de tir.
    /// </summary>
    public void TryToAttackWhileHolding()
    {
        anim.SetTrigger("ShotWhileHolding"); // Animation du tir
    }
    #endregion

    #region Air Attack Methods
    /// <summary>
    /// Réinitialise la possibilité de continuer une attaque en l'air.
    /// </summary>
    public void ResetContinueAttackInAir()
    {
        continueAirAttack = false;
        movement.EnableMovementAndJump(true); // Débloque le mouvement
        anim.applyRootMotion = true; // Réactive le root motion
    }

    /// <summary>
    /// Prépare le joueur pour une attaque en chute.
    /// </summary>
    public void PrepareAirAttackFall() => airAttackRequested = true;

    /// <summary>
    /// Instancie et lance une flèche depuis l'arc du joueur.
    /// </summary>
    public void LaunchArrow()
    {
        if (arrow == null || arrowSocket == null) return;

        GameObject newArrow = Instantiate(arrow, arrowSocket.position, arrowSocket.rotation); // Crée la flèche
        Rigidbody arrowRb = newArrow.GetComponent<Rigidbody>(); 
        if (arrowRb == null) return;
        
        Vector3 direction = arrowSocket.right; // Direction de tir
        newArrow.transform.rotation = Quaternion.LookRotation(direction) * arrowModelOffset; // Ajuste l'orientation

        newArrow.transform.SetParent(null); // Détache la flèche du socket
        arrowRb.isKinematic = false; // Active la physique
        arrowRb.linearVelocity = direction * speedLaunchArrow; // Applique la vitesse
    }

    public void SetDamageToEnemyWithSword()
    {
        float attackRange = 2f;
        float attackRadius = 0.5f;
        Vector3 attackPoint = transform.position + transform.forward * attackRange / 2;

        Collider[] hits = Physics.OverlapSphere(attackPoint, attackRadius, LayerMask.GetMask("Enemy"));
        Debug.Log("Hits count: " + hits.Length);

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.ApplyDamage(25f);
                Debug.Log("Hit enemy : " + enemy.name);
            }
        }

    }
    #endregion
}
