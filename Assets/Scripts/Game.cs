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
    [SerializeField] private Cell[,] tab;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;

    private bool gameStarted =false;

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

        /*if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("posx:" + (int)mouseInWorld.x + "\n posy:" + (int)mouseInWorld.y + "\n posz: " + (int)mouseInWorld.z);
            Debug.LogWarning("mouse in world: " + mouseInWorld);
        }*/

        HandleFirstCLick();
         
        
        if (Input.GetMouseButtonDown(0))
        {
            
            if (mouseInWorld.x <= width && mouseInWorld.x > 0 && mouseInWorld.y <= width && mouseInWorld.y > 0)
            {

                /**/
                Vector3Int poscell = new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0);
                
                /**/
               
                if (GetCellFromPosition(poscell).flagged == false && GetCellFromPosition(poscell).revealed == false)
                {
                    ModifyCell(true, 0,poscell);
                    if(GetCellFromPosition(poscell).secretTile == Cell.Type.Empty)
                    {
                        board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileRevealed);
                    }
                    else if (GetCellFromPosition(poscell).secretTile == Cell.Type.Bomb)
                    {
                        board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileBomb);
                        Debug.LogWarning("Macron explosion");
                    }

                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (mouseInWorld.x <= width && mouseInWorld.x > 0 && mouseInWorld.y <= width && mouseInWorld.y > 0)
            {

                /**/
                Vector3Int poscell = new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0);
                /**/


                if (GetCellFromPosition(poscell).revealed == false)
                {
                    if (GetCellFromPosition(poscell).flagged == false)
                    {
                        ModifyCell(true, 1, poscell);
                        board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileFlag);
                    }else if (GetCellFromPosition(poscell).flagged == true)
                    {
                        ModifyCell(false, 1, poscell);
                        board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileUnknown);
                    }
                }
            }

        }

    }

    /// <summary>
    /// Create a new game according width and height.
    /// </summary>
    private void NewGame()
    {
        tab = new Cell[width, height];
        GenerateTiles();
        board.Draw(tab);
        Camera.main.transform.position = new Vector3(width, height, -30);
        // set camera size to fit the board
        Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, width / 2 + 1);
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
                    type = Cell.Type.Unknown,
                    secretTile = Cell.Type.Empty,
                };

                tab[x, y] = cell;

            }

        }
    }




    /// <summary>
    /// Generate the bombs.
    /// </summary>
    private void GenerateBombs()
    {
        int bomb = width*height/4;
        while (bomb>0)
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    if (!((int)mouseInWorld.x == w && (int)mouseInWorld.y == h))
                    {
                        if (bomb > 0)
                        {
                            if (probability())
                            {
                                tab[w, h].secretTile = Cell.Type.Bomb;
                                //Debug.Log("bomb at: " + h + " , " + w);
                                bomb--;
                            }
                        }
                    }
                }
            }

        }

    }

    private bool probability()
    {
        int taille = width * height;
        int randomNumber = Random.Range(0, taille);
        if (randomNumber <= 10)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Count the bombs
    /// </summary>
    /// <param name="cellX">The x position of the cell</param>
    /// <param name="cellY">The y position of the cell</param>
    /// <returns>The number of bombs</returns>
    private int CountBombs(int cellX, int cellY)
    {
        int bombCount = 0;
        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                if(adjacentX == cellX && adjacentY == cellY)
                {
                    continue;
                }

                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                if (IsInBounds(new Vector3Int(x, y, 0)))
                {
                    if (GetCellFromPosition(new Vector3Int(x, y, 0)).type == Cell.Type.Bomb)
                    {
                        bombCount++;
                    }
                }
            }
        }
        return bombCount;
    }


    private void HandleFirstCLick()
    {
        if (Input.GetMouseButtonDown(0) && gameStarted == false)
        {
            gameStarted = true;
            GenerateBombs();
            // GenerateBombs(Cell cell);
        }
        

    }


    /// <summary>
    /// Get the cell and modify the cell from the given position , a value and a type ( reaveled / flagged ).
    /// </summary>
    /// <param name="position">The position of the cell</param>
    /// /// <param name="type">The type of the modificaton</param>
    /// /// <param name="value">The new value of the cell </param>
    private void ModifyCell(bool value, int type, Vector3Int position)
    {
        if(IsInBounds(position)){
            if (type == 0)
            {
                tab[position.x, position.y].revealed = value;
            }
            else
            {
                tab[position.x, position.y].flagged = value;
            }
        }
    }



    /// <summary>
    /// Get the cell from the given position.
    /// </summary>
    /// <param name="position">The position of the cell</param>
    /// <returns>The cell at given position</returns>
    private Cell GetCellFromPosition(Vector3Int position)
    {
        if (!IsInBounds(position))
        {
            return new Cell();
        }
        else
        {
            return tab[position.x, position.y];
        }
    }

    /// <summary>
    /// Check if the given position is in bounds.
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <returns>True if the position is in bounds</returns>
    private bool IsInBounds(Vector3Int position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }
}
