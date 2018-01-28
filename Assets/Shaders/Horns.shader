Shader "Unlit/Horns"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_T("t",Float) = 0
			[HDR]
		_Color1("color1",Color) = (0,1,0,1)
			[HDR]
		_Color2("color2",Color) = (0,1,0,1)
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
			float _T;
			float4 _Color1;
			float4 _Color2;
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
				float c = tex2D(_MainTex,i.uv);
				float t = saturate(_T - (i.uv.x)*10);
				float segments = sin(i.uv.x * 200)*0.2 + 0.8*sin(i.uv.y*200)*0.5+0.5;
				return _Color1*segments*c*t*(sin(_Time.x * 100)*0.2 + 0.8);
			}
			ENDCG
		}
	}
}
