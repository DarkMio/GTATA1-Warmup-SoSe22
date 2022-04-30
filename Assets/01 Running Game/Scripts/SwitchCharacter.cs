using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    // referenses to game objects
	// referenses to controlled game objects
	public GameObject char1, char2;

	// variable contains which character is on and active
	int whichCharIsOn = 1;

	// Use this for initialization
	void Start () {

		// enable first avatar and disable another one
		char1.gameObject.SetActive (true);
		char2.gameObject.SetActive (false);
	}

	// public method to switch characters by pressing next button
	public void SwitchChar()
	{

		// processing whichCharIsOn variable
		switch (whichCharIsOn) {

		// if the first character is on
		case 1:

			// then the second character is on now
			whichCharIsOn = 2;

			// disable the first one and anable the second one
			char1.gameObject.SetActive (false);
			char2.gameObject.SetActive (true);
			break;

		// if the second avatar is on
		case 2:

			// then the first character is on now
			whichCharIsOn = 1;

			// disable the second one and anable the first one
			char1.gameObject.SetActive (true);
			char2.gameObject.SetActive (false);
			break;
		}
			
	}
}
