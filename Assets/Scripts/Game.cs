
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Cell[,] tab;
    [SerializeField] private int width = 0;
    [SerializeField] private int height = 0;

    public Difficulty? currentDifficulty = null;
    private bool gameOver = false;
    private bool gameWon = false;
    private bool gameStarted = false;
    private bool radarUse = false;
    private Vector3Int radarpos;
    private int radarInitValue;
    private int radarActualValue;
    private int radarCountToGain;
    private int numberTileRevealed = 0;
    private bool canGainRadar = false;

    public enum Difficulty
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
    Vector3Int vect;
    Vector3 newPosition;

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
        SetDifficulty();
        NewGame();
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

    // Create Zoom function using mousepos in world
    /// <summary>
    /// Update function from Unity.
    /// </summary>
    void Update()
    {

        Vector3 mouseInScreen = Input.mousePosition;
        mouseInScreen.z = distanceFromCamera;
        mouseInWorld = Camera.main.ScreenToWorldPoint(mouseInScreen);
        mouseInWorld.x = (float)(mouseInWorld.x / 2.56);
        mouseInWorld.y = (float)(mouseInWorld.y / 2.56);

        // Moves
        HandleZoom();
        HandleMove();

        // Handle First click to start the game
        if (Input.GetMouseButtonDown(0) && gameStarted == false)
        {
            if (IsInBounds(mouseInWorld))
            {
                HandleFirstClick();
            }
        }
        // Usage of radar
        if (Input.GetKeyDown(KeyCode.I) && gameStarted == true && radarUse == false && GetRadarCount() > 0)
        {
            Vector3Int poscell = new((int)mouseInWorld.x, (int)mouseInWorld.y, 0);
            radarActualValue--;
            Radar(poscell);
        }
        // Reveal part
        if (Input.GetMouseButtonDown(0))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene("Menu/loseScene");
            }
            else if (mouseInWorld.x <= width && mouseInWorld.x > 0 && mouseInWorld.y <= width && mouseInWorld.y > 0)
            {
                if (radarUse == true)
                {
                    radarUse = false;
                    EndRadar();
                }

                /**/
                Vector3Int poscell = new((int)mouseInWorld.x, (int)mouseInWorld.y, 0);
                /**/
                ClickCleanAround(poscell);
                RevealTile(poscell);
                if (canGainRadar == true)
                {
                    if (GetNumberTileRevealed() != 0 && GetNumberTileRevealed() % radarCountToGain == 0)
                    {
                        GainRadar();
                    }
                }
            }
        }
        // Flag part
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
                        CheckWin();
                    }
                    else if (GetCellFromPosition(poscell).flagged == true)
                    {
                        ModifyCell(false, 1, poscell);
                        board.ChangeTile(new Vector3Int((int)mouseInWorld.x, (int)mouseInWorld.y, 0), board.TileUnknown);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Add +1 to the radar count.
    /// </summary>
    private void GainRadar()
    {
        radarActualValue++;
    }

    /// <summary>
    /// Get the actual count of the radar.
    /// </summary>
    /// <returns> The number of radar.</returns>
    private int GetRadarCount()
    {
        return radarActualValue;
    }

    /// <summary>
    /// Get the number of tile revealed.
    /// </summary>
    /// <returns> The number of tile revealed.</returns>
    private int GetNumberTileRevealed()
    {
        numberTileRevealed = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tab[i, j].revealed == true)
                {
                    numberTileRevealed++;
                }
            }
        }
        return numberTileRevealed;
    }

    /// <summary>
    /// Set the difficulty by switching in the Enum Difficulty.
    /// </summary>
    private void SetDifficulty()
    {
        switch (PlayerPrefs.GetInt("Difficulty", (int)Difficulty.Medium))
        {
            case 0:
                currentDifficulty = Difficulty.Easy;
                width = 10;
                height = 10;
                difficulty = 6;
                difficulty = 5;
                radarInitValue = 4;
                canGainRadar = false;
                break;
            case 1:
                currentDifficulty = Difficulty.Medium;
                width = 20;
                height = 20;
                difficulty = 4;
                radarInitValue = 3;
                canGainRadar = false;
                break;
            case 2:
                currentDifficulty = Difficulty.Hard;
                width = 30;
                height = 30;
                difficulty = 3;
                radarInitValue = 2;
                radarCountToGain = 40;
                radarActualValue = radarInitValue; // Set actual value to the initial value at the start of the game
                canGainRadar = true;
                break;
            case 3:
                currentDifficulty = Difficulty.Madness;
                width = 50;
                height = 50;
                difficulty = 2;
                radarInitValue = 1;
                radarCountToGain = 25;
                radarActualValue = radarInitValue; // Set actual value to the initial value at the start of the game
                canGainRadar = true;
                break;
        }
    }

    /// <summary>
    /// Handle the zoom feature.
    /// </summary>
    private void HandleZoom()
    {
        int maxZoom = 5;
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            // zoom to mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 zoomPos = Camera.main.transform.position + ((mousePos - Camera.main.transform.position) * 0.1f);
            Camera.main.transform.position = zoomPos;

            // increase to max zoom 
            if (Camera.main.orthographicSize != maxZoom)
            {
                Camera.main.orthographicSize -= 1;
            }




            // Camera.main.orthographicSize -= 1;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            // zoom to mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 zoomPos = Camera.main.transform.position + ((mousePos - Camera.main.transform.position) * 0.1f);
            Camera.main.transform.position = zoomPos;

            if (Camera.main.orthographicSize != maxZoom - 1)
                Camera.main.orthographicSize += 1;
        }
    }

    /// <summary>
    /// Handle the move feature with keys.
    /// </summary>
    private void HandleMove()
    {
        float offset = 0.1f;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.position += new Vector3(0, offset, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.position += new Vector3(0, -offset, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.position += new Vector3(-offset, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.position += new Vector3(offset, 0, 0);
        }
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

    /// <summary>
    /// Reveal the tiles.
    /// </summary>
    /// <param name="poscell">The position of the tile.</param>
    private void RevealTile(Vector3Int poscell)
    {
        if (GetCellFromPosition(poscell).flagged == false && GetCellFromPosition(poscell).revealed == false)
        {

            ModifyCell(true, 0, poscell);
            if (GetCellFromPosition(poscell).secretTile == Cell.Type.Empty)
            {
                board.ChangeTile(new Vector3Int(poscell.x, poscell.y, 0), board.TileRevealed);
                DestroyEmptyCase(poscell);
            }
            else if (GetCellFromPosition(poscell).secretTile == Cell.Type.Bomb)
            {
                board.ChangeTile(new Vector3Int(poscell.x, poscell.y, 0), board.TileBomb);
                RevealAllBombs();
                Explode(GetCellFromPosition(poscell));
            }
            else if (GetCellFromPosition(poscell).secretTile == Cell.Type.Number)
            {
                int number = GetCellFromPosition(poscell).number;
                if (number == 1)
                {
                    board.ChangeTile(new Vector3Int(poscell.x, poscell.y, 0), board.TileNumber1);
                }
                if (number == 2)
                {
                    board.ChangeTile(new Vector3Int(poscell.x, poscell.y, 0), board.TileNumber2);
                }
                if (number == 3)
                {
                    board.ChangeTile(new Vector3Int(poscell.x, poscell.y, 0), board.TileNumber3);
                }
                if (number == 4)
                {
                    board.ChangeTile(new Vector3Int(poscell.x, poscell.y, 0), board.TileNumber4);
                }
                if (number == 5)
                {
                    board.ChangeTile(new Vector3Int(poscell.x, poscell.y, 0), board.TileNumber5);
                }
                if (number == 6)
                {
                    board.ChangeTile(new Vector3Int(poscell.x, poscell.y, 0), board.TileNumber6);
                }
                if (number == 7)
                {
                    board.ChangeTile(new Vector3Int(poscell.x, poscell.y, 0), board.TileNumber7);
                }
                if (number == 8)
                {
                    board.ChangeTile(new Vector3Int(poscell.x, poscell.y, 0), board.TileNumber8);
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

    /// <summary>
    /// Destroy the empty cases.
    /// </summary>
    /// <param name="position">The position where we destroy</param>
    private void DestroyEmptyCase(Vector3Int position)
    {

        for (int loop = 0; loop < 8; loop++)
        {

            switch (loop)
            {
                case 0:
                    vect = new Vector3Int(position.x - 1, position.y - 1, 0);
                    break;
                case 1:
                    vect = new Vector3Int(position.x - 1, position.y, 0);
                    break;
                case 2:
                    vect = new Vector3Int(position.x - 1, position.y + 1, 0);
                    break;
                case 3:
                    vect = new Vector3Int(position.x, position.y - 1, 0);
                    break;
                case 4:
                    vect = new Vector3Int(position.x, position.y + 1, 0);
                    break;
                case 5:
                    vect = new Vector3Int(position.x + 1, position.y - 1, 0);
                    break;
                case 6:
                    vect = new Vector3Int(position.x + 1, position.y, 0);
                    break;
                case 7:
                    vect = new Vector3Int(position.x + 1, position.y + 1, 0);
                    break;
                default:
                    break;
            }
            if (GetCellFromPosition(vect).revealed == false && IsInBounds(vect))
            {
                RevealTile(vect);
            }
        }
    }



    /// <summary>
    /// Generate the bombs.
    /// </summary>
    private void GenerateBombs()
    {
        int bomb = width * height / difficulty;
        while (bomb > 0)
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
                                bomb--;
                            }
                        }
                    }
                }
            }

        }

    }

    /// <summary>
    /// Look around the position for the number of bombs around.
    /// </summary>
    /// <param name="x">The x position</param>
    /// <param name="y">The y position</param>
    /// <returns>The number of bombs around</returns>
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
    /// <summary>
    /// Generate the numbers.
    /// </summary>
    private void GenerateNumbers()
    {

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                if (tab[w, h].secretTile != Cell.Type.Bomb)
                {

                    int nombre = LookAround(w, h);

                    switch (nombre)
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


    /// <summary>
    /// The probability to generate bombs.
    /// </summary>
    /// <returns>True if the bomb is generated</returns>
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
    private int CountBombs()
    {
        int bombCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = -1; y < height; y++)
            {
                if (GetCellFromPosition(new Vector3Int(x, y, 0)).secretTile == Cell.Type.Bomb)
                {
                    bombCount++;
                }
            }
        }
        return bombCount;
    }
    // private int GetRemainingFlaggs()
    // {
    //     return CountBombs() - FlaggedBombs();
    // }


    /// <summary>
    /// Handle the first click and call GenerateBombs after first click.
    /// </summary>
    private void HandleFirstClick()
    {
        gameStarted = true;
        GenerateBombs();
        GenerateNumbers();
    }

    // private void HandleOneClick()
    // {
    //     if()
    // }
    /// <summary>
    /// Get the cell and modify the cell from the given position , a value and a type ( reaveled / flagged ).
    /// </summary>
    /// <param name="value">The new value of the cell </param>
    /// <param name="type">The type of the modificaton</param>
    /// <param name="position">The position of the cell</param>
    private void ModifyCell(bool value, int type, Vector3Int position)
    {
        if (IsInBounds(position))
        {
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
    private bool IsInBounds(Vector3 position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    /// <summary>
    /// Make the bomb goes BOOOOOOOM!.
    /// </summary>
    /// <param name="cell">The cell that exploded</param>
    private void Explode(Cell cell)
    {
        RevealTile(GetCellFromPosition(new Vector3Int(cell.position.x, cell.position.y, 0)).position);

        gameOver = true;
        Debug.LogWarning("BOOM !");
    }


    /// <summary>
    /// Reveal all the bombs on the map.
    private void RevealAllBombs()
    {
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                if (tab[w, h].secretTile == Cell.Type.Bomb)
                {
                    board.ChangeTile(new Vector3Int(tab[w, h].position.x, tab[w, h].position.y, 0), board.TileBomb);
                }
            }
        }
    }

    /// <summary>
    /// check around the tile and reveal the bomb.
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <returns></returns>
    private void Radar(Vector3Int position)
    {

        radarpos = position;
        radarUse = true;

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {

                Vector3Int vect = new(position.x + x, position.y + y, 0);

                if (IsInBounds(vect))
                {
                    if (GetCellFromPosition(vect).revealed == false)
                    {
                        OpacityTile(vect);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check if the player won.
    /// </summary>
    private void CheckWin()
    {
        // Debug.LogWarning("CheckWin");
        // Debug.LogWarning("CountBombs : " + CountBombs());
        // Debug.LogWarning("GetRemainingUnknownTiles : " + GetRemainingUnknownTiles());
        if (CountBombs() == GetRemainingUnknownTiles())
        {
            gameWon = true;
            Debug.LogWarning("You won !");
            SceneManager.LoadScene("Menu/winScene");
        }
    }
    /// <summary>
    /// Reveal if a tile is a bomb or another tile.
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <returns></returns>
    private void OpacityTile(Vector3Int position)
    {
        switch (GetCellFromPosition(position).secretTile)
        {
            case Cell.Type.Bomb:
                board.ChangeTile(new Vector3Int(position.x, position.y, 0), board.TileBomb);
                break;
            default:

                board.ChangeTile(new Vector3Int(position.x, position.y, 0), board.TileEmpty);
                break;
        }
    }

    /// <summary>
    /// if a reaveled tiled is clicked and revealed the tile around if name='VerifyBombFlaggedAround' is verified .
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <returns>The number of remaining unknown tiles</returns>
    private void ClickCleanAround(Vector3Int position)
    {
        if (GetCellFromPosition(position).revealed == true)
        {
            if (VerifyBombFlaggedAround(position))
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        Vector3Int vect = new(position.x + x, position.y + y, 0);

                        if (IsInBounds(vect))
                        {
                            if (GetCellFromPosition(vect).revealed == false)
                            {
                                RevealTile(vect);
                            }
                        }
                    }
                }
            }

        }
    }

    /// <summary>
    /// check if number of flag is equal at the tile number in position.
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <returns>The number of remaining unknown tiles</returns>
    private bool VerifyBombFlaggedAround(Vector3Int position)
    {
        int number = GetCellFromPosition(position).number;
        int flagged = 0;
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                Vector3Int vect = new(position.x + x, position.y + y, 0);
                if (IsInBounds(vect))
                {

                    if (GetCellFromPosition(vect).flagged == true)
                    {
                        flagged++;
                    }

                }
            }
        }
        return number == flagged;
    }

    /// <summary>
    /// Get the remaining unknown tiles.
    /// </summary>
    /// <returns>The number of remaining unknown tiles</returns>
    private int GetRemainingUnknownTiles()
    {
        int count = 0;
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                Cell cell = tab[w, h];
                if (cell.revealed == false)
                {
                    count++;
                }
            }
        }
        return count;
    }
    /// <summary>
    /// after use the radar hide the tile was not revealed  .
    /// </summary>
    /// <returns></returns>
    private void EndRadar()
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                Vector3Int vect = new(radarpos.x + x, radarpos.y + y, 0);
                if (IsInBounds(vect))
                {
                    if (GetCellFromPosition(vect).revealed == false)
                    {
                        UnOpacityTile(vect);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Unreveal the tiles that have been showed by the radar .
    /// </summary>
    /// <param name="position">The position to check</param>
    private void UnOpacityTile(Vector3Int position)
    {
        board.ChangeTile(new Vector3Int(position.x, position.y, 0), board.TileUnknown);
    }
}
