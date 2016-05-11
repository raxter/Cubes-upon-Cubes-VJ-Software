using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeTunnle : MonoBehaviour
{

    public List<MeshRenderer> cubes = new List<MeshRenderer>();

    public float randomRotationSpeed = 0;

    public float flashChance = 0.2f;

    Dictionary<MeshRenderer, Vector3> rotationAngles = new Dictionary<MeshRenderer, Vector3>();


    float [] scale;

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
            mr.material.SetColor("_Color", Color.Lerp(c, Color.white, scale[i]));
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
            mr.material.SetColor("_Color", Color.Lerp(c, Color.white, scale[i]));
        }
    }

    void Awake()
    {
        scale = new float[cubes.Count];
        for (int i = 0; i < cubes.Count; i++)
        {
            scale[i] = 1;
        }
    }

    // Use this for initialization
    void Start()
    {

        cubes.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
        foreach (var mr in cubes)
        {
            rotationAngles[mr] = Random.onUnitSphere;
            //StartCoroutine(ChangeRotationOverRandomTime(mr));
        }
    }

    IEnumerator ChangeRotationOverRandomTime(MeshRenderer mr)
    {
        float time = Random.Range(5, 9);
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
        StartCoroutine(ChangeRotationOverRandomTime(mr));
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            if (BeatFinder.beatHitThisFrame && Random.value < flashChance)
                scale[i] = 1f;

            //cubes[i].transform.localScale = Vector3.one * scale[i];

            scale[i] = Mathf.Lerp(scale[i], 0, 3 * Time.deltaTime);
        }

        foreach (var mr in cubes)
        {
            mr.transform.Rotate(rotationAngles[mr], randomRotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
