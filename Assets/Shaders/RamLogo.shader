﻿Shader "Unlit/RamLogo"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Meta("Meta", 2D) = "white" {}
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
			sampler2D _Meta;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 meta = tex2D(_Meta, i.uv);
				float t = (meta.y+meta.z)*col.w*(sin(i.uv.x*-10+_Time.x*20)*0.5+0.5);
				//t += meta.z*col.w*(sin(i.uv.y*20+_Time*10)*0.5+0.5)*0.5;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col+t*0.5;
			}
			ENDCG
		}
	}
}
