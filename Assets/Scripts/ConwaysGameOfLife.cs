using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConwaysGameOfLife : MonoBehaviour {


    public GameObject lifePrefab;
    public GameObject tilePrefab;

	// Use this for initialization
	void Start () {
        PoolManager.Instance.CreatePoolFor(Constants.POOL_NAME_LIFE, lifePrefab);
        PoolManager.Instance.CreatePoolFor(Constants.POOL_NAME_TILE, tilePrefab);

        /*GameObject life = PoolManager.Instance.GetItemFromPool(Constants.POOL_NAME_LIFE);
        life.transform.SetParent(transform);
        life.SetActive(true);
        GameObject tile = PoolManager.Instance.GetItemFromPool(Constants.POOL_NAME_TILE);
        tile.transform.SetParent(transform);
        tile.SetActive(true);

        StartCoroutine("ReturnPoolItem", life);
        StartCoroutine("ReturnPoolItem", tile);

        UnityAction action = () =>
        {
            GameObject boardObject = new GameObject("BoardOfLife");
            boardObject.AddComponent<BoardOfLife>().CreateBoard(10, 10, Vector2.one*2);
        };
        StartCoroutine(ExecuteAfter( 10, action));
        */
    }

    /*IEnumerator ExecuteAfter(int secs, UnityAction action)
    {
        yield return new WaitForSeconds(secs);

        action.Invoke();
    }

    IEnumerator ReturnPoolItem(GameObject poolObject)
    {
        yield return new WaitForSeconds(10);

        PoolManager.Instance.ReturnItemToPool(poolObject.GetComponent<PoolItem>());
    }*/
	
	// Update is called once per frame
	void Update () {
		
	}
}
