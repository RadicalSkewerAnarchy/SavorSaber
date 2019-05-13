Shader "Sprites/PixelLightingClean"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

    }
    SubShader
    {
        Tags 
		{ 
			"LightMode" = "ForwardAdd"
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"

		}
        LOD 100

		Cull Off
		Lighting On
		ZWrite Off
		Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#include "UnitySprites.cginc"
			v2f vert(appdata_t IN)
			{
				v2f OUT = SpriteVert(IN);
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb; //Ambient light

				//Sample the actual sprite
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				c.rgb *= c.a;

				return float4(ambient, 1) * c;
			}

            ENDCG
        }
    }
}
