Shader "Custom/RimLighting"
{
    Properties
    {
        _BaseMap ("Texture", 2D) = "white" {}
        _RimColor ("Rim Color", Color) = (0, 0.5, 1, 1)
        _RimPower ("Rim Power", Range(0.1, 8.0)) = 3.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _RimColor;
                float _RimPower;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.viewDirWS = normalize(GetWorldSpaceViewDir(TransformObjectToWorld(IN.positionOS.xyz)));

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Base texture
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);

                // Rim lighting calculation
                float rim = 1.0 - saturate(dot(normalize(IN.viewDirWS), normalize(IN.normalWS)));
                float rimFactor = pow(rim, _RimPower);
                float3 rimColor = _RimColor.rgb * rimFactor;

                // Combine base color and rim
                float3 finalColor = baseColor.rgb + rimColor;

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}