using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public GameObject tileObject;
    public Material tileHighlightMterial;
    public Material tileOriginalMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeTileMaterial(Material material)
    {
        if (material != null && tileObject != null && tileObject.GetComponent<MeshRenderer>() != null)
            tileObject.GetComponent<MeshRenderer>().material = material;
    }

    public void HighlightTile()
    {
        ChangeTileMaterial(tileHighlightMterial);
    }

    public void ResetTileHighlight()
    {
        ChangeTileMaterial(tileOriginalMaterial);
    }
}
