using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Cell[,] state;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    // private readonly bool gameOver = false;
    // private readonly bool gameWon = false;
    private bool gameStarted = false;



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
        NewGame();
    }

    /// <summary>
    /// Create a new game according width and height.
    /// </summary>
    private void NewGame()
    {
        state = new Cell[width, height];
        GenerateTiles();
        board.Draw(state);
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
                };
                
                state[x, y] = cell;
            }
        }
    }

    

    private void GenerateBombs(Cell cell)
    {

    }

    private void HandleFirstCLick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameStarted = true;
            // GenerateBombs(Cell cell);
        }
    }

}
