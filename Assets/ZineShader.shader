Shader "Hidden/ZineShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[MaterialToggle] _Preview("Preview Actual Colours", Float) = 0
		_Pink("Pink", Color) = (1,0.5,0.5,1)
		_Blue ("Blue", Color) = (0.2,0.2,1,1)
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float _Preview;
			float4 _Pink;
			float4 _Blue;
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float brightness = sqrt(0.299*col.r * col.r + 0.587*col.g*col.g + 0.114*col.b*col.b);
				//col.rgb = brightness.rrr;
				col = float4(col.r, 0, col.b, 1);
				float4 one = float4(1, 1, 1, 1);
				col = lerp(col, one - ((one - _Pink)*col.r + (one - _Blue) * col.b), _Preview);
				col.a = 1;
				return col;
			}
			ENDCG
		}
	}
}
