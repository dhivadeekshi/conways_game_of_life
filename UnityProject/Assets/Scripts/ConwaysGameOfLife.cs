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

    private bool isPopulationReady = false;
    private bool isInitialized = false;

    public UnityAction<List<int>> onPopulateLife;
    public UnityAction onRetrySimulation;

    #region Getters
    public int InitialPopulationCount { get; private set; }
    public int CurrentPopulationCount { get { return currentPopulation.Count; } }
    public int LifeToDieCount { get { return lifeDieInCells.Count; } }
    public int LifeToBeBorn { get { return lifeSpawnInCells.Count; } }
    #endregion

    void Awake()
    {
        CreateListeners();
        CreateElementPools();
        CreateActiveLifeContainer();
    }

    // Use this for initialization
    void Start ()
    {
        StartCoroutine("Initialize");
        StartSimulation();
    }

    #region Initializers
    private void CreateElementPools()
    {
        PoolManager.Instance.CreatePoolFor(Constants.POOL_NAME_LIFE, lifePrefab);
        PoolManager.Instance.CreatePoolFor(Constants.POOL_NAME_TILE, tilePrefab);
    }

    IEnumerator Initialize()
    {
        CreateBoardOfLife();
        yield return boardOfLife.WaitUntillBoardCreated();
        CreateRandomPopulation();

        yield return new WaitForEndOfFrame();
        isInitialized = true;
    }

    private void CreateBoardOfLife()
    {
        GameObject boardObject = new GameObject("BoardOfLife");
        boardOfLife = boardObject.AddComponent<BoardOfLife>();
        boardOfLife.CreateBoard(Constants.NO_OF_COLS, Constants.NO_OF_ROWS, Vector2.one * Constants.TILE_SIZE, Constants.TILE_GAP);
    }

    private void CreateActiveLifeContainer()
    {
        GameObject lifeContainerObject = new GameObject("ActiveLifeContainer");
        lifeContainer = lifeContainerObject.transform;
    }

    private void CreateListeners()
    {
        onPopulateLife += HighlightInitialTiles;
        onPopulateLife += StartPopulation;
        onRetrySimulation += ResetAllTileHighlights;
    }
    #endregion

    #region Create Population
    private void CreateRandomPopulation()
    {
        HashSet<int> populationIndexes = PopulationGenerator.GetRandomPopulation(tileManager.NoOfRows, tileManager.NoOfCols);
        List<TileLocation> population = new List<TileLocation>();
        foreach (int index in populationIndexes)
            population.Add(tileManager.GetTileLocationFromIndex(index));
        PopulateLife(population);
    }

    private void CreateToadPopulation()
    {
        PopulateLife(PopulationGenerator.GetPopulationToad());
    }

    private void CreateLoadPopulation()
    {
        PopulateLife(PopulationGenerator.GetPopulationLoaf());
    }
    #endregion

    #region Manipulate Tiles
    private void HighlightInitialTiles(List<int> tiles)
    {
        boardOfLife.HighlightTiles(tiles);
    }

    private void ResetAllTileHighlights()
    {
        boardOfLife.ResetAllTileHighlights();
    }
    #endregion

    #region Retry Simulation
    IEnumerator RetrySimulation(UnityAction loadPopulation)
    {
        if (onRetrySimulation != null)
            onRetrySimulation.Invoke();

        // Clear new life to be born
        lifeSpawnInCells.Clear();

        // Clear the current Population
        lifeDieInCells.Clear();
        lifeDieInCells.AddRange(currentPopulation.Keys);
        yield return KillLifeInCells();

        // Create new initial population
        loadPopulation.Invoke();

        // Start simulation
        StartSimulation();
    }

    public void LoadRandomSimulation()
    {
        if (isPopulationReady)
        {
            StopAllCoroutines();
            StartCoroutine(RetrySimulation(CreateRandomPopulation));
        }
    }

    public void LoadToadSimulation()
    {
        if (isPopulationReady)
        {
            StopAllCoroutines();
            StartCoroutine(RetrySimulation(CreateToadPopulation));
        }
    }

    public void LoadLoafSimulation()
    {
        if (isPopulationReady)
        {
            StopAllCoroutines();
            StartCoroutine(RetrySimulation(CreateLoadPopulation));
        }
    }
    #endregion

    #region Manipulate life
    private void PopulateLife(List<TileLocation> population)
    {
        isPopulationReady = false;
        InitialPopulationCount = population.Count;
        foreach (var life in population)
        {
            lifeSpawnInCells.Add(tileManager.GetTileIndexFor(life));
        }
        if (onPopulateLife != null)
            onPopulateLife.Invoke(lifeSpawnInCells);
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
    #endregion

    #region Simulation
    private void StartSimulation()
    {
        StartCoroutine("SimulateGameOfLife");
    }

    private void StartPopulation(List<int> population)
    {
        StartCoroutine("SpawnLifeInCells");
        StartCoroutine("WaitForInitialPopulation");
    }

    IEnumerator SimulateGameOfLife()
    {
        while (true)
        {
            yield return new WaitUntil(() => { return isInitialized; });
            yield return new WaitUntil(() => { return isPopulationReady; });

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
            yield return new WaitForEndOfFrame();
            neighbourCount = 0;
            tileIndex = tileManager.GetTileIndexFor(tileLocation);
            /// Any live cell with fewer than two live neighbors dies, as if by underpopulation.
            /// Any live cell with two or three live neighbors lives on to the next generation.
            /// Any live cell with more than three live neighbors dies, as if by overpopulation.
            /// Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
            foreach (var neighbourTile in tileManager.GetAllNeighbourTilesIndexFor(tileLocation))
            {
                if (currentPopulation.ContainsKey(neighbourTile))
                    neighbourCount++;
            }

            // Create life
            if (neighbourCount == 3 && !currentPopulation.ContainsKey(tileIndex))
                lifeSpawnInCells.Add(tileIndex);
            // Kill life
            else if ((neighbourCount < 2 || neighbourCount > 3) && currentPopulation.ContainsKey(tileIndex))
                lifeDieInCells.Add(tileIndex);
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator SpawnLifeInCells()
    {
        foreach(var tileIndex in lifeSpawnInCells)
        {
            CreateLifeAt(tileManager.GetTileLocationFromIndex(tileIndex));
        }
        lifeSpawnInCells.Clear();
        yield return new WaitForEndOfFrame();
    }

    IEnumerator KillLifeInCells()
    {
        foreach(var tileIndex in lifeDieInCells)
        {
            LifeDiedAt(tileIndex);
        }
        lifeDieInCells.Clear();
        yield return new WaitForEndOfFrame();
    }

    IEnumerator WaitForInitialPopulation()
    {
        yield return new WaitUntil(() => { return lifeSpawnInCells.Count == 0; });
        isPopulationReady = true;
    }
    #endregion
}

