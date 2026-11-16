using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private Vector3 fieldSize = new Vector3(100f, 100f, 100f);
    [SerializeField] private int asteroidCount = 50;
    [SerializeField] private float minSpacing = 5f;

    private const int AttemptsPerAsteroid = 10;

    private void Start()
    {
        GenerateAsteroids();
    }

    private void GenerateAsteroids()
    {
        if (asteroidPrefab == null || asteroidCount <= 0)
        {
            return;
        }

        List<Vector3> asteroidPositions = new List<Vector3>();
        int maxAttempts = asteroidCount * AttemptsPerAsteroid;
        int attempts = 0;

        while (asteroidPositions.Count < asteroidCount && attempts < maxAttempts)
        {
            attempts++;

            Vector3 localPosition = new Vector3(
                Random.Range(-fieldSize.x * 0.5f, fieldSize.x * 0.5f),
                Random.Range(-fieldSize.y * 0.5f, fieldSize.y * 0.5f),
                Random.Range(-fieldSize.z * 0.5f, fieldSize.z * 0.5f));

            if (!HasEnoughSpacing(localPosition, asteroidPositions))
            {
                continue;
            }

            GameObject asteroid = Instantiate(asteroidPrefab, transform);
            asteroid.transform.localPosition = localPosition;
            asteroid.transform.localRotation = Random.rotation;
            asteroidPositions.Add(localPosition);
        }

        if (asteroidPositions.Count < asteroidCount)
        {
            Debug.LogWarning($"Only generated {asteroidPositions.Count} of {asteroidCount} asteroids. Increase the field size or reduce the minimum spacing.", this);
        }
    }

    private bool HasEnoughSpacing(Vector3 candidatePosition, List<Vector3> asteroidPositions)
    {
        float minimumDistanceSquared = minSpacing * minSpacing;

        foreach (Vector3 asteroidPosition in asteroidPositions)
        {
            if ((candidatePosition - asteroidPosition).sqrMagnitude < minimumDistanceSquared)
            {
                return false;
            }
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, fieldSize);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
