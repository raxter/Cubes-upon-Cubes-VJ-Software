using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatFinder : MonoBehaviour
{
    public float bpm_display = 100;

    public static float BPM = 100;
    public static bool beatHitThisFrame = false;

    public float SecondsPerBeat { get { return 60 / BPM; } }

    float lastHitTime = 0;

    List<float> lastHitTimes = new List<float>();

    float firstRecordTime = 0;
    public float oldBeatsSinceFirstRecord = 0;
    public float beatsSinceFirstRecord = 0;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.LeftControl))
                lastHitTimes.Clear();

            lastHitTime = Time.time;
            lastHitTimes.Add(lastHitTime);
            Debug.Log("Recording Hit "+ Time.time);
            
            firstRecordTime = lastHitTimes[0];
            if (lastHitTimes.Count > 1)
            {
                Debug.Log("Estimating BPM");
                float totalAverageDistances = 0;
                for (int i = 0; i < lastHitTimes.Count - 1; i++)
                    totalAverageDistances += (lastHitTimes[i + 1] - lastHitTimes[i]);
                totalAverageDistances /= (lastHitTimes.Count - 1);
                BPM = 60 / totalAverageDistances;
            }
        }
        if (Time.time - lastHitTime > SecondsPerBeat*4)
        {
            lastHitTimes.Clear();
        }

        oldBeatsSinceFirstRecord = beatsSinceFirstRecord;
        beatsSinceFirstRecord = (Time.time - firstRecordTime) / SecondsPerBeat;

        if (oldBeatsSinceFirstRecord % 1 > beatsSinceFirstRecord % 1)
            beatHitThisFrame = true;
        else
            beatHitThisFrame = false;

        bpm_display = BPM;
    }
}
