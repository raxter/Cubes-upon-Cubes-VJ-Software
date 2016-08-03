using UnityEngine;
using System.Collections;

public class MaterialController : MonoBehaviour 
{

	public Material baseMaterial;

	public int gradientCount = 64;
	Material [] gradients;

	static MaterialController instance;
	void Awake()
	{
		instance = this;
	}

	public static Material GetColor(float lerpAmount)
	{
		//Debug.Log (Mathf.RoundToInt(lerpAmount * instance.gradients.Length));
		return instance.gradients[Mathf.RoundToInt(lerpAmount * (instance.gradients.Length-1))];
	}

	public static void SetGradient(Gradient g1, Gradient g2, float lerpAmount)
	{
		instance.SetGradientInternal (g1, g2, lerpAmount);
	}

	public void SetGradientInternal(Gradient g1, Gradient g2, float lerpAmount)
	{
		if (gradients == null || gradients.Length != gradientCount) 
		{
			if (gradients != null)
				foreach (Material m in gradients)
					Destroy (m);
			gradients = new Material[gradientCount];
			for (int i = 0; i < gradientCount; i++) 
			{
				gradients[i] = new Material(baseMaterial);
				gradients[i].name += " "+i+"/"+gradientCount;
			}
		}


		for (int i = 0; i < gradientCount; i++) 
		{
			float f = (float)i / (gradientCount - 1);
			Color c = Color.Lerp(g1.Evaluate(f), g2.Evaluate(f), lerpAmount);
			gradients [i].SetColor ("_Color", c);
		}
	}

}
