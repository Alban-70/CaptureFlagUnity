using System.Collections;
using UnityEngine;

public class SpawnSkeleton : MonoBehaviour
{
    [SerializeField] private PlayerDialogue playerDialogue;
    [SerializeField] private GameObject skeletonPrefab;


    private int maxNumberEnemies = 15;
    private float spawnDelay = 7f;

    private BoxCollider boxCollider;
    private int currentEnemies = 0;
    private bool isSpawning = false;



    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (!playerDialogue.canStartQuest)
            return;
        
        if (currentEnemies >= maxNumberEnemies)
            return;

        if (!isSpawning)
            StartCoroutine(SpawnWithDelay());
    }

    private IEnumerator SpawnWithDelay()
    {
        isSpawning = true;
        SpawnEnemy();

        yield return new WaitForSecondsRealtime(spawnDelay);
        isSpawning = false;
    }

    private void OnEnemyDeath()
    {
        currentEnemies = Mathf.Max(0, currentEnemies - 1);
    }

    private void SpawnEnemy()
    {
        Vector3 randomPosition = GetRandomPointInBounds(boxCollider.bounds);

        if (!Physics.Raycast(randomPosition, Vector3.down, out RaycastHit hit, 100f, LayerMask.GetMask("Ground")))
            return;

        if (!UnityEngine.AI.NavMesh.SamplePosition(
                hit.point,
                out UnityEngine.AI.NavMeshHit navHit,
                2f,
                UnityEngine.AI.NavMesh.AllAreas))
            return;
            
        GameObject enemy = Instantiate(skeletonPrefab, navHit.position, Quaternion.identity);

        SkeletonLogic logic = enemy.GetComponent<SkeletonLogic>();
        if (logic != null)
            logic.SetTarget(playerDialogue.transform);

        currentEnemies++;

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
                health.OnDeath += OnEnemyDeath;

    }

    

    private Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.center.y + 50f, // ou bounds.center.y si besoin
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }


}
