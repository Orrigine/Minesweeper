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

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    /// <summary>
    /// Draw the Board with all the Tiles in.
    /// </summary>
    /// <param name="state">The state of tile (bomb, unknown etc...</param>
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
    /// <summary>
    /// Get the tile.
    /// </summary>
    /// <param name="cell">The instance of the cell</param>
    /// <returns>The type of the cell</returns>
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

    /// <summary>
    /// Get the revealed tile.
    /// </summary>
    /// <param name="cell">The instance of the cell</param>
    /// <returns>The type of the revealed cell</returns>
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
    /// <summary>
    /// Get the number of bombs around the tile.
    /// </summary>
    /// <param name="number">The number of bombs around the tile</param>
    /// <returns>The tile type with the number of bombs around it</returns>
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
    
    /// <summary>
    /// Change the tile at a given position with the given tile.
    /// </summary>
    /// <param name="number">The number of bombs around the tile</param>
    public void ChangeTile(Vector3Int position, Tile tile)
    {
        tilemap.SetTile(position, tile);
    }
}
