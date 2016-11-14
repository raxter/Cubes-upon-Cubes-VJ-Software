using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeTunnle : MonoBehaviour
{
	public int ID { get; set; }

	public static float GridLayoutLerpAmount = 0;
	public static float OceanLayoutLerpAmount = 0;


    public float randomRotationSpeed = 0;

    public float flashChance = 0.2f;

	public List<MeshRendererInfo> cubesInfo = new List<MeshRendererInfo>();
	public List<MeshRenderer> cubes = new List<MeshRenderer>();
	//Dictionary<MeshRenderer, Vector3> rotationAngles = new Dictionary<MeshRenderer, Vector3>();
    //Dictionary<MeshRenderer, float> startPositions = new Dictionary<MeshRenderer, float>();

	//Dictionary<MeshRenderer, float> orthStarts = new Dictionary<MeshRenderer, float> ();
	//Dictionary<MeshRenderer, bool> flipOrtho = new Dictionary<MeshRenderer, bool> ();

	//float [] scale;
	//bool [] weAreTheFlash;

	public class MeshRendererInfo
	{
		public MeshRenderer mr;
		public Vector3 rotationAngle;
		public float startPosition;
		public float orthStart;
		public bool flipOrtho;
		public float flashScale;
		public bool weAreTheFlash;
		public float linearScale;
        public float finalScale;
    }

	public static bool StraightenCubes; 
	public static float ScaleFlashesAmount; 

    public static Color FlashColor;
    public static float FlashScaleAmount; // vs custom: linear -> 0, flash -> 1

    public static float ForwardOffset = 0;

	public static bool PulseRather = false;

	public static float RandomRotationSpeedMultiplier = 1;
	public static float DefinedRotationSpeedMultiplier = 1;

    float straightenAmount = 1;


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
		foreach (var mr in cubesInfo)
        {
            mr.mr.transform.localRotation = Random.rotationUniform;
        }
    }

    public void SetColour(ColorScheme c1, ColorScheme c2, float f, float randomShift, float twistAdjust, float offsetAdjust)
    {
        float lerpAmount = CubeTunnlesController.LerpAmount;
        float twist = Mathf.Lerp(c1.twist, c2.twist, lerpAmount) + twistAdjust;
        float randomness = Mathf.Lerp(c1.randomness, c2.randomness, lerpAmount);
        float offset = Mathf.Lerp(c1.offset, c2.offset, lerpAmount) + offsetAdjust;

        randomShift *= randomness;

        float totalTwist = 0;
        //Debug.Log("Setting Colour");
        for (int i = 0; i < cubesInfo.Count; i++)
        {
			var mr = cubesInfo[i].mr;
            float t = ((Random.Range(totalTwist + f - randomShift, totalTwist + f + randomShift) + 1) + 100 + offset) % 1;
			//mr.sharedMaterial = MaterialController.GetColor (t);
			Color c = Color.Lerp(c1.gradient.Evaluate(t), c2.gradient.Evaluate(t), lerpAmount);
            
            mr.material.SetColor("_Color", Color.Lerp(c, FlashColor, cubesInfo[i].finalScale));
            
			totalTwist += twist;
        }
    }

    /*public void SetColour(Gradient gradient, float f, float randomShift)
    {
        Debug.Log("Setting Colour");
        for (int i = 0; i < cubesInfo.Count; i++)
        {
			var mr = cubesInfo[i].mr;
            float t = (Random.Range(f - randomShift, f + randomShift) + 1) % 1;
            Color c = gradient.Evaluate(t);
			mr.material.SetColor("_Color", Color.Lerp(c, FlashColor, cubesInfo[i].scale));
        }
    }*/

	public bool interlaceAfter32 = false;

    void Awake()
    {
		cubesInfo = new List<MeshRendererInfo> ();
		if (interlaceAfter32)
		{
			for (int i = 33; i < cubesInfo.Count; i += 2)
			{
				cubes [i].gameObject.SetActive (false);
			}
		}
		cubes.RemoveAll ((x) => x.gameObject.activeSelf == false);
		//scale = new float[cubes.Count];
		//weAreTheFlash = new bool[cubes.Count];
        for (int i = 0; i < cubes.Count; i++)
		{
			cubesInfo.Add (new MeshRendererInfo () {
				flashScale = 1,
				weAreTheFlash = false,
				mr = cubes [i]
			}
			);
			//scale[i] = 1;
			//weAreTheFlash[i] = false;
        }
    }



    // Use this for initialization
    void Start()
    {
        cubes.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
        foreach (var ci in cubesInfo)
        {
			ci.rotationAngle = Random.onUnitSphere;
			ci.startPosition = ci.mr.transform.localPosition.z;
            //StartCoroutine(ChangeRotationOverRandomTime(mr));
			maxZPosition = Mathf.Max(maxZPosition, ci.startPosition);
			ci.orthStart = Random.value * 0.5f;
			ci.flipOrtho  = Random.value > 0.5f;
        }
    }

	IEnumerator ChangeRotationOverRandomTime(MeshRendererInfo mr, bool repeat = true, float minTime = 5, float maxTime = 9)
    {
		float time = Random.Range(minTime, maxTime);
        float timeElapsed = 0;
        Vector3 oldRandom = mr.rotationAngle;
        Vector3 newRandom = Random.onUnitSphere;
        while (timeElapsed < time)
        {
            float f = timeElapsed / time;
			mr.rotationAngle = Vector3.Lerp(oldRandom, newRandom, f);
            timeElapsed += Time.deltaTime * RandomRotationSpeedMultiplier;
            yield return null;
        }
        mr.rotationAngle = newRandom;
		if (repeat)
        	StartCoroutine(ChangeRotationOverRandomTime(mr));
    }


	void UpdatePositionThings(int i, MeshRenderer mr)
	{
		Vector3 pos = mr.transform.localPosition;
		float blockOffset = Mathf.Ceil (Mathf.Abs (ForwardOffset) / maxZPosition) * maxZPosition;
		//if (startPositions [mr] % maxZPosition > ForwardOffset % maxZPosition)
		//	blockOffset += maxZPosition;
		pos.x = 0;
		pos.y = 2;
		pos.z = (cubesInfo [i].startPosition + ForwardOffset + blockOffset) % maxZPosition;
		mr.transform.localPosition = pos;
	}

	void UpdateLerpThings(int i, MeshRenderer mr)
	{
		int x = (i - cubes.Count / 2);
		int y = (ID - 6);
		int z = 6 - Mathf.FloorToInt (x / 15) + 1;
		x %= 15;
		float gridLerp = Mathf.InverseLerp (cubesInfo [i].orthStart, 1, GridLayoutLerpAmount);
		mr.transform.position = Vector3.Lerp (mr.transform.position, new Vector3 ((x + 0.5f * (y % 2)) * Mathf.Sqrt (2)/*+cubes.Count/2*/, (y) * 1.22f, z), gridLerp);

		float oceanLerp = Mathf.InverseLerp (cubesInfo[i].orthStart, 1, OceanLayoutLerpAmount);
		float globalZ = mr.transform.position.z;

		float spreadFactor = 0.001f;// 0.002f;
		Vector3 newPosiion = Vector3.Lerp (mr.transform.position, new Vector3 ((ID-6)*(1-gridLerp) * (1+(Mathf.Pow(globalZ, 2)*spreadFactor)*(1-GridLayoutLerpAmount)),-1.5f*(1-gridLerp)+ Mathf.Pow(globalZ, 2) * 0.006f*(1-GridLayoutLerpAmount), gridLerp*2+globalZ/2), oceanLerp);
		mr.transform.position = newPosiion;
	}

	bool allSteadyLastFrame = false;
    // Update is called once per frame
    void Update()
    {
		//bool allSteadyThisFrame = 
		//	(GridLayoutLerpAmount == 0 || GridLayoutLerpAmount == 1) && 
		//	(OceanLayoutLerpAmount == 0 || OceanLayoutLerpAmount == 1);

        

        for (int i = 0; i < cubesInfo.Count; i++)
        {

			if (PulseRather) 
			{
				if (BeatFinder.sin1 < -0.9f)
				{
					cubesInfo[i].weAreTheFlash = Random.value < flashChance;
				}
				if (cubesInfo[i].weAreTheFlash) 
				{
					cubesInfo[i].flashScale = Mathf.Lerp (cubesInfo[i].flashScale, (BeatFinder.sin1 + 1) * 0.5f, 1f);
				}
			}
			else
			{
                if (BeatFinder.beatHitThisFrame)
                {
                    bool randomChance = Random.value < flashChance;

                    cubesInfo[i].weAreTheFlash = randomChance;
                    if (randomChance)
                    {
                        cubesInfo[i].flashScale = Random.Range(0.99f, 1.01f);
                    }
                }
			}

            if (cubesInfo[i].weAreTheFlash)
                cubesInfo[i].linearScale = CubeTunnlesController.CustomFlashValue;
            else
                cubesInfo[i].linearScale = 0;

            cubesInfo[i].finalScale = Mathf.Lerp(cubesInfo[i].linearScale, cubesInfo[i].flashScale, CubeTunnlesController.LinearScaleVsFlashScale);

            cubesInfo[i].mr.transform.localScale = Vector3.one * Mathf.Lerp(1, 1.4f, cubesInfo[i].finalScale* ScaleFlashesAmount);

			//if (!PulseRather) 
			cubesInfo[i].flashScale = Mathf.Lerp(cubesInfo[i].flashScale, 0, 3 * Time.deltaTime);

			UpdatePositionThings (i, cubesInfo [i].mr);

			//if (!(allSteadyLastFrame && allSteadyThisFrame))
			UpdateLerpThings(i, cubesInfo[i].mr);
		}

		//allSteadyLastFrame = allSteadyThisFrame;

		float oldStrightenAmount = straightenAmount;
		float targetStraightenAmount = StraightenCubes ? 1 : 0;
		float dampenFactor = straightenAmount < targetStraightenAmount ? 3 : 10;
		straightenAmount = Mathf.Lerp(straightenAmount, targetStraightenAmount, Time.deltaTime * dampenFactor * RandomRotationSpeedMultiplier);
        //if (ID == 0)
        //    Debug.Log(straightenAmount+ "\t" + targetStraightenAmount + "\t" + Time.deltaTime +"\t"+ dampenFactor + "\t" + RandomRotationSpeedMultiplier);

        if (straightenAmount == 0 && oldStrightenAmount != straightenAmount) 
		{
			StopAllCoroutines ();
			//Debug.LogError ("Straightening");
			foreach (var ci in cubesInfo)
			{
				//rotationAngles[mr] = Random.onUnitSphere;
				StartCoroutine(ChangeRotationOverRandomTime(ci, false, 5, 15));
			}
		}
		float rotSpeed = randomRotationSpeed * RandomRotationSpeedMultiplier * DefinedRotationSpeedMultiplier;
        //if (ID == 0)
        //    Debug.Log(straightenAmount);
		for (int i = 0; i < cubesInfo.Count; i++)
		{
			var ci = cubesInfo [i];
			//var ciMod = cubesInfo [i%1];
			ci.mr.transform.localRotation = Quaternion.Slerp (ci.mr.transform.localRotation, Quaternion.identity, straightenAmount * RandomRotationSpeedMultiplier);
			ci.mr.transform.rotation = Quaternion.Slerp (ci.mr.transform.rotation, Quaternion.Euler ((ci.flipOrtho ? -1 : 1)*52, 0,  45), GridLayoutLerpAmount* straightenAmount * RandomRotationSpeedMultiplier);

			ci.mr.transform.Rotate(ci.rotationAngle, rotSpeed * Time.deltaTime * (1f - straightenAmount), Space.Self);
        }
    }
}
