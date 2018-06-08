/*
Yusuke Nakata
輪郭・エフェクト
2018/05/29
*/


Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_MainTex("texture",2D) = "white"{}
        _SelectEffectColor("EffectColor(RGB)", Color) = (1.0,1.0,1.0,1.0)//選択時エフェクト色
		_OutLineWidth("OutLineWidth",Range(0.0,1)) = 0.1//輪郭の幅
		_SelectEffectWidth("SelectEffectWidth",Range(0.0,1)) = 0.1//選択時エフェクトの幅
		[Enum(ON,0,OFF,1)]_IsSelectEffect("IsSelectEffect",Float) = 1//選択時エフェクトON OFF
		_Spec1Power("Specular Power", Range(0, 30)) = 10
		_Spec1Color("Specular Color", Color) = (0.5,0.5,0.5,1)
	}

	SubShader
	{
		Tags { 
		"RenderType"="Transparent"
		"Queue"="AlphaTest"
		 }


        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

		////選択時エフェクト
		//pass
  //      {
  //          Cull Front

  //          CGPROGRAM
  //          #pragma vertex vert
  //          #pragma fragment frag

  //          #include "UnityCG.cginc"

		//	fixed4 _SelectEffectColor;
		//	float _SelectEffectWidth;
		//	float _IsSelectEffect;

		//	uniform  float4x4 EffectWVP;

  //          struct appdata
  //          {
  //              float4 vertex : POSITION;
  //              float3 normal : NORMAL;
  //          };

  //          struct v2f
  //          {
  //              float4 vertex : SV_POSITION;
  //          };

  //          v2f vert (appdata v)
  //          {
  //              v2f o;
		//		v.vertex += float4(v.normal * _SelectEffectWidth, 0);
		//		o.vertex = UnityObjectToClipPos(v.vertex); 

  //              return o;
  //          }
            
  //          fixed4 frag (v2f i) : SV_Target
		//	{
		//		//選択されていない時は抜ける
		//		if(_IsSelectEffect)
		//		{
		//			discard;
		//		}
  //              fixed4 col = _SelectEffectColor;
		//		col.w = abs(sin(_Time.y)) * 0.8;                
  //              return col;
  //          }
  //          ENDCG
  //      }

		//		Tags { 
		//"RenderType"="Transparent"
		//"Queue"="Background"
		// }

		//	//輪郭
		//Pass
  //      {

  //          Cull Front

  //          CGPROGRAM
  //          #pragma vertex vert
  //          #pragma fragment frag

  //          #include "UnityCG.cginc"

  //          struct appdata
  //          {
  //              float4 vertex : POSITION;
  //              float3 normal : NORMAL;
  //          };

  //          struct v2f
  //          {
  //              float4 vertex : SV_POSITION;
  //          };


  //          v2f vert (appdata v)
  //          {
  //              v2f o;
               
  //              o.vertex = UnityObjectToClipPos(v.vertex); 
  //              return o;
  //          }
            
  //          fixed4 frag (v2f i) : SV_Target
		//	{
  //              return fixed4(0,0,0,1);
  //          }
  //          ENDCG
  //      }


		////本体
		Tags { "RenderType" = "Opaque" }
        Pass
        {
			Tags{
			 "LightMode" = "ForwardBase"
			}


			Ztest Always
			Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "AutoLight.cginc"

			sampler2D _MainTex;
			float _OutLineWidth;


			uniform float4x4 WVP;
			uniform float _Spec1Power;
			uniform float4 _Spec1Color;
			//ひとつめのライトの色
			uniform float4 _LightColor0;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
				float2 uv : TEXCOORD0;

            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;

				float3 diffuse : TEXCOORD2;
				float3 specular : TEXCOORD3;
		  };
		 v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float4 vertexW = mul(unity_ObjectToWorld, v.vertex);

				o.uv = v.uv;
				float3 normal = UnityObjectToWorldNormal(v.normal);


				float3 L = normalize(_WorldSpaceLightPos0.xyz);
				float3 V = normalize(_WorldSpaceCameraPos - vertexW.xyz);
				float3 N = normal;
				float3 H = normalize(L + V);
				float3 lightCol = _LightColor0.rgb * LIGHT_ATTENUATION(i);;
				float3 NdotL = dot(N, L);

				o.diffuse = (NdotL*0.5 + 0.5) * lightCol;
				o.specular = pow(max(0.0, dot(H, N)), _Spec1Power) * _Spec1Color.xyz * lightCol;  // Half vector

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{

				// texture albedo
				float4 col = float4(0,0,0,1);
				float4 outputColor;
				outputColor.xyz =  i.diffuse * col + i.specular;
				outputColor.w = 1;
				return outputColor;
		}
            ENDCG
        }
		//深度値出力Pass
		//UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}




