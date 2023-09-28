Shader "Unlit/VertFrag"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        // ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        // Cull front 

        LOD 100

       Pass
        {
            Tags {"LightMode"="ForwardBase" }
            // Tags {
            // // "IgnoreProjector"="True"
            // "RenderType"="Transparent"
            // // "LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            // compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"

            struct v2f
            {
                UNITY_VERTEX_OUTPUT_STEREO
                float2 uv : TEXCOORD0;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
            };
            v2f vert (appdata_base v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                float4x4 projection = UNITY_MATRIX_P;
                float4x4 view = UNITY_MATRIX_V;
                float4x4 model = unity_ObjectToWorld;

                float scale_factor = 0.1f;
                float4x4 scale_minus = {
                    scale_factor, 0, 0, 0,
                    0, scale_factor, 0, 0,
                    0, 0, scale_factor, 0,
                    0, 0, 0, 1.0f
                };

                o.pos = mul(projection, mul(view, mul(scale_minus, mul(model, v.vertex))));
                o.uv = v.texcoord;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal,1));
                // compute shadows data
                TRANSFER_SHADOW(o)
                return o;
            }

            sampler2D _MainTex;
            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                // darken light's illumination with shadow, keep ambient intact
                fixed3 lighting = i.diff * shadow + i.ambient;
                col.rgb *= lighting;
                // col.a = _Color.a;
                return col;
            }
            ENDCG
        }

        // shadow caster rendering pass, implemented manually
        // using macros from UnityCG.cginc
    //     Pass
    //     {
    //         Tags {"LightMode"="ShadowCaster"}

    //         CGPROGRAM
    //         #pragma vertex vert
    //         #pragma fragment frag
    //         #pragma multi_compile_shadowcaster
    //         #include "UnityCG.cginc"

    //         struct v2f { 
    //             V2F_SHADOW_CASTER;
    //         };

    //         v2f vert(appdata_base v)
    //         {
    //             // float4x4 projection = UNITY_MATRIX_P;
    //             // float4x4 view = UNITY_MATRIX_V;
    //             // float4x4 model = unity_ObjectToWorld;

    //             // float scale_factor = 0.5f;
    //             // float4x4 scale_minus = {
    //             //     scale_factor, 0, 0, 0,
    //             //     0, scale_factor, 0, 0,
    //             //     0, 0, scale_factor, 0,
    //             //     0, 0, 0, 1.0f
    //             // };

    //             v2f o;
    //             // o.pos = mul(projection, mul(view, mul(scale_minus, mul(model, v.vertex))));
    //             TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
    //             return o;
    //         }

    //         float4 frag(v2f i) : SV_Target
    //         {
    //             SHADOW_CASTER_FRAGMENT(i)
    //         }
    //         ENDCG
    //     }
    }
}
