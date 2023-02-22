using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap tilemap { get; private set; }

    public Tile[,] state { get; private set; }
    public Tile tileEmpty { get; private set; }
    public Tile tileUnknown { get; private set; }
    public Tile tileBomb { get; private set; }
    public Tile tileNumber { get; private set; }
    public Tile tileFlag { get; private set; }
    public Tile tileExploded { get; private set; }
    public Tile tileRevealed { get; private set; }
    public Tile tileNumber1 { get; private set; }
    public Tile tileNumber2 { get; private set; }
    public Tile tileNumber3 { get; private set; }
    public Tile tileNumber4 { get; private set; }
    public Tile tileNumber5 { get; private set; }


    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }
    public void DrawTile(Tile[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        // 
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = state[x, y];
                // tilemap.SetTile(tile.position, GetTile(tile));
            }
        }
    }

    private Tile GetTile(Tile tile)
    {
        if (tile.revealed) { return tileRevealed; }
        else if (tile.flagged) { return tileFlag; }
        else if (tile.exploded) { return tileExploded; }
        else if (tile.type == Tile.Type.Empty) { return tileEmpty; }
        else if (tile.type == Tile.Type.Bomb) { return tileBomb; }
        // else if (tile.type == Tile.Type.Number)
        // {
           
        // }
        else { return tileUnknown; }
    }

    private Tile GetRevealedTile(Tile tile)
    {
        switch (tile.type)
        {
            case Tile.Type.Empty:
                return tileEmpty;
            case Tile.Type.Bomb:
                return tileBomb;
            case Tile.Type.Number:
                switch (tile.number)
                {
                    case 1:
                        return tileNumber1;
                    case 2:
                        return tileNumber2;
                    case 3:
                        return tileNumber3;
                    case 4:
                        return tileNumber4;
                    case 5:
                        return tileNumber5;
                    default:
                        return tileNumber;
                }
            default:
                return tileUnknown;
        }
    }

    private Tile GetNumberTile(int number)
    {
        switch (number)
        {
            case 1:
                return tileNumber1;
            case 2:
                return tileNumber2;
            case 3:
                return tileNumber3;
            case 4:
                return tileNumber4;
            case 5:
                return tileNumber5;
            default:
                return tileNumber;
        }
    }
    [SerializeField] GameObject Original;

    [SerializeField] GameObject[,] Tilemap = new GameObject[10, 10];
    // 
    void InitTilemap()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Instantiate(Original, new Vector2(i, j), Quaternion.identity);
            }
        }
    }
    void Start()
    {
        InitTilemap();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Time.timeSinceLevelLoad > spawnRate)
        // {
        //     Instantiate(Original, new Vector2(2, -2), Quaternion.identity);
        //     spawnRate += 1f;
        // }
    }
}
