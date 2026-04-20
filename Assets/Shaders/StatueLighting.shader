Shader "Custom/SimpleStatueShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (0.2, 0.2, 0.2, 1)
        _GlowColor ("Glow Color", Color) = (0.5, 0.5, 0.5, 1)
        _GlowStrength ("Glow Strength", Range(0, 5)) = 1
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

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float glow : TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _GlowColor;
            float _GlowStrength;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float3 normalDir = normalize(v.normal);
                o.glow = 1 - abs(normalDir.z);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _Color;
                col.rgb += _GlowColor.rgb * i.glow * _GlowStrength;
                return col;
            }
            ENDCG
        }
    }
}