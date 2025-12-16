using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerDialogue playerDialogue;

    [Header("Prefabs")]
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private GameObject golemPrefab;


    private int maxSkeletons;
    private int maxGolems;
    private float skeletonSpawnDelay;
    private float golemSpawnDelay;

    #region Easy values
    private int easyMaxS = 15;  // Nombre maximum de squelettes
    private int easyMaxG = 3;   // Nombre maximum de golems
    private float easySpawnDelayS = 7f;     // Temps en secondes
    private float easySpawnDelayG = 20f;    // Temps en secondes
    #endregion

    #region Hard values
    private int hardMaxS = 30;  // Nombre maximum de squelettes
    private int hardMaxG = 6;   // Nombre maximum de golems
    private float hardSpawnDelayS = 4f;     // Temps en secondes
    private float hardSpawnDelayG = 10f;    // Temps en secondes
    #endregion
    

    private BoxCollider boxCollider;
    private int currentSkeletons = 0;
    private int currentGolems = 0;

    private Coroutine skeletonRoutine;
    private Coroutine golemRoutine;

    [HideInInspector] public bool getCoin = false;



    void Awake()
    {
        
        boxCollider = GetComponent<BoxCollider>();
        SetEasyLevel();
    }

    void Update()
    {
        getCoin = gameManager.getCoin;

        if (!playerDialogue.canStartQuest)
            return;
        
        if (!getCoin)
        {
            if (skeletonRoutine == null)
                skeletonRoutine = StartCoroutine(SkeletonSpawnLoop());

            if (golemRoutine == null)
                golemRoutine = StartCoroutine(GolemSpawnLoop());
        }
        
    }


    #region Spawn Loops
    private IEnumerator SkeletonSpawnLoop()
    {
        while (playerDialogue.canStartQuest)
        {
            if (getCoin)
                break;
            if (currentSkeletons < maxSkeletons)
                SpawnEnemy(skeletonPrefab, OnSkeletonDeath, ref currentSkeletons);

            yield return new WaitForSecondsRealtime(skeletonSpawnDelay);
        }
    }

    private IEnumerator GolemSpawnLoop()
    {
        while (playerDialogue.canStartQuest)
        {
            if (getCoin)
                break;
            if (currentGolems < maxGolems)
                SpawnEnemy(golemPrefab, OnGolemDeath, ref currentGolems);

            yield return new WaitForSecondsRealtime(golemSpawnDelay);
        }
    }
    #endregion

    
    #region Spawn Logic
    private void SpawnEnemy(GameObject prefab, System.Action onDeath, ref int counter)
    {
        Vector3 randomPoint = GetRandomPointInBounds(boxCollider.bounds);

        if (!Physics.Raycast(randomPoint, Vector3.down, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
            return;

        if (!NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
            return;

        GameObject enemy = Instantiate(prefab, navHit.position, Quaternion.identity);
        counter++;

        EnemyLogic logic = enemy.GetComponent<EnemyLogic>();
        if (logic != null)
            logic.SetTarget(playerDialogue.transform);

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
            health.OnDeath += onDeath;
    }
    #endregion

    
    #region Death Callbacks

    private void OnSkeletonDeath()
    {
        currentSkeletons = Mathf.Max(0, currentSkeletons - 1);
    }

    private void OnGolemDeath()
    {
        currentGolems = Mathf.Max(0, currentGolems - 1);
    }

    #endregion

    private Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.center.y + 50f, // ou bounds.center.y si besoin
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }


    public void SetEasyLevel()
    {
        maxSkeletons = easyMaxS;
        maxGolems = easyMaxG;
        skeletonSpawnDelay = easySpawnDelayS;
        golemSpawnDelay = easySpawnDelayG;
    }

    public void SetHardLevel()
    {
        maxSkeletons = hardMaxS;
        maxGolems = hardMaxG;
        skeletonSpawnDelay = hardSpawnDelayS;
        golemSpawnDelay = hardSpawnDelayG;
    }
}