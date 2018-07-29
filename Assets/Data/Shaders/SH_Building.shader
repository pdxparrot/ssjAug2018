// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/SH_Building"
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
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.5
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
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
		uniform float4 _Material1_TintColor;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material1_Array );
		uniform float4 _Material2_TintColor;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material2_Array );
		uniform float4 _Material3_TintColor;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material3_Array );

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 tex2DNode100 = tex2D( _Mask, uv_Mask );
			float2 uv_TexCoord10 = i.uv_texcoord * _Material1_Tiling + _Material1_Offset;
			float4 tex2DNode101 = tex2D( _Mask, uv_Mask );
			float2 uv_TexCoord36 = i.uv_texcoord * _Material2_Tiling + _Material2_Offset;
			float2 uv_Mask13 = i.uv_texcoord;
			float4 tex2DNode13 = tex2D( _Mask, uv_Mask13 );
			float2 uv_TexCoord58 = i.uv_texcoord * _Material3_Tiling + _Material3_Offset;
			o.Normal = ( ( ( tex2DNode100.r * tex2D( _Material1_Normal, uv_TexCoord10 ) ) + ( tex2DNode101.g * tex2D( _Material2_Normal, uv_TexCoord36 ) ) ) + ( tex2DNode13.b * tex2D( _Material3_Normal, uv_TexCoord58 ) ) ).rgb;
			float4 texArray81 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_TexCoord10, (float)0)  );
			float4 texArray93 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_TexCoord36, (float)0)  );
			float4 texArray104 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_TexCoord58, (float)0)  );
			o.Albedo = ( ( ( tex2DNode100.r * saturate( ( _Material1_TintColor + texArray81 ) ) ) + ( tex2DNode101.g * saturate( ( _Material2_TintColor + texArray93 ) ) ) ) + ( tex2DNode13.b * saturate( ( _Material3_TintColor + texArray104 ) ) ) ).rgb;
			float4 texArray83 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(uv_TexCoord10, (float)1)  );
			float4 texArray95 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(uv_TexCoord36, (float)1)  );
			float4 texArray109 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(uv_TexCoord58, (float)1)  );
			o.Metallic = ( ( ( tex2DNode100.r * texArray83.r ) + ( tex2DNode101.g * texArray95.r ) ) + ( tex2DNode13.b * texArray109.x ) );
			o.Smoothness = ( 1.0 - ( ( ( tex2DNode100.r * texArray83.g ) + ( tex2DNode101.g * texArray95.g ) ) + ( tex2DNode13.b * texArray109.y ) ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1590.4;96;1162;725;831.9528;-1666.329;1.311135;True;False
Node;AmplifyShaderEditor.CommentaryNode;33;-2091.402,894.226;Float;False;1470.136;885.9641;Material2;18;35;34;92;36;98;113;96;38;51;120;119;52;49;50;114;95;101;93;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;28;-2090.151,42.84201;Float;False;1465.348;835.3372;Material1;18;24;15;26;25;117;118;89;86;10;12;11;111;83;88;20;81;112;100;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;35;-2042.299,1178.771;Float;False;Property;_Material2_Tiling;Material2_Tiling;9;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;11;-2040.171,346.4831;Float;False;Property;_Material1_Tiling;Material1_Tiling;4;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;12;-2040.171,472.9795;Float;False;Property;_Material1_Offset;Material1_Offset;5;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;34;-2042.299,1305.267;Float;False;Property;_Material2_Offset;Material2_Offset;10;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;57;-2094.003,1804.855;Float;False;1471.308;892.8647;Material3;19;67;66;65;68;122;121;13;69;104;115;109;106;14;103;58;74;73;105;116;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;89;-1767.569,168.0863;Float;True;Property;_Material1_Array;Material1_Array;1;1;[NoScaleOffset];Create;True;0;0;False;0;c90d7a96c0a623441b73da001e669a4e;c90d7a96c0a623441b73da001e669a4e;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.IntNode;86;-1729.7,484.5027;Float;False;Constant;_AlbedoIndex;AlbedoIndex;26;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1768.12,365.5446;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;92;-1831.929,961.5697;Float;True;Property;_Material2_Array;Material2_Array;6;1;[NoScaleOffset];Create;True;0;0;False;0;9e68fd9253d6b5f49a538eb963199c94;9e68fd9253d6b5f49a538eb963199c94;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.IntNode;96;-1794.429,1293.026;Float;False;Constant;_Int0;Int 0;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1780.201,1162.995;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;74;-2048.766,2192.619;Float;False;Property;_Material3_Offset;Material3_Offset;15;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;73;-2048.766,2066.123;Float;False;Property;_Material3_Tiling;Material3_Tiling;14;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.IntNode;88;-1696.22,756.0168;Float;False;Constant;_MRIndex;MRIndex;26;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;105;-1706.558,2220.114;Float;False;Constant;_Int3;Int 3;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-1776.711,2085.185;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;103;-1800.227,1872.688;Float;True;Property;_Material3_Array;Material3_Array;11;1;[NoScaleOffset];Create;True;0;0;False;0;126ad50cba129524e82f6ec7fe0fa046;126ad50cba129524e82f6ec7fe0fa046;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureArrayNode;93;-1497.046,1135.56;Float;True;Property;_Material2_Sample;Material2_Sample;17;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;38;-1403.862,950.508;Float;False;Property;_Material2_TintColor;Material2_TintColor;8;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;20;-1351.615,90.43407;Float;False;Property;_Material1_TintColor;Material1_TintColor;3;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;81;-1449.083,271.3447;Float;True;Property;_Material1_Sample;Material1_Sample;16;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;98;-1783.606,1641.164;Float;False;Constant;_Int2;Int 2;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.TextureArrayNode;83;-1453.675,661.5338;Float;True;Property;_Material1_MR;Material1_MR;21;0;Create;True;0;0;False;0;None;0;Instance;81;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;14;-2063.299,2490.708;Float;True;Property;_Mask;Mask;0;1;[NoScaleOffset];Create;True;0;0;False;0;00e81767994d62f4ba5a3e48f0e93787;e151bab8f799fe541b2218a36b0220d6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.IntNode;106;-1759.771,2614.008;Float;False;Constant;_Int4;Int 4;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.SamplerNode;100;-1132.891,533.038;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;13;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;95;-1497.046,1551.56;Float;True;Property;_TextureArray2;Texture Array 2;23;0;Create;True;0;0;False;0;None;0;Instance;93;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;101;-1190.215,1407.393;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;13;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;113;-1791.856,1382.178;Float;True;Property;_Material2_Normal;Material2_Normal;7;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;119;-1098.41,1117.715;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;69;-1433.34,1894.34;Float;False;Property;_Material3_TintColor;Material3_TintColor;13;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;118;-1096.241,255.1392;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureArrayNode;104;-1524.538,2065.825;Float;True;Property;_Material3_Sample;Material3_Sample;18;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;111;-1773.973,562.1171;Float;True;Property;_Material1_Normal;Material1_Normal;2;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;116;-1791.442,2306.107;Float;True;Property;_Material3_Normal;Material3_Normal;12;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;114;-1500.795,1336.721;Float;True;Property;_TextureSample3;Texture Sample 3;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;121;-1151.779,2053.904;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;117;-968.8907,256.6352;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;112;-1445.01,465.6404;Float;True;Property;_TextureSample2;Texture Sample 2;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;120;-956.2825,1119.211;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureArrayNode;109;-1509.926,2493.574;Float;True;Property;_TextureArray5;Texture Array 5;23;0;Create;True;0;0;False;0;None;0;Instance;104;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-1165.432,2360.13;Float;True;Property;_MaskSample;MaskSample;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-783.9514,621.2926;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-764.5082,1455.108;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-800.7288,2457.795;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-473.6226,1461.394;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;115;-1529.008,2270.749;Float;True;Property;_TextureSample4;Texture Sample 4;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;122;-1009.651,2055.4;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-769.455,1095.99;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-783.9514,733.2927;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-783.9514,241.8569;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-792.7642,449.8569;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-771.7173,1317.212;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-764.5082,1567.108;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;-154.6636,2424.751;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-799.9037,2568.979;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-809.9223,2034.416;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-480.308,1091.127;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-464.1774,1309.627;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-487.9713,1594.693;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-798.4568,2240.348;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;123;-4.626838,2345.494;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;-153.1699,2535.038;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-143.7336,2003.89;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;-147.2281,2212.481;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;198.7791,2196.979;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;Yeti/SH_Building;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;58;0;73;0
WireConnection;58;1;74;0
WireConnection;93;6;92;0
WireConnection;93;0;36;0
WireConnection;93;1;96;0
WireConnection;81;6;89;0
WireConnection;81;0;10;0
WireConnection;81;1;86;0
WireConnection;83;0;10;0
WireConnection;83;1;88;0
WireConnection;95;0;36;0
WireConnection;95;1;98;0
WireConnection;119;0;38;0
WireConnection;119;1;93;0
WireConnection;118;0;20;0
WireConnection;118;1;81;0
WireConnection;104;6;103;0
WireConnection;104;0;58;0
WireConnection;104;1;105;0
WireConnection;114;0;113;0
WireConnection;114;1;36;0
WireConnection;121;0;69;0
WireConnection;121;1;104;0
WireConnection;117;0;118;0
WireConnection;112;0;111;0
WireConnection;112;1;10;0
WireConnection;120;0;119;0
WireConnection;109;0;58;0
WireConnection;109;1;106;0
WireConnection;13;0;14;0
WireConnection;25;0;100;1
WireConnection;25;1;83;2
WireConnection;50;0;101;2
WireConnection;50;1;95;2
WireConnection;66;0;13;3
WireConnection;66;1;109;2
WireConnection;55;0;25;0
WireConnection;55;1;50;0
WireConnection;115;0;116;0
WireConnection;115;1;58;0
WireConnection;122;0;121;0
WireConnection;51;0;101;2
WireConnection;51;1;120;0
WireConnection;26;0;100;1
WireConnection;26;1;83;1
WireConnection;15;0;100;1
WireConnection;15;1;117;0
WireConnection;24;0;100;1
WireConnection;24;1;112;0
WireConnection;49;0;101;2
WireConnection;49;1;114;0
WireConnection;52;0;101;2
WireConnection;52;1;95;1
WireConnection;79;0;55;0
WireConnection;79;1;66;0
WireConnection;68;0;13;3
WireConnection;68;1;109;1
WireConnection;67;0;13;3
WireConnection;67;1;122;0
WireConnection;53;0;15;0
WireConnection;53;1;51;0
WireConnection;54;0;24;0
WireConnection;54;1;49;0
WireConnection;56;0;26;0
WireConnection;56;1;52;0
WireConnection;65;0;13;3
WireConnection;65;1;115;0
WireConnection;123;0;79;0
WireConnection;80;0;56;0
WireConnection;80;1;68;0
WireConnection;77;0;53;0
WireConnection;77;1;67;0
WireConnection;78;0;54;0
WireConnection;78;1;65;0
WireConnection;0;0;77;0
WireConnection;0;1;78;0
WireConnection;0;3;80;0
WireConnection;0;4;123;0
ASEEND*/
//CHKSM=E2823A1BADFB3404EA884D265208770835A633C9