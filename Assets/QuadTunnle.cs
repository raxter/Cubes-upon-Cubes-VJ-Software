using UnityEngine;
using System.Collections.Generic;

public class QuadTunnle : MonoBehaviour
{

    public MeshRenderer[] quads;

    public float speed = 5;
    public float rotateSpeed = 0;
    public float hue = 0;
    public float saturation = 0;

    public Dictionary<MeshRenderer, float> rotationSpeeds = new Dictionary<MeshRenderer, float>();

    float random = 0;
    void Start()
    {
        random = Random.Range(0, Mathf.PI * 2);
        hue = Random.value;
        RefreshRotationSpeeds();
    }

    void RefreshRotationSpeeds()
    {
        rotationSpeeds.Clear();
        foreach (var quad in quads)
        {
            rotationSpeeds[quad] = (Random.value - 0.5f) * 2f;
        }
    }

    float time = 0;

    void Update()
    {
        time += Time.deltaTime * speed;

        foreach (var quad in quads)
        {
            //Color c = quad.material.GetColor("_TintColor");
            Color c = quad.material.color;// GetColor("_TintColor");
            c = Color.HSVToRGB(hue, saturation, 1);
            float val = Mathf.InverseLerp(-1, 1, Mathf.Sin(random + quad.transform.localPosition.z / 10 + time));
            quad.transform.localEulerAngles = new Vector3(0, 0, quad.transform.localEulerAngles.z + rotationSpeeds[quad]*rotateSpeed * Time.deltaTime);

            if (rotateSpeed == 0)
            { 
                quad.transform.localEulerAngles = new Vector3(0, 0, ((quad.transform.localEulerAngles.z + 180) % 360) - 180);
                quad.transform.localEulerAngles = Vector3.Lerp(quad.transform.localEulerAngles, Vector3.zero, 2 * Time.deltaTime);
            }

            //val = Mathf.Lerp(-0.5f, 0.5f, val) < -0.4f ? 0 : 0.5f;
            c.a = val * 0.5f;
            //quad.material.SetColor("_TintColor", c);
            quad.material.color = c;

            ReturnToDirectoin(quad.transform);

            if (BeatFinder.beatHitThisFrame)
            {
                PuchDirectoin(quad.transform);
            }
        }

	}

    void ReturnToDirectoin(Transform quad)
    {
        Vector2 offset = Vector2.Lerp(quad.localPosition, Vector2.zero, 3 * Time.deltaTime);
        quad.localPosition = new Vector3(offset.x, offset.y, quad.localPosition.z);
    }

    void PuchDirectoin(Transform quad)
    {
        Vector2 offset = Random.insideUnitCircle.normalized*0.1f;
        quad.localPosition += new Vector3(offset.x, offset.y, 0);
    }
}
