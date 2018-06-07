Shader "Custom/light" {

    SubShader {
        Tags {
            "Queue"="Geometry"
        }

			CGPROGRAM

			#pragma surface surf Lambert vertex:vert
			struct Input {
				float4 vertexColor : COLOR;
			};

			 float _Amount = 0.1;
		  void vert (inout appdata_full v) {
		   v.normal = normalize( v.normal);
			  v.vertex.xyz += v.normal * 0.0001;
		  }

			void surf (Input IN, inout SurfaceOutput o) {
				o.Albedo = fixed4(1,1,1,1);
			}

			ENDCG

    }
}

