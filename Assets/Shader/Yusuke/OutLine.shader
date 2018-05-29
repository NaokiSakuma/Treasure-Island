Shader "Unlit/NewUnlitShader"
{
	Properties
	{
        _SelectEffectColor("EffectColor(RGB)", Color) = (1.0,1.0,1.0,1.0)
		_OutLineWidth("OutLineWidth",Range(0.0,1)) = 0.1
		_SelectEffectWidth("SelectEffectWidth",Range(0.0,1)) = 0.1
		[Enum(ON,0,OFF,1)]_IsSelectEffect("IsSelectEffect",Float) = 1
	}

	SubShader
	{
		Tags { 
		"RenderType"="Opaque"
		"Queue"="AlphaTest"
		 }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
 Pass
        {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			fixed4 _SelectEffectColor;
			float _SelectEffectWidth;
			float _IsSelectEffect;

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

        Pass
        {
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			float _OutLineWidth;


            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            v2f vert (appdata v)
            {
                v2f o;
				v.vertex -= float4(v.normal * _OutLineWidth, 0);   
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {                
                return fixed4(1,1,1,1);                
            }
            ENDCG
        }
	}
}
