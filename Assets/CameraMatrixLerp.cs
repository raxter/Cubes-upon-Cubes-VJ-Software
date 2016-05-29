using UnityEngine;
using System.Collections;

public class CameraMatrixLerp : MonoBehaviour {

	public Camera target;	
	public Camera a;	
	public Camera b;

	public static float LerpAmount;

	// Update is called once per frame
	void Update () {
		Matrix4x4 lerped = new Matrix4x4 ();
		for (int i = 0; i < 4; i++) {
			lerped.SetColumn(i, Vector4.Lerp(a.projectionMatrix.GetColumn(i), b.projectionMatrix.GetColumn(i), LerpAmount)); 
		}
		target.projectionMatrix = lerped;
	}
}
