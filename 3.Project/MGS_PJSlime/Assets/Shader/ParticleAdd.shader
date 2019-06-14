Shader "HEM/Particles/CustomParticleAdd"
{
	Properties
	{
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Intensity("Intensity", float) = 2
		[Space(5)]
		[Header(Flow)]
		_U_Speed ("U_Speed", Range(-10, 10)) = 0
        _V_Speed ("V_Speed", Range(-10, 10)) = 0
		//[Space(5)]
		//[Header(Vertex Color)]
		//_AlphaMult("AlphaMultiplier" , float) = 20
		[Space(5)]
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Off : 2side", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True"}
		LOD 100
		Cull [_Cull]

		Pass
		{
			ZWrite Off
			Blend One One
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 vertexColor : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			fixed _Intensity;

			half _U_Speed;
			half _V_Speed;
			//fixed _AlphaMult;
			//fixed _AlphaPow;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.vertexColor = v.vertexColor;
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed3 frag (v2f i) : SV_Target
			{
				half2 newUV = frac(  half2( i.uv.x + (_U_Speed * _Time.y) , i.uv.y + (_V_Speed * _Time.y) )  );

				// sample the texture
				fixed4 col = tex2D(_MainTex, newUV);
				

				//fixed3 finalCol = col.rgb * i.vertexColor.rgb * (col.a * ( pow(i.vertexColor.a, _AlphaPow) * _AlphaMult) );
				fixed3 finalCol = col.rgb * col.a * i.vertexColor.rgb * _Color.rgb * _Intensity * i.vertexColor.a;
				
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);

				return finalCol;
			}
			ENDCG
		}
	}
}
