Shader "Custom/DistortionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortionCenter ("Distortion Center", Vector) = (0, 0, 0, 0)
        _DistortionRadius ("Distortion Radius", float) = 0.01
        _DistortionStrength ("Distortion Strength", float) = 0.1
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _DistortionCenter;
            float _DistortionRadius;
            float _DistortionStrength;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 dist = uv - _DistortionCenter.xy;
                float len = length(dist);

                if (len < _DistortionRadius)
                {
                    uv += normalize(dist) * (1.0 - len / _DistortionRadius) * _DistortionStrength;
                }

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
