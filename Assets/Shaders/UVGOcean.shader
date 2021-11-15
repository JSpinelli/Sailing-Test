// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "UVGOcean"
{
	Properties
	{
		_ASEOutlineWidth( "Outline Width", Float ) = 0
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,0)
		_WaveDirection("Wave Direction", Vector) = (1,1,0,0)
		_ToonRamp("Toon Ramp", 2D) = "white" {}
		_WaveSpeed("Wave Speed", Float) = 0
		_RampScale("Ramp Scale", Float) = 0
		_WaveScale("Wave Scale", Float) = 0
		_WaveStretch("Wave Stretch", Vector) = (0,0,0,0)
		_WaveTile("WaveTile", Float) = 1
		_WindPatternScale("Wind Pattern Scale", Float) = 0
		_RimOffset("Rim Offset", Float) = 0
		_WaterColor("Water Color", Color) = (0,0,0,0)
		_RimPower("Rim Power", Range( 0 , 1)) = 0
		_TopColor("Top Color", Color) = (0,0,0,0)
		_Tesselation("Tesselation", Float) = 8
		_NormalMap("Normal Map", 2D) = "bump" {}
		_RimTint("Rim Tint", Color) = (0,0,0,0)
		_Gloss("Gloss", Range( 0 , 1)) = 0
		_NormalTileBase("Normal Tile Base", Float) = 1
		_SecondTileFactor("Second Tile Factor", Float) = 5
		_WaveUp("Wave Up", Vector) = (0,0,0,0)
		_NormalMainSpeed("Normal Main Speed", Float) = 0
		_SpecIntensity("Spec Intensity", Range( 0 , 1)) = 0
		_NormalStrength("Normal Strength", Float) = 1
		_SpecMap("Spec Map", 2D) = "white" {}
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
		_SpecColorLightContribution("Spec Color Light Contribution", Range( 0 , 1)) = 0
		_WaveHeights("Wave Heights", Float) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc tessellate:tessFunction 
		
		float4 _ASEOutlineColor;
		float _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v )
		{
			v.vertex.xyz += ( v.normal * _ASEOutlineWidth );
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float3 _WaveUp;
		uniform float _WaveHeights;
		uniform float _WaveSpeed;
		uniform float2 _WaveDirection;
		uniform float2 _WaveStretch;
		uniform float _WaveTile;
		uniform float _WaveScale;
		uniform float _WindPatternScale;
		uniform float4 _WaterColor;
		uniform float4 _TopColor;
		uniform sampler2D _ToonRamp;
		uniform sampler2D _NormalMap;
		uniform float _NormalMainSpeed;
		uniform float _NormalTileBase;
		uniform float _NormalStrength;
		uniform float _SecondTileFactor;
		uniform float _RampScale;
		uniform float _RimOffset;
		uniform float _RimPower;
		uniform float4 _RimTint;
		uniform float _Gloss;
		uniform sampler2D _SpecMap;
		uniform float4 _SpecMap_ST;
		uniform float4 _SpecularColor;
		uniform float _SpecColorLightContribution;
		uniform float _SpecIntensity;
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


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			float temp_output_112_0 = ( _WaveHeights * _Tesselation );
			float tesselation114 = temp_output_112_0;
			float4 temp_cast_3 = (tesselation114).xxxx;
			return temp_cast_3;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float temp_output_92_0 = ( _Time.y * _WaveSpeed );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float4 appendResult126 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 worldSpaceTile127 = appendResult126;
			float4 WaveTileUv123 = ( ( worldSpaceTile127 * float4( _WaveStretch, 0.0 , 0.0 ) ) * _WaveTile );
			float2 panner95 = ( temp_output_92_0 * _WaveDirection + WaveTileUv123.xy);
			float simplePerlin2D99 = snoise( panner95*_WaveScale );
			simplePerlin2D99 = simplePerlin2D99*0.5 + 0.5;
			float2 panner97 = ( temp_output_92_0 * _WaveDirection + ( WaveTileUv123 * float4( 0.1,0.1,0,0 ) ).xy);
			float simplePerlin2D100 = snoise( panner97*_WindPatternScale );
			simplePerlin2D100 = simplePerlin2D100*0.5 + 0.5;
			float WavePattern102 = ( simplePerlin2D99 + simplePerlin2D100 );
			float3 WaveHeight108 = ( _WaveUp * _WaveHeights * WavePattern102 );
			v.vertex.xyz += WaveHeight108;
			v.vertex.w = 1;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float temp_output_92_0 = ( _Time.y * _WaveSpeed );
			float3 ase_worldPos = i.worldPos;
			float4 appendResult126 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 worldSpaceTile127 = appendResult126;
			float4 WaveTileUv123 = ( ( worldSpaceTile127 * float4( _WaveStretch, 0.0 , 0.0 ) ) * _WaveTile );
			float2 panner95 = ( temp_output_92_0 * _WaveDirection + WaveTileUv123.xy);
			float simplePerlin2D99 = snoise( panner95*_WaveScale );
			simplePerlin2D99 = simplePerlin2D99*0.5 + 0.5;
			float2 panner97 = ( temp_output_92_0 * _WaveDirection + ( WaveTileUv123 * float4( 0.1,0.1,0,0 ) ).xy);
			float simplePerlin2D100 = snoise( panner97*_WindPatternScale );
			simplePerlin2D100 = simplePerlin2D100*0.5 + 0.5;
			float WavePattern102 = ( simplePerlin2D99 + simplePerlin2D100 );
			float clampResult155 = clamp( WavePattern102 , 0.0 , 1.0 );
			float4 lerpResult156 = lerp( _WaterColor , _TopColor , clampResult155);
			float4 albedo157 = lerpResult156;
			float4 temp_output_136_0 = ( ( worldSpaceTile127 / float4( 10,10,10,10 ) ) * _NormalTileBase );
			float2 panner144 = ( 1.0 * _Time.y * ( float2( 1,0 ) * _NormalMainSpeed ) + temp_output_136_0.xy);
			float2 panner142 = ( 1.0 * _Time.y * ( float2( -1,0 ) * 0.0 ) + ( temp_output_136_0 * _SecondTileFactor ).xy);
			float3 Normals148 = BlendNormals( UnpackScaleNormal( tex2D( _NormalMap, panner144 ), _NormalStrength ) , UnpackScaleNormal( tex2D( _NormalMap, panner142 ), _NormalStrength ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult16 = dot( (WorldNormalVector( i , Normals148 )) , ase_worldlightDir );
			float normalLightDir17 = dotResult16;
			float2 temp_cast_9 = ((normalLightDir17*_RampScale + _RampScale)).xx;
			float4 shadow62 = ( albedo157 * tex2D( _ToonRamp, temp_cast_9 ) );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			UnityGI gi56 = gi;
			float3 diffNorm56 = WorldNormalVector( i , Normals148 );
			gi56 = UnityGI_Base( data, 1, diffNorm56 );
			float3 indirectDiffuse56 = gi56.indirect.diffuse + diffNorm56 * 0.0001;
			float4 lighting78 = ( shadow62 * ( ase_lightColor * float4( ( ase_lightAtten + indirectDiffuse56 ) , 0.0 ) ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float dotResult11 = dot( (WorldNormalVector( i , Normals148 )) , ase_worldViewDir );
			float normalViewdir14 = dotResult11;
			float4 rim79 = ( saturate( ( pow( ( 1.0 - saturate( ( _RimOffset + normalViewdir14 ) ) ) , _RimPower ) * ( normalLightDir17 * ase_lightAtten ) ) ) * ( ase_lightColor * _RimTint ) );
			float dotResult39 = dot( ( ase_worldViewDir + _WorldSpaceLightPos0.xyz ) , (WorldNormalVector( i , Normals148 )) );
			float smoothstepResult60 = smoothstep( 1.1 , 1.4 , pow( dotResult39 , _Gloss ));
			float2 uv_SpecMap = i.uv_texcoord * _SpecMap_ST.xy + _SpecMap_ST.zw;
			float4 lerpResult58 = lerp( _SpecularColor , ase_lightColor , _SpecColorLightContribution);
			float4 Spec80 = ( ase_lightAtten * ( ( smoothstepResult60 * ( tex2D( _SpecMap, uv_SpecMap ) * lerpResult58 ) ) * _SpecIntensity ) );
			c.rgb = ( ( lighting78 + rim79 ) + Spec80 ).rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
			float temp_output_92_0 = ( _Time.y * _WaveSpeed );
			float3 ase_worldPos = i.worldPos;
			float4 appendResult126 = (float4(ase_worldPos.x , ase_worldPos.z , 0.0 , 0.0));
			float4 worldSpaceTile127 = appendResult126;
			float4 WaveTileUv123 = ( ( worldSpaceTile127 * float4( _WaveStretch, 0.0 , 0.0 ) ) * _WaveTile );
			float2 panner95 = ( temp_output_92_0 * _WaveDirection + WaveTileUv123.xy);
			float simplePerlin2D99 = snoise( panner95*_WaveScale );
			simplePerlin2D99 = simplePerlin2D99*0.5 + 0.5;
			float2 panner97 = ( temp_output_92_0 * _WaveDirection + ( WaveTileUv123 * float4( 0.1,0.1,0,0 ) ).xy);
			float simplePerlin2D100 = snoise( panner97*_WindPatternScale );
			simplePerlin2D100 = simplePerlin2D100*0.5 + 0.5;
			float WavePattern102 = ( simplePerlin2D99 + simplePerlin2D100 );
			float clampResult155 = clamp( WavePattern102 , 0.0 , 1.0 );
			float4 lerpResult156 = lerp( _WaterColor , _TopColor , clampResult155);
			float4 albedo157 = lerpResult156;
			float4 temp_output_187_0 = albedo157;
			o.Albedo = temp_output_187_0.rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
250;73;1323;654;2990.844;450.2695;1.729694;True;False
Node;AmplifyShaderEditor.CommentaryNode;124;-2774.982,2171.665;Inherit;False;756.8882;308.4;Comment;3;127;126;125;World Space UVs;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;125;-2724.982,2239.141;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;126;-2477.642,2224.522;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;127;-2248.494,2221.665;Inherit;True;worldSpaceTile;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;128;-4549.601,498.8458;Inherit;False;2219.788;972.1626;Comment;20;148;147;146;145;144;143;142;141;140;139;138;137;136;135;134;133;132;131;130;129;Normals;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;129;-4503.501,627.9066;Inherit;True;127;worldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;117;-2785.428,1682.966;Inherit;False;857.101;344.4004;Comment;6;123;122;121;120;119;118;Wave Tilling;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;131;-4344.44,860.5457;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;10,10,10,10;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;130;-4415.607,1043.187;Inherit;False;Property;_NormalTileBase;Normal Tile Base;18;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;119;-2710.362,1810.28;Inherit;False;Property;_WaveStretch;Wave Stretch;5;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;118;-2735.428,1732.966;Inherit;False;127;worldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;-2491.344,1759.436;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;137;-4167.201,548.8458;Inherit;False;Constant;_MainNormalPan;Main Normal Pan;18;0;Create;True;0;0;0;False;0;False;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;133;-4023.56,1355.008;Inherit;False;Constant;_SecondaryNormalSpeed;Secondary Normal Speed;19;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;120;-2526.543,1911.967;Inherit;False;Property;_WaveTile;WaveTile;6;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;-4264.788,1127.407;Inherit;False;Property;_SecondTileFactor;Second Tile Factor;19;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-4132.922,869.8167;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;135;-4174.546,705.6197;Inherit;False;Property;_NormalMainSpeed;Normal Main Speed;22;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;132;-4012.424,1230.87;Inherit;False;Constant;_SecondaryNormalPan;Secondary Normal Pan;18;0;Create;True;0;0;0;False;0;False;-1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-2319.259,1819.406;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;-3954.663,591.2797;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;-3736.94,1200.363;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-3816.505,1033.816;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;143;-3417.4,935.6276;Inherit;False;Property;_NormalStrength;Normal Strength;27;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;142;-3559.103,1093.398;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;87;-1681.179,1681.421;Inherit;False;1344.789;956.2808;Comment;15;102;101;100;99;98;97;96;95;94;93;92;91;90;89;88;Wave Pattern;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;123;-2153.126,1824.628;Inherit;False;WaveTileUv;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;144;-3556.667,700.7086;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;141;-3690.105,866.4637;Inherit;True;Property;_NormalMap;Normal Map;14;0;Create;True;0;0;0;False;0;False;6a416c2541c3e604da58ccc563658381;6a416c2541c3e604da58ccc563658381;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleTimeNode;88;-1619.867,2024.726;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;145;-3197.473,981.1617;Inherit;True;Property;_TextureSample0;Texture Sample 0;16;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;90;-1631.179,2120.763;Inherit;False;Property;_WaveSpeed;Wave Speed;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;146;-3204.81,745.1246;Inherit;True;Property;_Normals;Normals;12;0;Create;True;0;0;0;False;0;False;-1;6a416c2541c3e604da58ccc563658381;6a416c2541c3e604da58ccc563658381;True;0;True;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;89;-1546.698,2265.523;Inherit;False;123;WaveTileUv;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;94;-1424.128,1737.421;Inherit;False;123;WaveTileUv;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-1339.906,2306.525;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.1,0.1,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-1403.532,2021.898;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;93;-1548.438,1840.927;Inherit;False;Property;_WaveDirection;Wave Direction;0;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.BlendNormalsNode;147;-2824.597,915.4286;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;97;-1136.985,2115.91;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;3;-1148.214,-272.4738;Inherit;False;1036.72;400.4;;5;14;11;8;7;6;Normal View Dir;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;148;-2553.812,921.4396;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-1087.96,2303.301;Inherit;False;Property;_WindPatternScale;Wind Pattern Scale;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1150.367,2018.877;Inherit;False;Property;_WaveScale;Wave Scale;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;95;-1188.418,1815.105;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;100;-927.2654,2102.947;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;-1129.407,-218.7257;Inherit;False;148;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;5;-1151.141,189.6702;Inherit;False;988.4906;405.8927;;5;17;16;12;10;9;Normal Light Dir;1,1,1,1;0;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;99;-953.5354,1847.896;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;9;-1131.966,238.2594;Inherit;False;148;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;7;-942.6145,-222.4738;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;8;-944.2145,-41.67383;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;101;-729.4565,1998.173;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;10;-944.3716,416.4701;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;102;-568.2604,2002.008;Inherit;False;WavePattern;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;12;-900.1725,239.6702;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;149;-2564.483,-317.3459;Inherit;False;1324.618;734.172;;8;157;156;155;154;153;152;151;150;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;11;-520.2142,-99.27373;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;150;-2344.525,294.1691;Inherit;False;102;WavePattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;16;-611.5725,343.6702;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;13;-1713.841,-1375.577;Inherit;False;1964.983;510.6031;Comment;17;79;76;72;70;66;63;61;57;49;48;45;41;38;35;29;23;21;Rim Light;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-336.2933,-104.074;Inherit;False;normalViewdir;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;155;-2120.081,181.2495;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;21;-1663.841,-1215.176;Inherit;False;14;normalViewdir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-389.0503,367.8851;Inherit;False;normalLightDir;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;20;-2107.763,645.296;Inherit;False;2394.51;866.6852;Comment;23;80;77;74;73;68;67;60;59;58;55;54;53;52;44;43;42;40;39;33;32;27;25;24;Specular;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1607.839,-1325.577;Inherit;False;Property;_RimOffset;Rim Offset;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;19;-2078.717,-719.017;Inherit;False;1122.778;376.1779;;7;62;51;47;37;36;28;26;Shadow;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;151;-2307.4,-237.014;Inherit;False;Property;_TopColor;Top Color;11;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;154;-2362.083,89.32248;Inherit;False;Property;_WaterColor;Water Color;9;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;25;-2031.917,964.1733;Inherit;True;148;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;156;-1831.597,44.00701;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;24;-2057.763,852.7897;Inherit;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;26;-2015.715,-497.306;Inherit;False;Property;_RampScale;Ramp Scale;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;27;-1977.387,695.2958;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;28;-2035.725,-614.9329;Inherit;False;17;normalLightDir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-1409.44,-1311.177;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;35;-1219.04,-1315.977;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;36;-1816.674,-563.2615;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-1790.209,747.7333;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;31;-879.2695,-767.4943;Inherit;False;1151.278;421.8586;;9;78;75;71;69;65;64;56;50;46;Lighting;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;157;-1560.379,27.18304;Inherit;False;albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;33;-1799.501,955.6376;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LightAttenuation;41;-1015.839,-1072.776;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-1512.846,-664.1473;Inherit;False;157;albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1574.176,961.4451;Inherit;False;Property;_Gloss;Gloss;17;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1457.853,1395.981;Inherit;False;Property;_SpecColorLightContribution;Spec Color Light Contribution;31;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;-1437.811,1098.231;Inherit;False;Property;_SpecularColor;Specular Color;30;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;39;-1607.859,829.0369;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1179.039,-1232.776;Inherit;False;Property;_RimPower;Rim Power;10;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;38;-1057.44,-1314.376;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-1075.04,-1147.977;Inherit;False;17;normalLightDir;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;-829.2695,-461.6348;Inherit;False;148;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightColorNode;43;-1240.527,1230.795;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;37;-1618.119,-584.9257;Inherit;True;Property;_ToonRamp;Toon Ramp;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-779.0396,-1157.577;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-1202.504,1039.264;Inherit;False;Constant;_max;max;9;0;Create;True;0;0;0;False;0;False;1.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;56;-650.2137,-462.9106;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;49;-878.2395,-1315.977;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;58;-854.0745,1239.948;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1306.505,-648.6011;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;54;-1293.099,848.7817;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;50;-700.269,-564.6348;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1200.181,957.9603;Inherit;False;Constant;_min;min;9;0;Create;True;0;0;0;False;0;False;1.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;52;-1033.993,1016.791;Inherit;True;Property;_SpecMap;Spec Map;29;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;60;-996.9225,829.0367;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-433.269,-497.6348;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightColorNode;64;-478.8398,-621.6666;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-555.0393,-1309.576;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;63;-559.8399,-1074.376;Inherit;False;Property;_RimTint;Rim Tint;16;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-1156.782,-652.5977;Inherit;False;shadow;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-662.3027,1080.978;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;66;-547.0397,-1197.576;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-302.2398,-1154.376;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;69;-465.3996,-717.494;Inherit;False;62;shadow;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-281.269,-557.6348;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;72;-374.2398,-1309.576;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-526.2045,991.8875;Inherit;False;Property;_SpecIntensity;Spec Intensity;26;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-614.8875,832.3062;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;103;-193.3793,1719.833;Inherit;False;820.2766;418.5838;Comment;5;108;107;106;105;205;Wave Height;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-155.0398,-1299.977;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-125.5691,-640.295;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightAttenuation;74;-603.1828,717.535;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-304.6788,856.2756;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;113;658.1788,1755.472;Inherit;False;Property;_Tesselation;Tesselation;13;0;Create;True;0;0;0;False;0;False;8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;78;46.40918,-657.6392;Inherit;False;lighting;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;106;-143.3793,1769.833;Inherit;False;Property;_WaveUp;Wave Up;20;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;22.65228,-1303.686;Inherit;False;rim;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;105;21.76349,2036.387;Inherit;False;102;WavePattern;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;205;-146.5019,1944.892;Inherit;False;Property;_WaveHeights;Wave Heights;33;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-129.1229,831.1373;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;223.9542,1850.235;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;80;62.74619,816.452;Inherit;False;Spec;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;63.83648,190.3471;Inherit;False;79;rim;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;82;63.82328,99.32824;Inherit;False;78;lighting;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;878.0555,1843.713;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;190;422.0461,-984.0186;Inherit;False;1566.161;448.5387;Comment;9;204;201;199;198;196;194;193;192;191;Refraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;108;400.668,1852.856;Inherit;False;WaveHeight;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;251.9042,236.925;Inherit;False;80;Spec;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;288.9896,138.3486;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;114;1343.319,1968.419;Inherit;False;tesselation;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;158;-4734.383,-500.1584;Inherit;False;2007.937;756.582;Comment;12;176;175;171;169;168;166;165;164;163;162;160;159;Sea Foam;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;195;438.2797,-450.8051;Inherit;False;900.1146;217.2898;Comment;4;203;202;200;197;Depth;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;161;-4383.146,-1481.951;Inherit;False;1475.615;809.4247;;14;185;184;183;182;181;180;179;178;177;174;173;172;170;167;Edge;1,1,1,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;175;-3206.663,-139.7208;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;-4324.543,-74.21606;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-4412.831,-439.4881;Inherit;False;Property;_SeaFoamTile;Sea Foam Tile;28;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;162;-4225.354,-335.6914;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;10,10,10,10;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;166;-4033.594,-400.9553;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;-3483.286,-300.9149;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;165;-4102.881,-141.7696;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0.3,-0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;169;-3831.915,-186.8676;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;168;-3820.01,-416.0022;Inherit;True;Property;_TextureSample1;Texture Sample 1;22;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;160;-4528.636,-340.8365;Inherit;True;127;worldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TexturePropertyNode;167;-4320.873,-883.5455;Inherit;True;Property;_SeaFoa_M;SeaFoa_M;21;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.GetLocalVarNode;187;103.4855,-68.50037;Inherit;False;157;albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;748.1044,-735.7237;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;193;516.1117,-651.4803;Inherit;False;148;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;191;510.9274,-735.7237;Inherit;False;Property;_RefractAmount;Refract Amount;23;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;192;472.0461,-934.0186;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;194;742.9203,-852.3676;Inherit;False;True;True;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;152;-2280.395,-48.93902;Inherit;False;176;SeaFoam;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;201;1520.549,-873.1047;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;197;488.2796,-391.6758;Inherit;False;Property;_Depth;Depth;25;0;Create;True;0;0;0;False;0;False;-4;-4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;198;983.985,-871.8087;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenColorNode;199;1236.715,-893.8416;Inherit;False;Global;_GrabScreen0;Grab Screen 0;24;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;200;649.1613,-400.8051;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;159;-4557.072,-17.93646;Inherit;False;Constant;_FoamMask;Foam Mask;23;0;Create;True;0;0;0;False;0;False;0.03;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;153;-2014.071,-79.21503;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;203;1114.394,-381.2739;Inherit;False;depth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;176;-2986.922,-128.2202;Inherit;False;SeaFoam;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;204;1764.207,-853.6636;Inherit;False;Refraction;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;115;436.0867,278.1487;Inherit;False;108;WaveHeight;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;-4050.403,-1132.28;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;-3496.287,-1260.126;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;180;-3842.594,-1224.01;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;710.0175,2088.572;Inherit;False;Property;_MaxTessallation;Max Tessallation;34;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;179;-3978.641,-938.2315;Inherit;True;Property;_TextureSample2;Texture Sample 2;22;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;188;452.039,-102.1786;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;177;-4092.331,-1339.196;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;111;742.2145,1965.377;Inherit;False;Property;_MinTessa;Min Tessa;32;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;182;-3833.015,-1395.338;Inherit;False;Property;_EdgePower;Edge Power;15;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;189;187.2787,15.68201;Inherit;False;203;depth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;173;-4279.146,-1014.75;Inherit;False;Property;_EdgeFoamTile;Edge Foam Tile;24;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;172;-4319.763,-1453.66;Inherit;False;Property;_DepthDistance;Depth Distance;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;186;126.6438,-145.1682;Inherit;False;204;Refraction;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;170;-4373.181,-1233.133;Inherit;False;127;worldSpaceTile;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;116;420.1957,362.2598;Inherit;False;114;tesselation;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;174;-4175.157,-1162.946;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;10,10,10,10;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;86;446.9042,173.925;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;181;-3634.521,-1096.818;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DistanceBasedTessNode;109;1042.321,2021.351;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;185;-3117.015,-942.9586;Inherit;True;Edge;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;184;-3376.354,-1126.482;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;202;931.1558,-392.5154;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;730.0603,0.2279963;Float;False;True;-1;6;ASEMaterialInspector;0;0;CustomLighting;UVGOcean;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;126;0;125;1
WireConnection;126;1;125;3
WireConnection;127;0;126;0
WireConnection;131;0;129;0
WireConnection;121;0;118;0
WireConnection;121;1;119;0
WireConnection;136;0;131;0
WireConnection;136;1;130;0
WireConnection;122;0;121;0
WireConnection;122;1;120;0
WireConnection;139;0;137;0
WireConnection;139;1;135;0
WireConnection;140;0;132;0
WireConnection;140;1;133;0
WireConnection;138;0;136;0
WireConnection;138;1;134;0
WireConnection;142;0;138;0
WireConnection;142;2;140;0
WireConnection;123;0;122;0
WireConnection;144;0;136;0
WireConnection;144;2;139;0
WireConnection;145;0;141;0
WireConnection;145;1;142;0
WireConnection;145;5;143;0
WireConnection;146;0;141;0
WireConnection;146;1;144;0
WireConnection;146;5;143;0
WireConnection;91;0;89;0
WireConnection;92;0;88;0
WireConnection;92;1;90;0
WireConnection;147;0;146;0
WireConnection;147;1;145;0
WireConnection;97;0;91;0
WireConnection;97;2;93;0
WireConnection;97;1;92;0
WireConnection;148;0;147;0
WireConnection;95;0;94;0
WireConnection;95;2;93;0
WireConnection;95;1;92;0
WireConnection;100;0;97;0
WireConnection;100;1;98;0
WireConnection;99;0;95;0
WireConnection;99;1;96;0
WireConnection;7;0;6;0
WireConnection;101;0;99;0
WireConnection;101;1;100;0
WireConnection;102;0;101;0
WireConnection;12;0;9;0
WireConnection;11;0;7;0
WireConnection;11;1;8;0
WireConnection;16;0;12;0
WireConnection;16;1;10;0
WireConnection;14;0;11;0
WireConnection;155;0;150;0
WireConnection;17;0;16;0
WireConnection;156;0;154;0
WireConnection;156;1;151;0
WireConnection;156;2;155;0
WireConnection;29;0;23;0
WireConnection;29;1;21;0
WireConnection;35;0;29;0
WireConnection;36;0;28;0
WireConnection;36;1;26;0
WireConnection;36;2;26;0
WireConnection;32;0;27;0
WireConnection;32;1;24;1
WireConnection;157;0;156;0
WireConnection;33;0;25;0
WireConnection;39;0;32;0
WireConnection;39;1;33;0
WireConnection;38;0;35;0
WireConnection;37;1;36;0
WireConnection;57;0;48;0
WireConnection;57;1;41;0
WireConnection;56;0;46;0
WireConnection;49;0;38;0
WireConnection;49;1;45;0
WireConnection;58;0;42;0
WireConnection;58;1;43;0
WireConnection;58;2;44;0
WireConnection;51;0;47;0
WireConnection;51;1;37;0
WireConnection;54;0;39;0
WireConnection;54;1;40;0
WireConnection;60;0;54;0
WireConnection;60;1;53;0
WireConnection;60;2;55;0
WireConnection;65;0;50;0
WireConnection;65;1;56;0
WireConnection;61;0;49;0
WireConnection;61;1;57;0
WireConnection;62;0;51;0
WireConnection;59;0;52;0
WireConnection;59;1;58;0
WireConnection;70;0;66;0
WireConnection;70;1;63;0
WireConnection;71;0;64;0
WireConnection;71;1;65;0
WireConnection;72;0;61;0
WireConnection;68;0;60;0
WireConnection;68;1;59;0
WireConnection;76;0;72;0
WireConnection;76;1;70;0
WireConnection;75;0;69;0
WireConnection;75;1;71;0
WireConnection;73;0;68;0
WireConnection;73;1;67;0
WireConnection;78;0;75;0
WireConnection;79;0;76;0
WireConnection;77;0;74;0
WireConnection;77;1;73;0
WireConnection;107;0;106;0
WireConnection;107;1;205;0
WireConnection;107;2;105;0
WireConnection;80;0;77;0
WireConnection;112;0;205;0
WireConnection;112;1;113;0
WireConnection;108;0;107;0
WireConnection;84;0;82;0
WireConnection;84;1;81;0
WireConnection;114;0;112;0
WireConnection;175;0;171;0
WireConnection;163;0;160;0
WireConnection;163;1;159;0
WireConnection;162;0;160;0
WireConnection;166;0;164;0
WireConnection;166;1;162;0
WireConnection;171;0;168;1
WireConnection;171;1;169;0
WireConnection;165;0;163;0
WireConnection;169;0;165;0
WireConnection;168;0;167;0
WireConnection;168;1;166;0
WireConnection;196;0;191;0
WireConnection;196;1;193;0
WireConnection;194;0;192;0
WireConnection;201;0;199;0
WireConnection;198;0;194;0
WireConnection;198;1;196;0
WireConnection;199;0;198;0
WireConnection;200;0;197;0
WireConnection;153;1;152;0
WireConnection;203;0;202;0
WireConnection;176;0;175;0
WireConnection;204;0;201;0
WireConnection;178;0;174;0
WireConnection;178;1;173;0
WireConnection;183;0;182;0
WireConnection;183;1;181;0
WireConnection;180;0;177;0
WireConnection;179;0;167;0
WireConnection;179;1;178;0
WireConnection;188;0;186;0
WireConnection;188;1;187;0
WireConnection;188;2;189;0
WireConnection;177;0;172;0
WireConnection;174;0;170;0
WireConnection;86;0;84;0
WireConnection;86;1;83;0
WireConnection;181;0;180;0
WireConnection;181;1;179;0
WireConnection;109;0;112;0
WireConnection;109;1;111;0
WireConnection;109;2;110;0
WireConnection;185;0;184;0
WireConnection;184;0;183;0
WireConnection;202;0;200;0
WireConnection;0;0;187;0
WireConnection;0;13;86;0
WireConnection;0;11;115;0
WireConnection;0;14;116;0
ASEEND*/
//CHKSM=636FBE2F9A1F4DEB7406122C87172ED68A98AFCB