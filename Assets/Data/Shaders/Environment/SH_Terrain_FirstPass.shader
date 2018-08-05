// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/Terrain/SH_Terrain_FirstPass"
{
	Properties
	{
		_Splat0("Splat 0", 2D) = "white" {}
		_Normal0("Normal 0", 2D) = "white" {}
		_Splat1("Splat 1", 2D) = "white" {}
		_Normal1("Normal 1", 2D) = "white" {}
		_Splat2("Splat 2", 2D) = "white" {}
		_Normal2("Normal 2", 2D) = "white" {}
		_Splat3("Splat 3", 2D) = "white" {}
		_Normal3("Normal 3", 2D) = "white" {}
		_Normal_Scale("Normal_Scale", Float) = 1
		_Control("Control", 2D) = "white" {}
		[HideInInspector]_Smoothness0("Smoothness0", Range( 0 , 1)) = 1
		[HideInInspector]_Smoothness1("Smoothness1", Range( 0 , 1)) = 1
		[HideInInspector]_Smoothness2("Smoothness2", Range( 0 , 1)) = 1
		[HideInInspector]_Smoothness3("Smoothness3", Range( 0 , 1)) = 1
		[HideInInspector][Gamma]_Metallic0("Metallic0", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic1("Metallic1", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic2("Metallic2", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic3("Metallic3", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry-100" "SplatCount"="8" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma multi_compile_fog
		#pragma exclude_renderers gles 
		#pragma surface surf Standard keepalpha vertex:vertexDataFunc  finalcolor:SplatmapFinalColor
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Control;
		uniform float4 _Control_ST;
		uniform sampler2D _Normal0;
		uniform sampler2D _Splat0;
		uniform float4 _Splat0_ST;
		uniform sampler2D _Normal1;
		uniform sampler2D _Splat1;
		uniform float4 _Splat1_ST;
		uniform sampler2D _Normal2;
		uniform sampler2D _Splat2;
		uniform float4 _Splat2_ST;
		uniform sampler2D _Normal3;
		uniform sampler2D _Splat3;
		uniform float4 _Splat3_ST;
		uniform float _Normal_Scale;
		uniform float _Smoothness0;
		uniform float _Smoothness1;
		uniform float _Smoothness2;
		uniform float _Smoothness3;
		uniform float _Metallic0;
		uniform float _Metallic1;
		uniform float _Metallic2;
		uniform float _Metallic3;


		void SplatmapFinalColor( Input SurfaceIn , SurfaceOutputStandard SurfaceOut , inout fixed4 FinalColor )
		{
			FinalColor *= SurfaceOut.Alpha;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float localCalculateTangents47 = ( 0.0 );
			v.tangent.xyz = cross ( v.normal, float3( 0, 0, 1 ) );
			v.tangent.w = -1;
			float3 temp_cast_0 = (localCalculateTangents47).xxx;
			v.vertex.xyz += temp_cast_0;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Control = i.uv_texcoord * _Control_ST.xy + _Control_ST.zw;
			float4 tex2DNode20 = tex2D( _Control, uv_Control );
			float dotResult67 = dot( tex2DNode20 , float4(1,1,1,1) );
			float localSplatClip81 = ( dotResult67 );
			float SplatWeight81 = dotResult67;
			#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
				clip(SplatWeight81 == 0.0f ? -1 : 1);
			#endif
			float4 temp_output_84_0 = ( tex2DNode20 / ( localSplatClip81 + 0.001 ) );
			float2 uv_Splat0 = i.uv_texcoord * _Splat0_ST.xy + _Splat0_ST.zw;
			float2 uv_Splat1 = i.uv_texcoord * _Splat1_ST.xy + _Splat1_ST.zw;
			float2 uv_Splat2 = i.uv_texcoord * _Splat2_ST.xy + _Splat2_ST.zw;
			float2 uv_Splat3 = i.uv_texcoord * _Splat3_ST.xy + _Splat3_ST.zw;
			float4 weightedBlendVar33 = temp_output_84_0;
			float4 weightedBlend33 = ( weightedBlendVar33.x*tex2D( _Normal0, uv_Splat0 ) + weightedBlendVar33.y*tex2D( _Normal1, uv_Splat1 ) + weightedBlendVar33.z*tex2D( _Normal2, uv_Splat2 ) + weightedBlendVar33.w*tex2D( _Normal3, uv_Splat3 ) );
			o.Normal = UnpackScaleNormal( weightedBlend33 ,_Normal_Scale );
			float4 appendResult49 = (float4(1.0 , 1.0 , 1.0 , _Smoothness0));
			float4 appendResult52 = (float4(1.0 , 1.0 , 1.0 , _Smoothness1));
			float4 appendResult56 = (float4(1.0 , 1.0 , 1.0 , _Smoothness2));
			float4 appendResult60 = (float4(1.0 , 1.0 , 1.0 , _Smoothness3));
			float4 weightedBlendVar32 = temp_output_84_0;
			float4 weightedBlend32 = ( weightedBlendVar32.x*( appendResult49 * tex2D( _Splat0, uv_Splat0 ) ) + weightedBlendVar32.y*( appendResult52 * tex2D( _Splat1, uv_Splat1 ) ) + weightedBlendVar32.z*( appendResult56 * tex2D( _Splat2, uv_Splat2 ) ) + weightedBlendVar32.w*( appendResult60 * tex2D( _Splat3, uv_Splat3 ) ) );
			o.Albedo = weightedBlend32.xyz;
			float4 appendResult70 = (float4(_Metallic0 , _Metallic1 , _Metallic2 , _Metallic3));
			float dotResult72 = dot( temp_output_84_0 , appendResult70 );
			o.Metallic = dotResult72;
			o.Smoothness = (weightedBlend32).w;
			o.Alpha = dotResult67;
		}

		ENDCG
	}

	Dependency "BaseMapShader"="Yeti/Terrain/SH_Terrain_BaseMap"
	Dependency "AddPassShader"="Yeti/Terrain/SH_Terrain_AddPass"
	Fallback "Nature/Terrain/Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
2752.8;311.2;1534;840;4316.499;2152.503;4.130189;True;False
Node;AmplifyShaderEditor.Vector4Node;66;-2171.902,94.28035;Float;False;Constant;_Vector0;Vector 0;9;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;-2171.005,-113.1043;Float;True;Property;_Control;Control;10;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;67;-1840.415,12.78823;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1584.451,-1579.792;Float;False;Property;_Smoothness0;Smoothness0;11;1;[HideInInspector];Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1441.219,-1657.68;Float;False;Constant;_Float2;Float 2;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1392.401,-885.5223;Float;False;Constant;_Float4;Float 4;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-1535.633,-807.6342;Float;False;Property;_Smoothness2;Smoothness2;13;1;[HideInInspector];Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1392.401,-523.0414;Float;False;Constant;_Float6;Float 6;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-1664.862,129.7534;Float;False;Constant;_Float3;Float 3;9;0;Create;True;0;0;False;0;0.001;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-1550.049,-1180.442;Float;False;Property;_Smoothness1;Smoothness1;12;1;[HideInInspector];Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1406.817,-1258.331;Float;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-1535.633,-445.1534;Float;False;Property;_Smoothness3;Smoothness3;14;1;[HideInInspector];Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;81;-1647.154,11.98405;Float;False;#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)$	clip(SplatWeight == 0.0f ? -1 : 1)@$#endif;1;True;1;True;SplatWeight;FLOAT;0;In;;SplatClip;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-1403.785,-366.8485;Float;True;Property;_Splat3;Splat 3;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;49;-1293.604,-1653.952;Float;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;3;-1403.495,-1482.928;Float;True;Property;_Splat0;Splat 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-1388.635,-1093.52;Float;True;Property;_Splat1;Splat 1;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-1624.417,228.3894;Float;False;0;3;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;60;-1244.786,-519.3134;Float;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-1622.121,613.9884;Float;False;0;6;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;56;-1244.786,-881.7943;Float;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;6;-1399.709,-731.1627;Float;True;Property;_Splat2;Splat 2;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;43;-1622.121,423.4841;Float;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-1612.94,820.5591;Float;False;0;5;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;52;-1259.202,-1254.603;Float;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;83;-1357.369,51.12891;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-257.6345,560.923;Float;False;Property;_Metallic1;Metallic1;16;2;[HideInInspector];[Gamma];Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-1025.331,-405.7385;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;10;-1369.096,811.8519;Float;True;Property;_Normal3;Normal 3;8;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;84;-1232.863,-110.2468;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-252.6345,658.9229;Float;False;Property;_Metallic2;Metallic2;17;2;[HideInInspector];[Gamma];Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-259.3025,472.0748;Float;False;Property;_Metallic0;Metallic0;15;2;[HideInInspector];[Gamma];Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-241.6343,756.9226;Float;False;Property;_Metallic3;Metallic3;18;2;[HideInInspector];[Gamma];Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1039.747,-1141.028;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1025.331,-768.2193;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;9;-1373.26,610.2549;Float;True;Property;_Normal2;Normal 2;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1074.149,-1540.378;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;7;-1377.217,213.556;Float;True;Property;_Normal0;Normal 0;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-1372.484,408.5556;Float;True;Property;_Normal1;Normal 1;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;70;100.5127,580.2011;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-800,512;Float;False;Property;_Normal_Scale;Normal_Scale;9;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SummedBlendNode;33;-800,320;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SummedBlendNode;32;-708.7783,-471.2748;Float;False;5;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CustomExpressionNode;47;505.4956,707.3541;Float;False;v.tangent.xyz = cross ( v.normal, float3( 0, 0, 1 ) )@$v.tangent.w = -1@;1;True;0;CalculateTangents;True;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;64;383.4958,317.6718;Float;False;FLOAT;3;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;34;653.6402,217.7935;Float;False;FinalColor *= SurfaceOut.Alpha@;7;False;3;True;SurfaceIn;OBJECT;0;In;Input;True;SurfaceOut;OBJECT;0;In;SurfaceOutputStandard;True;FinalColor;OBJECT;0;InOut;fixed4;SplatmapFinalColor;False;True;4;0;FLOAT;0;False;1;OBJECT;0;False;2;OBJECT;0;False;3;OBJECT;0;False;2;FLOAT;0;OBJECT;4
Node;AmplifyShaderEditor.DotProductOpNode;72;271.3834,449.1334;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;85;-1624.225,1045.526;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;11;-576,320;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;696.0403,374.5324;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Yeti/Terrain/SH_Terrain_FirstPass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;-100;False;Opaque;;Geometry;All;True;True;True;False;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;Nature/Terrain/Diffuse;0;-1;-1;-1;1;SplatCount=8;False;2;BaseMapShader=Yeti/Terrain/SH_Terrain_BaseMap;AddPassShader=Yeti/Terrain/SH_Terrain_AddPass;0;False;-1;-1;0;False;-1;1;Pragma;multi_compile_fog;1;finalcolor:SplatmapFinalColor;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;67;0;20;0
WireConnection;67;1;66;0
WireConnection;81;0;67;0
WireConnection;81;1;67;0
WireConnection;49;0;50;0
WireConnection;49;1;50;0
WireConnection;49;2;50;0
WireConnection;49;3;48;0
WireConnection;60;0;61;0
WireConnection;60;1;61;0
WireConnection;60;2;61;0
WireConnection;60;3;63;0
WireConnection;56;0;57;0
WireConnection;56;1;57;0
WireConnection;56;2;57;0
WireConnection;56;3;59;0
WireConnection;52;0;53;0
WireConnection;52;1;53;0
WireConnection;52;2;53;0
WireConnection;52;3;55;0
WireConnection;83;0;81;0
WireConnection;83;1;82;0
WireConnection;62;0;60;0
WireConnection;62;1;5;0
WireConnection;10;1;45;0
WireConnection;84;0;20;0
WireConnection;84;1;83;0
WireConnection;54;0;52;0
WireConnection;54;1;4;0
WireConnection;58;0;56;0
WireConnection;58;1;6;0
WireConnection;9;1;44;0
WireConnection;51;0;49;0
WireConnection;51;1;3;0
WireConnection;7;1;42;0
WireConnection;8;1;43;0
WireConnection;70;0;74;0
WireConnection;70;1;71;0
WireConnection;70;2;75;0
WireConnection;70;3;69;0
WireConnection;33;0;84;0
WireConnection;33;1;7;0
WireConnection;33;2;8;0
WireConnection;33;3;9;0
WireConnection;33;4;10;0
WireConnection;32;0;84;0
WireConnection;32;1;51;0
WireConnection;32;2;54;0
WireConnection;32;3;58;0
WireConnection;32;4;62;0
WireConnection;64;0;32;0
WireConnection;72;0;84;0
WireConnection;72;1;70;0
WireConnection;85;0;67;0
WireConnection;11;0;33;0
WireConnection;11;1;16;0
WireConnection;0;0;32;0
WireConnection;0;1;11;0
WireConnection;0;3;72;0
WireConnection;0;4;64;0
WireConnection;0;9;85;0
WireConnection;0;11;47;0
ASEEND*/
//CHKSM=E5DB47FF1BE179B37B32E94566B9232667922CE8