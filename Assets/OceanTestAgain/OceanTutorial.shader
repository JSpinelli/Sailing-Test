// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "OceanTutorial"
{
	Properties
	{
		_WaveDirection("Wave Direction", Vector) = (1,1,0,0)
		_WaveSpeed("Wave Speed", Float) = 0
		_WaveScale("Wave Scale", Float) = 0
		_WindPatternScale("Wind Pattern Scale", Float) = 0
		_WaveStretch("Wave Stretch", Vector) = (0,0,0,0)
		_WaveTile("WaveTile", Float) = 1
		_Tesselation("Tesselation", Float) = 8
		_WaveUp("Wave Up", Vector) = (0,0,0,0)
		_WaveHeight("Wave Height", Float) = 2
		_WaterColor("Water Color", Color) = (0,0,0,0)
		_TopColor("Top Color", Color) = (0,0,0,0)
		_DepthDistance("Depth Distance", Float) = 0
		_EdgePower("Edge Power", Range( 0 , 1)) = 1
		_NormalMap("Normal Map", 2D) = "bump" {}
		_NormalTileBase("Normal Tile Base", Float) = 1
		_SecondTileFactor("Second Tile Factor", Float) = 5
		_NormalMainSpeed("Normal Main Speed", Float) = 0
		_NormalStrength("Normal Strength", Float) = 1
		_SeaFoa_M("SeaFoa_M", 2D) = "white" {}
		_EdgeFoamTile("Edge Foam Tile", Float) = 0
		_SeaFoamTile("Sea Foam Tile", Float) = 0
		_Smoothness("Smoothness", Float) = 0
		_RefractAmount("Refract Amount", Float) = 0
		_Depth("Depth", Float) = -4
		_MinTes("Min Tes", Float) = 0
		_MaxTessallation("Max Tessallation", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
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
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
		};

		uniform float3 _WaveUp;
		uniform float _WaveHeight;
		uniform float _WaveSpeed;
		uniform float2 _WaveDirection;
		uniform float2 _WaveStretch;
		uniform float _WaveTile;
		uniform float _WaveScale;
		uniform float _WindPatternScale;
		uniform sampler2D _NormalMap;
		uniform float _NormalMainSpeed;
		uniform float _NormalTileBase;
		uniform float _NormalStrength;
		uniform float _SecondTileFactor;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _RefractAmount;
		uniform float4 _WaterColor;
		uniform float4 _TopColor;
		uniform sampler2D _SeaFoa_M;
		uniform float _SeaFoamTile;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Depth;
		uniform float _EdgePower;
		uniform float _DepthDistance;
		uniform float _EdgeFoamTile;
		uniform float _Smoothness;
		uniform float _MinTes;
		uniform float _MaxTessallation;
		uniform float _Tesselation;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _MinTes,_MaxTessallation,( _WaveHeight * _Tesselation ));
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float temp_output_7_0 = ( _Time.y * _WaveSpeed );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult12 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 worldSpaceTile13 = appendResult12;
			float4 WaveTileUv25 = ( ( worldSpaceTile13 * float4( _WaveStretch, 0.0 , 0.0 ) ) * _WaveTile );
			float2 panner2 = ( temp_output_7_0 * _WaveDirection + WaveTileUv25.xy);
			float simplePerlin2D1 = snoise( panner2*_WaveScale );
			simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
			float2 panner28 = ( temp_output_7_0 * _WaveDirection + ( WaveTileUv25 * float4( 0.1,0.1,0,0 ) ).xy);
			float simplePerlin2D29 = snoise( panner28*_WindPatternScale );
			simplePerlin2D29 = simplePerlin2D29*0.5 + 0.5;
			float WavePattern36 = ( simplePerlin2D1 + simplePerlin2D29 );
			float3 WaveHeight40 = ( _WaveUp * _WaveHeight * WavePattern36 );
			v.vertex.xyz += WaveHeight40;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 appendResult12 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 worldSpaceTile13 = appendResult12;
			float4 temp_output_68_0 = ( ( worldSpaceTile13 / float4( 10,10,10,10 ) ) * _NormalTileBase );
			float2 panner72 = ( 1.0 * _Time.y * ( float2( 1,0 ) * _NormalMainSpeed ) + temp_output_68_0.xy);
			float2 panner73 = ( 1.0 * _Time.y * ( float2( -1,0 ) * 0.0 ) + ( temp_output_68_0 * _SecondTileFactor ).xy);
			float3 Normals85 = BlendNormals( UnpackScaleNormal( tex2D( _NormalMap, panner72 ), _NormalStrength ) , UnpackScaleNormal( tex2D( _NormalMap, panner73 ), _NormalStrength ) );
			o.Normal = Normals85;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor136 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( (ase_grabScreenPosNorm).xyzw + float4( ( _RefractAmount * Normals85 ) , 0.0 ) ).xy);
			float4 clampResult137 = clamp( screenColor136 , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 Refraction138 = clampResult137;
			float2 panner120 = ( 1.0 * _Time.y * float2( 0.3,-0.2 ) + ( worldSpaceTile13 * 0.03 ).xy);
			float simplePerlin2D121 = snoise( panner120 );
			simplePerlin2D121 = simplePerlin2D121*0.5 + 0.5;
			float clampResult123 = clamp( ( tex2D( _SeaFoa_M, ( _SeaFoamTile * ( worldSpaceTile13 / float4( 10,10,10,10 ) ) ).xy ).r * simplePerlin2D121 ) , 0.0 , 1.0 );
			float SeaFoam113 = clampResult123;
			float temp_output_7_0 = ( _Time.y * _WaveSpeed );
			float4 WaveTileUv25 = ( ( worldSpaceTile13 * float4( _WaveStretch, 0.0 , 0.0 ) ) * _WaveTile );
			float2 panner2 = ( temp_output_7_0 * _WaveDirection + WaveTileUv25.xy);
			float simplePerlin2D1 = snoise( panner2*_WaveScale );
			simplePerlin2D1 = simplePerlin2D1*0.5 + 0.5;
			float2 panner28 = ( temp_output_7_0 * _WaveDirection + ( WaveTileUv25 * float4( 0.1,0.1,0,0 ) ).xy);
			float simplePerlin2D29 = snoise( panner28*_WindPatternScale );
			simplePerlin2D29 = simplePerlin2D29*0.5 + 0.5;
			float WavePattern36 = ( simplePerlin2D1 + simplePerlin2D29 );
			float clampResult46 = clamp( WavePattern36 , 0.0 , 1.0 );
			float4 lerpResult45 = lerp( _WaterColor , ( _TopColor + SeaFoam113 ) , clampResult46);
			float4 albedo49 = lerpResult45;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth142 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth142 = abs( ( screenDepth142 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Depth ) );
			float clampResult143 = clamp( distanceDepth142 , 0.0 , 1.0 );
			float depth144 = clampResult143;
			float4 lerpResult145 = lerp( Refraction138 , albedo49 , depth144);
			o.Albedo = lerpResult145.rgb;
			float screenDepth51 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth51 = abs( ( screenDepth51 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) );
			float4 clampResult59 = clamp( ( _EdgePower * ( ( 1.0 - distanceDepth51 ) + tex2D( _SeaFoa_M, ( ( worldSpaceTile13 / float4( 10,10,10,10 ) ) * _EdgeFoamTile ).xy ) ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 Edge56 = clampResult59;
			o.Emission = Edge56.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
250;73;1323;654;668.5274;-74.15201;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;14;-2698.206,-387.6422;Inherit;False;756.8882;308.4;Comment;3;11;12;13;World Space UVs;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;11;-2648.206,-320.166;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;12;-2400.866,-334.7843;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;86;-3209.777,-2438.921;Inherit;False;2219.788;972.1626;Comment;20;72;73;68;66;69;80;79;82;78;81;75;77;70;61;43;64;84;83;85;89;Normals;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;27;-1601.821,-463.8393;Inherit;False;857.101;344.4004;Comment;6;15;16;17;18;19;25;Wave Tilling;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;13;-2171.718,-337.6422;Inherit;True;worldSpaceTile;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;66;-3163.677,-2309.86;Inherit;True;13;worldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;16;-1526.755,-336.5257;Inherit;False;Property;_WaveStretch;Wave Stretch;5;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;15;-1551.821,-413.8393;Inherit;False;13;worldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1307.737,-387.369;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1342.936,-234.8388;Inherit;False;Property;_WaveTile;WaveTile;6;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;89;-3004.616,-2077.221;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;10,10,10,10;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-3075.783,-1894.58;Inherit;False;Property;_NormalTileBase;Normal Tile Base;15;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;78;-2672.6,-1706.897;Inherit;False;Constant;_SecondaryNormalPan;Secondary Normal Pan;18;0;Create;True;0;0;0;False;0;False;-1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;80;-2834.722,-2232.147;Inherit;False;Property;_NormalMainSpeed;Normal Main Speed;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-2924.964,-1810.359;Inherit;False;Property;_SecondTileFactor;Second Tile Factor;16;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-2683.736,-1582.759;Inherit;False;Constant;_SecondaryNormalSpeed;Secondary Normal Speed;19;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;77;-2827.377,-2388.921;Inherit;False;Constant;_MainNormalPan;Main Normal Pan;18;0;Create;True;0;0;0;False;0;False;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;114;-4875.768,-227.4256;Inherit;False;2007.937;756.582;Comment;12;123;122;121;120;119;118;107;111;112;110;109;113;Sea Foam;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1135.652,-327.3998;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-2793.098,-2067.95;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-4698.458,254.7964;Inherit;False;Constant;_FoamMask;Foam Mask;23;0;Create;True;0;0;0;False;0;False;0.03;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;109;-4670.021,-68.10353;Inherit;True;13;worldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-969.5195,-322.1774;Inherit;False;WaveTileUv;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-2614.839,-2346.487;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-2397.116,-1737.404;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-2476.681,-1903.951;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;34;-2591.315,559.2725;Inherit;False;1344.789;956.2808;Comment;15;36;33;28;29;3;1;8;2;30;7;32;31;5;6;26;Wave Pattern;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;61;-2350.281,-2071.303;Inherit;True;Property;_NormalMap;Normal Map;14;0;Create;True;0;0;0;False;0;False;6a416c2541c3e604da58ccc563658381;6a416c2541c3e604da58ccc563658381;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;73;-2219.279,-1844.368;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-2077.576,-2002.139;Inherit;False;Property;_NormalStrength;Normal Strength;18;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;72;-2216.843,-2237.058;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2541.315,1000.142;Inherit;False;Property;_WaveSpeed;Wave Speed;1;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;57;-4524.531,-1209.218;Inherit;False;1475.615;809.4247;;14;104;103;102;101;56;59;54;105;55;53;51;52;106;100;Edge;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-4554.217,-166.7552;Inherit;False;Property;_SeaFoamTile;Sea Foam Tile;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;-4465.928,198.5168;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;111;-4366.739,-62.95843;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;10,10,10,10;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-2530.003,902.5782;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;-2456.834,1143.375;Inherit;False;25;WaveTileUv;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TexturePropertyNode;100;-4462.259,-610.8126;Inherit;True;Property;_SeaFoa_M;SeaFoa_M;19;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;43;-1864.986,-2192.642;Inherit;True;Property;_Normals;Normals;12;0;Create;True;0;0;0;False;0;False;-1;6a416c2541c3e604da58ccc563658381;6a416c2541c3e604da58ccc563658381;True;0;True;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;64;-1857.649,-1956.605;Inherit;True;Property;_TextureSample0;Texture Sample 0;16;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;26;-2313.264,609.2725;Inherit;False;25;WaveTileUv;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;3;-2458.574,718.7786;Inherit;False;Property;_WaveDirection;Wave Direction;0;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;-4174.979,-128.2224;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-2313.668,899.7502;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-2250.042,1184.377;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.1,0.1,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;120;-4244.266,130.9633;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.3,-0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2060.503,896.7284;Inherit;False;Property;_WaveScale;Wave Scale;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1998.096,1181.153;Inherit;False;Property;_WindPatternScale;Wind Pattern Scale;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;83;-1484.773,-2022.338;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;121;-3973.301,85.86525;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;28;-2047.121,993.762;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;107;-3961.395,-143.2693;Inherit;True;Property;_TextureSample2;Texture Sample 2;22;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;2;-2098.554,692.9571;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-3624.672,-28.18193;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;85;-1213.988,-2016.327;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;1;-1863.671,725.7476;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;29;-1837.401,980.799;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;-4514.566,-960.3998;Inherit;False;13;worldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;139;399.5299,-1302.153;Inherit;False;1566.161;448.5387;Comment;9;134;132;131;130;135;136;137;138;133;Refraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-1639.592,876.0249;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;133;493.5956,-969.6146;Inherit;False;85;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;132;488.4113,-1053.858;Inherit;False;Property;_RefractAmount;Refract Amount;23;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;130;449.5299,-1252.153;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;123;-3348.049,133.0121;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;104;-4420.532,-742.0166;Inherit;False;Property;_EdgeFoamTile;Edge Foam Tile;20;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-4461.148,-1180.927;Inherit;False;Property;_DepthDistance;Depth Distance;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;103;-4316.543,-890.213;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;10,10,10,10;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-4191.789,-859.5467;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;131;720.4042,-1170.502;Inherit;False;True;True;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;725.5884,-1053.858;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;36;-1478.396,879.8598;Inherit;False;WavePattern;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;148;415.7635,-768.9396;Inherit;False;900.1146;217.2898;Comment;4;141;142;143;144;Depth;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;48;-1118.954,-1387.094;Inherit;False;1324.618;734.172;;8;49;45;116;46;42;115;47;44;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;113;-3128.308,144.5127;Inherit;False;SeaFoam;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;51;-4233.717,-1066.463;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;44;-861.871,-1306.762;Inherit;False;Property;_TopColor;Top Color;11;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;141;465.7635,-709.8102;Inherit;False;Property;_Depth;Depth;24;0;Create;True;0;0;0;False;0;False;-4;-4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-898.9961,-775.5789;Inherit;False;36;WavePattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;135;961.4691,-1189.943;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;106;-4120.026,-665.4986;Inherit;True;Property;_TextureSample1;Texture Sample 1;22;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;115;-834.8661,-1118.687;Inherit;False;113;SeaFoam;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;53;-3983.979,-951.2772;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;142;626.6452,-718.9396;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;136;1214.199,-1211.976;Inherit;False;Global;_GrabScreen0;Grab Screen 0;24;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;38;-617.4557,-274.9477;Inherit;False;820.2766;418.5838;Comment;5;40;39;23;24;37;Wave Height;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;46;-674.552,-888.4984;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;105;-3775.907,-824.0854;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;-568.5422,-1148.963;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;42;-916.5543,-980.4255;Inherit;False;Property;_WaterColor;Water Color;10;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;55;-3974.401,-1122.605;Inherit;False;Property;_EdgePower;Edge Power;13;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;143;908.6399,-710.6498;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;137;1498.033,-1191.239;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;-402.3127,41.60612;Inherit;False;36;WavePattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-3637.673,-987.3926;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;45;-386.0686,-1025.741;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-567.1657,-38.56633;Inherit;False;Property;_WaveHeight;Wave Height;9;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;24;-567.4557,-224.9477;Inherit;False;Property;_WaveUp;Wave Up;8;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;59;-3517.74,-853.749;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-200.1217,-144.546;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;144;1091.878,-699.4083;Inherit;False;depth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;67.81741,279.7256;Inherit;False;Property;_Tesselation;Tesselation;7;0;Create;True;0;0;0;False;0;False;8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;-114.8504,-1042.565;Inherit;False;albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;138;1741.691,-1171.798;Inherit;False;Refraction;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-23.40774,-141.9248;Inherit;False;WaveHeight;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;9;-2733.494,59.86936;Inherit;False;511.4;216.2;Changing the tailing will streatch the noise to feel more like waves, should make this dinamic;2;4;10;Wave Feeling;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;151;74.15958,449.0363;Inherit;False;Property;_MaxTessallation;Max Tessallation;26;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;289.9695,236.0254;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;50;312.7754,-235.8676;Inherit;False;49;albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;140;303.569,-332.1951;Inherit;False;138;Refraction;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-3258.401,-670.2256;Inherit;True;Edge;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;146;391.9429,-138.9643;Inherit;False;144;depth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;150;49.48546,366.7897;Inherit;False;Property;_MinTes;Min Tes;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;623.8176,-42.49368;Inherit;False;85;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;126;757.6429,143.07;Inherit;False;Property;_Smoothness;Smoothness;22;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;10;-2700.858,125.0238;Inherit;False;Property;_NoiseTilling;Noise Tilling;4;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;58;624.773,34.32998;Inherit;False;56;Edge;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DistanceBasedTessNode;149;638.1999,309.7756;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;145;631.2613,-269.5458;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-2496.294,131.9693;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;87;514.079,208.1953;Inherit;False;40;WaveHeight;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;995.4926,-33.00482;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;OceanTutorial;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;False;0;False;Opaque;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;12;0;11;1
WireConnection;12;1;11;3
WireConnection;13;0;12;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;89;0;66;0
WireConnection;19;0;17;0
WireConnection;19;1;18;0
WireConnection;68;0;89;0
WireConnection;68;1;69;0
WireConnection;25;0;19;0
WireConnection;79;0;77;0
WireConnection;79;1;80;0
WireConnection;81;0;78;0
WireConnection;81;1;82;0
WireConnection;70;0;68;0
WireConnection;70;1;75;0
WireConnection;73;0;70;0
WireConnection;73;2;81;0
WireConnection;72;0;68;0
WireConnection;72;2;79;0
WireConnection;119;0;109;0
WireConnection;119;1;118;0
WireConnection;111;0;109;0
WireConnection;43;0;61;0
WireConnection;43;1;72;0
WireConnection;43;5;84;0
WireConnection;64;0;61;0
WireConnection;64;1;73;0
WireConnection;64;5;84;0
WireConnection;112;0;110;0
WireConnection;112;1;111;0
WireConnection;7;0;5;0
WireConnection;7;1;6;0
WireConnection;32;0;31;0
WireConnection;120;0;119;0
WireConnection;83;0;43;0
WireConnection;83;1;64;0
WireConnection;121;0;120;0
WireConnection;28;0;32;0
WireConnection;28;2;3;0
WireConnection;28;1;7;0
WireConnection;107;0;100;0
WireConnection;107;1;112;0
WireConnection;2;0;26;0
WireConnection;2;2;3;0
WireConnection;2;1;7;0
WireConnection;122;0;107;1
WireConnection;122;1;121;0
WireConnection;85;0;83;0
WireConnection;1;0;2;0
WireConnection;1;1;8;0
WireConnection;29;0;28;0
WireConnection;29;1;30;0
WireConnection;33;0;1;0
WireConnection;33;1;29;0
WireConnection;123;0;122;0
WireConnection;103;0;102;0
WireConnection;101;0;103;0
WireConnection;101;1;104;0
WireConnection;131;0;130;0
WireConnection;134;0;132;0
WireConnection;134;1;133;0
WireConnection;36;0;33;0
WireConnection;113;0;123;0
WireConnection;51;0;52;0
WireConnection;135;0;131;0
WireConnection;135;1;134;0
WireConnection;106;0;100;0
WireConnection;106;1;101;0
WireConnection;53;0;51;0
WireConnection;142;0;141;0
WireConnection;136;0;135;0
WireConnection;46;0;47;0
WireConnection;105;0;53;0
WireConnection;105;1;106;0
WireConnection;116;0;44;0
WireConnection;116;1;115;0
WireConnection;143;0;142;0
WireConnection;137;0;136;0
WireConnection;54;0;55;0
WireConnection;54;1;105;0
WireConnection;45;0;42;0
WireConnection;45;1;116;0
WireConnection;45;2;46;0
WireConnection;59;0;54;0
WireConnection;23;0;24;0
WireConnection;23;1;37;0
WireConnection;23;2;39;0
WireConnection;144;0;143;0
WireConnection;49;0;45;0
WireConnection;138;0;137;0
WireConnection;40;0;23;0
WireConnection;152;0;37;0
WireConnection;152;1;20;0
WireConnection;56;0;59;0
WireConnection;149;0;152;0
WireConnection;149;1;150;0
WireConnection;149;2;151;0
WireConnection;145;0;140;0
WireConnection;145;1;50;0
WireConnection;145;2;146;0
WireConnection;4;0;10;0
WireConnection;0;0;145;0
WireConnection;0;1;88;0
WireConnection;0;2;58;0
WireConnection;0;4;126;0
WireConnection;0;11;87;0
WireConnection;0;14;149;0
ASEEND*/
//CHKSM=B330AEA919BD7133BC18ACDE486E994B30935F18