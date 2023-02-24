using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using VSCodeEditor;

public struct Cell
    {
        public enum Type
        {
            Empty,
            Bomb, 
            Number,
        }
        public Type type;
        public Vector3Int position;
        public int number;
        public bool revealed;
        public bool flagged;
        public bool exploded;

    }

public class TileMap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Original;
    [SerializeField] GameObject[,] Tilemap = new GameObject[10, 10];

/*
    //
    UnityEvent m_event;
    //end
*/
    

    // 
    // void InitTilemap()
    // {
    //     for (int i = 0; i < 10; i++)
    //     {
    //         for (int j = 0; j < 10; j++)
    //         {
    //             Instantiate(Original, new Vector2(i, j), Quaternion.identity);
    //         }
    //     }
    // }
    // void Start()
    // {
    //     InitTilemap();
    // }

    // // Update is called once per frame



/*
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_event != null && revealed == false)
        {

            revealed = true;
            //change le sprite 

        }
        else if (Input.GetMouseButtonDown(1) && m_event != null && revealed == false)
        {
            if (flagged == false)
            {
                flagged = true;
                //change le sprite encase avec flag 
            }
            else
            {
                flagged = false;
                //change le sprite en case de base 
            }
        }

*/



        // if (Time.timeSinceLevelLoad > spawnRate)
        // {
        //     Instantiate(Original, new Vector2(2, -2), Quaternion.identity);
        //     spawnRate += 1f;
        // }
    //}
}
