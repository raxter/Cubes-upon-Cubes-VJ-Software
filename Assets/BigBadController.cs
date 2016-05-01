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

    public QuadTunnle[] quads;

    public ApplyImageEffect imageEffect;
    public UnityStandardAssets.ImageEffects.Bloom bloom;
	
	// Update is called once per frame
	void Update ()
    {
        bloom.bloomIntensity = maxBloom * BloomAmount;
        imageEffect.width = Mathf.Lerp(lowWidth, 1920, Fidelity);
        imageEffect.height = Mathf.Lerp(lowHeight, 1080, Fidelity);
        imageEffect.size = Mathf.Lerp(0.2f, 1.5f, Size);
        imageEffect.enabled = !GoSmooth;

        foreach (var q in quads)
        {
            q.speed = Mathf.Lerp(2, 20, Speed);
            q.rotateSpeed = Mathf.Lerp(0, 100f, RotateSpeed);
            q.saturation = Saturation;
            q.hue += HueShift * Time.deltaTime;
            q.hue %= 1;
        }
    }
}
