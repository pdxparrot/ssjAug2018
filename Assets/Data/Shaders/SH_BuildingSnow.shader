// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/SH_BuildingSnow"
{
	Properties
	{
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		[NoScaleOffset]_Material1_Array("Material1_Array", 2DArray) = "white" {}
		_Material1_TintColor("Material1_TintColor", Color) = (1,1,1,1)
		_Material1_TintStrength("Material1_TintStrength", Range( 0 , 1)) = 0
		_Material1_NormalScale("Material1_NormalScale", Range( 0 , 1)) = 1
		_Material1_Tiling("Material1_Tiling", Vector) = (1,1,0,0)
		_Material1_Offset("Material1_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Material2_Array("Material2_Array", 2DArray) = "white" {}
		_Material2_TintColor("Material2_TintColor", Color) = (1,1,1,1)
		_Material2_TintStrength("Material2_TintStrength", Range( 0 , 1)) = 0
		_Material2_NormalScale("Material2_NormalScale", Range( 0 , 1)) = 1
		_Material2_Tiling("Material2_Tiling", Vector) = (1,1,0,0)
		_Material2_Offset("Material2_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Material3_Array("Material3_Array", 2DArray) = "white" {}
		_Material3_TintColor("Material3_TintColor", Color) = (1,1,1,1)
		_Material3_TintStrength("Material3_TintStrength", Range( 0 , 1)) = 0
		_Material3_NormalScale("Material3_NormalScale", Range( 0 , 1)) = 1
		_Material3_Tiling("Material3_Tiling", Vector) = (1,1,0,0)
		_Material3_Offset("Material3_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Snow_Array("Snow_Array", 2DArray) = "white" {}
		_Snow_Amount("Snow_Amount", Range( 0 , 2)) = 0
		_Snow_NormalScale("Snow_NormalScale", Range( 0 , 1)) = 0
		_Snow_Tiling("Snow_Tiling", Vector) = (1,1,0,0)
		_Snow_Offset("Snow_Offset", Vector) = (0,0,0,0)
		_Snow_Metallic("Snow_Metallic", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.5
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
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material1_Array );
		uniform float4 _Material1_Array_ST;
		uniform float _Material1_NormalScale;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material2_Array );
		uniform float4 _Material2_Array_ST;
		uniform float _Material2_NormalScale;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material3_Array );
		uniform float4 _Material3_Array_ST;
		uniform float _Material3_NormalScale;
		uniform UNITY_DECLARE_TEX2DARRAY( _Snow_Array );
		uniform float4 _Snow_Array_ST;
		uniform float _Snow_NormalScale;
		uniform float _Snow_Amount;
		uniform float2 _Material1_Tiling;
		uniform float2 _Material1_Offset;
		uniform float4 _Material1_TintColor;
		uniform float _Material1_TintStrength;
		uniform float2 _Material2_Tiling;
		uniform float2 _Material2_Offset;
		uniform float4 _Material2_TintColor;
		uniform float _Material2_TintStrength;
		uniform float2 _Material3_Tiling;
		uniform float2 _Material3_Offset;
		uniform float4 _Material3_TintColor;
		uniform float _Material3_TintStrength;
		uniform float2 _Snow_Tiling;
		uniform float2 _Snow_Offset;
		uniform float _Snow_Metallic;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 tex2DNode100 = tex2D( _Mask, uv_Mask );
			float2 uv_Material1_Array = i.uv_texcoord * _Material1_Array_ST.xy + _Material1_Array_ST.zw;
			float3 texArray82 = UnpackScaleNormal( UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_Material1_Array, (float)1)  ) ,_Material1_NormalScale );
			float4 tex2DNode101 = tex2D( _Mask, uv_Mask );
			float2 uv_Material2_Array = i.uv_texcoord * _Material2_Array_ST.xy + _Material2_Array_ST.zw;
			float3 texArray94 = UnpackScaleNormal( UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_Material2_Array, (float)1)  ) ,_Material2_NormalScale );
			float4 tex2DNode13 = tex2D( _Mask, uv_Mask );
			float2 uv_Material3_Array = i.uv_texcoord * _Material3_Array_ST.xy + _Material3_Array_ST.zw;
			float3 texArray108 = UnpackScaleNormal( UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_Material3_Array, (float)1)  ) ,_Material3_NormalScale );
			float2 uv_Snow_Array = i.uv_texcoord * _Snow_Array_ST.xy + _Snow_Array_ST.zw;
			float3 texArray120 = UnpackScaleNormal( UNITY_SAMPLE_TEX2DARRAY(_Snow_Array, float3(uv_Snow_Array, (float)1)  ) ,_Snow_NormalScale );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float temp_output_128_0 = saturate( ( ase_worldNormal.y * _Snow_Amount ) );
			float3 lerpResult130 = lerp( ( ( ( tex2DNode100.r * texArray82 ) + ( tex2DNode101.g * texArray94 ) ) + ( tex2DNode13.b * texArray108 ) ) , texArray120 , temp_output_128_0);
			o.Normal = lerpResult130;
			float2 uv_TexCoord10 = i.uv_texcoord * _Material1_Tiling + _Material1_Offset;
			float4 texArray81 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_TexCoord10, (float)0)  );
			float4 blendOpSrc30 = _Material1_TintColor;
			float4 blendOpDest30 = texArray81;
			float4 lerpResult31 = lerp( texArray81 , ( saturate( (( blendOpDest30 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest30 - 0.5 ) ) * ( 1.0 - blendOpSrc30 ) ) : ( 2.0 * blendOpDest30 * blendOpSrc30 ) ) )) , _Material1_TintStrength);
			float2 uv_TexCoord36 = i.uv_texcoord * _Material2_Tiling + _Material2_Offset;
			float4 texArray93 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_TexCoord36, (float)0)  );
			float4 blendOpSrc40 = _Material2_TintColor;
			float4 blendOpDest40 = texArray93;
			float4 lerpResult46 = lerp( texArray93 , ( saturate( (( blendOpDest40 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest40 - 0.5 ) ) * ( 1.0 - blendOpSrc40 ) ) : ( 2.0 * blendOpDest40 * blendOpSrc40 ) ) )) , _Material2_TintStrength);
			float2 uv_TexCoord58 = i.uv_texcoord * _Material3_Tiling + _Material3_Offset;
			float4 texArray104 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_TexCoord58, (float)0)  );
			float4 blendOpSrc60 = _Material3_TintColor;
			float4 blendOpDest60 = texArray104;
			float4 lerpResult62 = lerp( texArray104 , ( saturate( (( blendOpDest60 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest60 - 0.5 ) ) * ( 1.0 - blendOpSrc60 ) ) : ( 2.0 * blendOpDest60 * blendOpSrc60 ) ) )) , _Material3_TintStrength);
			float2 uv_TexCoord116 = i.uv_texcoord * _Snow_Tiling + _Snow_Offset;
			float4 texArray112 = UNITY_SAMPLE_TEX2DARRAY(_Snow_Array, float3(uv_TexCoord116, (float)0)  );
			float temp_output_133_0 = saturate( ( WorldNormalVector( i , lerpResult130 ).y * _Snow_Amount ) );
			float4 lerpResult135 = lerp( ( ( ( tex2DNode100.r * lerpResult31 ) + ( tex2DNode101.g * lerpResult46 ) ) + ( tex2DNode13.b * lerpResult62 ) ) , texArray112 , temp_output_133_0);
			o.Albedo = lerpResult135.rgb;
			float4 texArray83 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_Material1_Array, (float)2)  );
			float4 texArray95 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_Material2_Array, (float)2)  );
			float4 texArray109 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_Material3_Array, (float)2)  );
			float lerpResult136 = lerp( _Snow_Metallic , ( ( ( tex2DNode100.r * texArray83.r ) + ( tex2DNode101.a * texArray95.r ) ) + ( tex2DNode13.a * texArray109.x ) ) , temp_output_133_0);
			o.Metallic = lerpResult136;
			float4 texArray121 = UNITY_SAMPLE_TEX2DARRAY(_Snow_Array, float3(uv_Snow_Array, (float)2)  );
			float lerpResult137 = lerp( ( ( ( tex2DNode100.r * texArray83.g ) + ( tex2DNode101.g * texArray95.g ) ) + ( tex2DNode13.b * texArray109.y ) ) , texArray121.g , temp_output_133_0);
			o.Smoothness = lerpResult137;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
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
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
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
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
Version=15401
1782.4;96;970;725;1223.301;-323.1419;4.316816;True;False
Node;AmplifyShaderEditor.CommentaryNode;28;-2245.847,-280.2991;Float;False;1717.034;1009.174;Material1;20;15;26;24;25;88;83;87;81;82;86;31;32;30;20;10;12;11;89;90;100;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;33;-2231.759,771.0452;Float;False;1649.352;965.5948;Material2;20;99;94;93;95;98;96;97;52;50;49;51;92;36;34;35;46;40;42;38;101;;1,1,1,1;0;0
Node;AmplifyShaderEditor.IntNode;87;-1792,544;Float;False;Constant;_NormalIndex;NormalIndex;26;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;97;-1692.538,1541.778;Float;False;Constant;_Int1;Int 1;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-1879.816,351.7733;Float;False;Property;_Material1_NormalScale;Material1_NormalScale;4;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-1812.201,1345.965;Float;False;Property;_Material2_NormalScale;Material2_NormalScale;10;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;57;-2234.244,1804.855;Float;False;1669.569;985.8406;Material3;20;67;65;66;13;108;104;109;110;105;106;107;58;73;74;103;68;62;70;60;69;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;100;-1233.174,330.4507;Float;True;Property;_TextureSample0;Texture Sample 0;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;124;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;110;-1864.338,2404.116;Float;False;Property;_Material3_NormalScale;Material3_NormalScale;16;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;94;-1500.538,1312.452;Float;True;Property;_TextureArray1;Texture Array 1;23;0;Create;True;0;0;False;0;None;0;Instance;93;Auto;True;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;107;-1742.658,2593.315;Float;False;Constant;_Int5;Int 5;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.Vector2Node;34;-2152.795,1149.737;Float;False;Property;_Material2_Offset;Material2_Offset;12;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;101;-1193.707,1376.285;Float;True;Property;_TextureSample1;Texture Sample 1;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;124;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;82;-1536,256;Float;True;Property;_Material1_Normal;Material1_Normal;20;0;Create;True;0;0;False;0;None;0;Instance;81;Auto;True;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;11;-2160.535,-16.19443;Float;False;Property;_Material1_Tiling;Material1_Tiling;5;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;35;-2152.795,1023.241;Float;False;Property;_Material2_Tiling;Material2_Tiling;11;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;12;-2160.535,110.3019;Float;False;Property;_Material1_Offset;Material1_Offset;6;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1880.742,1042.303;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-775.2091,1286.104;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;125;531.9811,1731.366;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;126;455.8491,1907.897;Float;False;Property;_Snow_Amount;Snow_Amount;20;0;Create;True;0;0;False;0;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;89;-1896.294,-223.8564;Float;True;Property;_Material1_Array;Material1_Array;1;1;[NoScaleOffset];Create;True;0;0;False;0;c90d7a96c0a623441b73da001e669a4e;c90d7a96c0a623441b73da001e669a4e;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureArrayNode;108;-1533.715,2314.765;Float;True;Property;_TextureArray4;Texture Array 4;23;0;Create;True;0;0;False;0;None;0;Instance;104;Auto;True;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-768,240;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;13;-1239.401,2398.132;Float;True;Property;_MaskSample;MaskSample;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;124;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;96;-1704.945,1447.533;Float;False;Constant;_Int0;Int 0;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.Vector2Node;74;-2199.645,2214.047;Float;False;Property;_Material3_Offset;Material3_Offset;18;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1888.483,2.867096;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;73;-2199.645,2087.551;Float;False;Property;_Material3_Tiling;Material3_Tiling;17;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.IntNode;86;-1792,448;Float;False;Constant;_AlbedoIndex;AlbedoIndex;26;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TexturePropertyNode;92;-1875.237,843.366;Float;True;Property;_Material2_Array;Material2_Array;7;1;[NoScaleOffset];Create;True;0;0;False;0;9e68fd9253d6b5f49a538eb963199c94;9e68fd9253d6b5f49a538eb963199c94;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;123;523.6347,1131.654;Float;False;Property;_Snow_NormalScale;Snow_NormalScale;21;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-712.3896,2296.452;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.IntNode;115;651.475,1308.659;Float;False;Constant;_Int7;Int 7;24;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-194.5857,1502.899;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;771.5062,1832.541;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;81;-1536,32;Float;True;Property;_Material1_Sample;Material1_Sample;25;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;93;-1500.538,1104.452;Float;True;Property;_Material2_Sample;Material2_Sample;26;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;38;-1424,825.5783;Float;False;Property;_Material2_TintColor;Material2_TintColor;8;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;103;-1917.988,1883.632;Float;True;Property;_Material3_Array;Material3_Array;13;1;[NoScaleOffset];Create;True;0;0;False;0;126ad50cba129524e82f6ec7fe0fa046;126ad50cba129524e82f6ec7fe0fa046;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.IntNode;105;-1739.91,2507.985;Float;False;Constant;_Int3;Int 3;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.ColorNode;20;-1408.458,-225.7663;Float;False;Property;_Material1_TintColor;Material1_TintColor;2;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-1927.59,2106.613;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;128;958.5882,1836.842;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;223.273,2167.729;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureArrayNode;120;837.9845,1111.872;Float;True;Property;_TextureArray6;Texture Array 6;25;0;Create;True;0;0;False;0;None;0;Instance;112;Auto;True;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;30;-1111.242,-222.4394;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1434.662,-52.20638;Float;False;Property;_Material1_TintStrength;Material1_TintStrength;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1450.204,999.1385;Float;False;Property;_Material2_TintStrength;Material2_TintStrength;9;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;98;-1692.538,1632.452;Float;False;Constant;_Int2;Int 2;21;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;88;-1792,640;Float;False;Constant;_MRIndex;MRIndex;26;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.ColorNode;69;-1426.484,1859.388;Float;False;Property;_Material3_TintColor;Material3_TintColor;14;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;104;-1528.721,2121.392;Float;True;Property;_Material3_Sample;Material3_Sample;27;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;40;-1126.784,828.9053;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;31;-956.012,51.61835;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;130;1274.632,2165.922;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-1452.688,2032.948;Float;False;Property;_Material3_TintStrength;Material3_TintStrength;15;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;46;-971.5543,1102.964;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureArrayNode;95;-1500.538,1520.452;Float;True;Property;_TextureArray2;Texture Array 2;23;0;Create;True;0;0;False;0;None;0;Instance;93;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;60;-1129.268,1862.715;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureArrayNode;83;-1536,512;Float;True;Property;_Material1_MR;Material1_MR;21;0;Create;True;0;0;False;0;None;0;Instance;81;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;106;-1739.91,2678.164;Float;False;Constant;_Int4;Int 4;21;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-768,1536;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;131;1446.992,1737.532;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-768,432;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-768,544;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;119;336.2921,1022.554;Float;False;Property;_Snow_Offset;Snow_Offset;23;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-768,32;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;62;-974.0378,2136.772;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-774.46,1089.095;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-768,1424;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;109;-1530.736,2558.839;Float;True;Property;_TextureArray5;Texture Array 5;23;0;Create;True;0;0;False;0;None;0;Instance;104;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;118;334.8161,895.6187;Float;False;Property;_Snow_Tiling;Snow_Tiling;22;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-162.3222,1715.839;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-711.6404,2176.024;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;1670.499,1889.772;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;114;652.3227,1220.185;Float;False;Constant;_Int6;Int 6;24;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-714.6615,2513.899;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-713.8365,2625.083;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;122;648.8196,1396.468;Float;False;Constant;_Int8;Int 8;26;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-165.5486,1915.873;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;116;570.5856,926.071;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;111;539.8334,703.2272;Float;True;Property;_Snow_Array;Snow_Array;19;1;[NoScaleOffset];Create;True;0;0;False;0;ad280c83e3b496f4293f616530e6d565;None;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-210.7163,1289.96;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;138;1930.06,2584.194;Float;False;Property;_Snow_Metallic;Snow_Metallic;24;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;218.6739,2356.297;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;133;1820.067,1892.017;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;213.6794,2524.978;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;219.4639,1970.094;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureArrayNode;112;844.343,898.7892;Float;True;Property;_Snow_Sample;Snow_Sample;28;0;Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;121;834.0652,1300.985;Float;True;Property;_TextureArray7;Texture Array 7;25;0;Create;True;0;0;False;0;None;0;Instance;112;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;124;756.4361,1523.369;Float;True;Property;_Mask_Sample;Mask_Sample;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;1105.738,1778.517;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;135;2219.217,1951.066;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;14;446.8049,1523.699;Float;True;Property;_Mask;Mask;0;1;[NoScaleOffset];Create;True;0;0;False;0;00e81767994d62f4ba5a3e48f0e93787;e151bab8f799fe541b2218a36b0220d6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.LerpOp;137;2238.332,2392.157;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;136;2238.941,2246.906;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;1976.127,2016.39;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2526.687,2156.231;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;Yeti/SH_BuildingSnow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;94;1;97;0
WireConnection;94;3;99;0
WireConnection;82;1;87;0
WireConnection;82;3;90;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;49;0;101;2
WireConnection;49;1;94;0
WireConnection;108;1;107;0
WireConnection;108;3;110;0
WireConnection;24;0;100;1
WireConnection;24;1;82;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;65;0;13;3
WireConnection;65;1;108;0
WireConnection;54;0;24;0
WireConnection;54;1;49;0
WireConnection;127;0;125;2
WireConnection;127;1;126;0
WireConnection;81;6;89;0
WireConnection;81;0;10;0
WireConnection;81;1;86;0
WireConnection;93;6;92;0
WireConnection;93;0;36;0
WireConnection;93;1;96;0
WireConnection;58;0;73;0
WireConnection;58;1;74;0
WireConnection;128;0;127;0
WireConnection;78;0;54;0
WireConnection;78;1;65;0
WireConnection;120;1;115;0
WireConnection;120;3;123;0
WireConnection;30;0;20;0
WireConnection;30;1;81;0
WireConnection;104;6;103;0
WireConnection;104;0;58;0
WireConnection;104;1;105;0
WireConnection;40;0;38;0
WireConnection;40;1;93;0
WireConnection;31;0;81;0
WireConnection;31;1;30;0
WireConnection;31;2;32;0
WireConnection;130;0;78;0
WireConnection;130;1;120;0
WireConnection;130;2;128;0
WireConnection;46;0;93;0
WireConnection;46;1;40;0
WireConnection;46;2;42;0
WireConnection;95;1;98;0
WireConnection;60;0;69;0
WireConnection;60;1;104;0
WireConnection;83;1;88;0
WireConnection;52;0;101;4
WireConnection;52;1;95;1
WireConnection;131;0;130;0
WireConnection;25;0;100;1
WireConnection;25;1;83;2
WireConnection;26;0;100;1
WireConnection;26;1;83;1
WireConnection;15;0;100;1
WireConnection;15;1;31;0
WireConnection;62;0;104;0
WireConnection;62;1;60;0
WireConnection;62;2;70;0
WireConnection;51;0;101;2
WireConnection;51;1;46;0
WireConnection;50;0;101;2
WireConnection;50;1;95;2
WireConnection;109;1;106;0
WireConnection;55;0;25;0
WireConnection;55;1;50;0
WireConnection;67;0;13;3
WireConnection;67;1;62;0
WireConnection;132;0;131;2
WireConnection;132;1;126;0
WireConnection;66;0;13;3
WireConnection;66;1;109;2
WireConnection;68;0;13;4
WireConnection;68;1;109;1
WireConnection;56;0;26;0
WireConnection;56;1;52;0
WireConnection;116;0;118;0
WireConnection;116;1;119;0
WireConnection;53;0;15;0
WireConnection;53;1;51;0
WireConnection;79;0;55;0
WireConnection;79;1;66;0
WireConnection;133;0;132;0
WireConnection;80;0;56;0
WireConnection;80;1;68;0
WireConnection;77;0;53;0
WireConnection;77;1;67;0
WireConnection;112;6;111;0
WireConnection;112;0;116;0
WireConnection;112;1;114;0
WireConnection;121;1;122;0
WireConnection;124;0;14;0
WireConnection;129;0;124;4
WireConnection;129;1;128;0
WireConnection;135;0;77;0
WireConnection;135;1;112;0
WireConnection;135;2;133;0
WireConnection;137;0;79;0
WireConnection;137;1;121;2
WireConnection;137;2;133;0
WireConnection;136;0;138;0
WireConnection;136;1;80;0
WireConnection;136;2;133;0
WireConnection;134;0;133;0
WireConnection;134;1;124;4
WireConnection;0;0;135;0
WireConnection;0;1;130;0
WireConnection;0;3;136;0
WireConnection;0;4;137;0
ASEEND*/
//CHKSM=CAD930CFCA74A7A846CF62C3F2D442A1CB7A7AD7