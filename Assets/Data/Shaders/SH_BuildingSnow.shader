// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/SH_BuildingSnow"
{
	Properties
	{
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		[NoScaleOffset]_Material1_Array("Material1_Array", 2DArray) = "white" {}
		[NoScaleOffset][Normal]_Material1_Normal("Material1_Normal", 2D) = "white" {}
		_Material1_TintColor("Material1_TintColor", Color) = (1,1,1,1)
		_Material1_Tiling("Material1_Tiling", Vector) = (1,1,0,0)
		_Material1_Offset("Material1_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Material2_Array("Material2_Array", 2DArray) = "white" {}
		[NoScaleOffset][Normal]_Material2_Normal("Material2_Normal", 2D) = "white" {}
		_Material2_TintColor("Material2_TintColor", Color) = (1,1,1,1)
		_Material2_Tiling("Material2_Tiling", Vector) = (1,1,0,0)
		_Material2_Offset("Material2_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Material3_Array("Material3_Array", 2DArray) = "white" {}
		[NoScaleOffset][Normal]_Material3_Normal("Material3_Normal", 2D) = "white" {}
		_Material3_TintColor("Material3_TintColor", Color) = (1,1,1,1)
		_Material3_Tiling("Material3_Tiling", Vector) = (1,1,0,0)
		_Material3_Offset("Material3_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Snow_Array("Snow_Array", 2DArray) = "white" {}
		[NoScaleOffset][Normal]_Snow_Normal("Snow_Normal", 2D) = "white" {}
		_Snow_Amount("Snow_Amount", Range( 0 , 2)) = 0
		_Snow_Tiling("Snow_Tiling", Vector) = (1,1,0,0)
		_Snow_Offset("Snow_Offset", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
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
		uniform sampler2D _Material1_Normal;
		uniform float2 _Material1_Tiling;
		uniform float2 _Material1_Offset;
		uniform sampler2D _Material2_Normal;
		uniform float2 _Material2_Tiling;
		uniform float2 _Material2_Offset;
		uniform sampler2D _Material3_Normal;
		uniform float2 _Material3_Tiling;
		uniform float2 _Material3_Offset;
		uniform sampler2D _Snow_Normal;
		uniform float2 _Snow_Tiling;
		uniform float2 _Snow_Offset;
		uniform float _Snow_Amount;
		uniform float4 _Material1_TintColor;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material1_Array );
		uniform float4 _Material2_TintColor;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material2_Array );
		uniform float4 _Material3_TintColor;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material3_Array );
		uniform UNITY_DECLARE_TEX2DARRAY( _Snow_Array );

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 tex2DNode100 = tex2D( _Mask, uv_Mask );
			float2 uv_TexCoord10 = i.uv_texcoord * _Material1_Tiling + _Material1_Offset;
			float4 tex2DNode101 = tex2D( _Mask, uv_Mask );
			float2 uv_TexCoord36 = i.uv_texcoord * _Material2_Tiling + _Material2_Offset;
			float4 tex2DNode13 = tex2D( _Mask, uv_Mask );
			float2 uv_TexCoord58 = i.uv_texcoord * _Material3_Tiling + _Material3_Offset;
			float2 uv_TexCoord116 = i.uv_texcoord * _Snow_Tiling + _Snow_Offset;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float temp_output_128_0 = saturate( ( ase_worldNormal.y * _Snow_Amount ) );
			float4 lerpResult130 = lerp( ( ( ( tex2DNode100.r * tex2D( _Material1_Normal, uv_TexCoord10 ) ) + ( tex2DNode101.g * tex2D( _Material2_Normal, uv_TexCoord36 ) ) ) + ( tex2DNode13.b * tex2D( _Material3_Normal, uv_TexCoord58 ) ) ) , tex2D( _Snow_Normal, uv_TexCoord116 ) , temp_output_128_0);
			o.Normal = lerpResult130.rgb;
			float4 texArray81 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_TexCoord10, (float)0)  );
			float4 texArray93 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_TexCoord36, (float)0)  );
			float4 texArray104 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_TexCoord58, (float)0)  );
			float4 texArray112 = UNITY_SAMPLE_TEX2DARRAY(_Snow_Array, float3(uv_TexCoord116, (float)0)  );
			float2 uv_Mask124 = i.uv_texcoord;
			float4 tex2DNode124 = tex2D( _Mask, uv_Mask124 );
			float temp_output_134_0 = ( saturate( ( WorldNormalVector( i , lerpResult130.rgb ).y * _Snow_Amount ) ) * tex2DNode124.a );
			float4 lerpResult135 = lerp( ( ( ( tex2DNode100.r * saturate( ( _Material1_TintColor + texArray81 ) ) ) + ( tex2DNode101.g * saturate( ( _Material2_TintColor + texArray93 ) ) ) ) + ( tex2DNode13.b * saturate( ( _Material3_TintColor + texArray104 ) ) ) ) , texArray112 , temp_output_134_0);
			o.Albedo = lerpResult135.rgb;
			float4 texArray83 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_TexCoord10, (float)1)  );
			float4 texArray95 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_TexCoord36, (float)1)  );
			float4 texArray109 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_TexCoord58, (float)1)  );
			float lerpResult136 = lerp( 0.0 , ( ( ( tex2DNode100.r * texArray83.r ) + ( tex2DNode101.a * texArray95.r ) ) + ( tex2DNode13.a * texArray109.x ) ) , temp_output_134_0);
			o.Metallic = lerpResult136;
			float4 texArray121 = UNITY_SAMPLE_TEX2DARRAY(_Snow_Array, float3(uv_TexCoord116, (float)1)  );
			float lerpResult137 = lerp( ( ( ( tex2DNode100.r * texArray83.g ) + ( tex2DNode101.g * texArray95.g ) ) + ( tex2DNode13.b * texArray109.y ) ) , texArray121.g , temp_output_134_0);
			o.Smoothness = ( 1.0 - lerpResult137 );
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
1590.4;96;1162;725;817.3403;-815.0754;2.308584;True;False
Node;AmplifyShaderEditor.CommentaryNode;28;-1712.951,47.70793;Float;False;1444.627;862.1458;Material1;18;83;88;142;11;12;15;20;89;86;10;151;152;81;100;143;26;25;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;33;-1710.892,925.0977;Float;False;1457.233;867.1923;Material2;18;51;52;50;49;92;96;36;35;34;145;154;153;38;95;93;98;101;144;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;11;-1684.991,435.9624;Float;False;Property;_Material1_Tiling;Material1_Tiling;4;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;35;-1615.527,1266.324;Float;False;Property;_Material2_Tiling;Material2_Tiling;9;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;12;-1684.991,562.4587;Float;False;Property;_Material1_Offset;Material1_Offset;5;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;57;-1749.251,1827.527;Float;False;1513.324;866.2158;Material3;18;74;73;103;105;58;146;67;65;68;66;156;155;69;109;104;106;147;13;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;34;-1615.527,1392.82;Float;False;Property;_Material2_Offset;Material2_Offset;10;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1465.908,451.9082;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;73;-1700.003,2154.167;Float;False;Property;_Material3_Tiling;Material3_Tiling;14;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;142;-1677.28,695.204;Float;True;Property;_Material1_Normal;Material1_Normal;2;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;145;-1602.491,1553.91;Float;True;Property;_Material2_Normal;Material2_Normal;7;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1406.735,1273.672;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;74;-1700.003,2280.664;Float;False;Property;_Material3_Offset;Material3_Offset;15;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;143;-1117.93,489.8604;Float;True;Property;_TextureSample2;Texture Sample 2;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;100;-815.1755,575.8163;Float;True;Property;_TextureSample0;Texture Sample 0;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;124;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;144;-1076.378,1340.037;Float;True;Property;_TextureSample3;Texture Sample 3;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;101;-747.8143,1401.478;Float;True;Property;_TextureSample1;Texture Sample 1;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;124;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;146;-1358.146,2406.329;Float;True;Property;_Material3_Normal;Material3_Normal;12;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-1449.922,2180.554;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-771.4981,2320.71;Float;True;Property;_MaskSample;MaskSample;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;124;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;119;336.2921,1022.554;Float;False;Property;_Snow_Offset;Snow_Offset;20;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;147;-1072.801,2264.045;Float;True;Property;_TextureSample4;Texture Sample 4;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;125;531.9811,1731.366;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector2Node;118;334.8161,895.6187;Float;False;Property;_Snow_Tiling;Snow_Tiling;19;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-439.252,469.3462;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;126;455.8491,1907.897;Float;False;Property;_Snow_Amount;Snow_Amount;18;0;Create;True;0;0;False;0;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-392.5741,1311.297;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-403.1723,2245.885;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;771.5062,1832.541;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;15.03199,1279.705;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;116;570.5856,926.071;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;149;577.4812,1123.045;Float;True;Property;_Snow_Normal;Snow_Normal;17;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;148;844.981,1105.894;Float;True;Property;_TextureSample5;Texture Sample 5;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;92;-1427.002,1079.42;Float;True;Property;_Material2_Array;Material2_Array;6;1;[NoScaleOffset];Create;True;0;0;False;0;9e68fd9253d6b5f49a538eb963199c94;9e68fd9253d6b5f49a538eb963199c94;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;89;-1448.546,250.358;Float;True;Property;_Material1_Array;Material1_Array;1;1;[NoScaleOffset];Create;True;0;0;False;0;c90d7a96c0a623441b73da001e669a4e;c90d7a96c0a623441b73da001e669a4e;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.IntNode;96;-1378.136,1400.671;Float;False;Constant;_Int0;Int 0;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;86;-1392.879,583.7573;Float;False;Constant;_AlbedoIndex;AlbedoIndex;26;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;307.1421,2233.419;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;128;958.5882,1836.842;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;98;-1300.532,1674.045;Float;False;Constant;_Int2;Int 2;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.ColorNode;20;-1033.941,104.2733;Float;False;Property;_Material1_TintColor;Material1_TintColor;3;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;88;-1414.312,805.5157;Float;False;Constant;_MRAOIndex;MRAOIndex;26;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.ColorNode;38;-964.4687,961.5842;Float;False;Property;_Material2_TintColor;Material2_TintColor;8;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;81;-1118.002,277.3655;Float;True;Property;_Material1_Sample;Material1_Sample;21;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;93;-1054.645,1129.644;Float;True;Property;_Material2_Sample;Material2_Sample;22;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;103;-1440.32,1957.572;Float;True;Property;_Material3_Array;Material3_Array;11;1;[NoScaleOffset];Create;True;0;0;False;0;126ad50cba129524e82f6ec7fe0fa046;126ad50cba129524e82f6ec7fe0fa046;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.LerpOp;130;1274.632,2165.922;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;105;-1358.817,2308.971;Float;False;Constant;_Int3;Int 3;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TextureArrayNode;104;-1060.818,2043.97;Float;True;Property;_Material3_Sample;Material3_Sample;23;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;95;-1054.645,1545.645;Float;True;Property;_TextureArray2;Texture Array 2;23;0;Create;True;0;0;False;0;None;0;Instance;93;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;83;-1117.808,700.8838;Float;True;Property;_Material1_MR;Material1_MR;21;0;Create;True;0;0;False;0;None;0;Instance;81;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;106;-1272.007,2600.742;Float;False;Constant;_Int4;Int 4;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;151;-747.6483,258.5878;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;69;-972.5157,1873.535;Float;False;Property;_Material3_TintColor;Material3_TintColor;13;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;153;-709.4156,1111.453;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldNormalVector;131;1446.992,1737.532;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;155;-721.1833,2029.08;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-385.365,1449.193;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;109;-1062.833,2481.417;Float;True;Property;_TextureArray5;Texture Array 5;23;0;Create;True;0;0;False;0;None;0;Instance;104;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;152;-609.3911,262.3756;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;1670.499,1889.772;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;154;-567.9142,1109.748;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;14;446.8049,1523.699;Float;True;Property;_Mask;Mask;0;1;[NoScaleOffset];Create;True;0;0;False;0;00e81767994d62f4ba5a3e48f0e93787;e151bab8f799fe541b2218a36b0220d6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-439.252,661.3463;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-439.252,773.3461;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-385.365,1561.193;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-394.1679,1088.515;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-439.252,240.7497;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;122;648.8196,1396.468;Float;False;Constant;_Int8;Int 8;26;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;124;756.4361,1523.369;Float;True;Property;_Mask_Sample;Mask_Sample;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;133;1820.067,1892.017;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-393.2375,2436.477;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;156;-575.8674,2029.08;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;4.276234,1433.08;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;111;539.8334,703.2272;Float;True;Property;_Snow_Array;Snow_Array;16;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-1.098644,1066.766;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;114;576.3923,1047.618;Float;False;Constant;_Int6;Int 6;24;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-2.25935,1540.457;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;1976.127,2016.39;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;319.0888,2418.679;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-392.4124,2547.661;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-397.541,2005.83;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureArrayNode;121;834.0652,1300.985;Float;True;Property;_TextureArray7;Texture Array 7;25;0;Create;True;0;0;False;0;None;0;Instance;112;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;137;2240.179,2368.15;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;1988.597,2458.828;Float;False;Constant;_Snow_Metallic;Snow_Metallic;24;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;316.5697,2009.312;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;310.7852,2564.196;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;112;844.343,898.7892;Float;True;Property;_Snow_Sample;Snow_Sample;24;0;Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;150;2404.629,2367.626;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;136;2238.941,2246.906;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;1105.738,1778.517;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;135;2219.217,1951.066;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2637.395,2161.503;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;Yeti/SH_BuildingSnow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;143;0;142;0
WireConnection;143;1;10;0
WireConnection;144;0;145;0
WireConnection;144;1;36;0
WireConnection;58;0;73;0
WireConnection;58;1;74;0
WireConnection;147;0;146;0
WireConnection;147;1;58;0
WireConnection;24;0;100;1
WireConnection;24;1;143;0
WireConnection;49;0;101;2
WireConnection;49;1;144;0
WireConnection;65;0;13;3
WireConnection;65;1;147;0
WireConnection;127;0;125;2
WireConnection;127;1;126;0
WireConnection;54;0;24;0
WireConnection;54;1;49;0
WireConnection;116;0;118;0
WireConnection;116;1;119;0
WireConnection;148;0;149;0
WireConnection;148;1;116;0
WireConnection;78;0;54;0
WireConnection;78;1;65;0
WireConnection;128;0;127;0
WireConnection;81;6;89;0
WireConnection;81;0;10;0
WireConnection;81;1;86;0
WireConnection;93;6;92;0
WireConnection;93;0;36;0
WireConnection;93;1;96;0
WireConnection;130;0;78;0
WireConnection;130;1;148;0
WireConnection;130;2;128;0
WireConnection;104;6;103;0
WireConnection;104;0;58;0
WireConnection;104;1;105;0
WireConnection;95;0;36;0
WireConnection;95;1;98;0
WireConnection;83;0;10;0
WireConnection;83;1;88;0
WireConnection;151;0;20;0
WireConnection;151;1;81;0
WireConnection;153;0;38;0
WireConnection;153;1;93;0
WireConnection;131;0;130;0
WireConnection;155;0;69;0
WireConnection;155;1;104;0
WireConnection;50;0;101;2
WireConnection;50;1;95;2
WireConnection;109;0;58;0
WireConnection;109;1;106;0
WireConnection;152;0;151;0
WireConnection;132;0;131;2
WireConnection;132;1;126;0
WireConnection;154;0;153;0
WireConnection;25;0;100;1
WireConnection;25;1;83;2
WireConnection;26;0;100;1
WireConnection;26;1;83;1
WireConnection;52;0;101;4
WireConnection;52;1;95;1
WireConnection;51;0;101;2
WireConnection;51;1;154;0
WireConnection;15;0;100;1
WireConnection;15;1;152;0
WireConnection;124;0;14;0
WireConnection;133;0;132;0
WireConnection;66;0;13;3
WireConnection;66;1;109;2
WireConnection;156;0;155;0
WireConnection;55;0;25;0
WireConnection;55;1;50;0
WireConnection;53;0;15;0
WireConnection;53;1;51;0
WireConnection;56;0;26;0
WireConnection;56;1;52;0
WireConnection;134;0;133;0
WireConnection;134;1;124;4
WireConnection;79;0;55;0
WireConnection;79;1;66;0
WireConnection;68;0;13;4
WireConnection;68;1;109;1
WireConnection;67;0;13;3
WireConnection;67;1;156;0
WireConnection;121;0;116;0
WireConnection;121;1;122;0
WireConnection;137;0;79;0
WireConnection;137;1;121;2
WireConnection;137;2;134;0
WireConnection;77;0;53;0
WireConnection;77;1;67;0
WireConnection;80;0;56;0
WireConnection;80;1;68;0
WireConnection;112;6;111;0
WireConnection;112;0;116;0
WireConnection;112;1;114;0
WireConnection;150;0;137;0
WireConnection;136;0;138;0
WireConnection;136;1;80;0
WireConnection;136;2;134;0
WireConnection;129;0;124;4
WireConnection;129;1;128;0
WireConnection;135;0;77;0
WireConnection;135;1;112;0
WireConnection;135;2;134;0
WireConnection;0;0;135;0
WireConnection;0;1;130;0
WireConnection;0;3;136;0
WireConnection;0;4;150;0
ASEEND*/
//CHKSM=EFC606220B63B21D5D3E8528240EA6B2114004F0