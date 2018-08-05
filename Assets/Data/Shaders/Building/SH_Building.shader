// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/SH_Building"
{
	Properties
	{
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		[NoScaleOffset]_Material1_Array("Material1_Array", 2DArray) = "white" {}
		[NoScaleOffset][Normal]_Material1_Normal("Material1_Normal", 2D) = "white" {}
		_Material1_TintColor("Material1_TintColor", Color) = (1,0,0,1)
		_Material1_RoughnessMult("Material1_RoughnessMult", Float) = 1
		_Material1_AOLift("Material1_AOLift", Float) = 0
		_Material1_Tiling("Material1_Tiling", Vector) = (1,1,0,0)
		_Material1_Offset("Material1_Offset", Vector) = (0,0,0,0)
		_Material1_Rotation("Material1_Rotation", Range( -360 , 360)) = 0
		[NoScaleOffset]_Material2_Array("Material2_Array", 2DArray) = "white" {}
		[NoScaleOffset][Normal]_Material2_Normal("Material2_Normal", 2D) = "white" {}
		_Material2_TintColor("Material2_TintColor", Color) = (0,1,0,1)
		_Material2_RoughnessMult("Material2_RoughnessMult", Float) = 1
		_Material2_AOLift("Material2_AOLift", Float) = 0
		_Material2_Tiling("Material2_Tiling", Vector) = (1,1,0,0)
		_Material2_Offset("Material2_Offset", Vector) = (0,0,0,0)
		_Material2_Rotation("Material2_Rotation", Range( -360 , 360)) = 0
		[NoScaleOffset]_Material3_Array("Material3_Array", 2DArray) = "white" {}
		[NoScaleOffset][Normal]_Material3_Normal("Material3_Normal", 2D) = "white" {}
		_Material3_TintColor("Material3_TintColor", Color) = (0,0,1,1)
		_Material3_RoughnessMult("Material3_RoughnessMult", Float) = 1
		_Material3_AOLift("Material3_AOLift", Float) = 0
		_Material3_Tiling("Material3_Tiling", Vector) = (1,1,0,0)
		_Material3_Offset("Material3_Offset", Vector) = (0,0,0,0)
		_Material3_Rotation("Material3_Rotation", Range( -360 , 360)) = 0
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
		uniform float _Material1_Rotation;
		uniform sampler2D _Material2_Normal;
		uniform float2 _Material2_Tiling;
		uniform float2 _Material2_Offset;
		uniform float _Material2_Rotation;
		uniform sampler2D _Material3_Normal;
		uniform float2 _Material3_Tiling;
		uniform float2 _Material3_Offset;
		uniform float _Material3_Rotation;
		uniform float4 _Material1_TintColor;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material1_Array );
		uniform float4 _Material2_TintColor;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material2_Array );
		uniform float4 _Material3_TintColor;
		uniform UNITY_DECLARE_TEX2DARRAY( _Material3_Array );
		uniform float _Material1_RoughnessMult;
		uniform float _Material2_RoughnessMult;
		uniform float _Material3_RoughnessMult;
		uniform float _Material1_AOLift;
		uniform float _Material2_AOLift;
		uniform float _Material3_AOLift;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 tex2DNode100 = tex2D( _Mask, uv_Mask );
			float2 uv_TexCoord10 = i.uv_texcoord * _Material1_Tiling + _Material1_Offset;
			float cos149 = cos( radians( _Material1_Rotation ) );
			float sin149 = sin( radians( _Material1_Rotation ) );
			float2 rotator149 = mul( uv_TexCoord10 - float2( 0,0 ) , float2x2( cos149 , -sin149 , sin149 , cos149 )) + float2( 0,0 );
			float4 tex2DNode101 = tex2D( _Mask, uv_Mask );
			float2 uv_TexCoord36 = i.uv_texcoord * _Material2_Tiling + _Material2_Offset;
			float cos153 = cos( radians( _Material2_Rotation ) );
			float sin153 = sin( radians( _Material2_Rotation ) );
			float2 rotator153 = mul( uv_TexCoord36 - float2( 0,0 ) , float2x2( cos153 , -sin153 , sin153 , cos153 )) + float2( 0,0 );
			float2 uv_Mask13 = i.uv_texcoord;
			float4 tex2DNode13 = tex2D( _Mask, uv_Mask13 );
			float2 uv_TexCoord58 = i.uv_texcoord * _Material3_Tiling + _Material3_Offset;
			float cos154 = cos( radians( _Material3_Rotation ) );
			float sin154 = sin( radians( _Material3_Rotation ) );
			float2 rotator154 = mul( uv_TexCoord58 - float2( 0,0 ) , float2x2( cos154 , -sin154 , sin154 , cos154 )) + float2( 0,0 );
			o.Normal = ( ( ( tex2DNode100.r * UnpackNormal( tex2D( _Material1_Normal, rotator149 ) ) ) + ( tex2DNode101.g * UnpackNormal( tex2D( _Material2_Normal, rotator153 ) ) ) ) + ( tex2DNode13.b * UnpackNormal( tex2D( _Material3_Normal, rotator154 ) ) ) );
			float4 texArray81 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(rotator149, (float)0)  );
			float4 texArray93 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(rotator153, (float)0)  );
			float4 texArray104 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(rotator154, (float)0)  );
			o.Albedo = ( ( ( tex2DNode100.r * saturate( ( _Material1_TintColor + texArray81 ) ) ) + ( tex2DNode101.g * saturate( ( _Material2_TintColor + texArray93 ) ) ) ) + ( tex2DNode13.b * saturate( ( _Material3_TintColor + texArray104 ) ) ) ).rgb;
			float4 texArray83 = UNITY_SAMPLE_TEX2DARRAY(_Material1_Array, float3(rotator149, (float)1)  );
			float4 texArray95 = UNITY_SAMPLE_TEX2DARRAY(_Material2_Array, float3(rotator153, (float)1)  );
			float4 texArray109 = UNITY_SAMPLE_TEX2DARRAY(_Material3_Array, float3(rotator154, (float)1)  );
			o.Metallic = ( ( ( tex2DNode100.r * texArray83.r ) + ( tex2DNode101.g * texArray95.r ) ) + ( tex2DNode13.b * texArray109.x ) );
			o.Smoothness = ( ( ( tex2DNode100.r * ( _Material1_RoughnessMult * texArray83.g ) ) + ( tex2DNode101.g * ( _Material2_RoughnessMult * texArray95.g ) ) ) + ( tex2DNode13.b * ( _Material3_RoughnessMult * texArray109.y ) ) );
			o.Occlusion = ( ( ( tex2DNode100.r * saturate( ( texArray83.b + _Material1_AOLift ) ) ) + ( tex2DNode101.g * saturate( ( texArray95.b + _Material2_AOLift ) ) ) ) + ( tex2DNode13.b * saturate( ( texArray109.z + _Material3_AOLift ) ) ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
2752.8;311.2;1534;840;3160.878;115.8882;2.49614;True;False
Node;AmplifyShaderEditor.CommentaryNode;33;-2383.393,801.6699;Float;False;1728.719;946.9641;Material2;27;92;50;134;51;52;49;146;101;120;114;127;145;119;126;113;140;95;38;93;98;36;96;34;35;153;152;158;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;28;-2402.226,-223.0729;Float;False;1754.832;950.4352;Material1;27;111;10;11;12;149;24;25;15;132;26;125;112;117;148;100;124;147;118;83;138;20;81;86;88;89;151;157;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;152;-2314.552,1360.334;Float;False;Property;_Material2_Rotation;Material2_Rotation;16;0;Create;True;0;0;False;0;0;0;-360;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;11;-2304,112;Float;False;Property;_Material1_Tiling;Material1_Tiling;6;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;12;-2304,256;Float;False;Property;_Material1_Offset;Material1_Offset;7;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;151;-2302.776,399.0105;Float;False;Property;_Material1_Rotation;Material1_Rotation;8;0;Create;True;0;0;False;0;0;0;-360;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;35;-2313.652,1095.059;Float;False;Property;_Material2_Tiling;Material2_Tiling;14;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;57;-2377.014,1804.855;Float;False;1759.268;975.3466;Material3;28;103;67;68;65;66;136;144;129;13;115;122;143;14;116;121;128;69;142;109;104;106;105;58;73;74;154;155;159;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;34;-2313.652,1221.555;Float;False;Property;_Material2_Offset;Material2_Offset;15;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2048,96;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RadiansOpNode;158;-2048.869,1363.614;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;74;-2305.243,2195.567;Float;False;Property;_Material3_Offset;Material3_Offset;23;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RadiansOpNode;157;-2031.995,403.4703;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;155;-2313.596,2342.426;Float;False;Property;_Material3_Rotation;Material3_Rotation;24;0;Create;True;0;0;False;0;0;0;-360;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;73;-2305.243,2069.071;Float;False;Property;_Material3_Tiling;Material3_Tiling;22;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-2085.231,1080.814;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;92;-1873.336,869.0136;Float;True;Property;_Material2_Array;Material2_Array;9;1;[NoScaleOffset];Create;True;0;0;False;0;None;9e68fd9253d6b5f49a538eb963199c94;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-2033.189,2088.133;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;96;-1792.975,1205.062;Float;False;Constant;_Int0;Int 0;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;98;-1825.013,1548.608;Float;False;Constant;_Int2;Int 2;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.RadiansOpNode;159;-2040.405,2343.962;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;89;-1788.024,-97.82858;Float;True;Property;_Material1_Array;Material1_Array;1;1;[NoScaleOffset];Create;True;0;0;False;0;None;c90d7a96c0a623441b73da001e669a4e;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RotatorNode;153;-1824,1088;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.IntNode;88;-1716.675,490.1015;Float;False;Constant;_MRIndex;MRIndex;26;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.RotatorNode;149;-1772.818,97.02851;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.IntNode;86;-1750.155,218.5876;Float;False;Constant;_AlbedoIndex;AlbedoIndex;26;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.ColorNode;38;-1445.269,857.9519;Float;False;Property;_Material2_TintColor;Material2_TintColor;11;0;Create;True;0;0;False;0;0,1,0,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;83;-1474.13,395.6185;Float;True;Property;_Material1_MR;Material1_MR;21;0;Create;True;0;0;False;0;None;0;Instance;81;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureArrayNode;95;-1538.453,1459.004;Float;True;Property;_TextureArray2;Texture Array 2;23;0;Create;True;0;0;False;0;None;0;Instance;93;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;140;-1440.447,1655.25;Float;False;Property;_Material2_AOLift;Material2_AOLift;13;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;93;-1538.453,1043.004;Float;True;Property;_Material2_Sample;Material2_Sample;26;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;20;-1372.07,-175.4808;Float;False;Property;_Material1_TintColor;Material1_TintColor;3;0;Create;True;0;0;False;0;1,0,0,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;106;-1759.771,2614.008;Float;False;Constant;_Int4;Int 4;21;0;Create;True;0;0;False;0;1;0;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;-1361.182,619.4901;Float;False;Property;_Material1_AOLift;Material1_AOLift;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;105;-1733.769,2218.3;Float;False;Constant;_Int3;Int 3;21;0;Create;True;0;0;False;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.TextureArrayNode;81;-1475.748,-3.855036;Float;True;Property;_Material1_Sample;Material1_Sample;25;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;154;-1773.867,2086.589;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;103;-1776,1862.305;Float;True;Property;_Material3_Array;Material3_Array;17;1;[NoScaleOffset];Create;True;0;0;False;0;None;126ad50cba129524e82f6ec7fe0fa046;False;white;Auto;Texture2DArray;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;111;-1794.428,296.2019;Float;True;Property;_Material1_Normal;Material1_Normal;2;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;113;-1833.263,1289.622;Float;True;Property;_Material2_Normal;Material2_Normal;10;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureArrayNode;109;-1509.926,2493.574;Float;True;Property;_TextureArray5;Texture Array 5;23;0;Create;True;0;0;False;0;None;0;Instance;104;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;147;-1128.093,599.93;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;142;-1411.002,2697.662;Float;False;Property;_Material3_AOLift;Material3_AOLift;21;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;119;-1145.008,909.2177;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-1177.158,280.1043;Float;False;Property;_Material1_RoughnessMult;Material1_RoughnessMult;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;69;-1433.34,1894.34;Float;False;Property;_Material3_TintColor;Material3_TintColor;19;0;Create;True;0;0;False;0;0,0,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;118;-1114.966,-99.02933;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-1246.62,1341.748;Float;False;Property;_Material2_RoughnessMult;Material2_RoughnessMult;12;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;145;-1195.851,1636.179;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureArrayNode;104;-1524.538,2065.825;Float;True;Property;_Material3_Sample;Material3_Sample;27;1;[HideInInspector];Create;True;0;0;False;0;None;0;Object;-1;Auto;False;7;6;SAMPLER2D;;False;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;114;-1542.202,1244.165;Float;True;Property;_TextureSample3;Texture Sample 3;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;117;-987.6153,-97.53334;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;120;-1002.88,910.7137;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;121;-1153.51,1922.389;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;101;-1183.168,1056.997;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;13;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;14;-2075.412,1867.74;Float;True;Property;_Mask;Mask;0;1;[NoScaleOffset];Create;True;0;0;False;0;00e81767994d62f4ba5a3e48f0e93787;e151bab8f799fe541b2218a36b0220d6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;100;-1136.041,45.62352;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;13;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;112;-1465.465,199.7253;Float;True;Property;_TextureSample2;Texture Sample 2;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;128;-1242.356,2345.426;Float;False;Property;_Material3_RoughnessMult;Material3_RoughnessMult;20;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;148;-982.3904,614.2142;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;-985.9679,479.5462;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;146;-1011.58,1637.607;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-966.3405,1527.736;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;116;-1802.326,2302.479;Float;True;Property;_Material3_Normal;Material3_Normal;18;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;143;-1143.896,2674.692;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;115;-1529.008,2270.749;Float;True;Property;_TextureSample4;Texture Sample 4;29;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-802.676,-112.3116;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-816.0529,887.4927;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;122;-1011.381,1923.884;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;-806.8091,590.9254;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-813.1238,1224.656;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;13;-1158.51,2062.489;Float;True;Property;_MaskSample;MaskSample;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-794.0803,345.5285;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;-967.1395,2579.346;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-813.2193,183.9419;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;144;-976.0808,2692.404;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-800.276,456.5739;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-792.926,1624.545;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-793.5234,1383.682;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-785.2622,1515.378;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-798.4568,2240.348;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-788.9069,2568.92;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;133;-502.1331,1668.347;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;136;-795.7921,2673.697;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-792.8106,2448.396;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-464.1774,1309.627;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-811.6528,1902.901;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-480.308,1091.127;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-480.8782,1422.094;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-494.9019,1546.512;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;135;-175.5116,2653.684;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;-147.2281,2212.481;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-143.7336,2003.89;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;-168.5057,2553.73;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;-164.5329,2451.342;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;198.7791,2196.979;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;Yeti/SH_Building;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;158;0;152;0
WireConnection;157;0;151;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;58;0;73;0
WireConnection;58;1;74;0
WireConnection;159;0;155;0
WireConnection;153;0;36;0
WireConnection;153;2;158;0
WireConnection;149;0;10;0
WireConnection;149;2;157;0
WireConnection;83;0;149;0
WireConnection;83;1;88;0
WireConnection;95;0;153;0
WireConnection;95;1;98;0
WireConnection;93;6;92;0
WireConnection;93;0;153;0
WireConnection;93;1;96;0
WireConnection;81;6;89;0
WireConnection;81;0;149;0
WireConnection;81;1;86;0
WireConnection;154;0;58;0
WireConnection;154;2;159;0
WireConnection;109;0;154;0
WireConnection;109;1;106;0
WireConnection;147;0;83;3
WireConnection;147;1;138;0
WireConnection;119;0;38;0
WireConnection;119;1;93;0
WireConnection;118;0;20;0
WireConnection;118;1;81;0
WireConnection;145;0;95;3
WireConnection;145;1;140;0
WireConnection;104;6;103;0
WireConnection;104;0;154;0
WireConnection;104;1;105;0
WireConnection;114;0;113;0
WireConnection;114;1;153;0
WireConnection;117;0;118;0
WireConnection;120;0;119;0
WireConnection;121;0;69;0
WireConnection;121;1;104;0
WireConnection;112;0;111;0
WireConnection;112;1;149;0
WireConnection;148;0;147;0
WireConnection;125;0;124;0
WireConnection;125;1;83;2
WireConnection;146;0;145;0
WireConnection;127;0;126;0
WireConnection;127;1;95;2
WireConnection;143;0;109;3
WireConnection;143;1;142;0
WireConnection;115;0;116;0
WireConnection;115;1;154;0
WireConnection;15;0;100;1
WireConnection;15;1;117;0
WireConnection;51;0;101;2
WireConnection;51;1;120;0
WireConnection;122;0;121;0
WireConnection;132;0;100;1
WireConnection;132;1;148;0
WireConnection;49;0;101;2
WireConnection;49;1;114;0
WireConnection;13;0;14;0
WireConnection;26;0;100;1
WireConnection;26;1;83;1
WireConnection;129;0;128;0
WireConnection;129;1;109;2
WireConnection;24;0;100;1
WireConnection;24;1;112;0
WireConnection;144;0;143;0
WireConnection;25;0;100;1
WireConnection;25;1;125;0
WireConnection;134;0;101;2
WireConnection;134;1;146;0
WireConnection;52;0;101;2
WireConnection;52;1;95;1
WireConnection;50;0;101;2
WireConnection;50;1;127;0
WireConnection;65;0;13;3
WireConnection;65;1;115;0
WireConnection;66;0;13;3
WireConnection;66;1;129;0
WireConnection;133;0;132;0
WireConnection;133;1;134;0
WireConnection;136;0;13;3
WireConnection;136;1;144;0
WireConnection;68;0;13;3
WireConnection;68;1;109;1
WireConnection;54;0;24;0
WireConnection;54;1;49;0
WireConnection;67;0;13;3
WireConnection;67;1;122;0
WireConnection;53;0;15;0
WireConnection;53;1;51;0
WireConnection;56;0;26;0
WireConnection;56;1;52;0
WireConnection;55;0;25;0
WireConnection;55;1;50;0
WireConnection;135;0;133;0
WireConnection;135;1;136;0
WireConnection;78;0;54;0
WireConnection;78;1;65;0
WireConnection;77;0;53;0
WireConnection;77;1;67;0
WireConnection;79;0;55;0
WireConnection;79;1;66;0
WireConnection;80;0;56;0
WireConnection;80;1;68;0
WireConnection;0;0;77;0
WireConnection;0;1;78;0
WireConnection;0;3;80;0
WireConnection;0;4;79;0
WireConnection;0;5;135;0
ASEEND*/
//CHKSM=AE4592037167F3BED792EF700915B9224D2B46EC