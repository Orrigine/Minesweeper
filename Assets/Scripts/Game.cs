using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Cell[,] tab;
    [SerializeField]  int width = 0;
    [SerializeField]  int height = 0;
    
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
        currentDifficulty = Difficulty.Hard;
        SetDifficulty();
        NewGame();
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
                break;
            case Difficulty.Medium:
                width = 25;
                height = 25;
                break;
            case Difficulty.Hard:
                width = 40;
                height = 40;
                break;
            case Difficulty.Madness:
                width = 69;
                height = 69;
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
        Camera.main.transform.position = new Vector3((width*0.5f) + 7, (height*0.5f) + 8, -30);
        
        // set camera size to fit the board
        Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, width + height );
        // Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, height );
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
                };
                
                tab[x, y] = cell;
            }
        }
    }

    

    /// <summary>
    /// Generate the bombs.
    /// </summary>
    private void GenerateBombs(Cell cell)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // if bomb is on clicked cell
                // {
                //      NO BOMB
                // }
                tab[x, y].type = Random.Range(0, 100) < 20 ? Cell.Type.Bomb : Cell.Type.Empty;
            }
        }
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
        if (Input.GetMouseButtonDown(0))
        {
            gameStarted = true;
            // GenerateBombs(Cell cell);
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
