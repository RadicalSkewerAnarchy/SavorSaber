Shader "Sprites/PixelLighting"
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

			#ifndef UNITY_SPRITES_INCLUDED
			#define UNITY_SPRITES_INCLUDED

			#include "UnityCG.cginc"

			#ifdef UNITY_INSTANCING_ENABLED

			UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
			// SpriteRenderer.Color while Non-Batched/Instanced.
			UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
			// this could be smaller but that's how bit each entry is regardless of type
			UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
			UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

			#define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
			#define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)

			#endif // instancing

			CBUFFER_START(UnityPerDrawSprite)
			#ifndef UNITY_INSTANCING_ENABLED
			fixed4 _RendererColor;
			fixed2 _Flip;
			#endif
			float _EnableExternalAlpha;
			CBUFFER_END

			// Material Color.
			fixed4 _Color;
			uniform float4 _LightColor0; //From UnityCG

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 vertexInWorldCoords: TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
			{
				return float4(pos.xy * flip, pos.z, 1.0);
			}

			v2f vert(appdata_t IN)
			{
				v2f OUT;

				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

				OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
				OUT.vertexInWorldCoords = mul(unity_ObjectToWorld, IN.vertex);
				OUT.vertex = UnityObjectToClipPos(OUT.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color * _RendererColor;

				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
				OUT.vertexInWorldCoords = UnityPixelSnap(OUT.vertexInWorldCoords);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

				#if ETC1_EXTERNAL_ALPHA
				fixed4 alpha = tex2D(_AlphaTex, uv);
				color.a = lerp(color.a, alpha.r, _EnableExternalAlpha);
				#endif

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				float3 P = IN.vertexInWorldCoords.xyz;
				//float3 N = normalize(i.normal);
				//float3 V = normalize(_WorldSpaceCameraPos);
				float3 L = normalize(_WorldSpaceLightPos0.xyz - P);

				float diffuseVal = 0.5;// max(abs(L.x + L.y), 0);//max(dot(N, L), 0);
				float3 diffuse = _LightColor0.rgb * diffuseVal;

				float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb; //Ambient light

				//Sample the actual sprite
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				c.rgb *= c.a;

				return float4(ambient + diffuse, 1) * c;
			}

			#endif // UNITY_SPRITES_INCLUDED

            ENDCG
        }
    }
}
