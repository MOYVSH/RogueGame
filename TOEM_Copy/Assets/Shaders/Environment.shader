//SRPºÊ»›

Shader "URPUnlitTemplate"
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
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
		#pragma multi_compile_instancing
		#pragma multi_compile _ DOTS_INSTANCING_ON		


		CBUFFER_START(UnityPerMaterial)
		half4 _BaseColor;
		float4 _BaseMap_ST;
		CBUFFER_END
		
		TEXTURE2D(_BaseMap);
		SAMPLER(sampler_BaseMap);
		TEXTURE2D(_EmissionMap);
		SAMPLER(sampler_EmissionMap);
		// NOTE: Do not ifdef the properties for dots instancing, but ifdef the actual usage.
		// Otherwise you might break CPU-side as property constant-buffer offsets change per variant.
		// NOTE: Dots instancing is orthogonal to the constant buffer above.
        // DOTS instancing definitions

		#ifdef UNITY_DOTS_INSTANCING_ENABLED
		UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
		UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
		UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)
		#define _BaseColor UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(half4 , Metadata_BaseColor)
		#endif
		ENDHLSL

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.5
			#pragma multi_compile_instancing
			#pragma multi_compile _ DOTS_INSTANCING_ON

			struct Attributes
			{
				float3 positionOS:POSITION;
				half2 uv:TEXCOORD0;
				#if UNITY_ANY_INSTANCING_ENABLED
				uint instanceID : INSTANCEID_SEMANTIC;
				#endif
			};
			
			struct Varyings
			{
				float4 positionHCS:SV_POSITION;
				half2 uv:TEXCOORD0;
				#if UNITY_ANY_INSTANCING_ENABLED
				uint instanceID : CUSTOM_INSTANCE_ID;
				#endif
			};			
		
			
			Varyings vert(Attributes IN)
			{
				Varyings o;				
				o.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
				o.uv = TRANSFORM_TEX(IN.uv,_BaseMap);		
				#if UNITY_ANY_INSTANCING_ENABLED
				o.instanceID = IN.instanceID;
				#endif		
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

		//pass 
		//{
		//	Tags{ "LightMode" = "ShadowCaster" }

		//	HLSLPROGRAM
		//	#pragma vertex vertShadow
		//	#pragma fragment fragShadow
		//	#pragma target 4.5
		//  #pragma multi_compile_instancing
		//	#pragma multi_compile _ DOTS_INSTANCING_ON
		//	struct appdata
		//	{
		//		float4 vertex : POSITION;
		//		half2 uv:TEXCOORD0;
		//	};
 
		//	struct v2f
		//	{
		//		float4 pos : SV_POSITION;
		//		half2 uv:TEXCOORD0;
		//	};
 
		//	v2f vertShadow(appdata v)
		//	{
		//		v2f o;
		//		o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
		//		o.uv = TRANSFORM_TEX(v.uv,_BaseMap);			
		//		return o;
		//	}
		//	float4 fragShadow(v2f i) : SV_Target
		//	{
		//		float4 color;
		//		color.xyz = float3(0.0, 0.0, 0.0);
		//		half4 alpha = SAMPLE_TEXTURE2D(_EmissionMap,sampler_EmissionMap,i.uv);
		//		clip(alpha.r - 0.99);
		//		return color;
		//	}
		//	ENDHLSL
		//}
	}
}
