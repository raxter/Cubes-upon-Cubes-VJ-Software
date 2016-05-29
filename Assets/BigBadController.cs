using UnityEngine;
using System.Collections;

public class BigBadController : MonoBehaviour
{
    [Range(0, 1)]
    public float BloomAmount = 0;
    public float maxBloom = 160;

    [Range(0, 1)]
    public float Fidelity = 0;
    public float lowWidth = 240;
    public float lowHeight = 135;

    [Range(0, 1)]
    public float Size = 0;
    
    public bool GoSmooth = false;

    [Range(0, 1)]
    public float Speed = 0;

    [Range(0, 1)]
    public float RotateSpeed = 0;

    [Range(0, 1)]
    public float Saturation = 0;
    
    [Range(0, 1)]
    public float HueShift = 0;

	public bool SnapToStraight = false;

	[Range(0,1)]
	public float ScreenShakeAmount = 0;

	[Range(0,1)]
	public float ScaleFlashesAmount = 0;

	[Range(0,1)]
	public float CameraPerspectiveLerp = 0;

	[Range(0,1)]
	public float OceanLayoutLerpAmount = 0;

	public bool CameraPerspectiveToggle = true;
	public bool OceanToggle = false;

	public float ForwardSpeed = 0;

	public bool Edges = false;

	public float OrthCameraMin = 6.46f;
	public float OrthCameraMax = 1.5f;

	[Range(0, 1)]
	public float OrthCameraZoomAmount = 0;

	public bool OrthCameraZoomToggle = false;

	[Range(0,1)]
	public float AnalogGlitchAmount = 0;
	[Range(0,1)]
	public float AnalogGlitchColor = 0;
	[Range(0,1)]
	public float AnalogGlitchVertJump = 0;
	[Range(0,1)]
	public float DigitalGlitchAmount = 0;

	public bool MotionBlur = false;

	public bool InvertColours = false;

    public ApplyImageEffect imageEffect;
	public UnityStandardAssets.ImageEffects.Bloom bloom;
	public UnityStandardAssets.ImageEffects.EdgeDetection edges;
	public Kino.AnalogGlitch analogGlitch;
	public Kino.DigitalGlitch digitalGlitch;
	public InvertColorEffect invertColours;
	public UnityStandardAssets.ImageEffects.MotionBlur motionBlur;
	public Camera orthReferenceCamera;

	// Update is called once per frame
	void Update ()
    {
        bloom.bloomIntensity = maxBloom * BloomAmount;
        imageEffect.width = Mathf.Lerp(lowWidth, 1920, Fidelity);
        imageEffect.height = Mathf.Lerp(lowHeight, 1080, Fidelity);
        imageEffect.size = Mathf.Lerp(0.2f, 1.5f, Size);
        imageEffect.enabled = !GoSmooth;


		edges.enabled = Edges;

		OrthCameraZoomAmount = Mathf.Lerp (OrthCameraZoomAmount, OrthCameraZoomToggle ? 1 : 0, Time.deltaTime * 3);
		orthReferenceCamera.orthographicSize = Mathf.Lerp (OrthCameraMin, OrthCameraMax, OrthCameraZoomAmount);

		CameraMatrixLerp.LerpAmount = CameraPerspectiveLerp;
		CubeTunnle.GridLayoutLerpAmount = CameraPerspectiveLerp;
		CubeTunnle.OceanLayoutLerpAmount = OceanLayoutLerpAmount;

		if (Mathf.Abs (ForwardSpeed) > 0.2f) {
			CubeTunnle.ForwardOffset -= ForwardSpeed * Time.deltaTime;
			//Debug.Log (CubeTunnle.ForwardOffset +":"+ ForwardSpeed);
		}
		else {
			CubeTunnle.ForwardOffset = Mathf.Lerp (CubeTunnle.ForwardOffset, Mathf.Round (CubeTunnle.ForwardOffset), 3 * Time.deltaTime); 
		}

		CameraPerspectiveLerp = Mathf.Lerp (CameraPerspectiveLerp, CameraPerspectiveToggle ? 0 : 1, 3 * Time.deltaTime);
		OceanLayoutLerpAmount = Mathf.Lerp (OceanLayoutLerpAmount, OceanToggle ? 1 : 0, 3 * Time.deltaTime);
		CubeTunnle.ScaleFlashesAmount = ScaleFlashesAmount;
		CubeTunnle.StraightenCubes = SnapToStraight;
		ScreenShake.ScreenShakeAmount = ScreenShakeAmount;

		invertColours.enabled = InvertColours;

		analogGlitch.scanLineJitter = Mathf.Lerp(0, 0.45f, AnalogGlitchAmount);
		analogGlitch.verticalJump = Mathf.Lerp(0, 0.04f, AnalogGlitchVertJump);
		analogGlitch.colorDrift = Mathf.Lerp(0,0.4f, AnalogGlitchColor);

		digitalGlitch.intensity = DigitalGlitchAmount;

		motionBlur.enabled = MotionBlur;
    }
}
