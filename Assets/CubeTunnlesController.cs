using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeTunnlesController : MonoBehaviour {

    public List<CubeTunnle> tunnles = new List<CubeTunnle>();

    public Gradient gradient;

    public bool updateEveryFrame = true;

	// Use this for initialization
	void Start ()
    {
        UpdateColours();

    }

    void UpdateColours()
    {
        float diff = 1f / tunnles.Count;
        for (int i = 0; i < tunnles.Count; i++)
        {
            float f = (float)i / tunnles.Count;
            tunnles[i].SetColour(gradient, f, diff);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (updateEveryFrame)
            UpdateColours();

    }
}
