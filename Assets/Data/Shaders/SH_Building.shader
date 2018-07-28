// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/SH_Building"
{
	Properties
	{
		[NoScaleOffset]_Mask1("Mask1", 2D) = "white" {}
		[NoScaleOffset]_Material1_Array("Material1_Array", 2DArray) = "white" {}
		_Material1_TintColor("Material1_TintColor", Color) = (1,1,1,1)
		_Material1_TintStrength("Material1_TintStrength", Range( 0 , 1)) = 0
		_Material1_NormalScale("Material1_NormalScale", Float) = 1
		_Material1_Tiling("Material1_Tiling", Vector) = (1,1,0,0)
		_Material1_Offset("Material1_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Material2_Array("Material2_Array", 2DArray) = "white" {}
		_Material2_TintColor("Material2_TintColor", Color) = (1,1,1,1)
		_Material2_TintStrength("Material2_TintStrength", Range( 0 , 1)) = 0
		_Material2_NormalScale("Material2_NormalScale", Float) = 1
		_Material2_Tiling("Material2_Tiling", Vector) = (1,1,0,0)
		_Material2_Offset("Material2_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Material3_Array("Material3_Array", 2DArray) = "white" {}
		_Material3_TintColor("Material3_TintColor", Color) = (1,1,1,1)
		_Material3_TintStrength("Material3_TintStrength", Range( 0 , 1)) = 0
		_Material3_NormalScale("Material3_NormalScale", Float) = 1
		_Material3_Tiling("Material3_Tiling", Vector) = (1,1,0,0)
		_Material3_Offset("Material3_Offset", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.5
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Mask1;
		uniform float4 _Mask1_ST;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material1_Array );
		uniform float4 _Material1_Array_ST;
		uniform float _Material1_NormalScale;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material2_Array );
		uniform float4 _Material2_Array_ST;
		uniform float _Material2_NormalScale;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material3_Array );
		uniform float4 _Material3_Array_ST;
		uniform float _Material3_NormalScale;
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

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Mask1 = i.uv_texcoord * _Mask1_ST.xy + _Mask1_ST.zw;
			float4 tex2DNode100 = tex2D( _Mask1, uv_Mask1 );
			float2 uv_Material1_Array = i.uv_texcoord * _Material1_Array_ST.xy + _Material1_Array_ST.zw;
			float3 texArray82 = UnpackScaleNormal( UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_Material1_Array, (float)1)  ) ,_Material1_NormalScale );
			float4 tex2DNode101 = tex2D( _Mask1, uv_Mask1 );
			float2 uv_Material2_Array = i.uv_texcoord * _Material2_Array_ST.xy + _Material2_Array_ST.zw;
			float3 texArray94 = UnpackScaleNormal( UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_Material2_Array, (float)1)  ) ,_Material2_NormalScale );
			float2 uv_Mask113 = i.uv_texcoord;
			float4 tex2DNode13 = tex2D( _Mask1, uv_Mask113 );
			float2 uv_Material3_Array = i.uv_texcoord * _Material3_Array_ST.xy + _Material3_Array_ST.zw;
			float3 texArray108 = UnpackScaleNormal( UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_Material3_Array, (float)1)  ) ,_Material3_NormalScale );
			o.Normal = ( ( ( tex2DNode100.r * texArray82 ) + ( tex2DNode101.g * texArray94 ) ) + ( tex2DNode13.b * texArray108 ) );
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
			o.Albedo = ( ( ( tex2DNode100.r * lerpResult31 ) + ( tex2DNode101.g * lerpResult46 ) ) + ( tex2DNode13.b * lerpResult62 ) ).rgb;
			float4 texArray83 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_Material1_Array, (float)2)  );
			float4 texArray95 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_Material2_Array, (float)2)  );
			float4 texArray109 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_Material3_Array, (float)2)  );
			o.Metallic = ( ( ( tex2DNode100.r * texArray83.r ) + ( tex2DNode101.g * texArray95.r ) ) + ( tex2DNode13.b * texArray109.x ) );
			o.Smoothness = ( ( ( tex2DNode100.r * texArray83.g ) + ( tex2DNode101.g * texArray95.g ) ) + ( tex2DNode13.b * texArray109.y ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1782.4;96;970;725;3247.916;261.0606;3.179848;True;False
Node;AmplifyShaderEditor.CommentaryNode;28;-2245.847,-280.2991;Float;False;1717.034;1009.174;Material1;20;15;26;24;25;88;83;87;81;82;86;31;32;30;20;10;12;11;89;90;100;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;33;-2231.759,771.0452;Float;False;1649.352;965.5948;Material2;20;99;94;93;95;98;96;97;52;50;49;51;92;36;34;35;46;40;42;38;101;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;34;-2152.795,1149.737;Float;False;Property;_Material2_Offset;Material2_Offset;12;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;12;-2160.535,110.3019;Float;False;Property;_Material1_Offset;Material1_Offset;6;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;35;-2152.795,1023.241;Float;False;Property;_Material2_Tiling;Material2_Tiling;11;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;11;-2160.535,-16.19443;Float;False;Property;_Material1_Tiling;Material1_Tiling;5;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;57;-2234.244,1804.855;Float;False;1669.569;985.8406;Material3;21;67;65;66;13;108;14;104;109;110;105;106;107;58;73;74;103;68;62;70;60;69;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;92;-1875.237,843.366;Float;True;Property;_Material2_Array;Material2_Array;7;1;[NoScaleOffset];Create;True;0;0;False;0;9e68fd9253d6b5f49a538eb963199c94;None;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1888.483,2.867096;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1880.742,1042.303;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;74;-2199.645,2214.047;Float;False;Property;_Material3_Offset;Material3_Offset;18;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;89;-1896.294,-223.8564;Float;True;Property;_Material1_Array;Material1_Array;1;1;[NoScaleOffset];Create;True;0;0;False;0;c90d7a96c0a623441b73da001e669a4e;None;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.IntNode;96;-1704.945,1447.533;Float;False;Constant;_Int0;Int 0;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;86;-1792,448;Float;False;Constant;_AlbedoIndex;AlbedoIndex;26;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.Vector2Node;73;-2199.645,2087.551;Float;False;Property;_Material3_Tiling;Material3_Tiling;17;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureArrayNode;93;-1500.538,1104.452;Float;True;Property;_TextureArray0;Texture Array 0;22;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-1927.59,2106.613;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;81;-1536,32;Float;True;Property;_Material1_Textures;Material1_Textures;19;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;38;-1424,825.5783;Float;False;Property;_Material2_TintColor;Material2_TintColor;8;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;105;-1739.91,2507.985;Float;False;Constant;_Int3;Int 3;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.ColorNode;20;-1408.458,-225.7663;Float;False;Property;_Material1_TintColor;Material1_TintColor;2;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;103;-2202.749,1858.871;Float;True;Property;_Material3_Array;Material3_Array;13;1;[NoScaleOffset];Create;True;0;0;False;0;126ad50cba129524e82f6ec7fe0fa046;None;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureArrayNode;104;-1528.721,2121.392;Float;True;Property;_TextureArray3;Texture Array 3;20;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;30;-1111.242,-222.4394;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;87;-1792,544;Float;False;Constant;_NormalIndex;NormalIndex;26;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;88;-1792,640;Float;False;Constant;_MRIndex;MRIndex;26;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.BlendOpsNode;40;-1126.784,828.9053;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-2152.811,1277.27;Float;False;Property;_Material2_NormalScale;Material2_NormalScale;10;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-1919.887,311.7015;Float;False;Property;_Material1_NormalScale;Material1_NormalScale;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;97;-1692.538,1541.778;Float;False;Constant;_Int1;Int 1;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;98;-1692.538,1632.452;Float;False;Constant;_Int2;Int 2;21;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1450.204,999.1385;Float;False;Property;_Material2_TintStrength;Material2_TintStrength;9;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;69;-1426.484,1859.388;Float;False;Property;_Material3_TintColor;Material3_TintColor;14;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;-1434.662,-52.20638;Float;False;Property;_Material1_TintStrength;Material1_TintStrength;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;14;-2180.339,2477.948;Float;True;Property;_Mask1;Mask1;0;1;[NoScaleOffset];Create;True;0;0;False;0;e151bab8f799fe541b2218a36b0220d6;e151bab8f799fe541b2218a36b0220d6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureArrayNode;82;-1536,256;Float;True;Property;_Material1_Normal;Material1_Normal;20;0;Create;True;0;0;False;0;None;0;Instance;81;Auto;True;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;94;-1500.538,1312.452;Float;True;Property;_TextureArray1;Texture Array 1;23;0;Create;True;0;0;False;0;None;0;Instance;93;Auto;True;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;46;-971.5543,1102.964;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;100;-1233.174,330.4507;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;13;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;60;-1129.268,1862.715;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;101;-1193.707,1376.285;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;13;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;110;-2196.624,2351.582;Float;False;Property;_Material3_NormalScale;Material3_NormalScale;16;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;31;-956.012,51.61835;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;106;-1739.91,2678.164;Float;False;Constant;_Int4;Int 4;21;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;107;-1742.658,2593.315;Float;False;Constant;_Int5;Int 5;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-1452.688,2032.948;Float;False;Property;_Material3_TintStrength;Material3_TintStrength;15;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;83;-1536,512;Float;True;Property;_Material1_MR;Material1_MR;21;0;Create;True;0;0;False;0;None;0;Instance;81;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;95;-1500.538,1520.452;Float;True;Property;_TextureArray2;Texture Array 2;23;0;Create;True;0;0;False;0;None;0;Instance;93;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-1239.401,2398.132;Float;True;Property;_MaskSample;MaskSample;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-768,32;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-774.46,1089.095;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-768,240;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureArrayNode;108;-1533.715,2314.765;Float;True;Property;_TextureArray4;Texture Array 4;23;0;Create;True;0;0;False;0;None;0;Instance;104;Auto;True;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-775.2091,1286.104;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;62;-974.0378,2136.772;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureArrayNode;109;-1530.736,2558.839;Float;True;Property;_TextureArray5;Texture Array 5;23;0;Create;True;0;0;False;0;None;0;Instance;104;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-768,544;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-768,1536;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-768,432;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-768,1424;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-713.8365,2625.083;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-162.3222,1715.839;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-711.6404,2176.024;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-194.5857,1502.899;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-165.5486,1915.873;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-712.3896,2296.452;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-210.7163,1289.96;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-714.6615,2513.899;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;213.6794,2524.978;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;223.273,2167.729;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;218.6739,2356.297;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;219.4639,1970.094;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;552.1943,2153.276;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;Yeti/SH_Building;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;93;6;92;0
WireConnection;93;0;36;0
WireConnection;93;1;96;0
WireConnection;58;0;73;0
WireConnection;58;1;74;0
WireConnection;81;6;89;0
WireConnection;81;0;10;0
WireConnection;81;1;86;0
WireConnection;104;6;103;0
WireConnection;104;0;58;0
WireConnection;104;1;105;0
WireConnection;30;0;20;0
WireConnection;30;1;81;0
WireConnection;40;0;38;0
WireConnection;40;1;93;0
WireConnection;82;1;87;0
WireConnection;82;3;90;0
WireConnection;94;1;97;0
WireConnection;94;3;99;0
WireConnection;46;0;93;0
WireConnection;46;1;40;0
WireConnection;46;2;42;0
WireConnection;60;0;69;0
WireConnection;60;1;104;0
WireConnection;31;0;81;0
WireConnection;31;1;30;0
WireConnection;31;2;32;0
WireConnection;83;1;88;0
WireConnection;95;1;98;0
WireConnection;13;0;14;0
WireConnection;15;0;100;1
WireConnection;15;1;31;0
WireConnection;51;0;101;2
WireConnection;51;1;46;0
WireConnection;24;0;100;1
WireConnection;24;1;82;0
WireConnection;108;1;107;0
WireConnection;108;3;110;0
WireConnection;49;0;101;2
WireConnection;49;1;94;0
WireConnection;62;0;104;0
WireConnection;62;1;60;0
WireConnection;62;2;70;0
WireConnection;109;1;106;0
WireConnection;26;0;100;1
WireConnection;26;1;83;1
WireConnection;52;0;101;2
WireConnection;52;1;95;1
WireConnection;25;0;100;1
WireConnection;25;1;83;2
WireConnection;50;0;101;2
WireConnection;50;1;95;2
WireConnection;68;0;13;3
WireConnection;68;1;109;1
WireConnection;55;0;25;0
WireConnection;55;1;50;0
WireConnection;67;0;13;3
WireConnection;67;1;62;0
WireConnection;54;0;24;0
WireConnection;54;1;49;0
WireConnection;56;0;26;0
WireConnection;56;1;52;0
WireConnection;65;0;13;3
WireConnection;65;1;108;0
WireConnection;53;0;15;0
WireConnection;53;1;51;0
WireConnection;66;0;13;3
WireConnection;66;1;109;2
WireConnection;80;0;56;0
WireConnection;80;1;68;0
WireConnection;78;0;54;0
WireConnection;78;1;65;0
WireConnection;79;0;55;0
WireConnection;79;1;66;0
WireConnection;77;0;53;0
WireConnection;77;1;67;0
WireConnection;0;0;77;0
WireConnection;0;1;78;0
WireConnection;0;3;80;0
WireConnection;0;4;79;0
ASEEND*/
//CHKSM=1A9D2504C035D90EAA86D1AD4312A9B14671BC5E