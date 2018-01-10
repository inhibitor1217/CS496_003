#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Aubergine/Object/BaseFX/Baloon-Overlay" {
	Properties {
		_Color("Tint (RGB)", Color) = (1,1,1,1)
		_Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
		_Amount("Baloon Amount", Range(0.0, 1.0)) = 0.1
	}

	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Overlay" "IgnoreProjector" = "True"}
		LOD 100

		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }

			Fog { Mode off }
			ZWrite Off ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma exclude_renderers xbox360 ps3 flash
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			fixed4 _Color;
			fixed _Amount, _Cutoff;

			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				fixed3 normal : TEXCOORD0;
				fixed3 view : TEXCOORD1;
			};

			v2f vert(a2v v) {
				v2f o;
				float3 n = v.normal * 1.0;
				v.vertex.xyz += n * _Amount;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.normal = normalize(v.normal);
				float3 dir = (mul(unity_WorldToObject, float4(_WorldSpaceCameraPos.xyz, 1)).xyz * 1.0) - v.vertex.xyz;
				o.view = normalize(dir);
				return o;
			}

			fixed4 frag(v2f i) : COLOR {
				fixed edge = 1.0 - dot(i.view, i.normal);
				edge = pow(edge, _Cutoff);
				return fixed4(_Color.rgb * edge, edge);
			}
			ENDCG 
		}
	}

	Fallback Off
}