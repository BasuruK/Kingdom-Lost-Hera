using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

	private Renderer BackgroundMat;
	private GameObject m_Camera;

	private float move_x;

	private float m_cameraMoveX;
	public Vector3 m_screenPos;
	public float m_cameraWidthChecker;
	public float m_walkSpeed;
	public float m_offsetSpeed;



	private Vector3 m_tileStartPosition;

	// Use this for initialization
	void Start () {
		BackgroundMat = GetComponent<Renderer> ();
		m_Camera = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		GetValuesFromCameraController ();

		m_offsetSpeed = CalculateOffsetValue (m_walkSpeed);

		#region Texture Offset Setting
		// If the Player is the begining range of the map. Then dont offset the texture
		bool isInBeginingRange = false;
		if(m_screenPos.x < (m_cameraWidthChecker * 96.0f / 100))
		{
			isInBeginingRange = true;
		}

		// Offset the texture based on the player location
		if (m_screenPos.x >= m_cameraWidthChecker && isInBeginingRange == false) {
			move_x -= m_offsetSpeed * Time.deltaTime;
			BackgroundMat.material.SetTextureOffset ("_MainTex", new Vector2 (move_x, 0));
		} 

		if (m_screenPos.x <= m_cameraWidthChecker - 5 && isInBeginingRange == false) {
			move_x += m_offsetSpeed * Time.deltaTime;
			BackgroundMat.material.SetTextureOffset ("_MainTex", new Vector2 (move_x, 0));
		}

		#endregion Texture Offset Setting
	}

	// Initialize Values at every update
	void GetValuesFromCameraController()
	{
		m_cameraMoveX = m_Camera.GetComponent<CameraController>().m_cameraMoveX;
		m_screenPos = m_Camera.GetComponent<CameraController> ().screenPos;
		m_cameraWidthChecker = m_Camera.GetComponent<CameraController> ().cameraWidthChecker;
		m_walkSpeed = m_Camera.GetComponent<CameraController> ().m_walkSpeed;
	}

	// Calculate the speed value for offsetting to take the parrelax effect
	float CalculateOffsetValue(float walkValue)
	{
		return walkValue / 5.0f;
	}

}
