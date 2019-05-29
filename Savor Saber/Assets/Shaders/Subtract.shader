Shader "Unlit/Subtract"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }
    SubShader
    {
        Tags{	"RenderType"="Transparent"
				"IgnoreProjector"="True"
				"PreviewType"="Plane"
				"CanUseSpriteAtlas"="True"
				"Queue" = "Transparent"
			}
        LOD 100

			Cull Off
			Lighting Off
			ZWrite Off

        Pass
        {
			BlendOp RevSub
			Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnitySprites.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
				v2f o = SpriteVert(v);
				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col = SampleSpriteTexture(i.texcoord);
				col.rgb *= col.a;

				
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
