using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject platformPrefab;
    public Transform cameraTransform;

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
    public int initialPlatformCount = 15;
    public float startY = -4f;

    [Header("Spawn Range")]
    public float minX = -2.5f;
    public float maxX = 2.5f;

    [Header("Vertical Spacing")]
    public float minVerticalSpacing = 0.5f;
    public float maxVerticalSpacing = 0.67f;

    [Header("Spawn Ahead")]
    public float spawnAheadDistance = 10f;

    [Header("Anti Overlap Rules")]
    public float minHorizontalGap = 1.2f;
    public float minVerticalGap = 1.0f;
    public int maxSpawnTries = 20;

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
        if (platformPrefab == null)
        {
            Debug.LogError("PlatformSpawner: chưa gán platformPrefab.");
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

            SpawnPlatform(spawnPos, allowSpring, allowPropeller);

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
            SpawnPlatform(spawnPos, true, true);
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

            if (dy < 0.8f && dx < minHorizontalGap)
            {
                return false;
            }

            if (dx < 0.6f && dy < minVerticalGap)
            {
                return false;
            }
        }

        return true;
    }

    void SpawnPlatform(Vector2 spawnPosition, bool allowSpring, bool allowPropeller)
    {
        GameObject spawnedPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        TrySpawnSpring(spawnedPlatform, allowSpring);
        TrySpawnPropeller(spawnedPlatform, allowPropeller);

        recentSpawnPositions.Add(spawnPosition);

        if (recentSpawnPositions.Count > 10)
        {
            recentSpawnPositions.RemoveAt(0);
        }
    }

    void TrySpawnSpring(GameObject spawnedPlatform, bool allowSpring)
    {
        if (!allowSpring) return;
        if (springPrefab == null) return;

        if (Random.value <= springChance)
        {
            Vector3 springSpawnPos = spawnedPlatform.transform.position + springOffset;
            Instantiate(springPrefab, springSpawnPos, Quaternion.identity, spawnedPlatform.transform);
        }
    }

    void TrySpawnPropeller(GameObject spawnedPlatform, bool allowPropeller)
    {
        if (!allowPropeller) return;
        if (propellerPickupPrefab == null) return;

        if (Random.value <= propellerChance)
        {
            Vector3 propellerSpawnPos = spawnedPlatform.transform.position + propellerOffset;
            Instantiate(propellerPickupPrefab, propellerSpawnPos, Quaternion.identity, spawnedPlatform.transform);
        }
    }
}