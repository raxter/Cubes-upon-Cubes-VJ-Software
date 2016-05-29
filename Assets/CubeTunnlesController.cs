using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeTunnlesController : MonoBehaviour {

    public List<CubeTunnle> tunnles = new List<CubeTunnle>();

    public ColorScheme colourScheme;
    ColorScheme oldColourScheme;

    public bool updateEveryFrame = true;
    public float transisionTime = 4;

	public bool pulseRather = false;

    float lerpAmount = 1;

	public float twistAdd = 0;
	public float offsetAdd = 0;
	public float flashChanceAdd = 0;

    float twistAdjust = 0;
    float offsetAdjust = 0;

    [Range(0,1)]
    float flashChanceBase = 0.4f;

	float FlashChance { get { return flashChanceBase + flashChanceAdd; } }

    // Use this for initialization
    void Start ()
    {
        Update();

		for (int i = 0; i < tunnles.Count; i++) {
			tunnles [i].ID = i;
		}
    }

    void UpdateColours()
    {
        Random.State oldState = Random.state;
        Random.InitState(0);
        float diff = 1f / tunnles.Count;
        for (int i = 0; i < tunnles.Count; i++)
        {
            float f = (float)i / tunnles.Count;
            tunnles[i].SetColour(oldColourScheme, colourScheme, lerpAmount, f, diff, twistAdjust, offsetAdjust);
            //tunnles[i].SetColour(colourScheme.gradient, f, diff);
        }
        Random.state = oldState;
    }

    bool lerping = false;
	void Update ()
    {
		CubeTunnle.PulseRather = pulseRather;

        if (oldColourScheme == null)
            oldColourScheme = colourScheme;

        if (lerpAmount == 1 && colourScheme != oldColourScheme)
        {
            if (lerping)
            {
                lerping = false;
                oldColourScheme = colourScheme;
            }
            else
            {
                lerping = true;
                lerpAmount = 0;
            }
        }
        if (lerping)
        {
            lerpAmount = Mathf.MoveTowards(lerpAmount, 1, Time.deltaTime/transisionTime);
        }

        RenderSettings.ambientSkyColor = Color.Lerp(oldColourScheme.skyColour, colourScheme.skyColour, lerpAmount);
        RenderSettings.ambientEquatorColor = Color.Lerp(oldColourScheme.horizonColour, colourScheme.horizonColour, lerpAmount);
        RenderSettings.ambientGroundColor = Color.Lerp(oldColourScheme.groundColour, colourScheme.groundColour, lerpAmount);
        RenderSettings.ambientIntensity = Mathf.Lerp(oldColourScheme.intensity, colourScheme.intensity, lerpAmount);

        float twistSpeed = Mathf.Lerp(oldColourScheme.twistPerSecond, colourScheme.twistPerSecond, 1/*lerpAmount*/);
        float offsetSpeed = Mathf.Lerp(oldColourScheme.offsetPerSecond, colourScheme.offsetPerSecond, 1/*lerpAmount*/);
		CubeTunnle.FlashColor = Color.Lerp (oldColourScheme.flashColor, colourScheme.flashColor, lerpAmount);

		twistSpeed += twistAdd;
		offsetSpeed += offsetAdd;

        if (twistSpeed == 0)
            twistAdjust = Mathf.MoveTowards(twistAdjust, 0, Time.deltaTime / transisionTime);
        else
            twistAdjust += twistSpeed * Time.deltaTime;

        if (offsetSpeed == 0)
            offsetAdjust = Mathf.MoveTowards(offsetAdjust, 0, Time.deltaTime / transisionTime);
        else
            offsetAdjust += offsetSpeed * Time.deltaTime;

        twistAdjust = ((twistAdjust + 0.5f) % 1f) - 0.5f;
        offsetAdjust = ((offsetAdjust + 0.5f) % 1f) - 0.5f;

        for (int i = 0; i < tunnles.Count; i++)
        {
			tunnles[i].flashChance = FlashChance;
        }

        if (updateEveryFrame)
            UpdateColours();

    }
}
