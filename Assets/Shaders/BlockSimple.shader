// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Bumped shader. Differences from regular Bumped one:
// - no Main Color
// - Normalmap uses Tiling/Offset of the Base texture
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Custom/BlockSimple" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _TileSize("Tile Size", Integer) = 16
        _TextureSize("Texture Size", Integer) = 256
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
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

            int _TileSize;
            int _TextureSize; 
            float4 _BaseColor;

        CBUFFER_END

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

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
               /* float3 wPos = mul(UNITY_MATRIX_P, i.position);

                float x = wPos.x;
                float y = wPos.y;
                float z = wPos.z;
                float2 offs = float2((x) % 0.0625, z % 0.0625);*/

                o.uv = i.uv;//+offs;
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
