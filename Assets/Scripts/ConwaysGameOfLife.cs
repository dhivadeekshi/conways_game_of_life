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
    private List<Life> lifeActive = new List<Life>();

	// Use this for initialization
	void Start () {
        CreateElementPools();
        CreateActiveLifeContainer();
        CreateBoardOfLife();
        CreateInitialPopulation();
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

    private void CreateInitialPopulation()
    {

    }

    private void CreateLifeAt(Vector2 position)
    {
        GameObject lifeObject = PoolManager.Instance.GetItemFromPool(Constants.POOL_NAME_LIFE);
        lifeObject.transform.position = position;
        lifeObject.transform.SetParent(lifeContainer);
        lifeObject.SetActive(true);
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

