using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * The BackgroundRepeter class reads the commands sent by the Server class and changes the position.
 **/
public class BackgroundRepeter : BackgroundRepeterServer {

	private GameObject Tile;
	private Renderer TileRenderer;
	private string Name;

	// Use this for initialization
	void Start () {
		Tile = gameObject;
		TileRenderer = GetComponent<Renderer> ();
		Name = gameObject.tag;
	}
	
	// Update is called once per frame
	void Update () {

		//Change the position for Tile1
		if (Name == "Tile1" && Tile1) {
			Tile.transform.position = new Vector3 (
				Tile.transform.position.x + 470f,
				Tile.transform.position.y,
				Tile.transform.position.z
			);
		}

		//Change the position for Tile2
		if (Name == "Tile2" && Tile2) {
			Tile.transform.position = new Vector3 (
				Tile.transform.position.x + 470f,
				Tile.transform.position.y,
				Tile.transform.position.z
			);
		}
	}
		
}
