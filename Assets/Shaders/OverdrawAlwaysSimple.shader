Shader "Custom/OverdrawAlwaysSimple"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" "ForceNoShadowCasting" = "True" }
        LOD 200

        ZWrite Off
        ZTest Always

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard noshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _EmissionTexture;

        struct Input
        {
            float2 uv_MainTex;
        };
        
        fixed4 _Color;
        float3 _EmissionColor;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        	// Metallic and smoothness come from slider variables
            o.Metallic = 0.0;
            o.Smoothness = 0.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
