// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Whale" {
	Properties {
		[HDR]
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_GlowMap("GlowMap", 2D) = "Black" {}
		[HDR]
		_GlowColorA("GlowColorA", Color) = (1,1,1,1)
		[HDR]
		_GlowColorB("GlowColorB", Color) = (1,1,1,1)
		_T("T",Float) = 0
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		#pragma vertex myvert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float seed;
			float3 localPos;
		};
			
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		sampler2D _GlowMap;
		fixed4 _GlowColorA;
		fixed4 _GlowColorB;
		float _T;
		fixed4 _G_Glow;
		void myvert(inout appdata_full v,out Input i)
		{
			i.uv_MainTex = float2(1, 1);
			i.localPos = v.vertex;
			i.seed = length	(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x))*500;
			v.vertex.y += sin(v.vertex.z+_Time.x*20)*0.5*pow(v.vertex.z-2,2)*0.05;
			v.vertex.y += sin(_Time.x * 20)*0.5;
			v.vertex.x += sin(_Time.x * 30)*0.1;
			//float t = saturate((-v.vertex.y) - .3);
			//v.vertex.x = lerp(v.vertex.x,v.vertex*sin(_Time.x*30+t)*(t + 1)*0.3,t);
			i.seed = sin(i.seed * 1000)*0.5 + 0.5;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {/*
			float l1 = distance(_Pos1.xyz, IN.worldPos);
			float l2 = distance(_Pos2.xyz, IN.worldPos);
			fixed4 pColor;
			if (l1 < 30 && l1<l2) {
				pColor = _Color1;
			}
			else if (l2 < 30) {
				pColor = _Color2;
			}
			else {
				pColor = _GlowColor;
			}*
			float div = l1 + l2;
			l1 /= div;*/
			float sweep = pow(sin(IN.localPos.z * 1.5 + _Time.x*50)*0.5 + 0.5,5)+0.1;
			fixed4 g = tex2D(_GlowMap, IN.uv_MainTex)*_G_Glow;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex)*_Color;
			o.Albedo = c.rgb;
			o.Emission = g.xyz*sweep;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
