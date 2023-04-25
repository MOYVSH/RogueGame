Shader "Unlit/Billboard"
{
	Properties 
	{
		_MainTex ("Main Tex", 2D) = "white" {}
		[HDR]_Color ("Color Tint", Color) = (1, 1, 1, 1)
		_VerticalBillboarding ("Vertical Restraints", Range(0, 1)) = 1 
	}
	SubShader 
	{
		Tags {"RenderPipeline" = "UniversalPipeline" "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "DisableBatching"="True"}
        //ȡ������������һ���������½�
		
		Pass 
		{ 
			Tags { "LightMode"="UniversalForward" }
			
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
            //���������ڶ��㶯��1�У���֮ǰ���������˽���
		
			HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			
			#pragma vertex vert
			#pragma fragment frag

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
			half4 _Color;
			float _VerticalBillboarding;
            CBUFFER_END

            TEXTURE2D(_MainTex);       
			SAMPLER(sampler_MainTex);

            struct a2v 
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			struct v2f 
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert (a2v v) 
			{
				v2f o;
				
				// ��ҪЧ����ģ�Ϳռ�ʵ�֣�������������̶�ģ�Ϳռ��е�ê�㣨��ԭ�㣩
				float3 center = float3(0, 0, 0);
                // �ѹ۲췽��任��ģ�Ϳռ�
				float3 viewer = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1));
				
				float3 normalDir = viewer - center;

				normalDir.y = normalDir.y * _VerticalBillboarding;
                // �˴���ʽ���Ƚϳ��󣬽����Լ���ʾ��ͼ��⡣���˵���������_VerticalBillboardingΪ1�����߷�����Զ���ڹ۲췽��
				// �۲췽��ı෨���������ȸı࣬����Զ����۲�ģ�͡���_VerticalBillboardingΪ0�����߷���0��0, 1������UP����0��1, 0����ֱ��
				// �����ɷ��ߺ�right����õ���up'��Զָ��0, 1��0����
				normalDir = normalize(normalDir);
				float3 upDir = abs(normalDir.y) > 0.999 ? float3(0, 0, 1) : float3(0, 1, 0);
                //��ֹ���߷�������Ϸ���ƽ�У����ƽ�У���ô����õ��Ľ�����Ǵ���ģ�
				float3 rightDir = normalize(cross(upDir, normalDir));
				upDir = normalize(cross(normalDir, rightDir));
				
				float3 centerOffs = v.vertex.xyz - center;
				float3 localPos = center + rightDir * centerOffs.x + upDir * centerOffs.y + normalDir * centerOffs.z;
                //�����ǰ�ģ�Ϳռ��µĶ�������������õ���������ʸ�����о������㣬�õ��µĿռ�λ��
              
				o.pos = TransformObjectToHClip(float4(localPos, 1));
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);

				return o;
			}
			
			half4 frag (v2f i) : SV_Target {
				half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				c.rgb *= _Color.rgb;
				
				return c;
			}
			
			ENDHLSL
		}
	} 
    FallBack "Packages/com.unity.render-pipelines.universal/FallbackError"
}
