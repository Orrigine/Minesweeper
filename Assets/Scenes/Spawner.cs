using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] private GameObject mover;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] GameObject Original;
    void Start()
    {
        // Instantiate(Original, new Vector2(1, 2), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > spawnRate)
        {
            Instantiate(Original, new Vector2(2, -2), Quaternion.identity);
            spawnRate += 1f;
        }
    }
}
