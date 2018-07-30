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
		_Material1_RoughnessMult("Material1_RoughnessMult", Float) = 1
		_Material1_AOLift("Material1_AOLift", Float) = 0
		_Material1_Tiling("Material1_Tiling", Vector) = (1,1,0,0)
		_Material1_Offset("Material1_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Material2_Array("Material2_Array", 2DArray) = "white" {}
		[NoScaleOffset][Normal]_Material2_Normal("Material2_Normal", 2D) = "white" {}
		_Material2_TintColor("Material2_TintColor", Color) = (1,1,1,1)
		_Material2_RoughnessMult("Material2_RoughnessMult", Float) = 1
		_Material2_AOLift("Material2_AOLift", Float) = 0
		_Material2_Tiling("Material2_Tiling", Vector) = (1,1,0,0)
		_Material2_Offset("Material2_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Material3_Array("Material3_Array", 2DArray) = "white" {}
		[NoScaleOffset][Normal]_Material3_Normal("Material3_Normal", 2D) = "white" {}
		_Material3_TintColor("Material3_TintColor", Color) = (1,1,1,1)
		_Material3_RoughnessMult("Material3_RoughnessMult", Float) = 1
		_Material3_AOLift("Material3_AOLift", Float) = 0
		_Material3_Tiling("Material3_Tiling", Vector) = (1,1,0,0)
		_Material3_Offset("Material3_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Snow_Array("Snow_Array", 2DArray) = "white" {}
		[NoScaleOffset][Normal]_Snow_Normal("Snow_Normal", 2D) = "white" {}
		_Snow_Amount("Snow_Amount", Range( 0 , 2)) = 0
		_Snow_RoughnessMult("Snow_RoughnessMult", Float) = 1
		_Snow_AOLift("Snow_AOLift", Float) = 0
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
		uniform float _Material1_RoughnessMult;
		uniform float _Material2_RoughnessMult;
		uniform float _Material3_RoughnessMult;
		uniform float _Snow_RoughnessMult;
		uniform float _Material1_AOLift;
		uniform float _Material2_AOLift;
		uniform float _Material3_AOLift;
		uniform float _Snow_AOLift;

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
			float2 uv_Mask124 = i.uv_texcoord;
			float4 tex2DNode124 = tex2D( _Mask, uv_Mask124 );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 lerpResult130 = lerp( ( ( ( tex2DNode100.r * tex2D( _Material1_Normal, uv_TexCoord10 ) ) + ( tex2DNode101.g * tex2D( _Material2_Normal, uv_TexCoord36 ) ) ) + ( tex2DNode13.b * tex2D( _Material3_Normal, uv_TexCoord58 ) ) ) , tex2D( _Snow_Normal, uv_TexCoord116 ) , ( tex2DNode124.a * saturate( ( ase_worldNormal.y * _Snow_Amount ) ) ));
			o.Normal = lerpResult130.rgb;
			float4 texArray81 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_TexCoord10, (float)0)  );
			float4 texArray93 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_TexCoord36, (float)0)  );
			float4 texArray104 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_TexCoord58, (float)0)  );
			float4 texArray112 = UNITY_SAMPLE_TEX2DARRAY(_Snow_Array, float3(uv_TexCoord116, (float)0)  );
			float temp_output_134_0 = ( saturate( ( WorldNormalVector( i , lerpResult130.rgb ).y * _Snow_Amount ) ) * tex2DNode124.a );
			float4 lerpResult135 = lerp( ( ( ( tex2DNode100.r * saturate( ( _Material1_TintColor + texArray81 ) ) ) + ( tex2DNode101.g * saturate( ( _Material2_TintColor + texArray93 ) ) ) ) + ( tex2DNode13.b * saturate( ( _Material3_TintColor + texArray104 ) ) ) ) , texArray112 , temp_output_134_0);
			o.Albedo = lerpResult135.rgb;
			float4 texArray83 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_TexCoord10, (float)1)  );
			float4 texArray95 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_TexCoord36, (float)1)  );
			float4 texArray109 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_TexCoord58, (float)1)  );
			float lerpResult136 = lerp( ( ( ( tex2DNode100.r * texArray83.r ) + ( tex2DNode101.a * texArray95.r ) ) + ( tex2DNode13.b * texArray109.x ) ) , 0.0 , temp_output_134_0);
			o.Metallic = lerpResult136;
			float4 texArray121 = UNITY_SAMPLE_TEX2DARRAY(_Snow_Array, float3(uv_TexCoord116, (float)1)  );
			float lerpResult137 = lerp( ( ( ( tex2DNode100.r * ( _Material1_RoughnessMult * texArray83.g ) ) + ( tex2DNode101.g * ( _Material2_RoughnessMult * texArray95.g ) ) ) + ( tex2DNode13.b * ( _Material3_RoughnessMult * texArray109.y ) ) ) , ( texArray121.g * _Snow_RoughnessMult ) , temp_output_134_0);
			o.Smoothness = lerpResult137;
			float lerpResult168 = lerp( ( ( ( tex2DNode100.r * saturate( ( texArray83.b + _Material1_AOLift ) ) ) + ( tex2DNode101.g * saturate( ( texArray95.b + _Material2_AOLift ) ) ) ) + ( tex2DNode13.b * saturate( ( texArray109.z + _Material3_AOLift ) ) ) ) , ( texArray121.b + _Snow_AOLift ) , temp_output_134_0);
			o.Occlusion = lerpResult168;
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
2752.8;311.2;1534;840;-121.0995;-1734.059;1.422609;True;False
Node;AmplifyShaderEditor.CommentaryNode;28;-1709.222,-86.5317;Float;False;1444.627;947.7268;Material1;24;163;15;26;152;25;151;157;83;158;81;20;86;89;88;24;143;100;142;10;12;11;169;180;181;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;33;-1722.079,881.3799;Float;False;1460.535;898.5584;Material2;24;172;50;51;165;52;159;154;95;160;153;98;38;93;96;92;49;144;101;36;145;35;34;179;178;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;12;-1681.262,428.219;Float;False;Property;_Material1_Offset;Material1_Offset;7;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;34;-1626.714,1349.102;Float;False;Property;_Material2_Offset;Material2_Offset;14;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;35;-1626.714,1222.606;Float;False;Property;_Material2_Tiling;Material2_Tiling;13;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;11;-1681.262,301.7227;Float;False;Property;_Material1_Tiling;Material1_Tiling;6;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;57;-1749.251,1827.527;Float;False;1499.019;930.5898;Material3;22;67;68;66;156;155;162;69;161;104;109;106;105;103;65;147;13;146;58;74;73;174;176;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1417.922,1229.954;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;145;-1613.678,1510.192;Float;True;Property;_Material2_Normal;Material2_Normal;9;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Vector2Node;73;-1700.003,2154.167;Float;False;Property;_Material3_Tiling;Material3_Tiling;20;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;74;-1700.003,2280.664;Float;False;Property;_Material3_Offset;Material3_Offset;21;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;142;-1673.551,560.9642;Float;True;Property;_Material1_Normal;Material1_Normal;2;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1462.179,317.6685;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;144;-1087.565,1296.319;Float;True;Property;_TextureSample3;Texture Sample 3;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-1449.922,2180.554;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;100;-796.1965,201.7328;Float;True;Property;_TextureSample0;Texture Sample 0;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;124;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;101;-746.4097,1140.473;Float;True;Property;_TextureSample1;Texture Sample 1;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;124;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;125;531.9811,1731.366;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexturePropertyNode;146;-1358.146,2406.329;Float;True;Property;_Material3_Normal;Material3_Normal;16;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;126;455.8491,1907.897;Float;False;Property;_Snow_Amount;Snow_Amount;24;0;Create;True;0;0;False;0;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;143;-1114.201,355.6207;Float;True;Property;_TextureSample2;Texture Sample 2;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;119;336.2921,1022.554;Float;False;Property;_Snow_Offset;Snow_Offset;28;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-435.5231,335.1065;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;118;334.8161,895.6187;Float;False;Property;_Snow_Tiling;Snow_Tiling;27;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-403.7607,1267.579;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;13;-746.2023,2108.227;Float;True;Property;_MaskSample;MaskSample;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;124;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;771.5062,1832.541;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;14;446.8049,1523.699;Float;True;Property;_Mask;Mask;0;1;[NoScaleOffset];Create;True;0;0;False;0;00e81767994d62f4ba5a3e48f0e93787;e151bab8f799fe541b2218a36b0220d6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;147;-1072.801,2264.045;Float;True;Property;_TextureSample4;Texture Sample 4;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;54;15.03199,1279.705;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;116;570.5856,926.071;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-403.1723,2245.885;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;96;-1389.323,1356.953;Float;False;Constant;_Int0;Int 0;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TexturePropertyNode;149;577.4812,1123.045;Float;True;Property;_Snow_Normal;Snow_Normal;23;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SaturateNode;128;958.5882,1836.842;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;88;-1410.583,671.2759;Float;False;Constant;_MRAOIndex;MRAOIndex;26;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;124;756.4361,1523.369;Float;True;Property;_Mask_Sample;Mask_Sample;28;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;92;-1438.189,1035.702;Float;True;Property;_Material2_Array;Material2_Array;8;1;[NoScaleOffset];Create;True;0;0;False;0;9e68fd9253d6b5f49a538eb963199c94;9e68fd9253d6b5f49a538eb963199c94;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.IntNode;98;-1311.719,1630.327;Float;False;Constant;_Int2;Int 2;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;86;-1389.15,449.5176;Float;False;Constant;_AlbedoIndex;AlbedoIndex;26;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TexturePropertyNode;89;-1444.817,116.1183;Float;True;Property;_Material1_Array;Material1_Array;1;1;[NoScaleOffset];Create;True;0;0;False;0;c90d7a96c0a623441b73da001e669a4e;c90d7a96c0a623441b73da001e669a4e;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;304.7579,2178.581;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;169;-1025.687,767.6505;Float;False;Property;_Material1_AOLift;Material1_AOLift;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;172;-959.3,1694.517;Float;False;Property;_Material2_AOLift;Material2_AOLift;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;38;-975.6553,917.8664;Float;False;Property;_Material2_TintColor;Material2_TintColor;10;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;1105.738,1778.517;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;148;844.981,1105.894;Float;True;Property;_TextureSample5;Texture Sample 5;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;105;-1358.817,2308.971;Float;False;Constant;_Int3;Int 3;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.ColorNode;20;-1030.212,-29.96634;Float;False;Property;_Material1_TintColor;Material1_TintColor;3;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;103;-1440.32,1957.572;Float;True;Property;_Material3_Array;Material3_Array;15;1;[NoScaleOffset];Create;True;0;0;False;0;126ad50cba129524e82f6ec7fe0fa046;126ad50cba129524e82f6ec7fe0fa046;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.IntNode;106;-1272.007,2600.742;Float;False;Constant;_Int4;Int 4;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.TextureArrayNode;95;-1065.832,1501.927;Float;True;Property;_TextureArray2;Texture Array 2;23;0;Create;True;0;0;False;0;None;0;Instance;93;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;83;-1114.079,566.644;Float;True;Property;_Material1_MR;Material1_MR;21;0;Create;True;0;0;False;0;None;0;Instance;81;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;93;-1065.832,1085.926;Float;True;Property;_Material2_Sample;Material2_Sample;30;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;81;-1114.273,143.1258;Float;True;Property;_Material1_Sample;Material1_Sample;29;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;178;-700.6788,1621.771;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;174;-940.2228,2683.608;Float;False;Property;_Material3_AOLift;Material3_AOLift;19;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;180;-779.0668,717.9016;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-808.4086,441.514;Float;False;Property;_Material1_RoughnessMult;Material1_RoughnessMult;4;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;69;-972.5157,1873.535;Float;False;Property;_Material3_TintColor;Material3_TintColor;17;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;130;1274.632,2165.922;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;151;-748.0785,103.5524;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;160;-788.7514,1374.081;Float;False;Property;_Material2_RoughnessMult;Material2_RoughnessMult;11;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;104;-1060.818,2043.97;Float;True;Property;_Material3_Sample;Material3_Sample;31;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;109;-1062.833,2481.417;Float;True;Property;_TextureArray5;Texture Array 5;23;0;Create;True;0;0;False;0;None;0;Instance;104;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;153;-720.6022,1034.008;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;159;-589.7578,1456.714;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;131;1446.992,1737.532;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;161;-767.4486,2343.625;Float;False;Property;_Material3_RoughnessMult;Material3_RoughnessMult;18;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;152;-609.8213,107.3402;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;154;-579.1008,1032.303;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;177;-692.8585,2646.451;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;179;-561.4987,1648.967;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;-721.1833,1934.642;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-612.8886,547.2923;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;181;-617.49,764.295;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-396.5516,1405.475;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;156;-575.8674,1934.642;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-396.5516,1517.475;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;1670.499,1889.772;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;-435.6217,748.0551;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-435.5231,527.1066;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;162;-587.0052,2448.18;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-435.5231,639.1064;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;122;648.8196,1396.468;Float;False;Constant;_Int8;Int 8;26;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-405.3546,1011.07;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;176;-550.9572,2661.705;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;-405.6832,1630.142;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-439.6823,85.71428;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-2.25935,1540.457;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-393.2375,2436.477;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-1.098644,1066.766;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;114;576.3923,1047.618;Float;False;Constant;_Int6;Int 6;24;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;4.276234,1433.08;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;121;834.0652,1300.985;Float;True;Property;_TextureArray7;Texture Array 7;25;0;Create;True;0;0;False;0;None;0;Instance;112;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;167;-374.6357,2655.459;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-381.3027,2547.661;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;185;1633.853,2439.751;Float;False;Property;_Snow_RoughnessMult;Snow_RoughnessMult;25;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;133;1820.067,1892.017;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;183;1773.981,2613.122;Float;False;Property;_Snow_AOLift;Snow_AOLift;26;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;164;-5.511901,1653.069;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;111;539.8334,703.2272;Float;True;Property;_Snow_Array;Snow_Array;22;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-397.541,1911.392;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;1976.127,2016.39;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;316.5697,2009.312;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureArrayNode;112;844.343,898.7892;Float;True;Property;_Snow_Sample;Snow_Sample;32;0;Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;80;308.401,2509.358;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;166;302.4845,2619.695;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;184;2016.041,2428.364;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;316.7046,2363.841;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;182;2083.899,2578.228;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;1700.144,2254.96;Float;False;Constant;_Snow_Metallic;Snow_Metallic;24;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;135;2219.217,1951.066;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;168;2232.635,2500.276;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;136;2238.941,2246.906;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;137;2240.179,2368.15;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2637.395,2161.503;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;Yeti/SH_BuildingSnow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;144;0;145;0
WireConnection;144;1;36;0
WireConnection;58;0;73;0
WireConnection;58;1;74;0
WireConnection;143;0;142;0
WireConnection;143;1;10;0
WireConnection;24;0;100;1
WireConnection;24;1;143;0
WireConnection;49;0;101;2
WireConnection;49;1;144;0
WireConnection;127;0;125;2
WireConnection;127;1;126;0
WireConnection;147;0;146;0
WireConnection;147;1;58;0
WireConnection;54;0;24;0
WireConnection;54;1;49;0
WireConnection;116;0;118;0
WireConnection;116;1;119;0
WireConnection;65;0;13;3
WireConnection;65;1;147;0
WireConnection;128;0;127;0
WireConnection;124;0;14;0
WireConnection;78;0;54;0
WireConnection;78;1;65;0
WireConnection;129;0;124;4
WireConnection;129;1;128;0
WireConnection;148;0;149;0
WireConnection;148;1;116;0
WireConnection;95;0;36;0
WireConnection;95;1;98;0
WireConnection;83;0;10;0
WireConnection;83;1;88;0
WireConnection;93;6;92;0
WireConnection;93;0;36;0
WireConnection;93;1;96;0
WireConnection;81;6;89;0
WireConnection;81;0;10;0
WireConnection;81;1;86;0
WireConnection;178;0;95;3
WireConnection;178;1;172;0
WireConnection;180;0;83;3
WireConnection;180;1;169;0
WireConnection;130;0;78;0
WireConnection;130;1;148;0
WireConnection;130;2;129;0
WireConnection;151;0;20;0
WireConnection;151;1;81;0
WireConnection;104;6;103;0
WireConnection;104;0;58;0
WireConnection;104;1;105;0
WireConnection;109;0;58;0
WireConnection;109;1;106;0
WireConnection;153;0;38;0
WireConnection;153;1;93;0
WireConnection;159;0;160;0
WireConnection;159;1;95;2
WireConnection;131;0;130;0
WireConnection;152;0;151;0
WireConnection;154;0;153;0
WireConnection;177;0;109;3
WireConnection;177;1;174;0
WireConnection;179;0;178;0
WireConnection;155;0;69;0
WireConnection;155;1;104;0
WireConnection;157;0;158;0
WireConnection;157;1;83;2
WireConnection;181;0;180;0
WireConnection;50;0;101;2
WireConnection;50;1;159;0
WireConnection;156;0;155;0
WireConnection;52;0;101;4
WireConnection;52;1;95;1
WireConnection;132;0;131;2
WireConnection;132;1;126;0
WireConnection;163;0;100;1
WireConnection;163;1;181;0
WireConnection;25;0;100;1
WireConnection;25;1;157;0
WireConnection;162;0;161;0
WireConnection;162;1;109;2
WireConnection;26;0;100;1
WireConnection;26;1;83;1
WireConnection;51;0;101;2
WireConnection;51;1;154;0
WireConnection;176;0;177;0
WireConnection;165;0;101;2
WireConnection;165;1;179;0
WireConnection;15;0;100;1
WireConnection;15;1;152;0
WireConnection;56;0;26;0
WireConnection;56;1;52;0
WireConnection;66;0;13;3
WireConnection;66;1;162;0
WireConnection;53;0;15;0
WireConnection;53;1;51;0
WireConnection;55;0;25;0
WireConnection;55;1;50;0
WireConnection;121;0;116;0
WireConnection;121;1;122;0
WireConnection;167;0;13;3
WireConnection;167;1;176;0
WireConnection;68;0;13;3
WireConnection;68;1;109;1
WireConnection;133;0;132;0
WireConnection;164;0;163;0
WireConnection;164;1;165;0
WireConnection;67;0;13;3
WireConnection;67;1;156;0
WireConnection;134;0;133;0
WireConnection;134;1;124;4
WireConnection;77;0;53;0
WireConnection;77;1;67;0
WireConnection;112;6;111;0
WireConnection;112;0;116;0
WireConnection;112;1;114;0
WireConnection;80;0;56;0
WireConnection;80;1;68;0
WireConnection;166;0;164;0
WireConnection;166;1;167;0
WireConnection;184;0;121;2
WireConnection;184;1;185;0
WireConnection;79;0;55;0
WireConnection;79;1;66;0
WireConnection;182;0;121;3
WireConnection;182;1;183;0
WireConnection;135;0;77;0
WireConnection;135;1;112;0
WireConnection;135;2;134;0
WireConnection;168;0;166;0
WireConnection;168;1;182;0
WireConnection;168;2;134;0
WireConnection;136;0;80;0
WireConnection;136;1;138;0
WireConnection;136;2;134;0
WireConnection;137;0;79;0
WireConnection;137;1;184;0
WireConnection;137;2;134;0
WireConnection;0;0;135;0
WireConnection;0;1;130;0
WireConnection;0;3;136;0
WireConnection;0;4;137;0
WireConnection;0;5;168;0
ASEEND*/
//CHKSM=1A6B9E50086DAF4DEF666AFDBD88A587ACB76DFB