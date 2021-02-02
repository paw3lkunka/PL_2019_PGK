Shader "Custom/OverdrawAlways"
{
    Properties
    {
        _Cutoff("Alpha cutoff", Range(0,1)) = 0
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    	[HDR] _EmissionColor ("Emission color", Color) = (0,0,0,0)
        _EmissionTexture ("Emission", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" "ForceNoShadowCasting" = "True" }
        LOD 200

    	ZWrite Off
        ZTest Always
    	
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard noshadow alphatest:_Cutoff

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
            o.Emission = tex2D(_EmissionTexture, IN.uv_MainTex) * _EmissionColor;
        	// Metallic and smoothness come from slider variables
            o.Metallic = 0.0;
            o.Smoothness = 0.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
