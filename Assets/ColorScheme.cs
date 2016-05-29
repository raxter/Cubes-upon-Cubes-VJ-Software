using UnityEngine;
using System.Collections;

public class ColorScheme : MonoBehaviour
{
    public Gradient gradient;

    public Color skyColour;
    public Color horizonColour;
    public Color groundColour;

    public float intensity = 1.34f;
    public float twist = 0f;
    public float twistPerSecond = 0f;
    public float offset = 0f;
    public float offsetPerSecond = 0f;
    public float randomness = 1f;

	public Color flashColor = Color.white;
}
