// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "InkedOcean"
{
	Properties
	{
		_BaseColor("Base Color", Color) = (0.8867924,0.8867924,0.8867924,0)
		_RippleDensity("Ripple Density", Float) = 3.43
		_Ripple("Ripple", Float) = 1.3
		[HDR]_RippleColor("Ripple Color", Color) = (0.4056604,0.380785,0.380785,0)
		_OffsetEdge("Offset Edge", Float) = 0
		_FoamTexture("FoamTexture", 2D) = "white" {}
		_FoamSpeed("Foam Speed", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPosition;
			float2 uv_texcoord;
		};

		uniform sampler2D _FoamTexture;
		uniform float _FoamSpeed;
		uniform float4 _FoamTexture_ST;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _OffsetEdge;
		uniform float4 _BaseColor;
		uniform float4 _RippleColor;
		uniform float _RippleDensity;
		uniform float _Ripple;


		inline float Dither4x4Bayer( int x, int y )
		{
			const float dither[ 16 ] = {
				 1,  9,  3, 11,
				13,  5, 15,  7,
				 4, 12,  2, 10,
				16,  8, 14,  6 };
			int r = y * 4 + x;
			return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
		}


		float2 voronoihash2( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi2( float2 v, float time, inout float2 id, inout float2 mr, float smoothness )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash2( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.5 * dot( r, r );
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			 		}
			 	}
			}
			return F1;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen43 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither43 = Dither4x4Bayer( fmod(clipScreen43.x, 4), fmod(clipScreen43.y, 4) );
			float2 temp_cast_0 = (_FoamSpeed).xx;
			float2 uv_FoamTexture = i.uv_texcoord * _FoamTexture_ST.xy + _FoamTexture_ST.zw;
			float2 panner58 = ( _Time.y * temp_cast_0 + uv_FoamTexture);
			float grayscale37 = Luminance(tex2D( _FoamTexture, panner58 ).rgb);
			dither43 = step( dither43, grayscale37 );
			float eyeDepth23 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float smoothstepResult28 = smoothstep( 0.0 , 1.0 , ( 1.0 - ( eyeDepth23 - ( ase_screenPos.w - min( _OffsetEdge , _SinTime.z ) ) ) ));
			float2 clipScreen18 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither18 = Dither4x4Bayer( fmod(clipScreen18.x, 4), fmod(clipScreen18.y, 4) );
			float time2 = ( _Time.y * _Ripple );
			float2 temp_output_1_0_g1 = i.uv_texcoord;
			float2 temp_output_11_0_g1 = ( temp_output_1_0_g1 - float2( 0.5,0.5 ) );
			float2 break18_g1 = temp_output_11_0_g1;
			float2 appendResult19_g1 = (float2(break18_g1.y , -break18_g1.x));
			float dotResult12_g1 = dot( temp_output_11_0_g1 , temp_output_11_0_g1 );
			float2 coords2 = ( temp_output_1_0_g1 + ( appendResult19_g1 * ( dotResult12_g1 * float2( 10,10 ) ) ) + float2( 0,0 ) ) * _RippleDensity;
			float2 id2 = 0;
			float2 uv2 = 0;
			float voroi2 = voronoi2( coords2, time2, id2, uv2, 0 );
			dither18 = step( dither18, ( _RippleColor * pow( voroi2 , 2.26 ) ).r );
			o.Albedo = ( 1.0 - ( ( dither43 * smoothstepResult28 ) + ( _BaseColor + dither18 ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
200.8;73.6;1057;439;2379.896;1229.629;3.646837;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;3;-992.3044,128.1114;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-967.3044,212.1114;Inherit;False;Property;_Ripple;Ripple;2;0;Create;True;0;0;0;False;0;False;1.3;1.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-962.178,-423.1996;Inherit;False;Property;_OffsetEdge;Offset Edge;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;56;-824.1794,-355.5396;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;13;-884.6098,-203.0426;Inherit;False;Radial Shear;-1;;1;c6dc9fc7fa9b08c4d95138f2ae88b526;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-974.3044,37.11139;Inherit;False;Property;_RippleDensity;Ripple Density;1;0;Create;True;0;0;0;False;0;False;3.43;3.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;57;-646.2427,-427.0811;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;59;-1272.048,-1049.864;Inherit;False;0;36;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;60;-1262.315,-877.4466;Inherit;False;Property;_FoamSpeed;Foam Speed;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;21;-940.2949,-614.6537;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;61;-1262.315,-777.3333;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-780.3044,140.1114;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;23;-682.2312,-679.6568;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;26;-486.0153,-510.1328;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;58;-950.8516,-945.5795;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-636.9769,328.9569;Inherit;False;Constant;_VoronoiPower;Voronoi Power;3;0;Create;True;0;0;0;False;0;False;2.26;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;2;-620.8358,53.43804;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SamplerNode;36;-695.2255,-1009.548;Inherit;True;Property;_FoamTexture;FoamTexture;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;8;-390.2135,60.03108;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;22;-360.8489,-598.5461;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-382.072,-152.255;Inherit;False;Property;_RippleColor;Ripple Color;3;1;[HDR];Create;True;0;0;0;False;0;False;0.4056604,0.380785,0.380785,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;33;-179.9174,-550.4279;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;37;-276.3503,-801.3983;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-154.6901,3.721489;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;28;-43.89081,-479.3581;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-355.3044,-332.8886;Inherit;False;Property;_BaseColor;Base Color;0;0;Create;True;0;0;0;False;0;False;0.8867924,0.8867924,0.8867924,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DitheringNode;18;-59.17493,-129.8887;Inherit;False;0;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;43;-5.844662,-749.5326;Inherit;False;0;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;151.1235,-239.4167;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;158.4297,-629.4059;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;423.8853,-483.6682;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;17;614.7751,-549.5936;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;860.6948,-624.7946;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;InkedOcean;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;57;0;25;0
WireConnection;57;1;56;3
WireConnection;5;0;3;0
WireConnection;5;1;6;0
WireConnection;26;0;21;4
WireConnection;26;1;57;0
WireConnection;58;0;59;0
WireConnection;58;2;60;0
WireConnection;58;1;61;0
WireConnection;2;0;13;0
WireConnection;2;1;5;0
WireConnection;2;2;7;0
WireConnection;36;1;58;0
WireConnection;8;0;2;0
WireConnection;8;1;9;0
WireConnection;22;0;23;0
WireConnection;22;1;26;0
WireConnection;33;0;22;0
WireConnection;37;0;36;0
WireConnection;11;0;10;0
WireConnection;11;1;8;0
WireConnection;28;0;33;0
WireConnection;18;0;11;0
WireConnection;43;0;37;0
WireConnection;12;0;1;0
WireConnection;12;1;18;0
WireConnection;30;0;43;0
WireConnection;30;1;28;0
WireConnection;32;0;30;0
WireConnection;32;1;12;0
WireConnection;17;0;32;0
WireConnection;0;0;17;0
ASEEND*/
//CHKSM=D803475847DEBCC98DB9F3398C134B182110C213