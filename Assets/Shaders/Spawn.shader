﻿Shader "Unlit/Spawn"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Location("Location",Float)=0
		_Length("Length",Float)=1
		[HDR]
		_Color("Color",Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Off
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
				float2 uv2 : TEXCOORD2;
				float3 clr : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 :TEXCOORD2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Location;
			float _Length;
			float4 _Color;
			v2f vert (appdata v)
			{
				v2f o;
				float xpos = abs(v.uv2.x * 2 - 1)*1;
				float dist = (v.uv2.y+xpos*10 - _Location);
				float t = saturate(1 - (dist - 2));
				t = saturate((dist - _Location) / _Length);
				v.vertex.xyz += v.clr *(1 - t)*5.;
				v.vertex *= t;
				v.vertex.y += 1 - t*2+1;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = v.uv2;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				float sizeL = 0.05;
				float sizeH = 0.95;
				float wave = sin(i.uv2.y*0.2 + abs(i.uv2.x-0.5)*-8 + _Time*100)*0.4+0.6;
				_Color *= wave+ pow(abs(i.uv2.x - 0.5),3)*10;
				if (i.uv.x > 0.95 || i.uv.y > 0.95 || i.uv.x < sizeL || i.uv.y < sizeL) {
					return _Color;
				}
				return fixed4(0, 0, 0, 1);
			}
			ENDCG
		}
	}
}
