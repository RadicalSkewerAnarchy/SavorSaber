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
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
        LOD 100

		Cull Off
		Lighting Off
		ZWrite Off

        Pass
        {
			Blend One OneMinusSrcAlpha
			Tags
			{
				"LightMode" = "ForwardBase"
			}
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

		Pass
		{
			Blend One One
			BlendOp Max
			Tags
			{
				"LightMode" = "ForwardAdd"
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 vertexInWorldCoords: TEXCOORD1;
			};

			uniform float4 _LightColor0; //From UnityCG

			v2f vert(appdata IN)
			{
				v2f OUT;
				OUT.vertex = IN.vertex;
				OUT.color = IN.color;
				OUT.texcoord - IN.texcoord;
				OUT.vertexInWorldCoords = mul(unity_ObjectToWorld, IN.vertex); 
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{	
				float3 P = IN.vertexInWorldCoords.xyz;
				//float3 N = normalize(i.normal);
				//float3 V = normalize(_WorldSpaceCameraPos);
				float3 L = normalize(_WorldSpaceLightPos0.xyz - P);

				float dist = length(_WorldSpaceLightPos0.xyz - P);

				float mult = 1;
				if (dist > 10)
					mult = 0;

				return _LightColor0 * mult;
			}


			ENDCG
		}
    }
}
