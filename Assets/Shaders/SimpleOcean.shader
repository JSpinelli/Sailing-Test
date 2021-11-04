// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SimpleOcean"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_WindDirection("Wind Direction", Vector) = (0,0,0,0)
		_SeaColor1("Sea Color", Color) = (0,0,0,0)
		_SeaColor2("Sea Color 2", Color) = (0,0,0,0)
		_ColorCutOff("ColorCutOff", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard alpha:fade keepalpha exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float2 _WindDirection;
		uniform float _ColorCutOff;
		uniform float4 _SeaColor1;
		uniform float4 _SeaColor2;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner18 = ( 1.0 * _Time.y * _WindDirection + i.uv_texcoord);
			float4 tex2DNode1 = tex2D( _TextureSample0, panner18 );
			o.Normal = tex2DNode1.rgb;
			float4 ifLocalVar45 = 0;
			if( tex2DNode1.b >= _ColorCutOff )
				ifLocalVar45 = _SeaColor1;
			else
				ifLocalVar45 = _SeaColor2;
			o.Albedo = ifLocalVar45.rgb;
			o.Alpha = ifLocalVar45.a;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
307.2;73.6;951;567;1664.287;88.04115;1.6;True;False
Node;AmplifyShaderEditor.Vector2Node;21;-2896.283,270.6905;Inherit;False;Property;_WindDirection;Wind Direction;2;0;Create;True;0;0;0;False;0;False;0,0;0.36,-0.16;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-2328.938,165.677;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;18;-2045.72,359.8605;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;10;-1985.342,-255.6326;Inherit;False;1012.609;468.3058;Comment;5;44;46;11;45;47;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;44;-1919.389,-28.22577;Inherit;False;Property;_SeaColor2;Sea Color 2;4;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.1445799,0.5377358,0.3607327,0.8313726;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1736.804,300.2118;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;26bad33503aebc440b9572ec6fb06a75;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;-1935.342,-203.0549;Inherit;False;Property;_SeaColor1;Sea Color;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.1445799,0.5377358,0.3607327,0.8313726;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;46;-1667.811,-193.9854;Inherit;False;Property;_ColorCutOff;ColorCutOff;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;3;-1755.702,801.3828;Inherit;False;1288.573;419.9508;Refraction + UV;3;7;6;5;Refraction + UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;39;-3156.026,-1143.904;Inherit;False;1610.788;831.0103;Comment;17;22;23;24;25;26;27;28;29;30;31;32;33;34;35;36;37;38;Collision Foam;1,1,1,1;0;0
Node;AmplifyShaderEditor.ConditionalIfNode;45;-1437.642,-65.64345;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;22;-3134.872,-862.5256;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;33;-2360.814,-747.481;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1649.704,1039.058;Inherit;False;Property;_Distort1;Distort;1;0;Create;True;0;0;0;False;0;False;0;0.142;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;6;-1301.778,888.9241;Inherit;False;DepthMaskedRefraction;-1;;1;c805f061214177c42bca056464193f81;2,40,0,103,0;2;35;FLOAT3;0,0,0;False;37;FLOAT;0.02;False;1;FLOAT2;38
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-1820.193,-882.3449;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;29;-2936.61,-504.8384;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DitheringNode;37;-2063.343,-942.7344;Inherit;False;0;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-3106.026,-1093.904;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;-3113.01,-956.2093;Inherit;False;Property;_FoamSpeed1;Foam Speed;7;0;Create;True;0;0;0;False;0;False;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;26;-2569.328,-1063.622;Inherit;True;Property;_FoamTexture1;FoamTexture;6;0;Create;True;0;0;0;False;0;False;-1;None;85e2496d0caa1d14884d80beb63b2957;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;-2773.455,-584.507;Inherit;False;Property;_OffsetEdge1;Offset Edge;5;0;Create;True;0;0;0;False;0;False;0;2.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;32;-2587.619,-622.5294;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;35;-2247.217,-1081.399;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;34;-2188.03,-595.0682;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;38;-1977.441,-786.6533;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;25;-2784.83,-989.6202;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenDepthNode;31;-2658.465,-788.4971;Inherit;False;1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;27;-2871.594,-788.2769;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;7;-896.8762,884.5118;Inherit;False;Global;_GrabScreen1;Grab Screen 0;16;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMinOpNode;30;-2505.542,-491.5463;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;47;-1126.165,36.57098;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-282.926,37.55653;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;SimpleOcean;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;3;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;0;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;18;0;20;0
WireConnection;18;2;21;0
WireConnection;1;1;18;0
WireConnection;45;0;1;3
WireConnection;45;1;46;0
WireConnection;45;2;11;0
WireConnection;45;3;11;0
WireConnection;45;4;44;0
WireConnection;33;0;31;0
WireConnection;33;1;32;0
WireConnection;6;35;1;0
WireConnection;6;37;5;0
WireConnection;36;0;37;0
WireConnection;36;1;38;0
WireConnection;37;0;35;0
WireConnection;26;1;25;0
WireConnection;32;0;27;4
WireConnection;32;1;28;0
WireConnection;35;0;26;0
WireConnection;34;0;33;0
WireConnection;38;0;33;0
WireConnection;25;0;24;0
WireConnection;25;2;21;0
WireConnection;25;1;22;0
WireConnection;7;0;6;38
WireConnection;30;0;28;0
WireConnection;30;1;29;3
WireConnection;47;0;45;0
WireConnection;0;0;45;0
WireConnection;0;1;1;0
WireConnection;0;9;47;3
ASEEND*/
//CHKSM=231E8C963A37E99E105B1F111F6EDE642BC1589F