using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeTunnle : MonoBehaviour
{

    public List<MeshRenderer> cubes = new List<MeshRenderer>();

    public float randomRotationSpeed = 0;

    Dictionary<MeshRenderer, Vector3> rotationAngles = new Dictionary<MeshRenderer, Vector3>();

    [ContextMenu("Randomize Rotations")]
    void RandomizeRotations()
    {
        foreach (var mr in cubes)
        {
            mr.transform.localRotation = Random.rotationUniform;
        }
    }

    public void SetColour(Gradient gradient, float f, float randomShift)
    {
        Debug.Log("Setting Colour");
        foreach (var mr in cubes)
        {
            float t = (Random.Range(f - randomShift, f + randomShift) + 1) % 1;
            mr.material.SetColor("_Color", gradient.Evaluate(t));
        }
    }

    

    // Use this for initialization
    void Start()
    {
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
        foreach (var mr in cubes)
        {
            mr.transform.Rotate(rotationAngles[mr], randomRotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
