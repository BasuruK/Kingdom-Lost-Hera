using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : MonoBehaviour {

	public GameObject mainCharacter;

	public Animator m_Animator;
	private Rigidbody rgbd; // Access the Rigid body
	public AudioSource AudioSource;
	public AudioClip SwordSwoosh;
	public AudioClip FallingScream;

	private Camera mainCamera;

	// Movement variables
	public static float move_x = 0;
	float move_z = 0;

	public int m_health;
	public float m_walkSpeed; // Walking Speed
	public float m_runningSpeed; // Running Speed
	public float m_jumpHeight; // Jumping Height
	public float m_movementTransitionSpeed; // Adjust how speed transitions between movements should take


	public int m_startMovingFrom;

	private float m_movementAnimationBlendSpeed; // Holds the speed for scrolling slider

	//State Machine Variables
	bool m_rotatedLeft = false;
	bool m_rotatedRight = true;
	private float m_tempWalkSpeed; // Detect if shift key is pressed and reset the value when key not pressed
	private bool m_isJumped = false; // Prevent the user from jumping twice
	[HideInInspector]
	public bool m_freezeMovement = false; // Freeze Movement Controls
	private bool m_isFalling = false;

	// Use this for initialization
	void Start () {
        move_x = 0;
        // Take the Animator from the player object
        m_Animator = mainCharacter.GetComponent<Animator> ();
		// Take the Ridgid body component
		rgbd = mainCharacter.GetComponent<Rigidbody> ();	
		// Detect Shift Press and Toggle Running Mode
		m_tempWalkSpeed = m_walkSpeed;
		// Save the current Z position of the user preventing going crazy
		move_z = 10f;
		// Get The audio Source
		AudioSource = mainCharacter.GetComponent<AudioSource> ();
		// Access main camera
		mainCamera = BackgroundRepeterServer.MainCamera;
	}

	// Update is called once per frame
	void Update () {

		// Move the Main Character Left and Right
		float moveHorizontal = Input.GetAxis ("Horizontal") * Time.deltaTime;

		//Check weather Freeze Movement command has issued
		if (m_freezeMovement) {
			moveHorizontal = 0f;
		}

		#region Character Rotation According to Axes
		if ((moveHorizontal > 0) && (m_rotatedRight == false)) {
			mainCharacter.transform.Rotate (Vector3.up * 180);
			m_rotatedLeft = false;
			m_rotatedRight = true;
		}
		else if ((moveHorizontal < 0) && (m_rotatedLeft == false)){
			mainCharacter.transform.Rotate (Vector3.down * 180);
			m_rotatedRight = false;
			m_rotatedLeft = true;
		}
		#endregion Character Rotation According to Axes

		// Detect if the Shift key is pressed
		bool shiftPressed = Input.GetKey(KeyCode.LeftShift);

		if (shiftPressed) {
			m_walkSpeed = m_runningSpeed;
		} else {
			m_walkSpeed = m_tempWalkSpeed;
		}

		UIPanelController.m_speed = m_walkSpeed; // update UI Controller speed

        float levelMax = 0;
        // get current level max distance
        if (Constant.current_level == 1)
        {
            levelMax = Constant.level_1_max;
        }
        else if (Constant.current_level == 2)
        {
            levelMax = Constant.level_2_max;
        }

        #region Player Movement
        // Move the character using regular pixel movements
        if (moveHorizontal > 0 && move_x < levelMax + Constant.view_margin_right)
        {
            move_x = move_x + m_walkSpeed;
            mainCharacter.transform.position = new Vector3(move_x, mainCharacter.transform.position.y, move_z);
        }
        else if (moveHorizontal < 0)
        {
            move_x = move_x - m_walkSpeed;
            mainCharacter.transform.position = new Vector3(move_x, mainCharacter.transform.position.y, move_z);
        }
		#endregion Player Movement

		UIPanelController.m_move_x = move_x; // update player position

        #region Running & Walking
        // Set the Animation to Walking, Running
        if ((m_walkSpeed == m_runningSpeed) && (moveHorizontal != 0)) {
			// Player is Running
			m_movementAnimationBlendSpeed = Mathf.MoveTowards (m_movementAnimationBlendSpeed, 0.5f, m_movementTransitionSpeed * Time.deltaTime + 0.05f); // Slowly increase the blend creating a realisting transitiion.
			m_Animator.SetFloat ("Walk_Run_Blend", m_movementAnimationBlendSpeed);
		} else if ((m_walkSpeed < m_runningSpeed) && (moveHorizontal != 0)) {
			// Player is Walking
			m_movementAnimationBlendSpeed = Mathf.MoveTowards (m_movementAnimationBlendSpeed, 0.25f, m_movementTransitionSpeed * Time.deltaTime);
			m_Animator.SetFloat ("Walk_Run_Blend", m_movementAnimationBlendSpeed);
		} else if (moveHorizontal == 0) {
			// Set the Player to idle state
			m_movementAnimationBlendSpeed = Mathf.MoveTowards (m_movementAnimationBlendSpeed, 0, m_movementTransitionSpeed * (Time.deltaTime + 0.02f));
			m_Animator.SetFloat ("Walk_Run_Blend", m_movementAnimationBlendSpeed);
		} 

		// Limit the user from going beyond the camera limit
		if(mainCharacter.transform.position.x < -10.00f)
		{
			move_x = -10.00f;
			m_movementAnimationBlendSpeed = Mathf.MoveTowards (m_movementAnimationBlendSpeed, 0, m_movementTransitionSpeed * (Time.deltaTime + 0.02f));
			m_Animator.SetFloat("Walk_Run_Blend", m_movementAnimationBlendSpeed);
		}
		#endregion Running & Walking

		// Trigger Jumping
		if (Input.GetKeyDown (KeyCode.Space)) {
            if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && mainCharacter.transform.position.y < 1)
            {
                m_Animator.SetTrigger("Jump");
                rgbd.AddForce(new Vector3(0, m_jumpHeight, 0), ForceMode.Impulse);
            }
		}

		// Trigger Sword Attack
		if(Input.GetKeyDown(KeyCode.LeftControl)){
			float pauseXMovement = mainCharacter.transform.position.x;
			move_x = pauseXMovement;
			m_Animator.SetTrigger ("Sword_Attack");
			AudioSource.clip = SwordSwoosh;
			AudioSource.Play ();
		}

		// Detect when the player is falling
		if (mainCharacter.transform.position.y < -4.8f && m_isFalling == false) {
			m_isFalling = true;
			AudioSource.clip = FallingScream;
			AudioSource.Play ();
			UIPanelController.getInstance ().viewGameOver(Constant.current_level,true);
		}
			
	}
}
