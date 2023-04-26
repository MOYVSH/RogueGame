Shader "Unlit/2DDiffuse&Outline"
{
	Properties
	{
		_MainTex("Base Map",2D) = "white"{}
		_BaseColor("Base Color",Color) = (1,1,1,1)
		_SpecularColor("Specular Color",Color)=(1,1,1,1)
		_LineWidth("Line Width",Range(0,10)) = 1
		[HDR]_OutlineColor("Line Color",Color) = (1,1,1,1)

		_Smoothness("Smoothness",float)=10
        _Cutoff("Cutoff",float)=0.5

		//_DefaultAlpha ("Default Alpha",Range(0,1)) = 0.5
	}
	SubShader
	{
		tags{"Queue" = "Transparent" "RenderPipeline"="UniversalPipeline"}

		Blend SrcAlpha OneMinusSrcAlpha

		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

		CBUFFER_START(UnityPerMaterial)	
		TEXTURE2D(_MainTex);
		SAMPLER(sampler_MainTex);
		float4 _MainTex_ST;

		float _LineWidth;

		half4 _BaseColor;
		half4 _SpecularColor;
		half4 _OutlineColor;

		//float _DefaultAlpha;

		float _Smoothness;
        float _Cutoff;

		float4 _MainTex_TexelSize; //Unity 提供的每个像素的尺寸
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
				float4 normalOS : NORMAL;
				half2 uv:TEXCOORD0;
			};
			
			struct Varyings
			{
				float4 positionHCS:SV_POSITION;
				half2 uv:TEXCOORD0;
				float3 positionWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float3 normalWS : TEXCOORD3;
			};			
		
			
			Varyings vert(Attributes IN)
			{
				Varyings OUT;				
				OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
				OUT.uv = TRANSFORM_TEX(IN.uv,_MainTex);	
				
				VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS.xyz);
                OUT.positionWS = positionInputs.positionWS;
                OUT.viewDirWS = GetCameraPositionWS() - positionInputs.positionWS;
                OUT.normalWS = normalInputs.normalWS;
				return OUT;
			}
			
			half4 frag(Varyings IN):SV_Target
			{
				half4 mainTex = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,IN.uv);

				////计算主光
    //            Light light = GetMainLight();
    //            half3 diffuse = LightingLambert(light.color, light.direction, IN.normalWS);
    //            half3 specular = LightingSpecular(light.color, light.direction, normalize(IN.normalWS), normalize(IN.viewDirWS), _SpecularColor, _Smoothness);
    //            //计算附加光照
    //            uint pixelLightCount = GetAdditionalLightsCount();
    //            for (uint lightIndex = 0; lightIndex < pixelLightCount; ++lightIndex)
    //            {
    //                Light light = GetAdditionalLight(lightIndex, IN.positionWS);
    //                diffuse += LightingLambert(light.color, light.direction, IN.normalWS);
    //                specular += LightingSpecular(light.color, light.direction, normalize(IN.normalWS), normalize(IN.viewDirWS), _SpecularColor, _Smoothness);
    //            }

    //            half3 color = mainTex.xyz * diffuse * _BaseColor + specular;

				//color *= _BaseColor;

				 half3 color = mainTex.rgb;

				if(_LineWidth == 0)
					return half4(color.rgb, mainTex.a);
				// 采样周围4个点
				float2 up_uv = IN.uv + float2(0,1) * _LineWidth * _MainTex_TexelSize.xy;
				float2 down_uv = IN.uv + float2(0,-1) * _LineWidth * _MainTex_TexelSize.xy;
				float2 left_uv = IN.uv + float2(-1,0) * _LineWidth * _MainTex_TexelSize.xy;
				float2 right_uv = IN.uv + float2(1,0) * _LineWidth * _MainTex_TexelSize.xy;
				// 如果有一个点透明度为0 说明是边缘
				float w = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,up_uv).a *
						  SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,down_uv).a * 
						  SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,left_uv).a * 
						  SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,right_uv).a;
				// 和原图做插值
				mainTex.rgb = lerp(_OutlineColor, color, w);
				return mainTex;
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
				o.uv = TRANSFORM_TEX(v.uv,_MainTex);			
				return o;
			}
			float4 fragShadow(v2f i) : SV_Target
			{
				float4 color;
				color.xyz = float3(0.0, 0.0, 0.0);
				return color;
			}
			ENDHLSL
		}
	}
}
