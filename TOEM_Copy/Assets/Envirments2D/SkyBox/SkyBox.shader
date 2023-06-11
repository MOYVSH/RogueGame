Shader "Unlit/SkyBox"
{
Properties

    {
        [HDR] _Color("Color",Color)=(1,1,1,1)  

        [HDR] _SunColor("SunColor",Color)=(0,0,0,1) 
        _SunRadius ("SunRadius",Range(0,1)) = 1
        [HDR] _MoonColor("MoonColor",Color)=(0,0,0,1) 
        _MoonRadius ("MoonRadius",Range(0,1)) = 1
        _CrescentOffset ("_CrescentOffset",Range(-1,1)) = 1
        _MainTex("MainTex",2D)="white"{}
        _MoonTex("MoonTex",2D)="white"{}
    }

    SubShader
    {
        //��Ҫ������Ⱦ����
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }

        LOD 100
        Pass
        {

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            //ǿ�Ƶ���HLSL
            #pragma prefer_hlslcc gles 
            #pragma target 3.0

            //���õ�����includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            //����
            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float4 _SunColor;
            float _SunRadius;
            float4 _MoonColor;
            float _MoonRadius;
            float _CrescentOffset;
            CBUFFER_END

            TEXTURE2D(_MainTex);//������
            float4 _MainTex_ST;
            //SAMPLER(sampler_MainTex);//����������
            #define smp _linear_repeat //��������������Ե� ��������
            SAMPLER(smp);//������

            TEXTURE2D(_MoonTex);//������
            float4 _MoonTex_ST;
            SAMPLER(sampler_MoonTex);//����������

            float4x4 LtoW;


            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv :TEXCOORD;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 uv :TEXCOORD0;
            };

            //Functions
            float SunDistance(half3 pos)
            {
                float sun = distance(pos, _MainLightPosition);
                float sunDis = saturate((1 - sun / _SunRadius) * 50);
                return sunDis;
            }

            float MoonDistance(half3 pos,float crescentOffset)
            {
                float moon = distance(pos, -_MainLightPosition);
                float moonDis = saturate((1 - moon / _MoonRadius) * 50);
                float crescent = distance(float3(pos.x + crescentOffset, pos.yz), -_MainLightPosition);
                float crescentDis = saturate((1 - crescent / _MoonRadius) * 50);
                moonDis = saturate(moonDis - crescentDis);
                return moonDis;
            }
            //FunctionsEnd


            v2f vert (appdata v)
            {
                v2f o=(v2f)0;
                float3 postionWS= TransformObjectToWorld(v.vertex).xyz;//ת��������ռ�
                o.vertex =TransformWorldToHClip(postionWS);//ת�����ü��ռ�
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target 
            {
                half4 c =SAMPLE_TEXTURE2D(_MainTex, smp,i.uv);//������� ����URP����fixed4  
                c *= _Color;

                float sunDis = SunDistance(i.uv.xyz);
                float moonDis = MoonDistance(i.uv.xyz,_CrescentOffset);
                half4 sunCol = sunDis * _SunColor;

                float3 sunUV = mul(i.uv.xyz, LtoW);
                float2 moonUV = sunUV.xy * _MoonTex_ST.xy * (1/_MoonRadius+0.001) + _MoonTex_ST.zw;
                float4 moonTex = SAMPLE_TEXTURE2D(_MoonTex, sampler_MoonTex, moonUV);
                float3 finalMoonColor = (_MoonColor * moonTex.rgb * moonTex.a) * step(0,sunUV.z);
                half4 moonCol = half4(finalMoonColor, 1);

                //half4 col = sunDis * _SunColor + moonDis * _MoonColor;
                half4 col = sunCol + moonCol;
                return col;
            }


            ENDHLSL
        }
    }
}
