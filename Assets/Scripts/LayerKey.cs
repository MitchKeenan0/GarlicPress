using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerKey : MonoBehaviour
{
	public string layerName = "LayerName";

	void Start()
    {
		GetComponent<Renderer>().sortingLayerName = layerName;
	}
}
