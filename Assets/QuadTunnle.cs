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
#if UNITY_5_2
			c = HSVToRGB(hue, saturation, 1);
#elif UNITY_5_4_OR_NEWER
            c = Color.HSVToRGB(hue, saturation, 1);
#endif
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

	
#if UNITY_5_2
	public static Color HSVToRGB(float hue, float saturation, float val, float a = 1)
	{
		float r = val;
		float g = val;
		float b = val;
		if (saturation != 0)
		{
			float max = val;
			float dif = val * saturation;
			float min = val - dif;
			
			float h = hue * 360f;
			
			if (h < 60f)
			{
				r = max;
				g = h * dif / 60f + min;
				b = min;
			}
			else if (h < 120f)
			{
				r = -(h - 120f) * dif / 60f + min;
				g = max;
				b = min;
			}
			else if (h < 180f)
			{
				r = min;
				g = max;
				b = (h - 120f) * dif / 60f + min;
			}
			else if (h < 240f)
			{
				r = min;
				g = -(h - 240f) * dif / 60f + min;
				b = max;
			}
			else if (h < 300f)
			{
				r = (h - 240f) * dif / 60f + min;
				g = min;
				b = max;
			}
			else if (h <= 360f)
			{
				r = max;
				g = min;
				b = -(h - 360f) * dif / 60 + min;
			}
			else
			{
				r = 0;
				g = 0;
				b = 0;
			}
		}
		
		return new Color(Mathf.Clamp01(r),Mathf.Clamp01(g),Mathf.Clamp01(b), a);
	}
#endif

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
