Shader "Unlit/CustomChroma"
{
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _TransparentColorKey("Transparent Color Key", Color) = (0,1,0,0)
        _TransparencyTolerance("Transparency Tolerance", Float) = 0.01
    }
        SubShader{
            Pass {
                Tags { "RenderType" = "Opaque" }
                LOD 200

                CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct a2v
                {
                    float4 pos : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                v2f vert(a2v input)
                {
                    v2f output;
                    output.pos = UnityObjectToClipPos(input.pos);
                    output.uv = input.uv;
                    return output;
                }

                sampler2D _MainTex;
                float3 _TransparentColorKey;
                float _TransparencyTolerance;

                float4 frag(v2f input) : SV_Target
                {
                    float4 Color = tex2D(_MainTex, input.uv);

                    float deltaR = abs(Color.r - _TransparentColorKey.r);
                    float deltaG = abs(Color.g - _TransparentColorKey.g);
                    float deltaB = abs(Color.b - _TransparentColorKey.b);

                    if (deltaR < _TransparencyTolerance && deltaG < _TransparencyTolerance && deltaB < _TransparencyTolerance)
                    {
                        return float4(0, 0, 0, 0);
                    }

                    return Color;
                }
                ENDCG
            }
        }
}