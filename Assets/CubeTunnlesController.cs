using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeTunnlesController : MonoBehaviour
{

    public List<CubeTunnle> tunnles = new List<CubeTunnle>();

    public ColorScheme colourScheme;
    ColorScheme oldColourScheme;

    public bool updateEveryFrame = true;
    public float transisionTime = 4;

    public bool pulseRather = false;

    public static float LerpAmount { get; private set; }
    public static float LinearScaleVsFlashScale { get; private set; } // 0 -> linear, 1 -> flash
    public static float CustomFlashValue { get; private set; }

    public float twistAdd = 0;
    public float offsetAdd = 0;
    public float flashChanceAdd = 0;


    float twistAdjust = 0;
    float offsetAdjust = 0;

    [Range(0, 1)]
    public float flashChanceBase = 0.4f;

    float FlashChance { get { return flashChanceBase + flashChanceAdd; } }
    public float FlashChanceBase { get { return flashChanceBase; } }

    // Use this for initialization
    void Start()
    {
        Update();

        for (int i = 0; i < tunnles.Count; i++)
        {
            tunnles[i].ID = i;
        }

        LerpAmount = 1;
    }

    void UpdateColours()
    {
        MaterialController.SetGradient(oldColourScheme.gradient, colourScheme.gradient, LerpAmount);
        Random.State oldState = Random.state;
        Random.InitState(0);
        float diff = 1f / tunnles.Count;
        for (int i = 0; i < tunnles.Count; i++)
        {
            float f = (float)i / tunnles.Count;
            tunnles[i].SetColour(oldColourScheme, colourScheme, f, diff, twistAdjust, offsetAdjust);
            //tunnles[i].SetColour(colourScheme.gradient, f, diff);
        }
        Random.state = oldState;
    }

    bool lerping = false;
    void Update()
    {
        CubeTunnle.PulseRather = pulseRather;

        if (oldColourScheme == null)
            oldColourScheme = colourScheme;

        if (LerpAmount == 1 && colourScheme != oldColourScheme)
        {
            if (lerping)
            {
                lerping = false;
                oldColourScheme = colourScheme;
            }
            else
            {
                lerping = true;
                LerpAmount = 0;
            }
        }
        if (lerping)
        {
            LerpAmount = Mathf.MoveTowards(LerpAmount, 1, Time.deltaTime / transisionTime);
        }

        LinearScaleVsFlashScale = Mathf.Lerp(oldColourScheme.useCustomFlashCurve ? 0 : 1, colourScheme.useCustomFlashCurve ? 0 : 1, LerpAmount);
        //Debug.Log(oldColourScheme.name + " -> " + LerpAmount + " -> " + colourScheme.name);

        float oldCustomFlashAmount = oldColourScheme.useCustomFlashCurve ? oldColourScheme.customFlashCurve.Evaluate(BeatFinder.linear1) : 0;
        float newCustomFlashAmount = colourScheme.useCustomFlashCurve ? colourScheme.customFlashCurve.Evaluate(BeatFinder.linear1) : 0;
        CustomFlashValue = Mathf.Lerp(oldCustomFlashAmount, newCustomFlashAmount, LerpAmount);

        RenderSettings.ambientSkyColor = Color.Lerp(oldColourScheme.skyColour, colourScheme.skyColour, LerpAmount);
        RenderSettings.ambientEquatorColor = Color.Lerp(oldColourScheme.horizonColour, colourScheme.horizonColour, LerpAmount);
        RenderSettings.ambientGroundColor = Color.Lerp(oldColourScheme.groundColour, colourScheme.groundColour, LerpAmount);
        RenderSettings.ambientIntensity = Mathf.Lerp(oldColourScheme.intensity, colourScheme.intensity, LerpAmount);

        float twistSpeed = Mathf.Lerp(oldColourScheme.twistPerSecond, colourScheme.twistPerSecond, 1/*lerpAmount*/);
        float offsetSpeed = Mathf.Lerp(oldColourScheme.offsetPerSecond, colourScheme.offsetPerSecond, 1/*lerpAmount*/);
        CubeTunnle.FlashColor = Color.Lerp(oldColourScheme.flashColor, colourScheme.flashColor, LerpAmount);

        CubeTunnle.DefinedRotationSpeedMultiplier = Mathf.Lerp(oldColourScheme.cubeRotationSpeed, colourScheme.cubeRotationSpeed, LerpAmount);

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
