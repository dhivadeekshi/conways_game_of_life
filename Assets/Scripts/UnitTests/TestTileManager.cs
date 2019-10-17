using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTileManager
{
    public static void Execute()
    {
        Debug.Log("Unit test TestTileManager started");
        string errorMessage = string.Empty;
        TestTileManagerFunctions(6, 6, out errorMessage);
        if (errorMessage != "") Debug.Log(errorMessage);
        TestTileManagerFunctions(6, 7, out errorMessage);
        if (errorMessage != "") Debug.Log(errorMessage);
        TestTileManagerFunctions(7, 6, out errorMessage);
        if (errorMessage != "") Debug.Log(errorMessage);
        Debug.Log("Unit test TestTileManager ended");
    }

    private static bool TestTileManagerFunctions(int rows, int cols, out string error)
    {
        error = string.Empty;
        string prefixMessage = "TestTileManagerFunctions for [" + rows + "," + cols + "]\n";
        TileManager tileManager = new TileManager(rows, cols);

        int index = 0;
        // Check Tile Index from Location
        for (int i = 1; i <= cols; i++)
        {
            for (int j = 1; j <= rows; j++)
            {
                if (index != tileManager.GetTileIndexFor(new TileLocation(j, i)))
                {
                    error = prefixMessage
                        + " GetTileIndexFor " + new TileLocation(j, i) + " failed"
                        + " ActualIndex = " + index
                        + " ReceivedIndex = " + tileManager.GetTileIndexFor(new TileLocation(j, i)) + "\n";
                }
                index++;
            }
        }

        // Check Tile Location from Index
        index = rows * cols;
        int row = 1;
        int col = 1;
        TileLocation location;
        for (int i = 0; i < index; i++)
        {
            location = tileManager.GetTileLocationFromIndex(i);
            if (!location.Equals(new TileLocation(row, col)))
            {
                error = prefixMessage
                    + " GetTileLocationFromIndex " + i + " failed"
                    + " ActualLocation = " + new TileLocation(row, col)
                    + " ReceivedIndex = " + location + "\n";
            }
            if (++row > rows)
            {
                row = 1;
                col++;
            }
        }

        // Check Neighbors Location


        // Check Neighbors Index


        return true;
    }
}
