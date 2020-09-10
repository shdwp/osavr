// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/RadarCameraViewShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        [PerRenderData] _VelocityVector ("Velocity", Vector) = (0, 0, 0, 1)
        [PerRenderData] _IFFResponse ("IFF Response", Int) = 0
        [PerRenderData] _WeatherFactor ("Weather Factor", Float) = 0
    }

    CGINCLUDE

    #define MAGNIFICATION_FACTOR 2.f, 10.f

    #define SPEED_BASE 1715 // (5 mach)
    #define SIGNAL_BASE 93
    #define BEAMSHAPE_LOSS_FACTOR 3
    #define SYSTEM_LOSS 11
    #define MOLECULAR_DISPERSION_LOSS 0.8
    #define IFF_RESPONSE_RANGE 3

    float BeamshapeLoss(float dist)
    {
        return pow(dist, 2) * BEAMSHAPE_LOSS_FACTOR + pow(dist, 35) * SIGNAL_BASE;
    }

    float CrossectionLoss(float3 normal)
    {
        float l = smoothstep(0.f, 1.f, (float)length(normal.xy));
        return pow(l, 0.8f) * SIGNAL_BASE;
    }

    float DistanceLoss(float distance, float weatherFactor)
    {
        return (distance / 1000.f) * (weatherFactor + MOLECULAR_DISPERSION_LOSS);
    }
    
    ENDCG

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
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float2 screenPos: TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _VelocityVector;
            float _WeatherFactor;
            int _IFFResponse;

            v2f vert(appdata v)
            {
                v2f o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                float size_factor = lerp(MAGNIFICATION_FACTOR, smoothstep(_ProjectionParams.y, _ProjectionParams.z, distance(_WorldSpaceCameraPos, o.worldPos)));
                o.vertex = UnityObjectToClipPos(v.vertex * size_factor);
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                o.screenPos = o.vertex.xyz / o.vertex.w;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float signal = SIGNAL_BASE;
                float distance_from_center = length(i.screenPos.xy);
                
                float distance_to_camera = distance(_WorldSpaceCameraPos, i.worldPos);
                float3 normal = normalize(mul((float3x3)UNITY_MATRIX_V, UnityObjectToWorldNormal(i.normal)));

                float3 up = UnityWorldToClipPos(float3(0.f, 1.f, 0.f));
                float3 proj = up - UnityWorldToClipPos(normalize(_VelocityVector.xyz));
                float angular_velocity = lerp(1.f, 0.f, abs(proj.x));
                
                signal -= BeamshapeLoss(distance_from_center);
                signal -= CrossectionLoss(normal);
                signal -= DistanceLoss(distance_to_camera, _WeatherFactor);
                signal -= SYSTEM_LOSS; 

                return fixed4(
                    lerp(1.f, 0.f, distance_to_camera / _ProjectionParams.z),
                    angular_velocity > 0.f ? (angular_velocity * length(_VelocityVector)) / SPEED_BASE : 0.f,
                    signal / SIGNAL_BASE,
                    _IFFResponse == 0 ? 0 : (float)_IFFResponse / IFF_RESPONSE_RANGE
                );
            }
            ENDCG
        }
    }
}