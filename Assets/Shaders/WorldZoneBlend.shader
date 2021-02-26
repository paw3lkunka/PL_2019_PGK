Shader "Custom/WorldZoneBlend"
{
    Properties
    {
        _ClippingTexture("Clipping Texture", 2D) = "white" {}
    	_Cutoff ("Cutoff", Float) = 0.5
    	_SmoothingFactor ("Smoothing Factor", Float) = 30
    	_Zone1WorldSize ("Zone1 Size", Float) = 10
        _Zone1Color ("Zone1 Color", Color) = (1,1,1,1)
        _Zone1Tex ("Zone1 Texture", 2D) = "white" {}
        _Zone2WorldSize("Zone2 Size", Float) = 20
    	_Zone2Color("Zone2 Color", Color) = (1,1,1,1)
        _Zone2Tex("Zone2 Texture", 2D) = "white" {}
        _Zone3WorldSize("Zone3 Size", Float) = 30
        _Zone3Color("Zone3 Color", Color) = (1,1,1,1)
        _Zone3Tex("Zone3 Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alphatest:_Cutoff

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        
        struct Input
        {
            float2 uv_ClippingTexture;
            float2 uv_Zone1Tex;
            float2 uv_Zone2Tex;
            float2 uv_Zone3Tex;
            float3 worldPos;
        };

        sampler2D _ClippingTexture;
        
        float _SmoothingFactor;
        
        float _Zone1WorldSize;
        float4 _Zone1Color;
        sampler2D _Zone1Tex;
        float _Zone2WorldSize;
        float4 _Zone2Color;
        sampler2D _Zone2Tex;
        float _Zone3WorldSize;
        float4 _Zone3Color;
        sampler2D _Zone3Tex;
        
        half _Glossiness;
        half _Metallic;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 zone1 = tex2D(_Zone1Tex, IN.uv_Zone1Tex) * _Zone1Color;
            float3 zone2 = tex2D(_Zone2Tex, IN.uv_Zone2Tex) * _Zone2Color;
            float3 zone3 = tex2D(_Zone3Tex, IN.uv_Zone3Tex) * _Zone3Color;
            float dist = distance(float3(0.0, 0.0, 0.0), IN.worldPos);
            float zone1dist = saturate(smoothstep(dist, dist + _SmoothingFactor, _Zone1WorldSize));
            float zone2dist = saturate(smoothstep(dist, dist + _SmoothingFactor, _Zone2WorldSize));
            float zone3dist = saturate(smoothstep(dist, dist + _SmoothingFactor, _Zone3WorldSize));
            o.Albedo = zone1 * zone1dist + (zone2dist - zone1dist) * zone2 + (zone3dist - zone1dist - zone2dist) * zone3;
        	//o.Albedo = zone1dist * zone1 + (zone2dist - zone1dist) * zone2 + (zone3dist - zone1dist - zone2dist) * zone3;
            //o.Albedo = float3(zone1dist, zone2dist - zone1dist, zone3dist - zone1dist - zone2dist);
        	// Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = tex2D(_ClippingTexture, IN.uv_ClippingTexture);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
