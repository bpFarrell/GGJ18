﻿Shader "Unlit/Skybox"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col1 = fixed4(0.2,0.2,1.,1.);
				fixed4 col2 = fixed4(0.8,0.2,0.8,1.);
				float black = 1 - saturate(pow(i.uv.y, 2)*3);
				//i.uv.x += -_Time.x*10;
				float stars = tex2D(_MainTex, i.uv)*pow((sin(_Time.x*40+i.uv.x*100+i.uv.y*10)*0.5+0.7),3);
				///stars *=  i.uv.x;
				//stars = 0;
				//return fixed4(i.uv, 0, 1);
				return lerp(col1,col2,i.uv.y+0.7)*saturate(black+stars*2);
			}
			ENDCG
		}
	}
}
