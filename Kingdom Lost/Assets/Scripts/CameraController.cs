using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private GameObject MainCharacter;
	private Camera MainCamera;
	public GameObject MainCharacterController;
	private KingController King;
	[HideInInspector]
	public Vector3 screenPos;
	[HideInInspector]
	public float cameraWidthChecker;
	public float move_x;

	public int m_startMovingFrom; // When should the camera move when the user enteres a propotion of the camera
	public float m_cameraMoveX;
	public float m_walkSpeed;
	public bool m_userMovedRight = false;


	// Use this for initialization
	void Start () {
		MainCamera = GetComponent<Camera> ();
		MainCharacterController = GameObject.Find ("MainCharacterController");
		MainCharacter = MainCharacterController.GetComponent<KingController> ().mainCharacter;
	}

	// Update is called once per frame
	void Update () {

		// Set values taken from King Controller
		GetValuesFromKingController();

		float levelMax = 0;
		// get current level max distance
		if (Constant.current_level == 1) {
			levelMax = Constant.level_1_max;
		} else if (Constant.current_level == 2) {
			levelMax = Constant.level_2_max;
		}


		#region CameraMovement
		if (move_x < levelMax)
		{
			// Move the Main Camera when the player is at 40% of camera range.
			screenPos = MainCamera.WorldToScreenPoint (MainCharacter.transform.position);
			int cameraWidth = MainCamera.pixelWidth;
			cameraWidthChecker = (cameraWidth * m_startMovingFrom) / 100;

			// Move the camera right
			if (screenPos.x >= cameraWidthChecker) {
				m_cameraMoveX = MainCamera.transform.position.x + m_walkSpeed;
				MainCamera.transform.position = new Vector3 (m_cameraMoveX, MainCamera.transform.position.y, 0);
				m_userMovedRight = true;
			}
			// Move the camera left
			if (screenPos.x <= cameraWidthChecker - 5 && m_userMovedRight == true){
				m_cameraMoveX = MainCamera.transform.position.x - m_walkSpeed;
				MainCamera.transform.position = new Vector3(m_cameraMoveX, MainCamera.transform.position.y, 0);
			}

			// Limit the camera range so that the player wont go beyond begining region
			if (MainCamera.transform.position.x < 27.46f){
				float tempCameraMoveX = 27.46f;
				MainCamera.transform.position = new Vector3(tempCameraMoveX, MainCamera.transform.position.y);
			}
		}
		#endregion CameraMovement

	}

	void GetValuesFromKingController()
	{
		m_walkSpeed = MainCharacterController.GetComponent<KingController> ().m_walkSpeed;
		m_startMovingFrom = MainCharacterController.GetComponent<KingController> ().m_startMovingFrom;
		move_x = KingController.move_x;
	}
}
