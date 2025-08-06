Shader "Custom/StandardWithVertexColor"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard

        // Include Unity's common shader code
        #include "UnityCG.cginc"

        // Input structure
        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR; // Vertex color input
        };

        // Shader properties
        sampler2D _MainTex;
        fixed4 _Color;
        half _Glossiness;
        half _Metallic;

        // Surface function
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Sample the texture
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);

            // Combine vertex color with the texture color
            o.Albedo = tex.rgb * IN.color.rgb;

            // Set metallic and smoothness properties
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
