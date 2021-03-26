// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 Shader "MysticArsenal/MysticHSVSoft" { 
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _HueShift("HueShift", Float ) = 0
        _Sat("Saturation", Float) = 1
        _Val("Value", Float) = 1
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
    }
	
	
    SubShader {
 
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType" = "Transparent" }
        ZWrite Off
        Blend SrcAlpha One
		AlphaTest Greater .01
		ColorMask RGB
        Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
			#pragma multi_compile_particles
 
            #include "UnityCG.cginc"
 
            float3 shift_col(float3 RGB, float3 shift)
            {
            float3 RESULT = float3(RGB);
            float VSU = shift.z*shift.y*cos(shift.x*3.14159265/180);
                float VSW = shift.z*shift.y*sin(shift.x*3.14159265/180);
               
                RESULT.x = (.299*shift.z+.701*VSU+.168*VSW)*RGB.x
                        + (.587*shift.z-.587*VSU+.330*VSW)*RGB.y
                        + (.114*shift.z-.114*VSU-.497*VSW)*RGB.z;
               
                RESULT.y = (.299*shift.z-.299*VSU-.328*VSW)*RGB.x
                        + (.587*shift.z+.413*VSU+.035*VSW)*RGB.y
                        + (.114*shift.z-.114*VSU+.292*VSW)*RGB.z;
               
                RESULT.z = (.299*shift.z-.3*VSU+1.25*VSW)*RGB.x
                        + (.587*shift.z-.588*VSU-1.05*VSW)*RGB.y
                        + (.114*shift.z+.886*VSU-.203*VSW)*RGB.z;
               
            return (RESULT);
            }
 
            struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 DisCoord : TEXCOORD1;
                float2 uv : TEXCOORD2;
				//#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD3;
				//#endif
            };
 
            float4 _MainTex_ST;
			float4 _Dist_ST;
 
            v2f vert (appdata_base v)
            {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				//#endif
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.DisCoord = TRANSFORM_TEX(v.texcoord,_Dist);
				
                return o;
            }
 
            sampler2D _MainTex;
			sampler2D_float _CameraDepthTexture;
            float _HueShift;
            float _Sat;
            float _Val;
			float _InvFade;
 
            half4 frag(v2f i) : COLOR
            {
                half4 col = tex2D(_MainTex, i.uv);
                float3 shift = float3(_HueShift, _Sat, _Val);
               	float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
                return half4( half3(shift_col(col, shift)), col.a * fade);
            }
			
            ENDCG
        }
    }
    Fallback "Particles/Additive"
}