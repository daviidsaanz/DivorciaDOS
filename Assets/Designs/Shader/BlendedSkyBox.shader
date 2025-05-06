Shader"Custom/BlendedSkybox"
{
    Properties
    {
        _Skybox1 ("Skybox 1", Cube) = "" {}
        _Skybox2 ("Skybox 2", Cube) = "" {}
        _Blend ("Blend", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Background" }
Cull Off
ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

samplerCUBE _Skybox1;
samplerCUBE _Skybox2;
float _Blend;

struct v2f
{
    float4 pos : SV_POSITION;
    float3 texcoord : TEXCOORD0;
};

v2f vert(float4 v : POSITION)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v);
    o.texcoord = v.xyz;
    return o;
}

half4 frag(v2f i) : SV_Target
{
    half4 col1 = texCUBE(_Skybox1, i.texcoord);
    half4 col2 = texCUBE(_Skybox2, i.texcoord);
    return lerp(col1, col2, _Blend);
}
            ENDCG
        }
    }
Fallback Off
}
