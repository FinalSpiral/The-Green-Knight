Shader "Unlit/Character"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_OutlineSize("Outline Size", Range(0,1)) = 0.5
		_OutlineOn ("Outline on", Int) = 0
    }
    SubShader
    {
		Tags {"Queue" = "AlphaTest+1" }
		//Outline Right
		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			

			#include "UnityCG.cginc"

			struct meshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct fragData
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _OutlineColor;
			float _OutlineSize;
			int _OutlineOn;

			fragData vert(meshData v)
			{
				fragData o;
				v.vertex.x += lerp(0,0.03,_OutlineSize);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}

			fixed4 frag(fragData i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				if (col.a == 0 || _OutlineOn == 0) {
					discard;
				}
				else {
					col = _OutlineColor;
				}

				return col;
			}
			ENDCG
		}
		//Outline Left
		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct meshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct fragData
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _OutlineColor;
			float _OutlineSize;
			int _OutlineOn;

			fragData vert(meshData v)
			{
				fragData o;
				v.vertex.x -= lerp(0,0.03,_OutlineSize);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(fragData i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				if (col.a == 0 || _OutlineOn == 0) {
					discard;
				}
				else {
					col = _OutlineColor;
				}

				return col;
			}
			ENDCG
		}
		//Outline Top
		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct meshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct fragData
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _OutlineColor;
			float _OutlineSize;
			int _OutlineOn;

			fragData vert(meshData v)
			{
				fragData o;
				v.vertex.y += lerp(0,0.03,_OutlineSize)*2;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(fragData i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				if (col.a == 0 || _OutlineOn == 0) {
					discard;
				}
				else {
					col = _OutlineColor;
				}

				return col;
			}
			ENDCG
		}
		//Outline Bottom
		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct meshData
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct fragData
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _OutlineColor;
			float _OutlineSize;
			int _OutlineOn;

			fragData vert(meshData v)
			{
				fragData o;
				v.vertex.y -= lerp(0,0.03,_OutlineSize)*2;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(fragData i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				if (col.a == 0 || _OutlineOn == 0) {
					discard;
				}
				else {
					col = _OutlineColor;
				}

				return col;
			}
			ENDCG
		}	
    }
}
