// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Bumped shader. Differences from regular Bumped one:
// - no Main Color
// - Normalmap uses Tiling/Offset of the Base texture
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Custom/ItemUnlit" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _TileSize("Tile Size", Float) = 16
        _TextureSize("Texture Size", Float) = 256
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [HideInInspector] _Offsets("Offsets", Vector) = (0,0,0,0)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 250

        HLSLINCLUDE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            //#include "UnityCG.cginc"
            //#include "UnityShaderVariables.cginc"

        CBUFFER_START(UnityPerMaterial)

            float _TileSize;
            float _TextureSize; 
            float4 _BaseColor;
            float4 _Offsets;

        CBUFFER_END

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        float4 _MainTex_ST;

        struct VertexInput {
            float4 position : POSITION;
            float2 uv : TEXCOORD0;  
            //float3 worldPos;
            //float3 worldPos : TEXCOORD1;
        };

        struct VertexOutput
        {
            float4 position : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        ENDHLSL

        Pass
        { 
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            float4x4 WorldViewProj : WorldViewProjection;

            VertexOutput vert(VertexInput i)
            {
                VertexOutput o;
                o.position = TransformObjectToHClip(i.position.xyz);

                float2 localOffset = float2(_Offsets.x - _Offsets.z, _Offsets.y  - _Offsets.w);
                float2 localUV = i.uv + localOffset*(_TileSize/ _TextureSize);

                o.uv = localUV;
                return o;
            }
            
            float4 frag(VertexOutput i) : SV_Target
            {

                 float4 baseTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv );
                 return baseTex * _BaseColor;
            }
            
            ENDHLSL
        }
    }
}
