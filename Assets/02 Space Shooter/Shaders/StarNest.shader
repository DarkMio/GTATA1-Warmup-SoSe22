Shader "Unlit/Starfield"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Offset ("Rendering Offset", Vector) = (0, 0, 0, 0)
        [HDR] _Color ("Grading Color", Color) = (0, 0, 0, 0)
        
        [Space]
        _Iterations ("Volumetric Iterations", Range(0, 50)) = 17
        _VolumetricSteps ("Volumetric Steps", Range(0, 50)) = 20
        _VolumetricStepSize ("Volumetric Step Size", Range(0, 1)) = 0.2
        _FormularParameter ("Formular Parameter", Range(0, 1)) = 0.53
        
        [Space]
        _Zoom ("Zoom", Range(0, 10)) = 0.8
        _Tile ("Tiling", Range(0, 10)) = 0.85
        _Speed ("Speed", Range(0, 0.1)) = 0.001
        
        [Space]
        _Brightness ("Brightness", Range(0, 0.01)) = 0.0015
        _DarkMatter ("Dark Matter", Range(0, 0.5)) = 0.3
        _DistFading ("Distance Fading", Range(0, 1)) = 0.73
        _Saturation ("Saturation", Range(0, 1.5)) = 0.85
        
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 _MainTex_TexelSize;
            float2 _Offset;
            float4 _Color;
            float _Interpolation;
            // Original Star Nest by Pablo Roman Andrioli, MIT License
            // Adapted for dynamic scrolling content
            float _Iterations;
            float _VolumetricSteps;
            float _VolumetricStepSize;
            float _FormularParameter;;
                
            float _Zoom;
            float _Tile;
            float _Speed;
                
            float _Brightness;
            float _DarkMatter;
            float _DistFading;
            float _Saturation;
        

            void mainImage(out float4 fragColor, in float2 fragCoord, in float2 offset)
            {
                //get coords and direction
                const float2 resolution = _MainTex_TexelSize.xy * _MainTex_TexelSize.zw;
   
                float2 uv = fragCoord.xy / resolution.xy - .5;
                uv.y *= resolution.y / resolution.x;
                const float3 direction = float3(uv * _Zoom, 0.1);

                float3 from = float3(1., .5, 0.5);
                from.xyz += float3(offset.x * _Speed, offset.y * _Speed, length(offset.xy) * 0.00001);

                //volumetric rendering
                float s = 0.1, fade = 1.;
                float3 v = float3(0, 0, 0);
                for (int r = 0; r < _VolumetricSteps; r++)
                {
                    float3 p = from + s * direction * .5;
                    p = abs(float3(_Tile.xxx) - fmod(p, float3(_Tile.xxx * 2.))); // tiling fold
                    float pa, a = pa = 0.;
                    for (int i = 0; i < _Iterations; i++)
                    {
                        p = abs(p) / dot(p, p) - _FormularParameter; // the magic formula
                        a += abs(length(p) - pa); // absolute sum of average change
                        pa = length(p);
                    }
                    float dm = max(0.,_DarkMatter - a * a * .001); //dark matter
                    a *= a * a; // add contrast
                    if (r > 6) fade *= 1. - dm; // dark matter, don't render near
                    v += fade;
                    v += float3(s, s * s, s * s * s * s) * a * _Brightness * fade; // coloring based on distance
                    fade *= _DistFading; // distance fading
                    s += _VolumetricStepSize;
                }
                v = lerp(float3(length(v).xxx), v,_Saturation); //color adjust
                fragColor = float4(v * .01, 1.);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col;
                mainImage(col, i.uv.xy, _Offset);
                // multiplying with the color preserves light peaks better, than lerp/clamping. 
                return col * _Color;
            }
            ENDCG
        }
    }
}