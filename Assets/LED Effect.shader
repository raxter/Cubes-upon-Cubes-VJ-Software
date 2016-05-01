Shader "Hidden/LED Effect"
{
	Properties
	{
			_MainTex("Texture", 2D) = "white" {}
			_Width("Width",  float) = 0
			_Height("Height", float) = 0
			_Size("Size", float) = 0.8
	}

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float _Width;
			float _Height;
			float _Size;

			fixed4 frag(v2f i) : SV_Target
			{
				float widthh = _Width;
				float height = _Height;
				float2 modPosition = float2(
					fmod(i.uv.x, 1/(widthh+1)) * widthh,
					fmod(i.uv.y, 1/(height+1)) * height
				);

				float dotFactor = step(length(modPosition - float2(0.5, 0.5)), _Size/2);
					//step(length(float2(fmod(i.uv.x, 1 / width), fmod(i.uv.y, 1 / height)) - float2(0.5 / width, 0.5 / height)), 0.5 / width);

				fixed4 col = tex2D(_MainTex, floor(i.uv*float2(widthh+1, height+1))/float2(widthh, height));

				//col = float4(modPosition, 0, 1);
				//col = float4(dotFactor, dotFactor, dotFactor, 1);
				return col*dotFactor;
			}
			ENDCG
		}
	}
}
