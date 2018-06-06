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
	}

	SubShader
	{
		Tags { 
		"RenderType"="Transparent"
		"Queue"="AlphaTest"
		 }


        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

		//選択時エフェクト
		pass
        {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			fixed4 _SelectEffectColor;
			float _SelectEffectWidth;
			float _IsSelectEffect;

			uniform  float4x4 EffectWVP;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
				v.vertex += float4(v.normal * _SelectEffectWidth, 0);
				o.vertex = UnityObjectToClipPos(v.vertex); 

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
			{
				//選択されていない時は抜ける
				if(_IsSelectEffect)
				{
					discard;
				}
                fixed4 col = _SelectEffectColor;
				col.w = abs(sin(_Time.y)) * 0.8;                
                return col;
            }
            ENDCG
        }

				Tags { 
		"RenderType"="Transparent"
		"Queue"="Background"
		 }

			//輪郭
		Ztest Always
		Pass
        {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o;
               
                o.vertex = UnityObjectToClipPos(v.vertex); 
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
			{
                return fixed4(0,0,0,1);
            }
            ENDCG
        }

		Tags { 
		"RenderType"="Transparent"
		"Queue"="AlphaTest"
		 }
				//本体
        Pass
        {
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			sampler2D _MainTex;
			float _OutLineWidth;


			uniform float4x4 WVP;


            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
				//法線方向に頂点を移動させる
				float3 nnormal;
				nnormal = normalize(v.normal);
				v.vertex -= float4(nnormal * _OutLineWidth, 0);
				//o.vertex = mul(WVP,v.vertex); 
				o.vertex = UnityObjectToClipPos(v.vertex); 

				o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {                
              return fixed4(1,0,0,1);
            }
            ENDCG
        }

	


		
		//深度値出力Pass
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}

}
