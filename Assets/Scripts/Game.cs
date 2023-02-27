using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.Events;
using VSCodeEditor;
using UnityEngine.UIElements;
using System.Linq.Expressions;

public class Game : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Cell[,] tab;
    [SerializeField] private int width = 0;
    [SerializeField] private int height = 0;


    [SerializeField] private Difficulty? currentDifficulty = null;
    // private readonly bool gameOver = false;
    // private readonly bool gameWon = false;
    private bool gameStarted = false;

    private enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Madness,
    }
    UnityEvent m_event;
    public float distanceFromCamera = 10;
    public Vector3 mouseInWorld = new();
    private int difficulty;

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
        m_event ??= new UnityEvent();
        currentDifficulty = Difficulty.Easy;
        SetDifficulty();
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

        HandleFirstClick();


        if (Input.GetMouseButtonDown(0))
        {

            if (mouseInWorld.x <= width && mouseInWorld.x > 0 && mouseInWorld.y <= width && mouseInWorld.y > 0)
            {
                /**/
                Vector3Int poscell = new((int)mouseInWorld.x, (int)mouseInWorld.y, 0);
                /**/
                RevealTile(poscell);

            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (mouseInWorld.x <= width && mouseInWorld.x > 0 && mouseInWorld.y <= width && mouseInWorld.y > 0)
            {

                /**/
                Vector3Int poscell = new((int)mouseInWorld.x, (int)mouseInWorld.y, 0);
                /**/


                if (GetCellFromPosition(poscell).revealed == false)
                {
                    if (GetCellFromPosition(poscell).flagged == false)
                    {
                        ModifyCell(true, 1, poscell);
                        board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileFlag);
                    } else if (GetCellFromPosition(poscell).flagged == true)
                    {
                        ModifyCell(false, 1, poscell);
                        board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileUnknown);
                    }
                }
            }

        }

    }


    /// <summary>
    /// Set the difficulty by switching in the Enum Difficulty.
    /// </summary>
    private void SetDifficulty()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                width = 10;
                height = 10;
                difficulty = 4;
                break;
            case Difficulty.Medium:
                width = 25;
                height = 25;
                difficulty = 4;
                break;
            case Difficulty.Hard:
                width = 40;
                height = 40;
                difficulty = 3;
                break;
            case Difficulty.Madness:
                width = 69;
                height = 69;
                difficulty = 2;
                break;
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
        SetCameraPosition();
        SetCameraSize();
    }


    /// <summary>
    /// Set the camera position. 
    /// 0.5f because the camera is in the center of the board.
    /// 2.56f because the size of the tile is 2.56f.
    /// </summary>
    private void SetCameraPosition()
    {
        Camera.main.transform.position = new Vector3((int)(width * 2.56f) * 0.5f, (int)(height * 2.56f) * 0.5f, -30);
    }

    /// <summary>
    /// Set the camera size.
    /// Scaled to the width and height of the board.
    /// </summary>
    private void SetCameraSize()
    {
        Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, width + (int)(width * 0.3f));
    }

    private void RevealTile(Vector3Int poscell)
    {
        if (GetCellFromPosition(poscell).flagged == false && GetCellFromPosition(poscell).revealed == false)
        {
            ModifyCell(true, 0, poscell);
            if (GetCellFromPosition(poscell).secretTile == Cell.Type.Empty)
            {
                board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileRevealed);
                DestroyEmptyCase(poscell);
            }
            else if (GetCellFromPosition(poscell).secretTile == Cell.Type.Bomb)
            {
                board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileBomb);
                Debug.LogWarning("Macron explosion");
            }
            else if (GetCellFromPosition(poscell).secretTile == Cell.Type.Number)
            {
                int nombre = GetCellFromPosition(poscell).number;
                if (nombre == 1)
                {
                    board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileNumber1);
                }
                if (nombre == 2)
                {
                    board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileNumber2);
                }
                if (nombre == 3)
                {
                    board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileNumber3);
                }
                if (nombre == 4)
                {
                    board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileNumber4);
                }
                if (nombre == 5)
                {
                    board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileNumber5);
                }
                if (nombre == 6)
                {
                    board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileNumber6);
                }
                if (nombre == 7)
                {
                    board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileNumber7);
                }
                if (nombre == 8)
                {
                    board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileNumber8);
                }
            }

        }
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


    private void DestroyEmptyCase(Vector3Int position)
    {
        for (int loop = 0; loop < 9; loop++)
        {
            if (GetCellFromPosition(vect).secretTile == Cell.Type.Empty)
            {
                if (GetCellFromPosition(vect).revealed == false)
                {
                    RevealTile(vect);
                    DestroyEmptyCase(vect);
                }
            }
        }
    }



    /// <summary>
    /// Generate the bombs.
    /// </summary>
    private void GenerateBombs()
    {
        int bomb = width*height/ difficulty;
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
                            if (Probability())
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

    private int LookAround(int x, int y)
    {
        int numberBombAround = 0;
        Vector3Int position = new(x, y, 0);
        if (GetCellFromPosition(new Vector3Int(position.x - 1, position.y - 1, 0)).secretTile == Cell.Type.Bomb) { numberBombAround++; }
        if (GetCellFromPosition(new Vector3Int(position.x - 1, position.y, 0)).secretTile == Cell.Type.Bomb) { numberBombAround++; }
        if (GetCellFromPosition(new Vector3Int(position.x - 1, position.y + 1, 0)).secretTile == Cell.Type.Bomb) { numberBombAround++; }
        if (GetCellFromPosition(new Vector3Int(position.x, position.y - 1, 0)).secretTile == Cell.Type.Bomb) { numberBombAround++; }
        if (GetCellFromPosition(new Vector3Int(position.x, position.y + 1, 0)).secretTile == Cell.Type.Bomb) { numberBombAround++; }
        if (GetCellFromPosition(new Vector3Int(position.x + 1, position.y - 1, 0)).secretTile == Cell.Type.Bomb) { numberBombAround++; }
        if (GetCellFromPosition(new Vector3Int(position.x + 1, position.y, 0)).secretTile == Cell.Type.Bomb) { numberBombAround++; }
        if (GetCellFromPosition(new Vector3Int(position.x + 1, position.y + 1, 0)).secretTile == Cell.Type.Bomb) { numberBombAround++; }


        return numberBombAround;
    }
    private void GenerateNumbers()
    {

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                if (tab[w, h].secretTile != Cell.Type.Bomb)
                {

                    int nombre = LookAround(w,h);

                    switch(nombre)
                    {
                        case 1:
                            tab[w, h].secretTile = Cell.Type.Number;
                            tab[w, h].number = nombre;
                            break;
                        case 2:
                            tab[w, h].secretTile = Cell.Type.Number;
                            tab[w, h].number = nombre;
                            break;
                        case 3:
                            tab[w, h].secretTile = Cell.Type.Number;
                            tab[w, h].number = nombre;
                            break;
                        case 4:
                            tab[w, h].secretTile = Cell.Type.Number;
                            tab[w, h].number = nombre;
                            break;
                        case 5:
                            tab[w, h].secretTile = Cell.Type.Number;
                            tab[w, h].number = nombre;
                            break;
                        case 6:
                            tab[w, h].secretTile = Cell.Type.Number;
                            tab[w, h].number = nombre;
                            break;
                        case 7:
                            tab[w, h].secretTile = Cell.Type.Number;
                            tab[w, h].number = nombre;
                            break;
                        case 8:
                            tab[w, h].secretTile = Cell.Type.Number;
                            tab[w, h].number = nombre;
                            break;
                        default:
                            break;
                    }

                }

            }
        }
    }



    private bool Probability()
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


    private void HandleFirstClick()
    {
        if (Input.GetMouseButtonDown(0) && gameStarted == false)
        {
            gameStarted = true;
            GenerateBombs();
            GenerateNumbers();
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
