using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardOfLife : MonoBehaviour {

    public List<Tile> tilesOfLife = new List<Tile>();

    public void CreateBoard(int noOfColumns, int noOfRows, Vector2 tileSize, float tileGap = 0f)
    {
        GameObject colObject;
        Vector3 tilePosition = GetStartingTilePosition(noOfColumns, noOfRows, tileSize, tileGap);
        Debug.Log(tilePosition);

        for (int col = 0; col < noOfColumns; col++)
        {
            colObject = new GameObject("Col_" + col);
            colObject.transform.SetParent(transform);

            for (int row = 0; row < noOfRows; row++)
            {
                tilesOfLife.Add(CreateTileAt(tilePosition, colObject.transform));
                tilePosition += Vector3.forward * (tileSize.x + tileGap);
            }

            tilePosition -= Vector3.forward * (tileSize.y + tileGap) * noOfRows; // Resetting x Position
            tilePosition += Vector3.right * (tileSize.y + tileGap);
        }
    }

    private Vector3 GetStartingTilePosition(float cols, float rows, Vector2 tileSize, float tileGap)
    {
        float halfRow = rows / 2f;
        float halfCol = cols / 2f;
        return Vector3.zero
            - (Vector3.forward * (halfRow - 0.5f) * (tileGap + tileSize.x))
            - (Vector3.right * (halfCol - 0.5f) * (tileGap + tileSize.y));
    }

    private Tile CreateTileAt(Vector3 position, Transform parent)
    {
        GameObject tileObject = PoolManager.Instance.GetItemFromPool(Constants.POOL_NAME_TILE);
        tileObject.transform.SetParent(parent);
        tileObject.transform.position = position;
        tileObject.SetActive(true);
        return tileObject.GetComponent<Tile>();
    }

    public void PopulateBoard(List<TileLocation> initialLife)
    {

    }

    public List<TileLocation> GetNeighbourTilesFor(TileLocation tileLocation)
    {
        return new List<TileLocation>();
    }


}
