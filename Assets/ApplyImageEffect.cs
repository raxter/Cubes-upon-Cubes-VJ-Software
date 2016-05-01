using UnityEngine;
using System.Collections;

public class ApplyImageEffect : MonoBehaviour
{
    public float width = 10;
    public float height = 10;
    public float size = 0.8f;

    public Shader imageEffect;

    Material m = null;

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m == null)
        {
            m = new Material(imageEffect);
        }
        m.SetFloat("_Width", width);
        m.SetFloat("_Height", height);
        m.SetFloat("_Size", size);

        Graphics.Blit(source, destination, m);
    }
}
