Shader "Custom/Outline"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
		//Tags { "RenderType" = "Opaque" }
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alphatest:_Cutoff
		#pragma vertex vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        //UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        //UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full v) {
			
		}

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Alpha = c.a;
        }
        ENDCG
    
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Lambert alphatest:_Cutoff
			#pragma vertex vert
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			fixed4 _Color;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			//UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			//UNITY_INSTANCING_BUFFER_END(Props)

			void vert(inout appdata_full v) {
				//v.vertex.z += 0.01;
				v.vertex.x += 0.02;
			}


			void surf(Input IN, inout SurfaceOutput o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = _Color;
				// Metallic and smoothness come from slider variables
				o.Alpha = c.a;
			}
			ENDCG

				CGPROGRAM
				// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Lambert alphatest:_Cutoff
#pragma vertex vert
// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			fixed4 _Color;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			//UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			//UNITY_INSTANCING_BUFFER_END(Props)

			void vert(inout appdata_full v) {
				//v.vertex.z += 0.01;
				v.vertex.x -= 0.02;
			}


			void surf(Input IN, inout SurfaceOutput o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = _Color;
				// Metallic and smoothness come from slider variables
				o.Alpha = c.a;
			}
			ENDCG
				CGPROGRAM
				// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Lambert alphatest:_Cutoff
#pragma vertex vert
// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			fixed4 _Color;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			//UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			//UNITY_INSTANCING_BUFFER_END(Props)

			void vert(inout appdata_full v) {
				//v.vertex.z += 0.01;
				v.vertex.y += 0.04;
			}


			void surf(Input IN, inout SurfaceOutput o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = _Color;
				// Metallic and smoothness come from slider variables
				o.Alpha = c.a;
			}
			ENDCG
				CGPROGRAM
				// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Lambert alphatest:_Cutoff
#pragma vertex vert
// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			fixed4 _Color;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			//UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			//UNITY_INSTANCING_BUFFER_END(Props)

			void vert(inout appdata_full v) {
				//v.vertex.z += 0.01;
				v.vertex.y -= 0.04;
			}


			void surf(Input IN, inout SurfaceOutput o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = _Color;
				// Metallic and smoothness come from slider variables
				o.Alpha = c.a;
			}
			ENDCG
		}
    FallBack "Legacy Shaders/Transparent/Cutout/VertexLit"
}
