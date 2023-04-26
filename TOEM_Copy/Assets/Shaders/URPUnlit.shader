//SRPºÊ»›

Shader "Hidden/URPUnlit"
{
	Properties
	{
		_BaseMap("Base Map",2D) = "white"{}
		_EmissionMap("Emission Map",2D) = "white"{}
		_BaseColor("Base Color",Color) = (1,1,1,1)
	}
	SubShader
	{
		tags{"RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}
		
		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		CBUFFER_START(UnityPerMaterial)	
		TEXTURE2D(_BaseMap);
		SAMPLER(sampler_BaseMap);
		TEXTURE2D(_EmissionMap);
		SAMPLER(sampler_EmissionMap);


		half4 _BaseColor;
		float4 _BaseMap_ST;
		CBUFFER_END
		ENDHLSL 

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			
			struct Attributes
			{
				float3 positionOS:POSITION;
				half2 uv:TEXCOORD0;
			};
			
			struct Varyings
			{
				float4 positionHCS:SV_POSITION;
				half2 uv:TEXCOORD0;
			};			
		
			
			Varyings vert(Attributes IN)
			{
				Varyings o;				
				o.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
				o.uv = TRANSFORM_TEX(IN.uv,_BaseMap);				
				return o;
			}
			
			half4 frag(Varyings IN):SV_Target
			{
				half4 color = SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,IN.uv);
				half4 alpha = SAMPLE_TEXTURE2D(_EmissionMap,sampler_EmissionMap,IN.uv);
				clip(alpha.r - 0.99);
				return half4(color.rgb , alpha.r * color.a);
			}
			ENDHLSL
		}

		pass 
		{
			Tags{ "LightMode" = "ShadowCaster" }

			HLSLPROGRAM
			#pragma vertex vertShadow
			#pragma fragment fragShadow
 
			struct appdata
			{
				float4 vertex : POSITION;
				half2 uv:TEXCOORD0;
			};
 
			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv:TEXCOORD0;
			};
 
			v2f vertShadow(appdata v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				o.uv = TRANSFORM_TEX(v.uv,_BaseMap);			
				return o;
			}
			float4 fragShadow(v2f i) : SV_Target
			{
				float4 color;
				color.xyz = float3(0.0, 0.0, 0.0);
				half4 alpha = SAMPLE_TEXTURE2D(_EmissionMap,sampler_EmissionMap,i.uv);
				clip(alpha.r - 0.99);
				return color;
			}
			ENDHLSL
		}
	}
}
