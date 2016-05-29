using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphereGrid : MonoBehaviour
{
    public GameObject spherePrefab;

    public int width = 10;
    public int height = 10;

    public float spacing = 1;
	#if UNITY_EDITOR
    [ContextMenu("Update Grid")]
    void UpdateGrid()
    {
        if (Application.isPlaying)
            return;

        int safeCount = 100000;
        while(transform.childCount > 0)
        {
            //Debug.Log(transform.childCount);
            DestroyImmediate(transform.GetChild(0).gameObject);
            safeCount--;
            if (safeCount < 0)
            {
                Debug.Log("Limiting out");
                break;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject go = UnityEditor.PrefabUtility.InstantiatePrefab(spherePrefab) as GameObject;

                go.transform.parent = transform;
                float xP = (float)x - ((width - 1f) / 2);
                float yP = (float)y - ((height - 1f) / 2);
                go.transform.localPosition = new Vector3(xP, yP) * spacing;

                float u = (float)x / (width - 1);
                float v = (float)y / (height - 1);

                MeshFilter meshFilter = go.GetComponent<MeshFilter>();
                Color c = new Color(u,v,0,0);
                List<Color> colors = new List<Color>();
                for (int i = 0; i < meshFilter.sharedMesh.vertexCount; i++)
                {
                    colors.Add(c);
                }
                meshFilter.mesh.SetColors(colors);

                //Material m = new Material(go.GetComponent<MeshRenderer>().sharedMaterial);
                //m.SetFloat("_U", u);
                //m.SetFloat("_V", v);
                //go.GetComponent<MeshRenderer>().sharedMaterial = m;
            }
        }
    }
	#endif
    void Update()
    {

        if (!Application.isPlaying)
            return;

    }

}
