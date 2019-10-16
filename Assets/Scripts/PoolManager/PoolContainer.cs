using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolContainer : MonoBehaviour {
    private string poolName = string.Empty;
    private GameObject poolItemPrefab = null;
    private List<PoolItem> poolItems = new List<PoolItem>();

    public void InitPoolFor(string poolName, GameObject poolItemPrefab, int noOfItems)
    {
        this.poolName = poolName;
        this.poolItemPrefab = poolItemPrefab;
        CreatePoolItems(noOfItems);
    }

    public void CreatePoolItems(int noOfItems)
    {
        GameObject poolInstance;
        PoolItem poolItem;
        for (int i = 0; i < noOfItems; i++)
        {
            poolInstance = Instantiate(poolItemPrefab, transform);
            poolInstance.SetActive(false);
            poolItem = poolInstance.AddComponent<PoolItem>();
            poolItem.PoolName = poolName;
            poolItems.Add(poolItem);
        }
    }

    public bool IsPoolOf(string poolName) { return this.poolName.Equals(poolName); }

    public GameObject GetItemFromPool()
    {
        if (poolItems.Count == 0)
            CreatePoolItems(1);
        
        PoolItem poolItem = poolItems[0];
        poolItems.RemoveAt(0);

        return poolItem.gameObject;
    }

    public void ReturnItemToPool(PoolItem poolItem)
    {
        poolItem.gameObject.transform.SetParent(transform);
        poolItem.gameObject.SetActive(false);
        poolItem.transform.position = Vector3.zero;
        poolItems.Add(poolItem);
    }
}
