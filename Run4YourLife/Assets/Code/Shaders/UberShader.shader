// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/UberShader"
{
	Properties
	{
		_MainTex("Albedo Texture", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_RectangleTop("RectangleTop",Float)=200
		_RectangleBot("RectangleBot", Float) = 0
		_RectangleX("RectangleX", Float) = 100
	}

		SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 worldpos : TEXCOORD1;
			};

			float _RectangleTop;
			float _RectangleBot;
			float _RectangleX;

			sampler2D _MainTex;

			// vertex shader
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				OUT.worldpos = mul(unity_ObjectToWorld, IN.vertex);

				return OUT;
			}

			// fragment shader
			fixed4 frag(v2f IN) : COLOR
			{
				half4 tex = tex2D(_MainTex, IN.texcoord) * IN.color;
				if(IN.worldpos.x < _RectangleX)
				{
					tex.a = 0;
				}

				return tex;
			}
			ENDCG
		}
	}
}
