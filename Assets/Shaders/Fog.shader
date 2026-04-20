Shader "Custom/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0.5, 0.5, 0.5, 1)
        _FogStart ("Fog Start Distance", Float) = 0
        _FogEnd ("Fog End Distance", Float) = 50
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

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _FogColor;
            float _FogStart;
            float _FogEnd;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float fogFactor : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // Calculate world position
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Distance from camera
                float dist = distance(_WorldSpaceCameraPos, worldPos);

                // Linear fog factor
                o.fogFactor = saturate((dist - _FogStart) / (_FogEnd - _FogStart));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Blend with fog
                col.rgb = lerp(col.rgb, _FogColor.rgb, i.fogFactor);

                return col;
            }
            ENDCG
        }
    }
}