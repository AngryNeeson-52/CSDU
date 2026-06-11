Shader "Custom/Invisable"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Radius ("Radius", Float) = 0.1
        _Softness ("Softness", Float) = 0.01
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _Color;
            float _Radius;
            float _Softness;
            float2 _Center;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {

                float2 uv = i.uv;

                float2 centered = uv - _Center;

                centered.x *= _ScreenParams.x / _ScreenParams.y;

                float dist = length(centered);

                float alpha = smoothstep(_Radius, _Radius + _Softness, dist);

                return float4(_Color.rgb, alpha * _Color.a);
            }
            ENDCG
        }
    }
}
