Shader "Unlit/ShadowTest"
{
	SubShader
	{
		Tags {"Queue" = "AlphaTest" }
		Pass
		{
			Tags {"LightMode" = "ForwardBase" }
			Cull Back
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			
			struct meshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct interpolationData
			{
				float4 pos : SV_POSITION;
				float4 worldPos : TEXCOORD0;
				SHADOW_COORDS(1)
			};

			interpolationData vert(meshData v)
			{
				interpolationData o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				TRANSFER_SHADOW(o)
				return o;
			}

			fixed4 frag(interpolationData i) : SV_TARGET
			{
				UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos);
				float shadow = attenuation;
				fixed4 color = fixed4(1, 0, 0, 1);
				color.rgb *= shadow;
				return color;
			}
			ENDCG
		}
		Pass
		{
			Tags {"LightMode" = "ShadowCaster"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster

			#include "UnityCG.cginc"

			struct fragData
			{
				V2F_SHADOW_CASTER;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fragData vert(appdata_full v)
			{
				fragData o;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				o.uv = v.texcoord;
				return o;
			}

			fixed4 frag(fragData i) : SV_Target
			{
				/*fixed4 col = tex2D(_MainTex,i.uv);
				if (col.a == 0) {
					discard;
					SHADOW_CASTER_FRAGMENT(i)
				}
				else
				{
					SHADOW_CASTER_FRAGMENT(i)
				}

				clip(col.a - 0.9);*/
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
}
