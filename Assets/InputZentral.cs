using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;		// Be sure to include this if you want an object to have Xbox input

public class InputZentral : MonoBehaviour 
{

	public ColorScheme [] schemes;
	public BigBadController bbc;
	public CubeTunnlesController tunnleControl;


	void Update()
	{

		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene (0);
		}

        if (Input.GetKey(KeyCode.LeftShift) == false)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SetColor(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SetColor(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SetColor(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) SetColor(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) SetColor(4);
            if (Input.GetKeyDown(KeyCode.Alpha6)) SetColor(5);
            if (Input.GetKeyDown(KeyCode.Alpha7)) SetColor(6);
            if (Input.GetKeyDown(KeyCode.Alpha8)) SetColor(7);
            if (Input.GetKeyDown(KeyCode.Alpha9)) SetColor(8);
            if (Input.GetKeyDown(KeyCode.Alpha0)) SetColor(9);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SetColor(10);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SetColor(11);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SetColor(12);
            if (Input.GetKeyDown(KeyCode.Alpha4)) SetColor(13);
            if (Input.GetKeyDown(KeyCode.Alpha5)) SetColor(14);
            if (Input.GetKeyDown(KeyCode.Alpha6)) SetColor(15);
            if (Input.GetKeyDown(KeyCode.Alpha7)) SetColor(16);
            if (Input.GetKeyDown(KeyCode.Alpha8)) SetColor(17);
            if (Input.GetKeyDown(KeyCode.Alpha9)) SetColor(18);
            if (Input.GetKeyDown(KeyCode.Alpha0)) SetColor(19);
        }

            // Left Bumper/Trigger

        bbc.ScreenShakeAmount = XCI.GetAxis(XboxAxis.LeftTrigger) * 3;
		if (XCI.GetButtonDown(XboxButton.LeftBumper) || Input.GetKeyDown(KeyCode.O))
		{
			bbc.SnapToStraight = !bbc.SnapToStraight;
		}

		// Right Bumper/Trigger
		CubeTunnle.RandomRotationSpeedMultiplier = 1f - XCI.GetAxis(XboxAxis.RightTrigger);
		if (XCI.GetButtonDown(XboxButton.RightBumper))
		{
			bbc.InvertColours = !bbc.InvertColours;
		}

		// Left Stick
		bbc.ForwardSpeed += XCI.GetAxis (XboxAxis.LeftStickY)*Time.deltaTime*15;
		bbc.DigitalGlitchAmount += XCI.GetAxis (XboxAxis.LeftStickX) * Time.deltaTime * 1f;
		if (XCI.GetButtonDown (XboxButton.LeftStick)) 
		{
			bbc.MotionBlur = !bbc.MotionBlur;
		}

		// Right Stick
		bbc.Size += XCI.GetAxis (XboxAxis.RightStickY) * Time.deltaTime * 0.33f;
		bbc.Size = Mathf.Clamp01 (bbc.Size);
		bbc.Fidelity += XCI.GetAxis (XboxAxis.RightStickX) * Time.deltaTime * 0.33f;
		bbc.Fidelity = Mathf.Clamp01 (bbc.Fidelity);
		if(XCI.GetButtonDown(XboxButton.RightStick))
		{
			bbc.ScaleFlashesAmount = bbc.ScaleFlashesAmount == 0 ? 1 : 0;
		}


		// DPAD
		if (XCI.GetButtonDown (XboxButton.DPadLeft)) 
		{
			bbc.AnalogGlitchVertJump = bbc.AnalogGlitchVertJump == 0 ? 1 : 0;
		}
		if (XCI.GetButtonDown (XboxButton.DPadDown)) 
		{
			bbc.AnalogGlitchAmount = bbc.AnalogGlitchAmount == 0 ? 1 : 0;
		}
		if (XCI.GetButtonDown (XboxButton.DPadRight)) 
		{
			bbc.AnalogGlitchColor = bbc.AnalogGlitchColor == 0 ? 1 : 0;
		}
		if (XCI.GetButtonDown (XboxButton.DPadUp))
		{
			bbc.AnalogGlitchVertJump = 0;
			bbc.AnalogGlitchAmount   = 0;
			bbc.AnalogGlitchColor    = 0;
		}

		// Action Buttons

		// NOTEL: The "A" and "B" button are in the Beat Finder class!!
		if(XCI.GetButtonDown(XboxButton.Y))
		{
			bbc.CameraPerspectiveToggle = !bbc.CameraPerspectiveToggle;
		}

		if (XCI.GetButtonDown (XboxButton.X)) 
		{
			bbc.OceanToggle = !bbc.OceanToggle;

		}

		// Start/Select
		if (XCI.GetButtonDown (XboxButton.Start))
		{
			bbc.OrthCameraZoomToggle = !bbc.OrthCameraZoomToggle;
		}
		if (XCI.GetButtonDown (XboxButton.Back))
		{
			bbc.Edges = !bbc.Edges;
		}

		// Keyboard

		if (Input.GetKey (KeyCode.T)) 
		{
			BeatFinder.beatStartTimeOffsetHack -= Time.deltaTime * 0.33f;
		}
		if (Input.GetKey (KeyCode.Y)) 
		{
			BeatFinder.beatStartTimeOffsetHack += Time.deltaTime * 0.33f;
		}

		if (Input.GetKeyDown (KeyCode.R)) 
		{
			tunnleControl.pulseRather = !tunnleControl.pulseRather;
		}

		if (Input.GetKey (KeyCode.Q)) 
			tunnleControl.flashChanceAdd -= Time.deltaTime * 0.33f;
		if (Input.GetKey (KeyCode.W)) 
			tunnleControl.flashChanceAdd = 0;
		if (Input.GetKey (KeyCode.E)) 
			tunnleControl.flashChanceAdd += Time.deltaTime * 0.33f;

		if (Input.GetKey (KeyCode.Tab)) 
			tunnleControl.flashChanceAdd = -tunnleControl.FlashChanceBase + 0.05f;

		if (Input.GetKey (KeyCode.A))
			tunnleControl.offsetAdd -= Time.deltaTime * 0.33f;
		if (Input.GetKey (KeyCode.S))
			tunnleControl.offsetAdd = 0;
		if (Input.GetKey (KeyCode.D))
			tunnleControl.offsetAdd += Time.deltaTime * 0.33f;

		if (Input.GetKey (KeyCode.Z))
			tunnleControl.twistAdd -= Time.deltaTime * 0.33f;
		if (Input.GetKey (KeyCode.X))
			tunnleControl.twistAdd = 0;
		if (Input.GetKey (KeyCode.C))
			tunnleControl.twistAdd += Time.deltaTime * 0.33f;







	}

	void SetColor(int i )
	{
		tunnleControl.colourScheme = schemes [i % schemes.Length];
	}
}
