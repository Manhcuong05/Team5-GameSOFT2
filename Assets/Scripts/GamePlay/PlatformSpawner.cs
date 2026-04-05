using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject normalPlatformPrefab;
    public GameObject breakablePlatformPrefab;
    public Transform cameraTransform;

    [Header("Platform Type Chance")]
    [Range(0f, 1f)]
    public float breakableChance = 0.10f;
    public int noBreakableOnFirstPlatforms = 8;

    [Header("Spring Settings")]
    public GameObject springPrefab;
    [Range(0f, 1f)]
    public float springChance = 0.15f;
    public Vector3 springOffset = new Vector3(0f, 0.35f, 0f);
    public int noSpringOnFirstPlatforms = 5;

    [Header("Propeller Settings")]
    public GameObject propellerPickupPrefab;
    [Range(0f, 1f)]
    public float propellerChance = 0.08f;
    public Vector3 propellerOffset = new Vector3(0f, 0.5f, 0f);
    public int noPropellerOnFirstPlatforms = 10;

    [Header("Initial Spawn")]
    public int initialPlatformCount = 28;
    public float startY = -4f;

    [Header("Spawn Range")]
    public float minX = -2.5f;
    public float maxX = 2.5f;

    [Header("Vertical Spacing")]
    public float minVerticalSpacing = 0.22f;
    public float maxVerticalSpacing = 0.35f;

    [Header("Spawn Ahead")]
    public float spawnAheadDistance = 10f;

    [Header("Anti Overlap Rules")]
    public float minHorizontalGap = 0.45f;
    public float minVerticalGap = 0.35f;
    public int maxSpawnTries = 35;

    private float highestSpawnY;
    private List<Vector2> recentSpawnPositions = new List<Vector2>();

    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Start()
    {
        if (normalPlatformPrefab == null)
        {
            Debug.LogError("PlatformSpawner: chưa gán normalPlatformPrefab.");
            enabled = false;
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("PlatformSpawner: chưa gán cameraTransform.");
            enabled = false;
            return;
        }

        GenerateInitialPlatforms();
    }

    private void Update()
    {
        if (cameraTransform == null) return;
        SpawnMorePlatformsIfNeeded();
    }

    void GenerateInitialPlatforms()
    {
        float currentY = startY;

        for (int i = 0; i < initialPlatformCount; i++)
        {
            Vector2 spawnPos = GetValidSpawnPosition(currentY);

            bool allowSpring = i >= noSpringOnFirstPlatforms;
            bool allowPropeller = i >= noPropellerOnFirstPlatforms;
            bool allowBreakable = i >= noBreakableOnFirstPlatforms;

            SpawnPlatform(spawnPos, allowSpring, allowPropeller, allowBreakable);

            currentY += Random.Range(minVerticalSpacing, maxVerticalSpacing);
        }

        highestSpawnY = currentY;
    }

    void SpawnMorePlatformsIfNeeded()
    {
        while (highestSpawnY < cameraTransform.position.y + spawnAheadDistance)
        {
            highestSpawnY += Random.Range(minVerticalSpacing, maxVerticalSpacing);

            Vector2 spawnPos = GetValidSpawnPosition(highestSpawnY);
            SpawnPlatform(spawnPos, true, true, true);
        }
    }

    Vector2 GetValidSpawnPosition(float yPos)
    {
        Vector2 bestPos = new Vector2(Random.Range(minX, maxX), yPos);

        for (int i = 0; i < maxSpawnTries; i++)
        {
            float randomX = Random.Range(minX, maxX);
            Vector2 candidate = new Vector2(randomX, yPos);

            if (IsPositionValid(candidate))
            {
                return candidate;
            }
        }

        return bestPos;
    }

    bool IsPositionValid(Vector2 candidate)
    {
        for (int i = 0; i < recentSpawnPositions.Count; i++)
        {
            Vector2 existing = recentSpawnPositions[i];

            float dx = Mathf.Abs(candidate.x - existing.x);
            float dy = Mathf.Abs(candidate.y - existing.y);

            if (dx < minHorizontalGap && dy < minVerticalGap)
            {
                return false;
            }

            if (dy < 0.35f && dx < 0.45f)
            {
                return false;
            }

            if (dx < 0.25f && dy < 0.30f)
            {
                return false;
            }
        }

        return true;
    }

    void SpawnPlatform(Vector2 spawnPosition, bool allowSpring, bool allowPropeller, bool allowBreakable)
    {
        GameObject prefabToSpawn = normalPlatformPrefab;

        if (allowBreakable && breakablePlatformPrefab != null && Random.value <= breakableChance)
        {
            prefabToSpawn = breakablePlatformPrefab;
        }

        GameObject spawnedPlatform = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        bool isBreakablePlatform = prefabToSpawn == breakablePlatformPrefab;

        if (!isBreakablePlatform)
        {
            TrySpawnSingleItem(spawnedPlatform, allowSpring, allowPropeller);
        }

        recentSpawnPositions.Add(spawnPosition);

        if (recentSpawnPositions.Count > 12)
        {
            recentSpawnPositions.RemoveAt(0);
        }
    }

    void TrySpawnSingleItem(GameObject spawnedPlatform, bool allowSpring, bool allowPropeller)
    {
        if (spawnedPlatform == null) return;

        bool canSpawnSpring = allowSpring && springPrefab != null;
        bool canSpawnPropeller = allowPropeller && propellerPickupPrefab != null;

        if (!canSpawnSpring && !canSpawnPropeller) return;

        // Chỉ cho phép tối đa 1 vật phẩm trên 1 nền
        if (canSpawnSpring && Random.value <= springChance)
        {
            Vector3 springSpawnPos = spawnedPlatform.transform.position + springOffset;
            Instantiate(springPrefab, springSpawnPos, Quaternion.identity, spawnedPlatform.transform);
            return;
        }

        if (canSpawnPropeller && Random.value <= propellerChance)
        {
            Vector3 propellerSpawnPos = spawnedPlatform.transform.position + propellerOffset;
            Instantiate(propellerPickupPrefab, propellerSpawnPos, Quaternion.identity, spawnedPlatform.transform);
        }
    }
}