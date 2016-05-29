using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeTunnle : MonoBehaviour
{
	public int ID { get; set; }

	public static float GridLayoutLerpAmount = 0;
	public static float OceanLayoutLerpAmount = 0;

    public List<MeshRenderer> cubes = new List<MeshRenderer>();

    public float randomRotationSpeed = 0;

    public float flashChance = 0.2f;

	Dictionary<MeshRenderer, Vector3> rotationAngles = new Dictionary<MeshRenderer, Vector3>();
    Dictionary<MeshRenderer, float> startPositions = new Dictionary<MeshRenderer, float>();

	public static bool StraightenCubes; 
	public static float ScaleFlashesAmount; 

	public static Color FlashColor;

	public static float ForwardOffset = 0;

	public static bool PulseRather = false;

	public static float RandomRotationSpeedMultiplier = 1;

	float straightenAmount = 1;

	float [] scale;
	bool [] weAreTheFlash;

	float maxZPosition = 0;


	[ContextMenu("Reoder cubes")]
	void ReorderCubes()
	{
		int p = 0;
		foreach (Transform t in transform)
		{
			Vector3 pos = t.localPosition;
			pos.z = p;
			p++;
			t.localPosition = pos;
		}
	}

    [ContextMenu("Randomize Rotations")]
    void RandomizeRotations()
    {
        foreach (var mr in cubes)
        {
            mr.transform.localRotation = Random.rotationUniform;
        }
    }

    public void SetColour(ColorScheme c1, ColorScheme c2, float lerpAmount, float f, float randomShift, float twistAdjust, float offsetAdjust)
    {
        float twist = Mathf.Lerp(c1.twist, c2.twist, lerpAmount) + twistAdjust;
        float randomness = Mathf.Lerp(c1.randomness, c2.randomness, lerpAmount);
        float offset = Mathf.Lerp(c1.offset, c2.offset, lerpAmount) + offsetAdjust;

        randomShift *= randomness;

        float totalTwist = 0;
        //Debug.Log("Setting Colour");
        for (int i = 0; i < cubes.Count; i++)
        {
            var mr = cubes[i];
            float t = ((Random.Range(totalTwist + f - randomShift, totalTwist + f + randomShift) + 1) + 100 + offset) % 1;
            Color c = Color.Lerp(c1.gradient.Evaluate(t), c2.gradient.Evaluate(t), lerpAmount);
            mr.material.SetColor("_Color", Color.Lerp(c, FlashColor, scale[i]));
            totalTwist += twist;
        }
    }

    public void SetColour(Gradient gradient, float f, float randomShift)
    {
        Debug.Log("Setting Colour");
        for (int i = 0; i < cubes.Count; i++)
        {
            var mr = cubes[i];
            float t = (Random.Range(f - randomShift, f + randomShift) + 1) % 1;
            Color c = gradient.Evaluate(t);
			mr.material.SetColor("_Color", Color.Lerp(c, FlashColor, scale[i]));
        }
    }

	public bool interlaceAfter32 = false;

    void Awake()
    {
		if (interlaceAfter32)
		{
			for (int i = 33; i < cubes.Count; i += 2)
			{
				cubes [i].gameObject.SetActive (false);
			}
		}
		cubes.RemoveAll ((x) => x.gameObject.activeSelf == false);
		scale = new float[cubes.Count];
		weAreTheFlash = new bool[cubes.Count];
        for (int i = 0; i < cubes.Count; i++)
		{
			scale[i] = 1;
			weAreTheFlash[i] = false;
        }
    }

	Dictionary<MeshRenderer, float> orthStarts = new Dictionary<MeshRenderer, float> ();
	Dictionary<MeshRenderer, bool> flipOrtho = new Dictionary<MeshRenderer, bool> ();


    // Use this for initialization
    void Start()
    {
        cubes.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
        foreach (var mr in cubes)
        {
			rotationAngles[mr] = Random.onUnitSphere;
			startPositions[mr] = mr.transform.localPosition.z;
            //StartCoroutine(ChangeRotationOverRandomTime(mr));
			maxZPosition = Mathf.Max(maxZPosition, startPositions[mr]);
			orthStarts[mr] = Random.value * 0.5f;
			flipOrtho [mr] = Random.value > 0.5f;
        }
    }

	IEnumerator ChangeRotationOverRandomTime(MeshRenderer mr, bool repeat = true, float minTime = 5, float maxTime = 9)
    {
		float time = Random.Range(minTime, maxTime);
        float timeElapsed = 0;
        Vector3 oldRandom = rotationAngles[mr];
        Vector3 newRandom = Random.onUnitSphere;
        while (timeElapsed < time)
        {
            float f = timeElapsed / time;
            rotationAngles[mr] = Vector3.Lerp(oldRandom, newRandom, f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        rotationAngles[mr] = newRandom;
		if (repeat)
        	StartCoroutine(ChangeRotationOverRandomTime(mr));
    }


    // Update is called once per frame
    void Update()
    {


        for (int i = 0; i < cubes.Count; i++)
        {

			if (PulseRather) 
			{
				if (BeatFinder.sin1 < -0.9f)
				{
					weAreTheFlash[i] = Random.value < flashChance;
				}
				if (weAreTheFlash [i]) 
				{
					scale [i] = Mathf.Lerp (scale [i], (BeatFinder.sin1 + 1) * 0.5f, 1f);
				}
			}
			else
			{
				bool flashYes = BeatFinder.beatHitThisFrame && Random.value < flashChance;
				if (flashYes)
					scale [i] = Random.Range (0.99f, 1.01f);
			}

			cubes[i].transform.localScale = Vector3.one * Mathf.Lerp(1, 1.4f, scale[i] * ScaleFlashesAmount);

			//if (!PulseRather) 
            scale[i] = Mathf.Lerp(scale[i], 0, 3 * Time.deltaTime);

			MeshRenderer mr = cubes [i];
			Vector3 pos = mr.transform.localPosition;
			float blockOffset = Mathf.Ceil(Mathf.Abs(ForwardOffset) / maxZPosition)*maxZPosition;
			//if (startPositions [mr] % maxZPosition > ForwardOffset % maxZPosition)
			//	blockOffset += maxZPosition;
			pos.x = 0;
			pos.y = 2;
			pos.z = (startPositions [mr] + ForwardOffset + blockOffset)%maxZPosition;
			mr.transform.localPosition = pos;

			int x = (i - cubes.Count / 2);
			int y = (ID - 6);
			int z = 6 - Mathf.FloorToInt(x/15) + 1;
			x %= 15;
			float gridLerp = Mathf.InverseLerp (orthStarts[mr], 1, GridLayoutLerpAmount);
			mr.transform.position = Vector3.Lerp (mr.transform.position, new Vector3 ((x+ 0.5f*(y%2)) * Mathf.Sqrt(2)/*+cubes.Count/2*/, (y)*1.22f, z), gridLerp);

			float oceanLerp = Mathf.InverseLerp (orthStarts[mr], 1, OceanLayoutLerpAmount);
			float globalZ = mr.transform.position.z;

			float spreadFactor = 0.001f;// 0.002f;
			mr.transform.position = Vector3.Lerp (mr.transform.position, new Vector3 ((ID-6)*(1-gridLerp) * (1+(Mathf.Pow(globalZ, 2)*spreadFactor)*(1-GridLayoutLerpAmount)),-1.5f*(1-gridLerp)+ Mathf.Pow(globalZ, 2) * 0.005f*(1-GridLayoutLerpAmount), gridLerp*2+globalZ/2), oceanLerp);
        }

		float oldStrightenAmount = straightenAmount;
		float targetStraightenAmount = StraightenCubes ? 1 : 0;
		float dampenFactor = straightenAmount < targetStraightenAmount ? 3 : 10;
		straightenAmount = Mathf.Lerp(straightenAmount, targetStraightenAmount, Time.deltaTime * dampenFactor);

		if (straightenAmount < 0.1f)
			straightenAmount = 0;
		
		if (straightenAmount == 0 && oldStrightenAmount != straightenAmount) 
		{
			StopAllCoroutines ();
			//Debug.LogError ("Straightening");
			foreach (var mr in cubes)
			{
				//rotationAngles[mr] = Random.onUnitSphere;
				StartCoroutine(ChangeRotationOverRandomTime(mr, false, 5, 15));
			}
		}
		float rotSpeed = randomRotationSpeed * RandomRotationSpeedMultiplier;
        foreach (var mr in cubes)
		{
			mr.transform.localRotation = Quaternion.Slerp (mr.transform.localRotation, Quaternion.identity, straightenAmount);
			mr.transform.rotation = Quaternion.Slerp (mr.transform.rotation, Quaternion.Euler ((flipOrtho[mr] ? -1 : 1)*52, 0,  45), GridLayoutLerpAmount*straightenAmount);

			mr.transform.Rotate(rotationAngles[mr], rotSpeed * Time.deltaTime * (1f - straightenAmount), Space.Self);
        }
    }
}
