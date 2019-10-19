using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardOfLife : MonoBehaviour {

    public TileManager tileManager { get; private set; }
    private List<Tile> tilesOfLife = new List<Tile>();

    public bool IsBoardCreated { get; private set; }

    void Awake()
    {
        IsBoardCreated = false;
    }

    public void CreateBoard(int noOfCols, int noOfRows, Vector2 tileSize, float tileGap = 0f)
    {
        tileManager = new TileManager(noOfRows, noOfCols);
        StartCoroutine(CreateTiles(tileSize, tileGap));
    }

    IEnumerator CreateTiles(Vector2 tileSize, float tileGap)
    {
        GameObject colObject;
        Vector3 tilePosition = GetStartingTilePosition(tileManager.NoOfCols, tileManager.NoOfRows, tileSize, tileGap);

        for (int col = 0; col < tileManager.NoOfCols; col++)
        {
            colObject = new GameObject("Col_" + col);
            colObject.transform.SetParent(transform);

            for (int row = 0; row < tileManager.NoOfRows; row++)
            {
                tilesOfLife.Add(CreateTileAt(tilePosition, colObject.transform));
                tilePosition += Vector3.forward * (tileSize.x + tileGap);
                yield return new WaitForEndOfFrame();
            }

            tilePosition -= Vector3.forward * (tileSize.y + tileGap) * tileManager.NoOfRows; // Resetting x Position
            tilePosition += Vector3.right * (tileSize.y + tileGap);
        }
        IsBoardCreated = true;
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

    public Vector3 GetTilePositionFor(TileLocation tileLocation)
    {
        return GetTilePositionFor(tileManager.GetTileIndexFor(tileLocation));
    }

    public Vector3 GetTilePositionFor(int index)
    {
        return tilesOfLife[index].transform.position;
    }

    public void ResetAllTileHighlights()
    {
        foreach (var tile in tilesOfLife)
            tile.ResetTileHighlight();
    }

    public void HighlightTiles(List<int> tiles)
    {
        foreach (var tile in tiles)
            tilesOfLife[tile].HighlightTile();
    }

    public IEnumerator WaitUntillBoardCreated()
    {
        yield return new WaitUntil(() => { return IsBoardCreated; });
    }
}
