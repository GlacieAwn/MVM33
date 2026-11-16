using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Ranged Values")]
    [SerializeField] private float minRotation = 1f;
    [SerializeField] private float maxRotation = 10f;

    [SerializeField] private float minScale = 50f;
    [SerializeField] private float maxScale = 150f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomTransform = Random.Range(minScale, maxScale);
        transform.localScale = Vector3.one * randomTransform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Random.Range(minRotation, maxRotation) * Time.deltaTime, Random.Range(minRotation, maxRotation) * Time.deltaTime, Random.Range(minRotation, maxRotation) * Time.deltaTime);
    }
}
