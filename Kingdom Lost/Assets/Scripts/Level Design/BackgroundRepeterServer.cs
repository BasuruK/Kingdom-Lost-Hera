using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * The server class act as the Tile Controller
 * it tracks the camera and the position of the Tile and give a command to change the position
 **/
public class BackgroundRepeterServer : MonoBehaviour {
	
	protected static bool Tile1, Tile2;
	protected float m_Tile1Location, m_Tile2Location;

	private GameObject MainCameraRef;
	[HideInInspector]
	public static Camera MainCamera;
	private float m_MainCameraPosX;

	// Use this for initialization
	void Start () {
		//Tile1 = false;
		//Tile2 = false;

		//MainCameraRef = GameObject.FindGameObjectWithTag ("MainCamera");
		//MainCamera = MainCameraRef.GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		//TrackTileLocation ();

		//m_MainCameraPosX = MainCamera.transform.position.x;
		//m_Tile1Location += 160f;
		//m_Tile2Location += 160f;
		//// Debug.Log ("Tile1Loc " + m_MainCameraPosX + " > " + m_Tile1Location);
		//// Debug.Log ("Tile2Loc " + m_MainCameraPosX + " > " + m_Tile2Location);

		//if (m_MainCameraPosX > m_Tile1Location) {
		//	Tile1 = true;
		//} else {
		//	Tile1 = false;
		//}

		//if (m_MainCameraPosX > m_Tile2Location) {
		//	Tile2 = true;
		//} else {
		//	Tile2 = false;
		//}

	}

	void TrackTileLocation()
	{
		m_Tile1Location = GameObject.FindGameObjectWithTag ("Tile1").transform.position.x;
		m_Tile2Location = GameObject.FindGameObjectWithTag ("Tile2").transform.position.x;
	}
}
