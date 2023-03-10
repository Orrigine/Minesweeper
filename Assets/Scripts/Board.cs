using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.UI.Image;

public class Board : MonoBehaviour
{

    // Start is called before the first frame update
    public Tilemap tilemap { get; private set; }

    // public Tile[,] board { get; private set; }
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
    public Tile TileNumber6;
    public Tile TileNumber7;
    public Tile TileNumber8;


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
    /// <param name="board">The board of tile (bomb, unknown etc...</param>
    public void Draw(Cell[,] board)
    {
        int width = board.GetLength(0);
        int height = board.GetLength(1);
        //
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = board[x, y];
                tilemap.SetTile(cell.position, GetTile(cell.position, cell));
            }
        }
    }
    /// <summary>
    /// Get the tile.
    /// </summary>
    /// <param name="cell">The instance of the cell</param>
    /// <returns>The type of the cell</returns>
    private Tile GetTile(Vector3Int position, Cell cell)
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
        return cell.type switch
        {
            Cell.Type.Empty => TileEmpty,
            Cell.Type.Bomb => TileBomb,
            Cell.Type.Number => cell.number switch
            {
                1 => TileNumber1,
                2 => TileNumber2,
                3 => TileNumber3,
                4 => TileNumber4,
                5 => TileNumber5,
                _ => TileNumber,
            },
            _ => TileUnknown,
        };
    }
    /// <summary>
    /// Get the number of bombs around the tile.
    /// </summary>
    /// <param name="number">The number of bombs around the tile</param>
    /// <returns>The tile type with the number of bombs around it</returns>
    private Tile GetNumberTile(int number)
    {
        return number switch
        {
            1 => TileNumber1,
            2 => TileNumber2,
            3 => TileNumber3,
            4 => TileNumber4,
            5 => TileNumber5,
            6 => TileNumber6,
            7 => TileNumber7,
            8 => TileNumber8,
            _ => TileNumber,
        };
    }

    /// <summary>
    /// Change the tile at a given position with the given tile.
    /// </summary>
    /// <param name="position">The position of the tile.</param>
    /// <param name="tile">The new tile to add.</param>
    public void ChangeTile(Vector3Int position, Tile tile)
    {
        tilemap.SetTile(position, tile);
    }
}
