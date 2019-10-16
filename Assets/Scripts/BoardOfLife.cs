using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardOfLife : MonoBehaviour {

    public List<Tile> tilesOfLife = new List<Tile>();

    public void CreateBoard(int noOfColumns, int noOfRows, Vector2 tileSize)
    {
        GameObject colObject;
        for(int col=1;col<= noOfColumns; col++)
        {
            colObject = new GameObject("Col" + col);
            colObject.transform.SetParent(transform);

            for(int row = 1;row <= noOfRows; row++)
            {
                GameObject tileObject = PoolManager.Instance.GetItemFromPool(Constants.POOL_NAME_TILE);
                tileObject.transform.SetParent(colObject.transform);
                tileObject.transform.position = Vector3.forward * tileSize.x * row + Vector3.right * tileSize.y * col;
                tileObject.SetActive(true);
                tilesOfLife.Add(tileObject.GetComponent<Tile>());
            }
        }
    }

    public void PopulateBoard(List<TileLocation> initialLife)
    {

    }

    public List<TileLocation> GetNeighbourTilesFor(TileLocation tileLocation)
    {
        return new List<TileLocation>();
    }


}
