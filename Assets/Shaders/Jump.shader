Shader "Unlit/Jump"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_MainTex2("Texture", 2D) = "white" {}
	_Mask("Texture", 2D) = "white" {}
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
				float3 pos : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MainTex2;
			sampler2D _Mask;
			float _Offset;
			float4 _Color;
			float _Jumping;
			float _Lean;
			v2f vert (appdata v)
			{
				v2f o;
				float4 ball = float4(normalize(v.vertex.xyz),0);
				float4 vert = v.vertex;
				float leg = saturate(0.8-vert.y)*saturate(_Jumping)*0.7	;
				float lOff = _Offset+3.141529*0.5;
				float speed = 1;
				vert.y += leg*abs(cos(lOff * speed + vert.z))*0.8;
				vert.z += leg*sin(lOff*speed*2+vert.z);
				//vert.x *= (1 + leg*cos(lOff*speed*2 + vert.z+3.141529*0.5));
				vert.y += abs(sin(vert.z*0.3+_Offset))*_Jumping; 
				vert.x += (vert.y)*_Lean;
				o.pos = vert.xyz;
				o.vertex = UnityObjectToClipPos(vert);
				o.normal = normalize(UnityObjectToClipPos(v.normal));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float col = tex2D(_MainTex, i.uv).r*0.5;
				float colAnim = tex2D(_MainTex2, i.uv).r;
				float mask = tex2D(_Mask, i.uv);
				colAnim = (fmod(_Time.x * 20 + colAnim*-15,1)*0.1)*mask;
				col += colAnim*0.5;
				return col*_Color;
			}
			ENDCG
		}
	}
}
