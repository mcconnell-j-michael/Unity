Shader "Custom/Greyscale" {
	Properties{
		//_MainTex("Base (RGB)", 2D) = "white" {}
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		 _StencilComp("Stencil Comparison", Float) = 8
		// required for UI.Mask
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
	}
		SubShader{
			Tags
			{
				 "Queue" = "Transparent"
				 "IgnoreProjector" = "True"
				 "RenderType" = "Transparent"
				 "PreviewType" = "Plane"
				 "CanUseSpriteAtlas" = "True"
			}
			LOD 200

			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}
			ColorMask[_ColorMask]

			CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o) {

				// Get the original texture colour
				half4 tex = tex2D(_MainTex, IN.uv_MainTex);

				// Get the apparent brightness
				half brightness = dot(tex.rgb, half3(0.3, 0.59, 0.11));

				// Set RGB values equal to brightness
				o.Albedo = brightness;
			}
			ENDCG
	}
		FallBack "Diffuse"
}