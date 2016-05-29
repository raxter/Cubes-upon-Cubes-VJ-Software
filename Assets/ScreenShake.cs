using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour 
{	
	public static float ScreenShakeAmount;

	public float screenShakeDistance = 1;

	void Update () 
	{
		transform.localPosition = Random.onUnitSphere * ScreenShakeAmount * screenShakeDistance;

		//ScreenShakeAmount = Mathf.Lerp (ScreenShakeAmount, 0, Time.deltaTime * 3);
	}
}
