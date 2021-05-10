// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 16
		_TessMin( "Tess Min Distance", Float ) = 10
		_TessMax( "Tess Max Distance", Float ) = 25
		_Normals_3D("Normals_3D", 3D) = "white" {}
		_NormalPower("Normal Power", Range( 0 , 1)) = 0
		_Displace_3D("Displace_3D", 3D) = "white" {}
		_Tiling("Tiling", Float) = 0
		_WaveHeight("Wave Height", Range( 0 , 1)) = 0
		_SeaColor("Sea Color", Color) = (0,0,0,0)
		_WaveSpeed("Wave Speed", Float) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Distort("Distort", Range( 0 , 0.2)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		#pragma surface surf Standard alpha:fade keepalpha vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
		};

		uniform sampler3D _Displace_3D;
		uniform float _Tiling;
		uniform float _WaveSpeed;
		uniform float _WaveHeight;
		uniform sampler3D _Normals_3D;
		uniform float _NormalPower;
		uniform float4 _SeaColor;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Distort;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Smoothness;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float mulTime17 = _Time.y * _WaveSpeed;
			float4 appendResult15 = (float4(( ase_worldPos.x * _Tiling ) , ( ase_worldPos.z * _Tiling ) , mulTime17 , 0.0));
			float4 _3D_UVS18 = appendResult15;
			float4 tex3DNode26 = tex3Dlod( _Displace_3D, float4( _3D_UVS18.xyz, 0.0) );
			float4 vertexOffset30 = ( tex3DNode26 * float4( ( float3(0,1,0) * (0.0 + (_WaveHeight - 0.0) * (1.0 - 0.0) / (1.0 - 0.0)) ) , 0.0 ) );
			v.vertex.xyz += vertexOffset30.rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float mulTime17 = _Time.y * _WaveSpeed;
			float4 appendResult15 = (float4(( ase_worldPos.x * _Tiling ) , ( ase_worldPos.z * _Tiling ) , mulTime17 , 0.0));
			float4 _3D_UVS18 = appendResult15;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float lerpResult5 = lerp( 0.0 , _NormalPower , ase_worldNormal.y);
			float3 normals23 = UnpackScaleNormal( tex3D( _Normals_3D, _3D_UVS18.xyz ), lerpResult5 );
			o.Normal = normals23;
			float4 tex3DNode26 = tex3D( _Displace_3D, _3D_UVS18.xyz );
			float4 _displace29 = tex3DNode26;
			float4 lerpResult54 = lerp( _SeaColor , ( _SeaColor + 0.1 ) , _displace29);
			float4 Albedo56 = lerpResult54;
			o.Albedo = Albedo56.rgb;
			float4 ase_vertex4Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float3 ase_viewPos = UnityObjectToViewPos( ase_vertex4Pos );
			float ase_screenDepth = -ase_viewPos.z;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float eyeDepth28_g1 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float2 temp_output_20_0_g1 = ( (normals23).xy * ( _Distort / max( ase_screenDepth , 0.1 ) ) * saturate( ( eyeDepth28_g1 - ase_screenDepth ) ) );
			float eyeDepth2_g1 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ( float4( temp_output_20_0_g1, 0.0 , 0.0 ) + ase_screenPosNorm ).xy ));
			float2 temp_output_32_0_g1 = (( float4( ( temp_output_20_0_g1 * saturate( ( eyeDepth2_g1 - ase_screenDepth ) ) ), 0.0 , 0.0 ) + ase_screenPosNorm )).xy;
			float2 temp_output_1_0_g1 = ( ( floor( ( temp_output_32_0_g1 * (_CameraDepthTexture_TexelSize).zw ) ) + 0.5 ) * (_CameraDepthTexture_TexelSize).xy );
			float2 temp_output_71_38 = temp_output_1_0_g1;
			float4 screenColor72 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,temp_output_71_38);
			float4 _distortion74 = screenColor72;
			o.Emission = _distortion74.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
201;73;1276;652;6601.817;2407.173;6.272859;True;False
Node;AmplifyShaderEditor.CommentaryNode;10;-2233.553,-1145.031;Inherit;False;1159.055;647.9687;Comment;9;11;13;15;16;18;19;21;22;20;3D Texture UVs;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;21;-1787.969,-919.0488;Inherit;False;212;185;V;1;14;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;22;-1801.808,-700.0341;Inherit;False;248;161;UV Scrolling (W);1;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;20;-1787.518,-1108.296;Inherit;False;212;185;U;1;12;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2040.983,-655.4016;Inherit;False;Property;_WaveSpeed;Wave Speed;14;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-2172.67,-849.9507;Inherit;False;Property;_Tiling;Tiling;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;11;-2179.262,-1072.8;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleTimeNode;17;-1751.808,-650.0341;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1726.969,-871.0488;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1737.518,-1058.296;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;9;-878.8771,-1065.746;Inherit;False;1471.516;664.4742;Comment;4;23;3;4;25;NORMALS;1,1,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;-1497.478,-1045.36;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;25;-816.3774,-789.6975;Inherit;False;557.3066;355.1965;Makes sure no waves are proyected when the texture is completly vertical;3;5;7;6;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;-1274.473,-1048.699;Inherit;False;_3D_UVS;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldNormalVector;7;-740.8291,-628.6444;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;6;-795.1776,-749.2411;Inherit;False;Property;_NormalPower;Normal Power;6;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;5;-453.8708,-663.888;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;4;-462.9814,-1015.27;Inherit;True;18;_3D_UVS;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;38;-2263.996,-130.0542;Inherit;False;1247.701;744.4673;comment;10;27;26;29;28;30;32;31;34;33;50;Displace;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;3;-66.86259,-809.0389;Inherit;True;Property;_Normals_3D;Normals_3D;5;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;LockedToTexture3D;True;Object;-1;Auto;Texture3D;8;0;SAMPLER3D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;27;-2191.645,62.84591;Inherit;False;18;_3D_UVS;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2213.996,433.4132;Inherit;False;Property;_WaveHeight;Wave Height;10;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;26;-1918.53,25.21853;Inherit;True;Property;_Displace_3D;Displace_3D;7;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;LockedToTexture3D;False;Object;-1;Auto;Texture3D;8;0;SAMPLER3D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;329.353,-776.627;Inherit;False;normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;75;-3889.87,333.1379;Inherit;False;1288.573;419.9508;Refraction + UV;6;70;69;71;73;72;74;Refraction + UV;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;57;-3967.266,-405.7413;Inherit;False;999.1282;378.4342;Comment;6;51;52;53;55;54;56;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;-3810.105,383.1379;Inherit;False;23;normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;32;-1842.196,407.4133;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;34;-2042.395,233.2132;Inherit;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-1461.046,-80.05408;Inherit;False;_displace;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;51;-3917.266,-353.1638;Inherit;False;Property;_SeaColor;Sea Color;12;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;52;-3857.686,-146.3071;Inherit;False;Constant;_Float0;Float 0;12;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-3839.87,528.4739;Inherit;False;Property;_Distort;Distort;19;0;Create;True;0;0;0;False;0;False;0;0;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-3616.686,-260.3071;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;-3647.686,-143.3071;Inherit;False;29;_displace;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1623.194,250.5131;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;71;-3512.432,423.4109;Inherit;False;DepthMaskedRefraction;-1;;1;c805f061214177c42bca056464193f81;2,40,0,103,0;2;35;FLOAT3;0,0,0;False;37;FLOAT;0.02;False;1;FLOAT2;38
Node;AmplifyShaderEditor.LerpOp;54;-3410.686,-347.3071;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;72;-3085.304,404.3679;Inherit;False;Global;_GrabScreen0;Grab Screen 0;16;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1443.846,46.84593;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;50;-1449.031,225.7451;Inherit;False;357.1121;279.1684;Goes between the other multipliers;2;36;37;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;74;-2825.298,418.6888;Inherit;False;_distortion;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;78;-801.5203,9.185879;Inherit;False;716.804;631.1497;https://www.youtube.com/watch?v=zD6GV6bZenM;6;39;1;0;58;24;76;From tutorial;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;49;-4147.463,-1284.337;Inherit;False;1671.723;677.1751;Uses a texture as clipping mask (how to tell the shader were the shore is);10;41;40;42;44;43;46;47;45;48;35;Smoother;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-1240.295,40.81402;Inherit;False;vertexOffset;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-3192.138,-355.7413;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-641.9821,59.1858;Inherit;False;56;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-3819.436,-2072.691;Inherit;False;Property;_CausticsScale;Caustics Scale;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;65;-4053.435,-2227.392;Inherit;False;19;_Time;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-3848.035,-2215.692;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;63;-3356.646,-2349.592;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;62;-3113.544,-2307.99;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-2908.143,-2237.791;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;60;-3336.381,-2188.381;Inherit;False;Property;_CausticsColor;Caustics Color;15;0;Create;True;0;0;0;False;0;False;0.8773585,0.8235582,0.8235582,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;64;-3651.746,-2335.292;Inherit;False;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.ComponentMaskNode;68;-3950.736,-2372.991;Inherit;False;True;True;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TFHCRemapNode;47;-3423.529,-814.1619;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-644.0286,146.5482;Inherit;False;23;normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;-1421.267,421.3429;Inherit;False;35;_cheekySmoother;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-2848.152,-830.7573;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-751.5203,361.8262;Inherit;False;Property;_Smoothness;Smoothness;18;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;-722.4072,508.5262;Inherit;False;30;vertexOffset;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-3733.299,-796.7546;Inherit;False;Constant;_MaskStrentgh;Mask Strentgh;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;73;-3093.1,637.0888;Inherit;False;_refractUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1246.201,271.1139;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;40;-3817.593,-1210.517;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;19;-1275.5,-726.3013;Inherit;False;_Time;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;42;-3424.587,-1099.365;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-3855.909,-962.4539;Inherit;False;Property;_ProjectionOffset;Projection Offset;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;76;-695.4238,266.9768;Inherit;False;74;_distortion;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;45;-3192.601,-1054.316;Inherit;True;Property;_WaveMask;Wave Mask;16;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;-2716.74,-1012.254;Inherit;False;_cheekySmoother;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;41;-4097.463,-1234.337;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;-3853.102,-1055.064;Inherit;False;Property;_ProjectionScale;Projection Scale;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-3037.879,-2032.102;Inherit;False;Property;_CausticsPower;Caustics Power;13;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-339.7162,169.3356;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;1;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;0;16;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;16;0
WireConnection;14;0;11;3
WireConnection;14;1;13;0
WireConnection;12;0;11;1
WireConnection;12;1;13;0
WireConnection;15;0;12;0
WireConnection;15;1;14;0
WireConnection;15;2;17;0
WireConnection;18;0;15;0
WireConnection;5;1;6;0
WireConnection;5;2;7;2
WireConnection;3;1;4;0
WireConnection;3;5;5;0
WireConnection;26;1;27;0
WireConnection;23;0;3;0
WireConnection;32;0;31;0
WireConnection;29;0;26;0
WireConnection;53;0;51;0
WireConnection;53;1;52;0
WireConnection;33;0;34;0
WireConnection;33;1;32;0
WireConnection;71;35;69;0
WireConnection;71;37;70;0
WireConnection;54;0;51;0
WireConnection;54;1;53;0
WireConnection;54;2;55;0
WireConnection;72;0;71;38
WireConnection;28;0;26;0
WireConnection;28;1;33;0
WireConnection;74;0;72;0
WireConnection;30;0;28;0
WireConnection;56;0;54;0
WireConnection;66;0;65;0
WireConnection;63;0;64;0
WireConnection;62;0;63;0
WireConnection;61;0;62;0
WireConnection;61;1;60;0
WireConnection;64;0;68;0
WireConnection;64;1;66;0
WireConnection;64;2;67;0
WireConnection;47;0;46;0
WireConnection;48;0;45;0
WireConnection;48;1;47;0
WireConnection;73;0;71;38
WireConnection;37;1;36;0
WireConnection;40;0;41;1
WireConnection;40;2;41;3
WireConnection;19;0;17;0
WireConnection;42;0;40;0
WireConnection;42;1;44;0
WireConnection;42;2;43;0
WireConnection;45;1;42;0
WireConnection;35;0;48;0
WireConnection;0;0;58;0
WireConnection;0;1;24;0
WireConnection;0;2;76;0
WireConnection;0;4;1;0
WireConnection;0;11;39;0
ASEEND*/
//CHKSM=D925E5C2125287832230F45C82CD45C039DF0D23