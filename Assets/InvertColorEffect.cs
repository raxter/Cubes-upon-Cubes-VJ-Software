using UnityEngine;
using System.Collections;

public class InvertColorEffect : MonoBehaviour {


	public Shader imageEffect;

	Material m;

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (m == null)
			m = new Material (imageEffect);

		Graphics.Blit (src, dst, m);
	}
}
