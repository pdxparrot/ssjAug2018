// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/SH_BuildingSnos"
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
		_Snow_Amount("Snow_Amount", Range( 0 , 2)) = 0.13
		_Snow_NormalScale("Snow_NormalScale", Range( 0 , 1)) = 1
		_Snow_Tiling("Snow_Tiling", Vector) = (1,1,0,0)
		_Snow_Offset("Snow_Offset", Vector) = (0,0,0,0)
		_Snow_Metallic("Snow_Metallic", Float) = 0
		_Texture0("Texture 0", 2DArray) = "white" {}
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
		#pragma target 5.0
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
		uniform UNITY_DECLARE_TEX2DARRAY( _Material3_Array );
		uniform float4 _Material3_Array_ST;
		uniform float _Material1_NormalScale;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material1_Array );
		uniform float4 _Material1_Array_ST;
		uniform float _Material2_NormalScale;
		uniform float _Material3_NormalScale;
		uniform UNITY_DECLARE_TEX2DARRAY( _Texture0 );
		uniform float4 _Texture0_ST;
		uniform float _Snow_NormalScale;
		uniform float _Snow_Amount;
		uniform float2 _Material1_Tiling;
		uniform float2 _Material1_Offset;
		uniform float4 _Material1_TintColor;
		uniform float _Material1_TintStrength;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material2_Array );
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
			float4 tex2DNode180 = tex2D( _Mask, uv_Mask );
			float2 uv_Material3_Array = i.uv_texcoord * _Material3_Array_ST.xy + _Material3_Array_ST.zw;
			float3 texArray177 = UnpackScaleNormal( UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_Material3_Array, (float)1)  ) ,_Material1_NormalScale );
			float4 tex2DNode182 = tex2D( _Mask, uv_Mask );
			float2 uv_Material1_Array = i.uv_texcoord * _Material1_Array_ST.xy + _Material1_Array_ST.zw;
			float3 texArray178 = UnpackScaleNormal( UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_Material1_Array, (float)1)  ) ,_Material2_NormalScale );
			float4 tex2DNode190 = tex2D( _Mask, uv_Mask );
			float3 texArray194 = UnpackScaleNormal( UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_Material3_Array, (float)1)  ) ,_Material3_NormalScale );
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			float3 texArray228 = UnpackScaleNormal( UNITY_SAMPLE_TEX2DARRAY(_Texture0, float3(uv_Texture0, (float)1)  ) ,_Snow_NormalScale );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float2 uv_Mask214 = i.uv_texcoord;
			float4 tex2DNode214 = tex2D( _Mask, uv_Mask214 );
			float3 lerpResult120 = lerp( ( ( ( tex2DNode180.r * texArray177 ) + ( tex2DNode182.g * texArray178 ) ) + ( tex2DNode190.b * texArray194 ) ) , texArray228 , ( saturate( ( ase_worldNormal.y * _Snow_Amount ) ) * tex2DNode214.a ));
			o.Normal = lerpResult120;
			float2 uv_TexCoord150 = i.uv_texcoord * _Material1_Tiling + _Material1_Offset;
			float4 texArray159 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_TexCoord150, (float)0)  );
			float4 blendOpSrc165 = _Material1_TintColor;
			float4 blendOpDest165 = texArray159;
			float4 lerpResult184 = lerp( texArray159 , ( saturate( (( blendOpDest165 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest165 - 0.5 ) ) * ( 1.0 - blendOpSrc165 ) ) : ( 2.0 * blendOpDest165 * blendOpSrc165 ) ) )) , _Material1_TintStrength);
			float2 uv_TexCoord151 = i.uv_texcoord * _Material2_Tiling + _Material2_Offset;
			float4 texArray157 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_TexCoord151, (float)0)  );
			float4 blendOpSrc168 = _Material2_TintColor;
			float4 blendOpDest168 = texArray157;
			float4 lerpResult179 = lerp( texArray157 , ( saturate( (( blendOpDest168 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest168 - 0.5 ) ) * ( 1.0 - blendOpSrc168 ) ) : ( 2.0 * blendOpDest168 * blendOpSrc168 ) ) )) , _Material2_TintStrength);
			float2 uv_TexCoord158 = i.uv_texcoord * _Material3_Tiling + _Material3_Offset;
			float4 texArray164 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_TexCoord158, (float)0)  );
			float4 blendOpSrc181 = _Material3_TintColor;
			float4 blendOpDest181 = texArray164;
			float4 lerpResult196 = lerp( texArray164 , ( saturate( (( blendOpDest181 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest181 - 0.5 ) ) * ( 1.0 - blendOpSrc181 ) ) : ( 2.0 * blendOpDest181 * blendOpSrc181 ) ) )) , _Material3_TintStrength);
			float2 uv_TexCoord135 = i.uv_texcoord * _Snow_Tiling + _Snow_Offset;
			float4 texArray227 = UNITY_SAMPLE_TEX2DARRAY(_Texture0, float3(uv_TexCoord135, (float)0)  );
			float temp_output_140_0 = ( saturate( ( WorldNormalVector( i , lerpResult120 ).y * _Snow_Amount ) ) * tex2DNode214.a );
			float4 lerpResult129 = lerp( ( ( ( tex2DNode180.r * lerpResult184 ) + ( tex2DNode182.g * lerpResult179 ) ) + ( tex2DNode190.b * lerpResult196 ) ) , texArray227 , temp_output_140_0);
			o.Albedo = lerpResult129.rgb;
			float4 texArray188 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_Material3_Array, (float)2)  );
			float4 texArray189 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_Material1_Array, (float)2)  );
			float4 texArray197 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_Material3_Array, (float)2)  );
			float lerpResult131 = lerp( ( ( ( tex2DNode180.r * texArray188.r ) + ( tex2DNode182.g * texArray189.r ) ) + ( tex2DNode190.b * texArray197.x ) ) , _Snow_Metallic , temp_output_140_0);
			o.Metallic = lerpResult131;
			float4 texArray229 = UNITY_SAMPLE_TEX2DARRAY(_Texture0, float3(uv_Texture0, (float)2)  );
			float lerpResult128 = lerp( ( ( ( tex2DNode180.r * texArray188.g ) + ( tex2DNode182.g * texArray189.g ) ) + ( tex2DNode190.b * texArray197.y ) ) , texArray229.g , temp_output_140_0);
			o.Smoothness = lerpResult128;
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
			#pragma target 5.0
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
1782.4;96;970;725;-42.44806;-1912.39;2.349181;True;False
Node;AmplifyShaderEditor.CommentaryNode;143;-2192,1312;Float;False;1649.352;965.5948;Material2;20;201;199;195;192;189;182;179;178;173;172;171;169;168;160;157;154;151;149;146;144;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;142;-2192,256;Float;False;1717.034;1009.174;Material1;20;200;198;193;191;188;184;180;177;175;170;167;166;165;162;159;155;153;150;147;145;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;169;-1779.263,1874.597;Float;False;Property;_Material2_NormalScale;Material2_NormalScale;10;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;170;-1869.073,881.1888;Float;False;Property;_Material1_NormalScale;Material1_NormalScale;4;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;171;-1648,2080;Float;False;Constant;_Int5;Int 5;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;166;-1744,1088;Float;False;Constant;_Int3;Int 3;26;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.CommentaryNode;148;-2192,2336;Float;False;1669.569;985.8406;Material3;20;209;207;204;202;197;196;194;190;187;186;185;183;181;174;164;163;161;158;156;152;;1,1,1,1;0;0
Node;AmplifyShaderEditor.IntNode;186;-1696,3136;Float;False;Constant;_Int8;Int 8;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;182;-1152,1920;Float;True;Property;_TextureSample6;Texture Sample 6;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;214;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;178;-1456,1856;Float;True;Property;_TextureArray5;Texture Array 5;23;0;Create;True;0;0;False;0;None;0;Instance;159;Auto;True;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;183;-1830.279,2932.363;Float;False;Property;_Material3_NormalScale;Material3_NormalScale;16;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;180;-1184,864;Float;True;Property;_TextureSample3;Texture Sample 3;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;214;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;114;182.8481,2584.88;Float;False;Property;_Snow_Amount;Snow_Amount;19;0;Create;True;0;0;False;0;0.13;0.95;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;115;279.1086,2419.056;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureArrayNode;177;-1488,800;Float;True;Property;_TextureArray4;Texture Array 4;20;0;Create;True;0;0;False;0;None;0;Instance;164;Auto;True;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;145;-2112,640;Float;False;Property;_Material1_Offset;Material1_Offset;6;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;193;-720,784;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector2Node;147;-2108.81,512.048;Float;False;Property;_Material1_Tiling;Material1_Tiling;5;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;144;-2112,1680;Float;False;Property;_Material2_Offset;Material2_Offset;12;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;190;-1188.177,2924.622;Float;True;Property;_TextureSample10;Texture Sample 10;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;214;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;194;-1488,2848;Float;True;Property;_TextureArray8;Texture Array 8;23;0;Create;True;0;0;False;0;None;0;Instance;164;Auto;True;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;176;254.0637,2167.183;Float;True;Property;_Mask;Mask;0;1;[NoScaleOffset];Create;True;0;0;False;0;00e81767994d62f4ba5a3e48f0e93787;e151bab8f799fe541b2218a36b0220d6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;195;-736,1824;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;525.0842,2518.547;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;146;-2112,1568;Float;False;Property;_Material2_Tiling;Material2_Tiling;11;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;149;-1836.139,1382.07;Float;True;Property;_Material2_Array;Material2_Array;7;1;[NoScaleOffset];Create;True;0;0;False;0;9e68fd9253d6b5f49a538eb963199c94;9e68fd9253d6b5f49a538eb963199c94;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;153;-1838.064,318.0056;Float;True;Property;_Material1_Array;Material1_Array;1;1;[NoScaleOffset];Create;True;0;0;False;0;c90d7a96c0a623441b73da001e669a4e;c90d7a96c0a623441b73da001e669a4e;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.IntNode;231;511.5119,1909.402;Float;False;Constant;_Int10;Int 10;31;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;388.8383,1731.484;Float;False;Property;_Snow_NormalScale;Snow_NormalScale;20;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;151;-1840,1584;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;214;567.02,2166.841;Float;True;Property;_Mask_Sample;Mask_Sample;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;152;-2160,2752;Float;False;Property;_Material3_Offset;Material3_Offset;18;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;156;-2160,2624;Float;False;Property;_Material3_Tiling;Material3_Tiling;17;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.IntNode;155;-1744,992;Float;False;Constant;_Int1;Int 1;26;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.SaturateNode;119;682.1173,2522.622;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;205;-144,2048;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;207;-672,2832;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;150;-1840,544;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;154;-1664,1984;Float;False;Constant;_Int0;Int 0;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TextureArrayNode;157;-1456,1648;Float;True;Property;_TextureArray1;Texture Array 1;24;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;162;-1360,304;Float;False;Property;_Material1_TintColor;Material1_TintColor;2;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;160;-1376,1360;Float;False;Property;_Material2_TintColor;Material2_TintColor;8;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;158;-1888,2640;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;228;701.7347,1534.311;Float;True;Property;_TextureArray3;Texture Array 3;31;0;Create;True;0;0;False;0;None;0;Instance;227;Auto;True;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;163;-1874.73,2418.209;Float;True;Property;_Material3_Array;Material3_Array;13;1;[NoScaleOffset];Create;True;0;0;False;0;126ad50cba129524e82f6ec7fe0fa046;126ad50cba129524e82f6ec7fe0fa046;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.IntNode;161;-1696,3040;Float;False;Constant;_Int2;Int 2;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TextureArrayNode;159;-1488,576;Float;True;Property;_TextureArray2;Texture Array 2;25;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;211;191.7192,2895.807;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;886.6993,2823.155;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;167;-1744,1184;Float;False;Constant;_Int4;Int 4;26;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;172;-1648,2176;Float;False;Constant;_Int6;Int 6;21;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.BlendOpsNode;168;-1088,1360;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;165;-1072,320;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;120;1066.751,2893.8;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;175;-1392,480;Float;False;Property;_Material1_TintStrength;Material1_TintStrength;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;173;-1408,1536;Float;False;Property;_Material2_TintStrength;Material2_TintStrength;9;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;174;-1376,2400;Float;False;Property;_Material3_TintColor;Material3_TintColor;14;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;164;-1488,2656;Float;True;Property;_Material3_Sample;Material3_Sample;27;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;185;-1696,3216;Float;False;Constant;_Int7;Int 7;21;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.LerpOp;184;-912,592;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureArrayNode;188;-1488,1056;Float;True;Property;_TextureArray6;Texture Array 6;21;0;Create;True;0;0;False;0;None;0;Instance;164;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;187;-1408,2576;Float;False;Property;_Material3_TintStrength;Material3_TintStrength;15;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;179;-928,1648;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;181;-1088,2400;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;121;1185.812,2478.13;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TextureArrayNode;189;-1456,2064;Float;True;Property;_TextureArray7;Texture Array 7;23;0;Create;True;0;0;False;0;None;0;Instance;159;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;192;-736,1632;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;133;194.8297,1536.058;Float;False;Property;_Snow_Tiling;Snow_Tiling;21;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;1376.038,2571.319;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;200;-720,976;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;197;-1488,3104;Float;True;Property;_TextureArray9;Texture Array 9;23;0;Create;True;0;0;False;0;None;0;Instance;164;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;201;-720,1968;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;191;-720,576;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;196;-928,2672;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;199;-720,2080;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;-720,1088;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;134;194.8297,1664.058;Float;False;Property;_Snow_Offset;Snow_Offset;22;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.IntNode;232;507.1767,1997.547;Float;False;Constant;_Int11;Int 11;31;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;208;-160,1824;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;209;-672,3056;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;-672,2720;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;203;-112,2256;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;226;437.2937,1327.902;Float;True;Property;_Texture0;Texture 0;26;0;Create;True;0;0;False;0;ad280c83e3b496f4293f616530e6d565;ad280c83e3b496f4293f616530e6d565;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SaturateNode;123;1535.957,2570.852;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;230;511.5116,1818.369;Float;False;Constant;_Int9;Int 9;31;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;206;-112,2448;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;135;434.8298,1552.058;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;-672,3168;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;1739.189,2749.473;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;227;693.0907,1326.431;Float;True;Property;_Snow_Sample;Snow_Sample;28;0;Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;229;698.8705,1745.842;Float;True;Property;_TextureArray10;Texture Array 10;32;0;Create;True;0;0;False;0;None;0;Instance;227;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;213;191.7192,2703.807;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;212;191.7192,3034.283;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;210;185.7549,3164.177;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;132;1662.144,3231.734;Float;False;Property;_Snow_Metallic;Snow_Metallic;23;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;131;1916.597,3176.775;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;129;2031.262,2677.833;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;128;1942.096,3045.931;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2306.814,2877.322;Float;False;True;7;Float;ASEMaterialInspector;0;0;Standard;Yeti/SH_BuildingSnos;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;178;1;171;0
WireConnection;178;3;169;0
WireConnection;177;1;166;0
WireConnection;177;3;170;0
WireConnection;193;0;180;1
WireConnection;193;1;177;0
WireConnection;194;1;186;0
WireConnection;194;3;183;0
WireConnection;195;0;182;2
WireConnection;195;1;178;0
WireConnection;116;0;115;2
WireConnection;116;1;114;0
WireConnection;151;0;146;0
WireConnection;151;1;144;0
WireConnection;214;0;176;0
WireConnection;119;0;116;0
WireConnection;205;0;193;0
WireConnection;205;1;195;0
WireConnection;207;0;190;3
WireConnection;207;1;194;0
WireConnection;150;0;147;0
WireConnection;150;1;145;0
WireConnection;157;6;149;0
WireConnection;157;0;151;0
WireConnection;157;1;154;0
WireConnection;158;0;156;0
WireConnection;158;1;152;0
WireConnection;228;1;231;0
WireConnection;228;3;222;0
WireConnection;159;6;153;0
WireConnection;159;0;150;0
WireConnection;159;1;155;0
WireConnection;211;0;205;0
WireConnection;211;1;207;0
WireConnection;137;0;119;0
WireConnection;137;1;214;4
WireConnection;168;0;160;0
WireConnection;168;1;157;0
WireConnection;165;0;162;0
WireConnection;165;1;159;0
WireConnection;120;0;211;0
WireConnection;120;1;228;0
WireConnection;120;2;137;0
WireConnection;164;6;163;0
WireConnection;164;0;158;0
WireConnection;164;1;161;0
WireConnection;184;0;159;0
WireConnection;184;1;165;0
WireConnection;184;2;175;0
WireConnection;188;1;167;0
WireConnection;179;0;157;0
WireConnection;179;1;168;0
WireConnection;179;2;173;0
WireConnection;181;0;174;0
WireConnection;181;1;164;0
WireConnection;121;0;120;0
WireConnection;189;1;172;0
WireConnection;192;0;182;2
WireConnection;192;1;179;0
WireConnection;122;0;121;2
WireConnection;122;1;114;0
WireConnection;200;0;180;1
WireConnection;200;1;188;2
WireConnection;197;1;185;0
WireConnection;201;0;182;2
WireConnection;201;1;189;2
WireConnection;191;0;180;1
WireConnection;191;1;184;0
WireConnection;196;0;164;0
WireConnection;196;1;181;0
WireConnection;196;2;187;0
WireConnection;199;0;182;2
WireConnection;199;1;189;1
WireConnection;198;0;180;1
WireConnection;198;1;188;1
WireConnection;208;0;191;0
WireConnection;208;1;192;0
WireConnection;209;0;190;3
WireConnection;209;1;197;2
WireConnection;204;0;190;3
WireConnection;204;1;196;0
WireConnection;203;0;200;0
WireConnection;203;1;201;0
WireConnection;123;0;122;0
WireConnection;206;0;198;0
WireConnection;206;1;199;0
WireConnection;135;0;133;0
WireConnection;135;1;134;0
WireConnection;202;0;190;3
WireConnection;202;1;197;1
WireConnection;140;0;123;0
WireConnection;140;1;214;4
WireConnection;227;6;226;0
WireConnection;227;0;135;0
WireConnection;227;1;230;0
WireConnection;229;1;232;0
WireConnection;213;0;208;0
WireConnection;213;1;204;0
WireConnection;212;0;203;0
WireConnection;212;1;209;0
WireConnection;210;0;206;0
WireConnection;210;1;202;0
WireConnection;131;0;210;0
WireConnection;131;1;132;0
WireConnection;131;2;140;0
WireConnection;129;0;213;0
WireConnection;129;1;227;0
WireConnection;129;2;140;0
WireConnection;128;0;212;0
WireConnection;128;1;229;2
WireConnection;128;2;140;0
WireConnection;0;0;129;0
WireConnection;0;1;120;0
WireConnection;0;3;131;0
WireConnection;0;4;128;0
ASEEND*/
//CHKSM=6A44B872F67EAB6DDAFC9299E9F23EC85C42F019