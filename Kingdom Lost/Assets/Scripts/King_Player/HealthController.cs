using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {

	public int m_PlayerHealth;
	public Text m_PlayerHealthTextInUI;
	private Animator m_PlayerAnimator;
	public int WalkerHitAmount;
	public int MainEnemyHitAmount;

	private int m_blendSpeed;

	// Use this for initialization
	void Start () {

        m_PlayerHealthTextInUI = GameObject.FindGameObjectWithTag("PlayerHealthText").GetComponent<Text>();
        //m_PlayerHealth = GameObject.FindGameObjectWithTag("MainCharacterController").GetComponent<KingController> ().m_health;

		if (Constant.current_level == 1 ) {
			m_PlayerAnimator = GameObject.FindGameObjectWithTag ("KingDutugamunu").GetComponent<Animator> ();
		} else {
			m_PlayerAnimator = GameObject.FindGameObjectWithTag ("MainCharacter").GetComponent<Animator> ();
		}
	}
	
	// Update is called once per frame
	void Update () {

		UpdateHealth ();
		UpdateGUIText ();
		if (Input.GetKeyDown (KeyCode.F)) {
			WalkerHit ();
		}

		if (m_PlayerHealth == 0 || m_PlayerHealth < 0) {
			m_PlayerAnimator.SetTrigger ("Death");
			FreezeMovement ();
            UIPanelController.getInstance().viewGameOver(Constant.current_level, false);
		}
	}

	//Subtract Health when a Walker Enemy Attacks
	void WalkerHit()
	{
		m_PlayerHealth -= WalkerHitAmount;
		if (m_PlayerHealth < 0) {
			m_PlayerHealth = 0;
		}
		GameObject.FindGameObjectWithTag ("MainCharacterController").GetComponent<KingController> ().m_health = m_PlayerHealth;
	}

	// Main Enemy Attach Health Subtraction
	void MainEnemyHit()
	{
		m_PlayerHealth -= MainEnemyHitAmount;
		if (m_PlayerHealth < 0) {
			m_PlayerHealth = 0;
		}
		GameObject.FindGameObjectWithTag ("MainCharacterController").GetComponent<KingController> ().m_health = m_PlayerHealth;
	}
	//Update GUIText
	void UpdateGUIText()
	{
		m_PlayerHealthTextInUI.text = m_PlayerHealth.ToString();
	}
	// Update Player Helath in the Script
	void UpdateHealth()
	{
		m_PlayerHealth = GameObject.FindGameObjectWithTag("MainCharacterController").GetComponent<KingController> ().m_health;
	}

	// Freeze all movement. Called after deat
	void FreezeMovement()
	{
		GameObject.FindGameObjectWithTag ("MainCharacterController").GetComponent<KingController> ().m_freezeMovement = true;
	}

	// Run when the user collects a Health Potion
	void IncreaseHealthByPotion(int RefillAmount)
	{
		// Dont let the health Go beyond maximum
		if (m_PlayerHealth + RefillAmount > 100) {
			int tempHealth = m_PlayerHealth + RefillAmount;
			RefillAmount = tempHealth - RefillAmount;
			RefillAmount = 100 - RefillAmount;

			GameObject.FindGameObjectWithTag ("MainCharacterController").GetComponent<KingController> ().m_health += RefillAmount;
		} else {
			GameObject.FindGameObjectWithTag ("MainCharacterController").GetComponent<KingController> ().m_health += RefillAmount;
		}


	}

}
