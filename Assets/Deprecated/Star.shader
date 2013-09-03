Shader "Custom/Star" {
//	Properties {
//		_MainTex ("Base (RGB)", 2D) = "white" {}
//	}
//	SubShader {
//		Tags { "RenderType"="Opaque" }
//		LOD 200
//		
//		CGPROGRAM
//		#pragma surface surf Lambert
//
//		sampler2D _MainTex;
//
//		struct Input {
//			float2 uv_MainTex;
//		};
//
//		void surf (Input IN, inout SurfaceOutput o) {
//			half4 c = tex2D (_MainTex, IN.uv_MainTex);
//			o.Albedo = c.rgb;
//			o.Alpha = c.a;
//		}
//		ENDCG
//	} 
//	FallBack "Diffuse"

SubShader{
		Tags{ "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Lighting Off
		ZWrite Off
		Pass{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				struct data {
					float4 vertex : POSITION;
					fixed4 color: COLOR;
				};

				data vert (data v) {
					v.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					return v;
				}

				fixed4 frag(data f) : COLOR {
					return f.color;
				}
			ENDCG
		}
	}
}