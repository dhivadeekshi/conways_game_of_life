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
    private TileManager tileManager { get { return boardOfLife.tileManager; } }
    private Dictionary<int, Life> currentPopulation = new Dictionary<int, Life>(); // Holds the life on tile index

    private List<int> lifeDieInCells = new List<int>(); // To kill life on every iteration
    private List<int> lifeSpawnInCells = new List<int>(); // To create new life on every iteration

    // Use this for initialization
    void Start () {
        CreateElementPools();
        CreateActiveLifeContainer();
        CreateBoardOfLife();
        CreateInitialPopulationToad();
        //CreateInitialPopulationLoaf();
        StartSimulation();
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
        boardOfLife.CreateBoard(col, row, Vector2.one * Constants.TILE_SIZE, Constants.TILE_GAP);
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
        int tileIndex = tileManager.GetTileIndexFor(location);
        Vector3 tilePosition = boardOfLife.GetTilePositionFor(tileIndex);
        Life life = CreateLifeAt(tilePosition);
        currentPopulation.Add(tileIndex, life);
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
        currentPopulation.TryGetValue(index, out life);
        if(life != null)
        {
            PoolManager.Instance.ReturnItemToPool(life);
            currentPopulation.Remove(index);
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
            yield return KillLifeInCells();
            yield return SpawnLifeInCells();
        }
    }

    IEnumerator IterateEvoltion()
    {
        int tileIndex, neighbourCount;
        foreach(var tileLocation in tileManager.GetAllTileLocations())
        {
            neighbourCount = 0;
            tileIndex = tileManager.GetTileIndexFor(tileLocation);
            /// Any live cell with fewer than two live neighbours dies, as if by underpopulation.
            /// Any live cell with two or three live neighbours lives on to the next generation.
            /// Any live cell with more than three live neighbours dies, as if by overpopulation.
            /// Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
            foreach (var neighbourTile in tileManager.GetAllNeighbourTilesIndexFor(tileLocation))
            {
                if (currentPopulation.ContainsKey(neighbourTile))
                    neighbourCount++;
            }

            // Create new life
            if (neighbourCount == 3 && !currentPopulation.ContainsKey(tileIndex))
                lifeSpawnInCells.Add(tileIndex);
            // Kill life
            else if ((neighbourCount < 2 || neighbourCount > 3) && currentPopulation.ContainsKey(tileIndex))
                lifeDieInCells.Add(tileIndex);
            // Temp -------------------------
            /*Debug.Log("NewLifein");
            lifeSpawnInCells.PrintAllElements();
            Debug.Log("Dies in");
            lifeDieInCells.PrintAllElements();*/
            // -------------------------------

        }
        yield return null;
    }

    IEnumerator SpawnLifeInCells()
    {
        foreach(var tileIndex in lifeSpawnInCells)
        {
            CreateLifeAt(tileManager.GetTileLocationFromIndex(tileIndex));
        }
        lifeSpawnInCells.Clear();
        yield return null;
    }

    IEnumerator KillLifeInCells()
    {
        foreach(var tileIndex in lifeDieInCells)
        {
            LifeDiedAt(tileIndex);
        }
        lifeDieInCells.Clear();
        yield return null;
    }
}

