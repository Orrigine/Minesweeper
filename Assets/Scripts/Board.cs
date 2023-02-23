using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap tilemap { get; private set; }

    public Tile[,] State { get; private set; }
    public Tile TileEmpty;
    public Tile TileUnknown;
    public Tile TileBomb;
    public Tile TileNumber;
    public Tile TileFlag;
    public Tile TileExploded;
    public Tile TileRevealed;
    public Tile TileNumber1;
    public Tile TileNumber2;
    public Tile TileNumber3;
    public Tile TileNumber4;
    public Tile TileNumber5;


    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    /// <summary>
    /// Draw the Board with all the Tiles in.
    /// </summary>
    /// <param name="state">The texture containing all animations</param>
    /// <param name="frameSize">The size of one frame, in pixels</param>
    /// <param name="framesPerAnimation">Number of frames per animations</param>
    /// <param name="delayBetweenFrames">Delay in game frame between each animation frame</param>
    public void Draw(Cell[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);
        // 
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];
                tilemap.SetTile(cell.position, GetTile(cell));
            }
        }
    }

    private Tile GetTile(Cell cell)
    {
        if (cell.revealed) { return GetRevealedTile(cell); }
        else if (cell.flagged) { return TileFlag; }
        else if (cell.exploded) { return TileExploded; }
        else if (cell.type == Cell.Type.Empty) { return TileEmpty; }
        else if (cell.type == Cell.Type.Bomb) { return TileBomb; }
        // else if (tile.type == Tile.Type.Number)
        // {

        // }
        else { return TileUnknown; }
    }

    private Tile GetRevealedTile(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Empty:
                return TileEmpty;
            case Cell.Type.Bomb:
                return TileBomb;
            case Cell.Type.Number:
                switch (cell.number)
                {
                    case 1:
                        return TileNumber1;
                    case 2:
                        return TileNumber2;
                    case 3:
                        return TileNumber3;
                    case 4:
                        return TileNumber4;
                    case 5:
                        return TileNumber5;
                    default:
                        return TileNumber;
                }
            default:
                return TileUnknown;
        }
    }

    private Tile GetNumberTile(int number)
    {
        switch (number)
        {
            case 1:
                return TileNumber1;
            case 2:
                return TileNumber2;
            case 3:
                return TileNumber3;
            case 4:
                return TileNumber4;
            case 5:
                return TileNumber5;
            default:
                return TileNumber;
        }
    }

    [SerializeField] GameObject[,] Tilemap = new GameObject[10, 10];
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
}
