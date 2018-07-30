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
		_Material1_RoughnessMult("Material1_RoughnessMult", Float) = 1
		_Material1_AOMult("Material1_AOMult", Float) = 0
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
		uniform float _Material1_RoughnessMult;
		uniform float _Material2_RoughnessMult;
		uniform float _Material3_RoughnessMult;
		uniform float _Material1_AOMult;
		uniform float _Material2_AOLift;
		uniform float _Material3_AOLift;

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
			o.Smoothness = ( ( ( tex2DNode100.r * ( _Material1_RoughnessMult * texArray83.g ) ) + ( tex2DNode101.g * ( _Material2_RoughnessMult * texArray95.g ) ) ) + ( tex2DNode13.b * ( _Material3_RoughnessMult * texArray109.y ) ) );
			o.Occlusion = ( ( ( tex2DNode100.r * saturate( ( texArray83.b + _Material1_AOMult ) ) ) + ( tex2DNode101.g * saturate( ( texArray95.b + _Material2_AOLift ) ) ) ) + ( tex2DNode13.b * saturate( ( texArray109.z + _Material3_AOLift ) ) ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
2752.8;311.2;1534;840;3159.579;-1085.407;2.257106;True;False
Node;AmplifyShaderEditor.CommentaryNode;33;-2132.81,801.6699;Float;False;1478.136;946.9641;Material2;24;140;134;50;49;51;52;101;127;114;120;126;95;119;113;98;93;38;36;96;92;34;35;145;146;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;28;-2113.206,-223.0729;Float;False;1465.812;933.0244;Material1;24;132;124;125;100;15;117;118;24;26;25;112;111;83;81;20;88;10;86;89;12;11;138;147;148;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;57;-2094.003,1804.855;Float;False;1476.257;975.3466;Material3;25;128;129;14;103;13;67;122;121;65;68;115;66;109;116;104;69;106;58;105;73;74;136;142;144;143;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;12;-2060.626,207.0644;Float;False;Property;_Material1_Offset;Material1_Offset;7;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;35;-2083.707,1086.215;Float;False;Property;_Material2_Tiling;Material2_Tiling;13;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;34;-2083.707,1212.711;Float;False;Property;_Material2_Offset;Material2_Offset;14;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;11;-2060.626,80.56816;Float;False;Property;_Material1_Tiling;Material1_Tiling;6;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.IntNode;96;-1835.836,1200.47;Float;False;Constant;_Int0;Int 0;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.Vector2Node;74;-2048.766,2192.619;Float;False;Property;_Material3_Offset;Material3_Offset;21;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;73;-2048.766,2066.123;Float;False;Property;_Material3_Tiling;Material3_Tiling;20;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1821.608,1070.439;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1788.575,99.62965;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;89;-1788.024,-97.82858;Float;True;Property;_Material1_Array;Material1_Array;1;1;[NoScaleOffset];Create;True;0;0;False;0;c90d7a96c0a623441b73da001e669a4e;c90d7a96c0a623441b73da001e669a4e;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;92;-1873.336,869.0136;Float;True;Property;_Material2_Array;Material2_Array;8;1;[NoScaleOffset];Create;True;0;0;False;0;9e68fd9253d6b5f49a538eb963199c94;9e68fd9253d6b5f49a538eb963199c94;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.IntNode;98;-1825.013,1548.608;Float;False;Constant;_Int2;Int 2;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;88;-1716.675,490.1015;Float;False;Constant;_MRIndex;MRIndex;26;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;86;-1750.155,218.5876;Float;False;Constant;_AlbedoIndex;AlbedoIndex;26;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TextureArrayNode;93;-1538.453,1043.004;Float;True;Property;_Material2_Sample;Material2_Sample;23;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-1776.711,2085.185;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;81;-1475.748,-3.855036;Float;True;Property;_Material1_Sample;Material1_Sample;22;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;105;-1706.558,2220.114;Float;False;Constant;_Int3;Int 3;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.ColorNode;20;-1372.07,-175.4808;Float;False;Property;_Material1_TintColor;Material1_TintColor;3;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;38;-1445.269,857.9519;Float;False;Property;_Material2_TintColor;Material2_TintColor;10;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;138;-1361.182,619.4901;Float;False;Property;_Material1_AOMult;Material1_AOMult;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;83;-1474.13,395.6185;Float;True;Property;_Material1_MR;Material1_MR;21;0;Create;True;0;0;False;0;None;0;Instance;81;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;95;-1538.453,1459.004;Float;True;Property;_TextureArray2;Texture Array 2;23;0;Create;True;0;0;False;0;None;0;Instance;93;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;106;-1759.771,2614.008;Float;False;Constant;_Int4;Int 4;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;140;-1440.447,1655.25;Float;False;Property;_Material2_AOLift;Material2_AOLift;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;103;-1776,1862.305;Float;True;Property;_Material3_Array;Material3_Array;15;1;[NoScaleOffset];Create;True;0;0;False;0;126ad50cba129524e82f6ec7fe0fa046;126ad50cba129524e82f6ec7fe0fa046;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;113;-1833.263,1289.622;Float;True;Property;_Material2_Normal;Material2_Normal;9;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;118;-1114.966,-99.02933;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-1256.946,1316.965;Float;False;Property;_Material2_RoughnessMult;Material2_RoughnessMult;11;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;104;-1524.538,2065.825;Float;True;Property;_Material3_Sample;Material3_Sample;24;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;119;-1145.008,909.2177;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;111;-1794.428,296.2019;Float;True;Property;_Material1_Normal;Material1_Normal;2;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureArrayNode;109;-1509.926,2493.574;Float;True;Property;_TextureArray5;Texture Array 5;23;0;Create;True;0;0;False;0;None;0;Instance;104;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;142;-1411.002,2697.662;Float;False;Property;_Material3_AOLift;Material3_AOLift;19;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;-1212.373,1590.744;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;147;-1128.093,537.9727;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-1177.158,280.1043;Float;False;Property;_Material1_RoughnessMult;Material1_RoughnessMult;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;69;-1433.34,1894.34;Float;False;Property;_Material3_TintColor;Material3_TintColor;17;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;100;-1136.041,45.62352;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;13;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;128;-1242.356,2345.426;Float;False;Property;_Material3_RoughnessMult;Material3_RoughnessMult;18;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-986.9929,1401.758;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;148;-982.3904,552.2569;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;114;-1542.202,1244.165;Float;True;Property;_TextureSample3;Texture Sample 3;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;120;-1002.88,910.7137;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;101;-1183.168,1056.997;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;13;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;121;-1153.51,1922.389;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;117;-987.6153,-97.53334;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;116;-1791.442,2306.107;Float;True;Property;_Material3_Normal;Material3_Normal;16;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;112;-1465.465,199.7253;Float;True;Property;_TextureSample2;Texture Sample 2;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;14;-2075.412,1867.74;Float;True;Property;_Mask;Mask;0;1;[NoScaleOffset];Create;True;0;0;False;0;00e81767994d62f4ba5a3e48f0e93787;e151bab8f799fe541b2218a36b0220d6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;143;-1148.625,2648.684;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;146;-1028.102,1592.172;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;-959.1197,370.0886;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-813.1238,1224.656;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-804.4065,467.3775;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;122;-1011.381,1923.884;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;-806.8091,590.9254;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;115;-1529.008,2270.749;Float;True;Property;_TextureSample4;Texture Sample 4;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-805.9147,1474.552;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-816.0529,887.4927;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-802.676,-112.3116;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-809.448,1579.11;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-804.4065,355.3773;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-805.9147,1362.552;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-1158.51,2062.489;Float;True;Property;_MaskSample;MaskSample;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-978.9614,2458.765;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;144;-964.2589,2661.668;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-813.2193,183.9419;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-487.9713,1594.693;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-795.7921,2673.697;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-800.7288,2457.795;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-798.4568,2240.348;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-799.9037,2568.979;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-464.1774,1309.627;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;133;-495.0401,1715.635;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-473.6226,1461.394;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-480.308,1091.127;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-811.6528,1902.901;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;-147.2281,2212.481;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;-161.4126,2352.761;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;-162.1685,2460.799;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;135;-161.3254,2556.745;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-143.7336,2003.89;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;198.7791,2196.979;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;Yeti/SH_Building;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;93;6;92;0
WireConnection;93;0;36;0
WireConnection;93;1;96;0
WireConnection;58;0;73;0
WireConnection;58;1;74;0
WireConnection;81;6;89;0
WireConnection;81;0;10;0
WireConnection;81;1;86;0
WireConnection;83;0;10;0
WireConnection;83;1;88;0
WireConnection;95;0;36;0
WireConnection;95;1;98;0
WireConnection;118;0;20;0
WireConnection;118;1;81;0
WireConnection;104;6;103;0
WireConnection;104;0;58;0
WireConnection;104;1;105;0
WireConnection;119;0;38;0
WireConnection;119;1;93;0
WireConnection;109;0;58;0
WireConnection;109;1;106;0
WireConnection;145;0;95;3
WireConnection;145;1;140;0
WireConnection;147;0;83;3
WireConnection;147;1;138;0
WireConnection;127;0;126;0
WireConnection;127;1;95;2
WireConnection;148;0;147;0
WireConnection;114;0;113;0
WireConnection;114;1;36;0
WireConnection;120;0;119;0
WireConnection;121;0;69;0
WireConnection;121;1;104;0
WireConnection;117;0;118;0
WireConnection;112;0;111;0
WireConnection;112;1;10;0
WireConnection;143;0;109;3
WireConnection;143;1;142;0
WireConnection;146;0;145;0
WireConnection;125;0;124;0
WireConnection;125;1;83;2
WireConnection;49;0;101;2
WireConnection;49;1;114;0
WireConnection;26;0;100;1
WireConnection;26;1;83;1
WireConnection;122;0;121;0
WireConnection;132;0;100;1
WireConnection;132;1;148;0
WireConnection;115;0;116;0
WireConnection;115;1;58;0
WireConnection;52;0;101;2
WireConnection;52;1;95;1
WireConnection;51;0;101;2
WireConnection;51;1;120;0
WireConnection;15;0;100;1
WireConnection;15;1;117;0
WireConnection;134;0;101;2
WireConnection;134;1;146;0
WireConnection;25;0;100;1
WireConnection;25;1;125;0
WireConnection;50;0;101;2
WireConnection;50;1;127;0
WireConnection;13;0;14;0
WireConnection;129;0;128;0
WireConnection;129;1;109;2
WireConnection;144;0;143;0
WireConnection;24;0;100;1
WireConnection;24;1;112;0
WireConnection;56;0;26;0
WireConnection;56;1;52;0
WireConnection;136;0;13;3
WireConnection;136;1;144;0
WireConnection;66;0;13;3
WireConnection;66;1;129;0
WireConnection;65;0;13;3
WireConnection;65;1;115;0
WireConnection;68;0;13;3
WireConnection;68;1;109;1
WireConnection;54;0;24;0
WireConnection;54;1;49;0
WireConnection;133;0;132;0
WireConnection;133;1;134;0
WireConnection;55;0;25;0
WireConnection;55;1;50;0
WireConnection;53;0;15;0
WireConnection;53;1;51;0
WireConnection;67;0;13;3
WireConnection;67;1;122;0
WireConnection;78;0;54;0
WireConnection;78;1;65;0
WireConnection;79;0;55;0
WireConnection;79;1;66;0
WireConnection;80;0;56;0
WireConnection;80;1;68;0
WireConnection;135;0;133;0
WireConnection;135;1;136;0
WireConnection;77;0;53;0
WireConnection;77;1;67;0
WireConnection;0;0;77;0
WireConnection;0;1;78;0
WireConnection;0;3;80;0
WireConnection;0;4;79;0
WireConnection;0;5;135;0
ASEEND*/
//CHKSM=87448AFBBFF82BA6F2B741FA2DA024A1E0E5A2A3