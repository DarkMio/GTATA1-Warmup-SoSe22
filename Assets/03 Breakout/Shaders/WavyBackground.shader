Shader "Unlit/WavyBackground"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color 1", Color) = (0.05,0.05,0.3)
        _Color2 ("Color 2", Color) = (1.1,0.65,0.85)
        _Tiling ("Tiling", Int) = 20
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }


            float gradient(float p)
            {
                float2 pt0 = float2(0.00, 0.0);
                float2 pt1 = float2(0.86, 0.1);
                float2 pt2 = float2(0.955, 0.40);
                float2 pt3 = float2(0.99, 1.0);
                float2 pt4 = float2(1.00, 0.0);
                if (p < pt0.x) return pt0.y;
                if (p < pt1.x) return lerp(pt0.y, pt1.y, (p - pt0.x) / (pt1.x - pt0.x));
                if (p < pt2.x) return lerp(pt1.y, pt2.y, (p - pt1.x) / (pt2.x - pt1.x));
                if (p < pt3.x) return lerp(pt2.y, pt3.y, (p - pt2.x) / (pt3.x - pt2.x));
                if (p < pt4.x) return lerp(pt3.y, pt4.y, (p - pt3.x) / (pt4.x - pt3.x));
                return pt4.y;
            }

            float waveN(float2 uv, float2 s12, float2 t12, float2 f12, float2 h12)
            {
                float2 x12 = sin((_Time.y * s12 + t12 + uv.x) * f12) * h12;

                float g = gradient(uv.y / (0.5 + x12.x + x12.y));

                return g * 0.27;
            }

            float wave1(float2 uv)
            {
                return waveN(float2(uv.x, uv.y - 0.25), float2(0.03, 0.06), float2(0.00, 0.02), float2(8.0, 3.7),
                             float2(0.06, 0.05));
            }

            float wave2(float2 uv)
            {
                return waveN(float2(uv.x, uv.y - 0.25), float2(0.04, 0.07), float2(0.16, -0.37), float2(6.7, 2.89),
                             float2(0.06, 0.05));
            }

            float wave3(float2 uv)
            {
                return waveN(float2(uv.x, 0.75 - uv.y), float2(0.035, 0.055), float2(-0.09, 0.27), float2(7.4, 2.51),
                             float2(0.06, 0.05));
            }

            float wave4(float2 uv)
            {
                return waveN(float2(uv.x, 0.75 - uv.y), float2(0.032, 0.09), float2(0.08, -0.22), float2(6.5, 3.89),
                             float2(0.06, 0.05));
            }

            float3 _Color1;
            float3 _Color2;
            float _Tiling;

            void mainImage(out float4 fragColor, in float2 uv)
            {
                // float2 uv = fragCoord.xy / iResolution.xy;
                uv = ((_Tiling * uv) - frac(uv * _Tiling)) / _Tiling;
                float waves = wave1(uv) + wave2(uv) + wave3(uv) + wave4(uv);

                float x = uv.x;
                float y = 1.0 - uv.y;

                float3 bg = lerp(_Color1, _Color2, (x + y) * 0.55);
                float3 ac = bg + float3(1.0, 1.0, 1.0) * waves;

                fragColor = float4(ac, 1.0);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col;
                mainImage(col, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}