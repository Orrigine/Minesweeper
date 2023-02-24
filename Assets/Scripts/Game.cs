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

        if (Input.GetMouseButtonDown(0))
        {
            if (mouseInWorld.x <= width && mouseInWorld.x > 0 && mouseInWorld.y <= width && mouseInWorld.y > 0)
            {
                Cell cell = state[(int)mouseInWorld.x, (int)mouseInWorld.y];
                Debug.Log(cell.flagged);
                if (cell.flagged == false && cell.revealed == false)
                {
                    cell.revealed = true;
                    board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileRevealed);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (mouseInWorld.x <= width && mouseInWorld.x > 0 && mouseInWorld.y <= width && mouseInWorld.y > 0)
            {
                Cell cell = state[(int)mouseInWorld.x, (int)mouseInWorld.y];

                Debug.Log(state[(int)mouseInWorld.x, (int)mouseInWorld.y].flagged);
                if (state[(int)mouseInWorld.x, (int)mouseInWorld.y].revealed == false)
                {
                    if (state[(int)mouseInWorld.x, (int)mouseInWorld.y].flagged == false)
                    {
                        state[(int)mouseInWorld.x, (int)mouseInWorld.y].flagged = true;
                        Debug.Log(state[(int)mouseInWorld.x, (int)mouseInWorld.y].flagged);
                        Debug.LogWarning("Put flag pos:" + state[(int)mouseInWorld.x, (int)mouseInWorld.y].position);
                        board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileFlag);
                    }else if (state[(int)mouseInWorld.x, (int)mouseInWorld.y].flagged == true)
                    {
                        state[(int)mouseInWorld.x, (int)mouseInWorld.y].flagged = false;
                        Debug.LogWarning("unput flag" + state[(int)mouseInWorld.x, (int)mouseInWorld.y].position);
                        board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileUnknown);
                    }
                }
                Debug.Log(cell.flagged);
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
                };

                tab[x, y] = cell;
            }

        }
    }




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

    private void HandleFirstCLick()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            gameStarted = true;
            // GenerateBombs(Cell cell);
        }*/
    }

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

    private bool IsInBounds(Vector3Int position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }
}
