using UnityEngine;
using System.Collections;

public class ScreenShotUtility : MonoBehaviour
{
    
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            string filename = System.DateTime.Now.ToString("yyyy-MM-dd_HHmm_ss") + ".png";
            Debug.Log(filename);
            Application.CaptureScreenshot(filename);
        }
	
	}
}
