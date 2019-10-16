using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    private const int INITIAL_POOL_COUNT = 3;
    private static PoolManager instance = null;
    public static PoolManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject poolManagerObject = new GameObject("PoolManager");
                instance = poolManagerObject.AddComponent<PoolManager>();
            }
            return instance;
        }
    }

    private List<PoolContainer> poolContainers = new List<PoolContainer>();

    public void CreatePoolFor(string poolName, GameObject poolItemPrefab, int poolCount = INITIAL_POOL_COUNT)
    {
        GameObject gameObject = new GameObject("PoolFor" + poolName);
        gameObject.transform.SetParent(transform);
        gameObject.AddComponent<PoolContainer>().InitPoolFor(poolName, poolItemPrefab, poolCount);
        poolContainers.Add(gameObject.GetComponent<PoolContainer>());
    }

    public GameObject GetItemFromPool(string poolName)
    {
        PoolContainer poolContainer = poolContainers.Find((x) => { return x.IsPoolOf(poolName); });
        return poolContainer.GetItemFromPool();
    }

    public void ReturnItemToPool(PoolItem poolItem)
    {
        PoolContainer poolContainer = poolContainers.Find((x) => { return x.IsPoolOf(poolItem.PoolName); });
        poolContainer.ReturnItemToPool(poolItem);
    }
}
