using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeTunnlesController : MonoBehaviour {

    public List<CubeTunnle> tunnles = new List<CubeTunnle>();

    public ColorScheme colourScheme;
    ColorScheme oldColourScheme;

    public bool updateEveryFrame = true;
    public float transisionTime = 4;

    float lerpAmount = 1;


    float twistAdjust = 0;
    float offsetAdjust = 0;

    // Use this for initialization
    void Start ()
    {
        Update();
    }

    void UpdateColours()
    {
        Random.seed = 0;
        float diff = 1f / tunnles.Count;
        for (int i = 0; i < tunnles.Count; i++)
        {
            float f = (float)i / tunnles.Count;
            tunnles[i].SetColour(oldColourScheme, colourScheme, lerpAmount, f, diff, twistAdjust, offsetAdjust);
            //tunnles[i].SetColour(colourScheme.gradient, f, diff);
        }
    }

    bool lerping = false;
	void Update ()
    {
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

        if (updateEveryFrame)
            UpdateColours();

    }
}
