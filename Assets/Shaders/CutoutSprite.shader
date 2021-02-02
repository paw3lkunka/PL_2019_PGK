Shader "Custom/Cutout Sprite"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    	_OverlayColor ("Overlay color", Color) = (0,0,0,0)
    	_Cutoff ("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        _MainTex ("Texture", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+2"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alphatest:_Cutoff addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _OverlayColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float4 Lighten(float4 cBase, float4 cBlend)
        {
            float4 cNew;
            cNew.rgb = max(cBase.rgb, cBlend.rgb);
            cNew.a = 1.0;
            return cNew;
        }

        float4 Screen(float4 cBase, float4 cBlend)
        {
            return (1 - (1 - cBase) * (1 - cBlend));
        }

        float4 VividLight(float4 cBase, float4 cBlend)
        {
            float isLessOrEq = step(cBlend, 0.5);
            float4 cNew = lerp(1.0 - (1 - cBase) / (2.0 * (cBlend - 0.5)), cBase / (1.0 - 2.0 * cBlend), isLessOrEq);
            cNew.a = 1.0;
            return cNew;
        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = VividLight(c, _OverlayColor).rgb;
            //o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
