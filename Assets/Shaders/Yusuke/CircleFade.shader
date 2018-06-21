Shader "Unlit/CircleFade"
{
	Properties
	{
		_FadeColor ("TestColor", Color) = (0,0,0,1) 
		_FadeTime ("FadeTime", float) = 0
		[Enum(In,0,Out,1,No,2)]_FadeMode("FadeMode",Float) = 0//選
 	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			
			float4 _FadeColor;
			float _FadeTime;
			float _FadeMode;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				if(_FadeMode >= 1.5)
				discard;


				fixed4 col = _FadeColor;
				col.w = 0;
				float centerDistance = distance(float2(0.5,0.5),i.uv);
				const float offset_ = 0.1;
				
				if(_FadeMode)
				{
					col.w = step(0.7071067811865475244008 - _FadeTime * 0.7071067811865475244008,centerDistance + offset_);
				}
				else
				{
					col.w = step(_FadeTime * 0.7071067811865475244008,centerDistance + offset_);
				}
				return col;
			}
			ENDCG
		}
	}
}
