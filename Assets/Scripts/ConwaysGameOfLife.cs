using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConwaysGameOfLife : MonoBehaviour {

    [SerializeField]
    private GameObject lifePrefab;
    [SerializeField]
    private GameObject tilePrefab;
    private Transform lifeContainer;

    private BoardOfLife boardOfLife;
    //private List<Life> lifeActive = new List<Life>();
    private Dictionary<int, Life> lifeActive = new Dictionary<int, Life>(); // Holds the life on tile index

	// Use this for initialization
	void Start () {
        CreateElementPools();
        CreateActiveLifeContainer();
        CreateBoardOfLife();
        CreateInitialPopulationToad();
        StartSimulation();

        // TEMP
        TestTileManager.Execute();
    }

    private void CreateElementPools()
    {
        PoolManager.Instance.CreatePoolFor(Constants.POOL_NAME_LIFE, lifePrefab);
        PoolManager.Instance.CreatePoolFor(Constants.POOL_NAME_TILE, tilePrefab);
    }

    private void CreateBoardOfLife(int row = 6, int col = 6)
    {
        GameObject boardObject = new GameObject("BoardOfLife");
        boardOfLife = boardObject.AddComponent<BoardOfLife>();
        boardOfLife.CreateBoard(col, row, Vector2.one * 2, 0.5f);
    }

    private void CreateActiveLifeContainer()
    {
        GameObject lifeContainerObject = new GameObject("ActiveLifeContainer");
        lifeContainer = lifeContainerObject.transform;
    }

    private void CreateInitialPopulationLoaf()
    {
        // Sample initial population for 6x6 board - loaf
        List<TileLocation> population = new List<TileLocation>();
        population.Add(new TileLocation(4, 2));
        population.Add(new TileLocation(3, 3));
        population.Add(new TileLocation(5, 3));
        population.Add(new TileLocation(2, 4));
        population.Add(new TileLocation(5, 4));
        population.Add(new TileLocation(3, 5));
        population.Add(new TileLocation(4, 5));
        PopulateLife(population);
    }

    private void CreateInitialPopulationToad()
    {
        // Sample initial population for 6x6 board - toad
        List<TileLocation> population = new List<TileLocation>();
        population.Add(new TileLocation(4, 2));
        population.Add(new TileLocation(5, 2));
        population.Add(new TileLocation(4, 3));
        population.Add(new TileLocation(5, 3));
        population.Add(new TileLocation(2, 4));
        population.Add(new TileLocation(3, 4));
        population.Add(new TileLocation(2, 5));
        population.Add(new TileLocation(3, 5));
        PopulateLife(population);
    }

    private void PopulateLife(List<TileLocation> population)
    {
        foreach (var life in population)
            CreateLifeAt(life);
    }

    private void CreateLifeAt(TileLocation location)
    {
        int tileIndex = boardOfLife.tileManager.GetTileIndexFor(location);
        Vector3 tilePosition = boardOfLife.GetTilePositionFor(tileIndex);
        Life life = CreateLifeAt(tilePosition);
        lifeActive.Add(tileIndex, life);
    }

    private Life CreateLifeAt(Vector3 position)
    {
        GameObject lifeObject = PoolManager.Instance.GetItemFromPool(Constants.POOL_NAME_LIFE);
        lifeObject.transform.position = position;
        lifeObject.transform.SetParent(lifeContainer);
        lifeObject.SetActive(true);
        return lifeObject.GetComponent<Life>();
    }

    private void LifeDiedAt(int index)
    {
        Life life = null;
        lifeActive.TryGetValue(index, out life);
        if(life != null)
        {
            PoolManager.Instance.ReturnItemToPool(life);
            lifeActive.Remove(index);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void StartSimulation()
    {
        StartCoroutine("SimulateGameOfLife");
    }

    IEnumerator SimulateGameOfLife()
    {
        while (true)
        {
            yield return IterateEvoltion();

            //Debug.Log("Simulate Game Of Life at "+System.DateTime.Now);
        }
    }

    IEnumerator IterateEvoltion()
    {
        for(int i=0;i<lifeActive.Count;i++)
        {
            yield return new WaitForEndOfFrame();

        }

        // Temp
        //yield return new WaitForSeconds(5);
    }
}

