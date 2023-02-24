using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.Events;
using VSCodeEditor;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Cell[,] state;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    

    UnityEvent m_event;
    public float distanceFromCamera = 10;
    public Vector3 mouseInWorld = new Vector3();




    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {

        if (m_event == null)
        {
            m_event = new UnityEvent();
        }
        NewGame();
    }
     void Update()
    {
        Vector3 mouseInScreen = Input.mousePosition;
        mouseInScreen.z = distanceFromCamera;  
        mouseInWorld = Camera.main.ScreenToWorldPoint(mouseInScreen);
        mouseInWorld.x = (float)(mouseInWorld.x / 2.56);
        mouseInWorld.y = (float)(mouseInWorld.y / 2.56);


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("posx:" + (int)mouseInWorld.x + "\n posy:" + (int)mouseInWorld.y + "\n posz: " + (int)mouseInWorld.z);
            Debug.LogWarning("mouse in world: " + mouseInWorld);
        }

/*
        if (mouseInWorld.x <= width && mouseInWorld.x > 0 && mouseInWorld.y <= width && mouseInWorld.y > 0)
        {

            Cell cell = state[(int)mouseInWorld.x, (int)mouseInWorld.y];
            if (Input.GetMouseButtonDown(0) && m_event != null && cell.flagged == false)
            {

                cell.revealed = true;
                Debug.LogWarning("reveal pos:" + cell.position);


            }

            else if (Input.GetMouseButtonDown(1) && m_event != null && cell.revealed == false)
            {

                if (cell.flagged == false)
                {
                    cell.flagged = true;
                    Debug.LogWarning("Put flag pos:" + cell.position);
                    //change le sprite encase avec flag 
                }

                else
                {
                    cell.flagged = false;
                    Debug.LogWarning("unput flag" + cell.position);
                    //change le sprite en case de base 
                }

            }
        }*/

    }
    /// <summary>
    /// Create a new game according width and height.
    /// </summary>
    private void NewGame()
    {
        state = new Cell[width, height];
        GenerateTiles();
        board.Draw(state);
    }

    /// <summary>
    /// Generate the tiles.
    /// </summary>
    private void GenerateTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = new()
                {
                    position = new Vector3Int(x, y, 0),
                    type = Cell.Type.Empty
                };
                state[x, y] = cell;

            }
      
        }
    }
    

}
