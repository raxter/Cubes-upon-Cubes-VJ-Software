using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatFinder : MonoBehaviour
{
    /*public float bpm_display = 100;

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
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Return))
        {
			if (Input.GetKey(KeyCode.Return))
                lastHitTimes.Clear();

			lastHitTime = Time.time - Time.smoothDeltaTime;
            lastHitTimes.Add(lastHitTime);
			Debug.Log("Recording Hit "+ lastHitTime);
            
			firstRecordTime = lastHitTimes[lastHitTimes.Count-1];
            if (lastHitTimes.Count > 1)
            {
                Debug.Log("Estimating BPM");
                float totalAverageDistances = 0;
                for (int i = 0; i < lastHitTimes.Count - 1; i++)
                    totalAverageDistances += (lastHitTimes[i + 1] - lastHitTimes[i]);
                totalAverageDistances /= (lastHitTimes.Count - 1);
				BPM = Mathf.RoundToInt(60 / totalAverageDistances);
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
    }*/


	public static bool beatHitThisFrame = false;

	public float kBeatLerpSeconds = 1;

	public int		_tapCount;		// number of taps
	public float	_bpm = 60;			// beats per minute
	public float	_lengthOfBeat = 1;	// length of one beat in seconds
	 
	public float	_beatPerc;
	public float	_beatTime;
	public float	oldBeatTime;
	 
	public float startTime;

    public static float linear1;

    public static float sin1;
	public static float sin2;
	public static float sin4;
	public static float sin8;
	public static float sin16;

	public static float cos1;
	public static float cos2;
	public static float cos4;
	public static float cos8;
	public static float cos16;

	void Start()
	{
		startFresh ();
	}

	float lastCallTime;

	void startFresh() {
		Debug.Log("BPM::startFresh\n");
		_tapCount		= 1;
		startTime = Time.time;
		lastCallTime = Time.time;

		//		float iBeatTime;
		//		float fBeatTime;
		//		fBeatTime = modf(_beatTime, &fBeatTime);
		//		_beatTime = (int)iBeatTime % 16 + fBeatTime;
	}

	public static float beatStartTimeOffsetHack = 0;

	//--------------------------------------------------------------
	void Update() {

		if (Input.GetKeyDown (KeyCode.Return)|| XboxCtrlrInput.XCI.GetButtonDown(XboxCtrlrInput.XboxButton.B)) {
			beatStartTimeOffsetHack = 0;
			startFresh ();
		}
		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.Backslash) || XboxCtrlrInput.XCI.GetButtonDown(XboxCtrlrInput.XboxButton.A))
			tap ();
		

		float beatStartTime = getSeconds ();

		oldBeatTime = _beatTime;

		_beatTime			= beatStartTime / _lengthOfBeat;
		_beatPerc			= _beatTime - Mathf.Floor(_beatTime);

		float lerpSpeed;
		if(beatStartTime < kBeatLerpSeconds) {
			lerpSpeed = Mathf.InverseLerp(0, kBeatLerpSeconds, beatStartTime);
			//Debug.Log("lerpSpeed: \t"+ lerpSpeed);
		} else {
			lerpSpeed = 1;
		}

        if (_beatPerc < linear1)
            linear1 = _beatPerc;
        else
            linear1 += (_beatPerc - linear1) * lerpSpeed;

        sin1	+= (beatSinS(1) - sin1) * lerpSpeed;
		sin2	+= (beatSinS(2) - sin2) * lerpSpeed;
		sin4	+= (beatSinS(4) - sin4) * lerpSpeed;
		sin8	+= (beatSinS(8) - sin8) * lerpSpeed;
		sin16	+= (beatSinS(16) - sin16) * lerpSpeed;

		cos1	+= (beatCosS(1) - cos1) * lerpSpeed;
		cos2	+= (beatCosS(2) - cos2) * lerpSpeed;
		cos4	+= (beatCosS(4) - cos4) * lerpSpeed;
		cos8	+= (beatCosS(8) - cos8) * lerpSpeed;
		cos16	+= (beatCosS(16) - cos16) * lerpSpeed;




		if (oldBeatTime % 1 > _beatTime % 1)
			beatHitThisFrame = true;
		else
			beatHitThisFrame = false;
	}


	//--------------------------------------------------------------
	void tap() {
		float timeSinceLastCall = getSecondsSinceLastCall();

		if(timeSinceLastCall > 4) {
			startFresh();

		} else {
			_lengthOfBeat = getSeconds() / _tapCount;
			_bpm = 60.0f / _lengthOfBeat;
			_tapCount++;
		}

		Debug.Log("BPM "+ _tapCount+" " +_bpm);

	}

	float getSeconds()
	{
		return Time.time - startTime;
	}

	float getSecondsSinceLastCall() {
		float nowTime = Time.time;
		float diff = nowTime - lastCallTime;
		lastCallTime = nowTime;
		return diff;
	}

	//--------------------------------------------------------------
	float bpm() {
		return _bpm;
	}


	//--------------------------------------------------------------
	float beatTime() {
		return _beatTime;
	}


	//--------------------------------------------------------------
	float beatPerc() {
		return _beatPerc;
	}

	//--------------------------------------------------------------
	float beatSinU(float beats) {
		return 0.5f + 0.5f * beatSinS(beats);
	}

	//--------------------------------------------------------------
	float beatSinS(float beats) {
		return Mathf.Sin(beatTime() * Mathf.PI*2/beats);
	}

	//--------------------------------------------------------------
	float beatCosU(float beats) {
		return 0.5f + 0.5f * beatCosS(beats);
	}

	//--------------------------------------------------------------
	float beatCosS(float beats) {
		return Mathf.Cos(beatTime() * Mathf.PI*2/beats);
	}

	//--------------------------------------------------------------
	void setBpm(float bpm) {
		_bpm = bpm;
		_lengthOfBeat = 60f/_bpm;
	}


}
