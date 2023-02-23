using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] private GameObject mover;
    // [SerializeField] private float spawnRate = 2f;
    [SerializeField] GameObject Original;
  
    void Start()
    {
        // InitTilemap();
        for(int i  = 0; i<10 ; i++)
        {
            Instantiate( Original, new Vector2(i, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Time.timeSinceLevelLoad > spawnRate)
        // {
        
            
        //     spawnRate += 1f;
        // }
    }
}
