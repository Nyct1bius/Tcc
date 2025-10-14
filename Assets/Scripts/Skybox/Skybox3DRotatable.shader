Shader "Custom/Skybox3DRotatable_Cubemap"
{
    Properties
    {
        _Cube ("Cubemap", Cube) = "" {}
        _RotationSpeed ("Rotation Speed", Float) = 1.0
        _RotationX ("Rotation X", Float) = 0.0
        _RotationY ("Rotation Y", Float) = 1.0
        _RotationZ ("Rotation Z", Float) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Cull Off
        ZWrite Off
        Lighting Off
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            samplerCUBE _Cube;
            float _RotationSpeed;
            float _RotationX;
            float _RotationY;
            float _RotationZ;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 dir : TEXCOORD0;
            };

            float3 RotateVector(float3 v, float3 rotation)
            {
                float3 rad = radians(rotation);
                float3x3 rotX = float3x3(
                    1, 0, 0,
                    0, cos(rad.x), -sin(rad.x),
                    0, sin(rad.x), cos(rad.x)
                );
                float3x3 rotY = float3x3(
                    cos(rad.y), 0, sin(rad.y),
                    0, 1, 0,
                    -sin(rad.y), 0, cos(rad.y)
                );
                float3x3 rotZ = float3x3(
                    cos(rad.z), -sin(rad.z), 0,
                    sin(rad.z), cos(rad.z), 0,
                    0, 0, 1
                );

                return mul(rotZ, mul(rotY, mul(rotX, v)));
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.dir = normalize(v.vertex.xyz);

                float3 rotation = float3(_RotationX, _RotationY, _RotationZ) * _RotationSpeed * _Time.y;
                o.dir = RotateVector(o.dir, rotation);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return texCUBE(_Cube, i.dir);
            }
            ENDCG
        }
    }
    FallBack "RenderFX/Skybox"
}
