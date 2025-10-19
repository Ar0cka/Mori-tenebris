Shader "Unlit/OutlineSquare"
{
    Properties
    {
        _OutlineColor ("Outline color", Color) = (1,1,1,1)
        _OutlineWeight ("Outline weight", Float) = 1.0
        _Size ("Size square", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
            
            float4 _OutlineColor;
            float _OutlineWeight;
            float _Size;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2.0 - 1.0;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float squartSDF(float2 p, float2 size)
            {
                float2 d = abs(p) - size;
                return max(d.x, d.y);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float d = squartSDF(uv, float2(_Size, _Size));
                
                float edge = 1.0 - smoothstep(0.0, _OutlineWeight, abs(d));

                float3 col = _OutlineColor.rgb * edge;

                return float4(col, edge);
            }
            ENDCG
        }
    }
}
