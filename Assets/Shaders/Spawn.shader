Shader "Unlit/Spawn"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Location("Location",Float)=0
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
				float2 uv2 : TEXCOORD2;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Location;
			v2f vert (appdata v)
			{
				v2f o;
				/*
				float3 worldPos = float3(
					_Object2World[0][3], 
					_Object2World[1][3],
					_Object2World[2][3]); // mul(_Object2World, v.vertex);
					*/
				float xpos = abs(v.uv2.x * 2 - 1)*0.5;
				float dist = v.uv2.y+xpos-_Location;//length(worldPos);
				float t = saturate(1 - (dist*0.6 - 3));
				v.vertex *= t;
				v.vertex.z -= 1 - t*2;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
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
				if (i.uv.x > 0.95 || i.uv.y > 0.95 || i.uv.x < sizeL || i.uv.y < sizeL) {
					return fixed4(.1, .1, 1, 1);
				}
				return fixed4(0, 0, 0, 1);
			}
			ENDCG
		}
	}
}
