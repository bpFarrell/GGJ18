Shader "Unlit/Jump"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[HDR]
		_Color ("Color",Color) = (0,1,0,1)
		_Offset("Offset",Float) = 0
		_Jumping("Jumping",Float) = 1
		_Lean("Leaning",Float)=0
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
				float3 normal :NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Offset;
			float4 _Color;
			float _Jumping;
			float _Lean;
			v2f vert (appdata v)
			{
				v2f o;
				float4 ball = float4(normalize(v.vertex.xyz),0);
				float4 vert = v.vertex;
				vert.y += abs(sin(vert.z*0.3+_Offset))*_Jumping; 
				vert.x += (vert.y)*_Lean;
				o.vertex = UnityObjectToClipPos(vert);
				o.normal = normalize(UnityObjectToClipPos(v.normal));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return pow(abs(col.r*2-1),1.5)*_Color*(sin(_Time.x*100)*0.1+0.9);
			}
			ENDCG
		}
	}
}
