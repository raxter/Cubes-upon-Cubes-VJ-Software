using UnityEngine;
using System.Collections;

public class ZineImageEffect : MonoBehaviour
{

    public Shader imageEffect;
    Material m;

    public Color pink = new Color(1, 0.5f, 0.5f);
    public Color blue = new Color(0.2f, 0.2f, 1);
    public bool preview = false;

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m == null)
        {
            m = new Material(imageEffect);
        }

        m.SetColor("_Pink", pink);
        m.SetColor("_Blue", blue);
        m.SetFloat("_Preview", preview ? 1 : 0);

        Graphics.Blit(source, destination, m);
    }
}
