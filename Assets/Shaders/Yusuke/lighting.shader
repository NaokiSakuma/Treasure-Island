// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/lighting"{


    Properties {
        _SelectEffectColor("EffectColor(RGB)", Color) = (1.0,1.0,1.0,1.0)//選択時エフェクト色
		_OutLineWidth("OutLineWidth",Range(0.0,1)) = 0.1//輪郭の幅
		_SelectEffectWidth("SelectEffectWidth",Range(0.0,1)) = 0.1//選択時エフェクトの幅
		[Enum(ON,0,OFF,1)]_IsSelectEffect("IsSelectEffect",Float) = 1//選択時エフェクトON OFF
		_Spec1Power("Specular Power", Range(0, 30)) = 10
		_Spec1Color("Specular Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BumpMap ("Normal map", 2D) = "bump" {}
        _Color   ("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _Shininess ("Shininess", Range (0.03, 1.0)) = 0.078125
    }
    SubShader {

				Tags { 
		"RenderType"="Transparent"
		"Queue"="AlphaTest"
		 }


        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
		

				Tags { 
		"RenderType"="Transparent"
		"Queue"="Background"
		 }

        Alphatest Greater [_Cutoff]

        Tags { "Queue"="Geometry" "RenderType"="Opaque"}

				pass
        {
            Cull Back   
			ZWrite Off
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
				v.normal = normalize(v.normal);
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


		//輪郭
		Pass
        {
		    Cull Back 
			ZWrite Off  
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


        Pass {

            Tags { "LightMode"="ForwardBase" }
            CGPROGRAM

            float4 _LightColor0;
            float4 _Color;
            float4 _SpecColor;
            sampler2D _MainTex;
            sampler2D _BumpMap;
            half _Shininess;

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma fragmentoption ARB_fog_exp2
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma target 3.0

            #include "UnityCG.cginc"
			#include "AutoLight.cginc"

            struct vertInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 pos      : SV_POSITION;
                float2 uv       : TEXCOORD0;
                float3 viewDir  : TEXCOORD1;
                float3 lightDir : TEXCOORD2;
                LIGHTING_COORDS(3, 4)
            };
			uniform float _OutLineWidth;

            // Vertex shader function.
            v2f vert(appdata_tan v) {
                v2f o;
				v.normal = normalize(v.normal);
				v.vertex  -= float4(v.normal * _OutLineWidth, 0);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.texcoord.xy;
                TANGENT_SPACE_ROTATION;
                o.viewDir  = mul(rotation, ObjSpaceViewDir(v.vertex));
                o.lightDir = mul(rotation, ObjSpaceLightDir(v.vertex));

                TRANSFER_VERTEX_TO_FRAGMENT(o);

                return o;
            }

            // Fragment shader function.
            float4 frag(v2f i) : COLOR {
                i.viewDir  = normalize(i.viewDir);
                i.lightDir = normalize(i.lightDir);

                fixed atten = LIGHT_ATTENUATION(i);

                fixed4 tex = tex2D(_MainTex, i.uv);
                fixed  gloss = tex.a;
                tex *= _Color;
                fixed3 normal = UnpackNormal(tex2D(_BumpMap, i.uv));

                half3 h = normalize(i.lightDir + i.viewDir);

                fixed4 diff = saturate(dot(normal, i.lightDir));

                float nh = saturate(dot(normal, h));
                float spec = pow(nh, _Shininess * 128.0) * gloss;

                fixed4 color;
                color.rgb  = UNITY_LIGHTMODEL_AMBIENT.rgb * 2 * tex.rgb;
                color.rgb += (tex.rgb * _LightColor0.rgb * diff + _LightColor0.rgb * _SpecColor.rgb * spec) * (atten * 2);
                color.a    = tex.a + (_LightColor0.a * _SpecColor.a * spec * atten);
				color.a = 1;
                return color;
            }

            ENDCG
        }



    }
    FallBack "Diffuse"
}