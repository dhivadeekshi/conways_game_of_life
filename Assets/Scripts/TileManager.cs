using System.Collections.Generic;

public class TileManager {

    private int noOfRows = 0;
    private int noOfCols = 0;

    public TileManager(int noOfRows, int noOfCols)
    {
        this.noOfRows = noOfRows;
        this.noOfCols = noOfCols;
    }

    public List<TileLocation> GetNeighbourTilesLocationFor(TileLocation tileLocation)
    {
        List<TileLocation> neighbours = new List<TileLocation>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                if (tileLocation.row + i >= 1 && tileLocation.col + j >= 1
                    && tileLocation.row + i <= noOfRows && tileLocation.col + j <= noOfCols)
                {
                    neighbours.Add(new TileLocation(tileLocation.row + i, tileLocation.col + j));
                }
            }
        }

        return neighbours;
    }

    public List<int> GetNeighbourTilesIndexFor(TileLocation tileLocation)
    {
        List<TileLocation> neighbourLocations = GetNeighbourTilesLocationFor(tileLocation);
        List<int> neighbours = new List<int>();
        foreach (var neighbour in neighbourLocations)
        {
            neighbours.Add(GetTileIndexFor(neighbour));
        }
        return neighbours;
    }


    /// <summary>
    /// Function to fetch the tile index from tile location
    /// The index starts from 0
    /// The row/col starts from 1
    /// </summary>
    /// <param name="tileLocation"></param>
    /// <returns>Tile Index</returns>
    public int GetTileIndexFor(TileLocation tileLocation)
    {
        return ((tileLocation.col - 1) * noOfRows) + (tileLocation.row - 1);
    }

    /// <summary>
    /// Function to fetch the tile location from tile index
    /// The index starts from 0
    /// The row/col starts from 1
    /// </summary>
    /// <param name="index"></param>
    /// <returns>Tile location</returns>
    public TileLocation GetTileLocationFromIndex(int index)
    {
        int row = (index % noOfRows) + 1;
        int col = (index / noOfRows) + 1;
        return new TileLocation(row, col);
    }
}
