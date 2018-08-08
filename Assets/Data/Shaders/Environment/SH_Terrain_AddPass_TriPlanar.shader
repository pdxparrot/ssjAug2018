// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/Environment/SH_Terrain_AddPass_TriPlanar"
{
	Properties
	{
		[HideInInspector][NoScaleOffset]_Splat0("Splat 0", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_Normal0("Normal 0", 2D) = "white" {}
		_Splat0_Tiling("Splat0_Tiling", Float) = 0.1
		[HideInInspector][NoScaleOffset]_Splat1("Splat 1", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_Normal1("Normal 1", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_Splat2("Splat 2", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_Normal2("Normal 2", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_Splat3("Splat 3", 2D) = "white" {}
		[HideInInspector][NoScaleOffset]_Normal3("Normal 3", 2D) = "white" {}
		_Normal_Scale("Normal_Scale", Float) = 1
		[HideInInspector][NoScaleOffset]_Control("Control", 2D) = "white" {}
		[HideInInspector]_Smoothness0("Smoothness0", Range( 0 , 1)) = 1
		[HideInInspector]_Smoothness1("Smoothness1", Range( 0 , 1)) = 1
		[HideInInspector]_Smoothness2("Smoothness2", Range( 0 , 1)) = 1
		[HideInInspector]_Smoothness3("Smoothness3", Range( 0 , 1)) = 1
		[HideInInspector][Gamma]_Metallic0("Metallic0", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic1("Metallic1", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic2("Metallic2", Range( 0 , 1)) = 0
		[HideInInspector][Gamma]_Metallic3("Metallic3", Range( 0 , 1)) = 0
		_Splat1_Tiling("Splat1_Tiling", Float) = 0.1
		_Splat2_Tiling("Splat2_Tiling", Float) = 0.1
		_Splat3_Tiling("Splat3_Tiling", Float) = 0.1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry-99" "IgnoreProjector"="True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma multi_compile_fog
		#define TERRAIN_SPLAT_ADDPASS
		#define TERRAIN_STANDARD_SHADER
		#pragma exclude_renderers gles 
		#pragma surface surf Standard keepalpha vertex:vertexDataFunc  decal:add finalcolor:SplatmapFinalColor
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Control;
		uniform sampler2D _Normal0;
		uniform float _Splat0_Tiling;
		uniform sampler2D _Normal1;
		uniform float _Splat1_Tiling;
		uniform sampler2D _Normal2;
		uniform float _Splat2_Tiling;
		uniform sampler2D _Normal3;
		uniform float _Splat3_Tiling;
		uniform float _Normal_Scale;
		uniform float _Smoothness0;
		uniform sampler2D _Splat0;
		uniform float _Smoothness1;
		uniform sampler2D _Splat1;
		uniform float _Smoothness2;
		uniform sampler2D _Splat2;
		uniform float _Smoothness3;
		uniform sampler2D _Splat3;
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
			float2 uv_Control20 = i.uv_texcoord;
			float4 tex2DNode20 = tex2D( _Control, uv_Control20 );
			float dotResult67 = dot( tex2DNode20 , float4(1,1,1,1) );
			float localSplatClip81 = ( dotResult67 );
			float SplatWeight81 = dotResult67;
			#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
				clip(SplatWeight81 == 0.0f ? -1 : 1);
			#endif
			float4 temp_output_84_0 = ( tex2DNode20 / ( localSplatClip81 + 0.001 ) );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float2 appendResult126 = (float2(ase_vertex3Pos.y , ase_vertex3Pos.z));
			float Splat0_Tiling95 = _Splat0_Tiling;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 temp_output_108_0 = abs( mul( unity_WorldToObject, float4( ase_worldNormal , 0.0 ) ).xyz );
			float dotResult110 = dot( temp_output_108_0 , float3(1,1,1) );
			float3 BlendComponent112 = ( temp_output_108_0 / dotResult110 );
			float3 break133 = BlendComponent112;
			float2 appendResult123 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.z));
			float2 appendResult124 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.y));
			float2 appendResult180 = (float2(ase_vertex3Pos.y , ase_vertex3Pos.z));
			float Splat1_Tiling240 = _Splat1_Tiling;
			float3 break182 = BlendComponent112;
			float2 appendResult178 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.z));
			float2 appendResult183 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.y));
			float2 appendResult209 = (float2(ase_vertex3Pos.y , ase_vertex3Pos.z));
			float Splat2_Tiling241 = _Splat2_Tiling;
			float3 break215 = BlendComponent112;
			float2 appendResult212 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.z));
			float2 appendResult217 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.y));
			float2 appendResult223 = (float2(ase_vertex3Pos.y , ase_vertex3Pos.z));
			float Splat3_Tiling242 = _Splat3_Tiling;
			float3 break229 = BlendComponent112;
			float2 appendResult226 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.z));
			float2 appendResult231 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.y));
			float4 weightedBlendVar33 = temp_output_84_0;
			float4 weightedBlend33 = ( weightedBlendVar33.x*( ( ( tex2D( _Normal0, ( appendResult126 * Splat0_Tiling95 ) ) * break133.x ) + ( tex2D( _Normal0, ( Splat0_Tiling95 * appendResult123 ) ) * break133.y ) ) + ( tex2D( _Normal0, ( Splat0_Tiling95 * appendResult124 ) ) * break133.z ) ) + weightedBlendVar33.y*( ( ( tex2D( _Normal1, ( appendResult180 * Splat1_Tiling240 ) ) * break182.x ) + ( tex2D( _Normal1, ( Splat1_Tiling240 * appendResult178 ) ) * break182.y ) ) + ( tex2D( _Normal1, ( Splat1_Tiling240 * appendResult183 ) ) * break182.z ) ) + weightedBlendVar33.z*( ( ( tex2D( _Normal2, ( appendResult209 * Splat2_Tiling241 ) ) * break215.x ) + ( tex2D( _Normal2, ( Splat2_Tiling241 * appendResult212 ) ) * break215.y ) ) + ( tex2D( _Normal2, ( Splat2_Tiling241 * appendResult217 ) ) * break215.z ) ) + weightedBlendVar33.w*( ( ( tex2D( _Normal3, ( appendResult223 * Splat3_Tiling242 ) ) * break229.x ) + ( tex2D( _Normal3, ( Splat3_Tiling242 * appendResult226 ) ) * break229.y ) ) + ( tex2D( _Normal3, ( Splat3_Tiling242 * appendResult231 ) ) * break229.z ) ) );
			o.Normal = UnpackScaleNormal( weightedBlend33 ,_Normal_Scale );
			float4 appendResult49 = (float4(1.0 , 1.0 , 1.0 , _Smoothness0));
			float2 appendResult97 = (float2(ase_vertex3Pos.y , ase_vertex3Pos.z));
			float3 break115 = BlendComponent112;
			float2 appendResult98 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.z));
			float2 appendResult99 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.y));
			float4 appendResult52 = (float4(1.0 , 1.0 , 1.0 , _Smoothness1));
			float2 appendResult155 = (float2(ase_vertex3Pos.y , ase_vertex3Pos.z));
			float3 break162 = BlendComponent112;
			float2 appendResult157 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.z));
			float2 appendResult161 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.y));
			float4 appendResult56 = (float4(1.0 , 1.0 , 1.0 , _Smoothness2));
			float2 appendResult264 = (float2(ase_vertex3Pos.y , ase_vertex3Pos.z));
			float3 break257 = BlendComponent112;
			float2 appendResult254 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.z));
			float2 appendResult258 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.y));
			float4 appendResult60 = (float4(1.0 , 1.0 , 1.0 , _Smoothness3));
			float2 appendResult279 = (float2(ase_vertex3Pos.y , ase_vertex3Pos.z));
			float3 break272 = BlendComponent112;
			float2 appendResult269 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.z));
			float2 appendResult273 = (float2(ase_vertex3Pos.x , ase_vertex3Pos.y));
			float4 weightedBlendVar32 = temp_output_84_0;
			float4 weightedBlend32 = ( weightedBlendVar32.x*( appendResult49 * ( ( ( tex2D( _Splat0, ( appendResult97 * Splat0_Tiling95 ) ) * break115.x ) + ( tex2D( _Splat0, ( appendResult98 * Splat0_Tiling95 ) ) * break115.y ) ) + ( tex2D( _Splat0, ( appendResult99 * Splat0_Tiling95 ) ) * break115.z ) ) ) + weightedBlendVar32.y*( appendResult52 * ( ( ( tex2D( _Splat1, ( appendResult155 * Splat1_Tiling240 ) ) * break162.x ) + ( tex2D( _Splat1, ( appendResult157 * Splat1_Tiling240 ) ) * break162.y ) ) + ( tex2D( _Splat1, ( appendResult161 * Splat1_Tiling240 ) ) * break162.z ) ) ) + weightedBlendVar32.z*( appendResult56 * ( ( ( tex2D( _Splat2, ( appendResult264 * Splat2_Tiling241 ) ) * break257.x ) + ( tex2D( _Splat2, ( appendResult254 * Splat2_Tiling241 ) ) * break257.y ) ) + ( tex2D( _Splat2, ( appendResult258 * Splat2_Tiling241 ) ) * break257.z ) ) ) + weightedBlendVar32.w*( appendResult60 * ( ( ( tex2D( _Splat3, ( appendResult279 * Splat3_Tiling242 ) ) * break272.x ) + ( tex2D( _Splat3, ( appendResult269 * Splat3_Tiling242 ) ) * break272.y ) ) + ( tex2D( _Splat3, ( appendResult273 * Splat3_Tiling242 ) ) * break272.z ) ) ) );
			o.Albedo = weightedBlend32.xyz;
			float4 appendResult70 = (float4(_Metallic0 , _Metallic1 , _Metallic2 , _Metallic3));
			float dotResult72 = dot( temp_output_84_0 , appendResult70 );
			o.Metallic = dotResult72;
			o.Smoothness = (weightedBlend32).w;
			o.Alpha = dotResult67;
		}

		ENDCG
	}
	Fallback "Hidden/TerrainEngine/Splatmap/Diffuse-AddPass"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
2758.4;311.2;1523;835;6843.358;1725.88;5.958204;True;False
Node;AmplifyShaderEditor.CommentaryNode;294;-4383.881,-3875.382;Float;False;3338.573;3303.294;TriPlanar Albedo;116;106;105;107;108;109;110;111;89;112;98;92;88;99;115;116;91;146;86;149;148;118;147;100;102;94;150;104;53;55;101;151;48;50;103;52;152;49;51;54;117;78;97;79;3;95;80;4;251;249;6;248;59;57;56;154;157;158;159;162;161;165;166;168;167;153;155;160;240;243;255;257;258;259;261;263;264;265;268;269;270;271;273;274;241;244;246;254;256;253;247;250;284;283;286;289;290;62;288;291;272;276;292;293;285;287;278;279;242;245;5;280;61;63;60;58;252;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldToObjectMatrix;106;-4333.881,-3543.822;Float;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.WorldNormalVector;105;-4329.666,-3460.817;Float;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;-4061.881,-3479.822;Float;False;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.AbsOpNode;108;-3901.881,-3479.822;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;109;-3934.999,-3299.371;Float;False;Constant;_Vector1;Vector 1;-1;0;Create;True;0;0;False;0;1,1,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;110;-3727.981,-3413.423;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-2784.104,-3667.156;Float;False;Property;_Splat0_Tiling;Splat0_Tiling;3;0;Create;True;0;0;False;0;0.1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;89;-2791.499,-3527.312;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;243;-2755.482,-2896.295;Float;False;Property;_Splat1_Tiling;Splat1_Tiling;20;0;Create;True;0;0;False;0;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;263;-2730.991,-2235.509;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;78;-2795.365,-3825.382;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;268;-2749.435,-1051.361;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;154;-2765.892,-2754.194;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;153;-2762.166,-3052.263;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;111;-3565.881,-3479.822;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;239;-4248.655,575.9836;Float;False;2245.045;3615.654;TriPlanar Normals;88;119;121;122;120;126;136;123;133;132;131;124;169;8;142;7;130;144;145;170;172;171;143;174;9;193;206;196;197;195;201;202;173;175;176;177;180;189;179;178;181;184;182;185;183;190;187;191;221;224;225;226;228;229;230;233;205;10;194;199;200;198;203;204;207;208;209;211;212;214;216;210;215;235;236;213;217;219;227;231;237;238;222;223;139;137;138;140;141;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;253;-2738.617,-1960.84;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;278;-2745.709,-1372.248;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;245;-2726.289,-1196.848;Float;False;Property;_Splat3_Tiling;Splat3_Tiling;22;0;Create;True;0;0;False;0;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;244;-2725.21,-2062.293;Float;False;Property;_Splat2_Tiling;Splat2_Tiling;21;0;Create;True;0;0;False;0;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;92;-2783.17,-3324.002;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;121;-3573.408,1004.159;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;176;-3559.832,1971.083;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;221;-3577.919,3690.442;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;95;-2605.692,-3668.319;Float;False;Splat0_Tiling;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;97;-2507.365,-3777.382;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;157;-2508.379,-2704.496;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;208;-3581.446,2468.055;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;177;-3567.141,1599.388;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;112;-3405.881,-3479.822;Float;True;BlendComponent;1;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;207;-3574.138,2839.75;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;158;-2757.563,-2550.884;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;222;-3583.326,3324.45;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;98;-2533.987,-3477.614;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;264;-2442.991,-2187.509;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;261;-3292.173,-1827.02;Float;False;112;0;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;241;-2552.31,-2071.393;Float;False;Splat2_Tiling;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;242;-2553.389,-1198.147;Float;False;Splat3_Tiling;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;119;-3577.477,625.9836;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;168;-3323.348,-2643.774;Float;False;112;0;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;279;-2457.709,-1324.247;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;270;-2741.106,-848.0513;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;240;-2578.681,-2896.295;Float;False;Splat1_Tiling;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;255;-2726.388,-1734.13;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;155;-2474.166,-3004.263;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;276;-3309.426,-948.5471;Float;False;112;0;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;254;-2481.104,-1920.242;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;269;-2491.922,-1001.663;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;223;-3341.456,3369.933;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;225;-3576.207,3605.871;Float;False;242;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;280;-2249.19,-1323.517;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;159;-2290.175,-2694.259;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;210;-4190.999,2940.879;Float;False;112;0;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;212;-3349.702,2869.346;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;258;-2494.057,-1716.81;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;99;-2550.839,-3306.683;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;209;-3339.576,2513.538;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;256;-2261.599,-1919.105;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;273;-2508.775,-830.7313;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;271;-2273.718,-991.4263;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-2315.782,-3467.378;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;-2237.006,-2191.849;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;227;-3580.852,3907.107;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;161;-2525.232,-2533.565;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;211;-3572.426,2755.178;Float;False;241;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-2301.381,-3781.721;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;162;-3077.94,-2637.749;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.BreakToComponentsNode;257;-3046.765,-1820.995;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;179;-3558.12,1886.512;Float;False;240;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;123;-3348.972,1033.756;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;213;-3577.071,3035.508;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;115;-3103.547,-3410.868;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;136;-3571.697,919.5884;Float;False;95;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;-2268.182,-3008.603;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;180;-3325.271,1644.871;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;226;-3353.483,3720.039;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;272;-3064.018,-942.5222;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;122;-4198.655,1082.044;Float;False;112;0;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;192;-3138.823,-150.7967;Float;False;1144.639;462.3846;Control Texture;7;20;66;67;82;81;83;84;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;189;-4178.595,2068.411;Float;False;112;0;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;178;-3335.396,2000.679;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;126;-3335.606,671.4661;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PosVertexDataNode;120;-3580.143,1266.439;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;224;-4196.683,3787.771;Float;False;112;0;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;181;-3566.566,2233.363;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;131;-3141.788,674.2125;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;124;-3347.841,1284.4;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;-3123.113,1892.646;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;292;-2836.348,-1071.295;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;132;-3136.689,925.7219;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;133;-3927.951,1091.47;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.Vector4Node;66;-3088.823,106.588;Float;False;Constant;_Vector0;Vector 0;9;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;228;-3149.539,3366.977;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;285;-2072.888,-1089.329;Float;True;Property;_TextureSample14;Texture Sample 14;8;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;5;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;182;-3911.131,2058.394;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;146;-2092.113,-2768.369;Float;True;Property;_TextureSample4;Texture Sample 4;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;4;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;117;-2854.805,-3573.403;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;-2290.175,-2534.258;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;116;-2865.282,-3138.581;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;183;-3334.265,2251.324;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;283;-2783.849,-1981.724;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-2123.364,-3809.382;Float;True;Property;_Splat0;Splat 0;1;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-2315.782,-3307.377;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;215;-3923.535,2930.862;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.WireNode;167;-2851.809,-2779.764;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;217;-3354.272,3059.171;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;259;-2258.999,-1717.503;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;231;-3348.55,3945.975;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;214;-3145.758,2516.284;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;184;-3131.452,1647.617;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;274;-2273.718,-831.4244;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;246;-2075.739,-1942.17;Float;True;Property;_TextureSample12;Texture Sample 12;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;6;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;230;-3141.2,3612.005;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-2104.498,-3052.051;Float;True;Property;_Splat1;Splat 1;4;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-2077.127,-1352.259;Float;True;Property;_Splat3;Splat 3;8;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;-3087.926,-100.7966;Float;True;Property;_Control;Control;11;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;216;-3137.418,2761.313;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;86;-2116.5,-3520.482;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;3;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;229;-3929.219,3777.753;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SamplerNode;6;-2076.987,-2228.229;Float;True;Property;_Splat2;Splat 2;6;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;144;-3647.694,883.8595;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-2914.479,2489.944;Float;True;Property;_Normal2;Normal 2;7;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;166;-2801.336,-2358.082;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;293;-2795.784,-711.2908;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;142;-2940.83,924.2946;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;7;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;194;-2905.145,3578.414;Float;True;Property;_TextureSample9;Texture Sample 9;9;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;10;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;248;-1712.387,-2031.797;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;284;-2776.049,-1570.923;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;235;-3674.73,2727.36;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;286;-2047.747,-847.2873;Float;True;Property;_TextureSample15;Texture Sample 15;8;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;5;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;249;-1709.787,-1818.613;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;238;-3682.333,3582.652;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;219;-3127.644,3034.676;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;-3121.212,1238.998;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;67;-2757.336,25.09592;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;287;-1730.282,-1169.653;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;288;-1727.282,-961.1464;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;247;-2080.595,-1735.692;Float;True;Property;_TextureSample13;Texture Sample 13;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;6;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;8;-2915.997,1616.451;Float;True;Property;_Normal1;Normal 1;5;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;190;-3630.064,1862.755;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;169;-2920.611,1891.718;Float;True;Property;_TextureSample6;Texture Sample 6;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;8;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-1742.872,-3387.522;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;147;-2083.689,-2521.278;Float;True;Property;_TextureSample5;Texture Sample 5;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;4;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;94;-2114.163,-3308.789;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;3;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-2916.84,3337.022;Float;True;Property;_Normal3;Normal 3;9;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-1764.333,-3616.384;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;193;-2923.415,2738.176;Float;True;Property;_TextureSample8;Texture Sample 8;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;9;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;233;-3125.723,3925.281;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;187;-3107.636,2205.922;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;118;-1810.868,-3121.923;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-2940,647.3386;Float;True;Property;_Normal0;Normal 0;2;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-1743.98,-2872.007;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;149;-1748.49,-2629.017;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;-2551.199,2037.539;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;289;-1724.747,-704.2874;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;170;-2930.333,2128.271;Float;True;Property;_TextureSample7;Texture Sample 7;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;8;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;206;-2912.518,2980.005;Float;True;Property;_TextureSample11;Texture Sample 11;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;9;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;-2543.139,3480.753;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;-2547.958,1745.897;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;199;-2523.983,3764.265;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;143;-2939.101,1233.5;Float;True;Property;_TextureSample3;Texture Sample 3;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;7;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;236;-3651.92,3208.223;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-2553.925,1095.038;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-2561.36,786.1175;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-1720.162,-1271.055;Float;False;Property;_Smoothness3;Smoothness3;15;1;[HideInInspector];Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1576.93,-1348.942;Float;False;Constant;_Float6;Float 6;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;150;-1731.555,-2397.324;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;205;-2897.161,3862.756;Float;True;Property;_TextureSample10;Texture Sample 10;9;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;10;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;237;-3657.623,4059.713;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;191;-3623.583,2377.99;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;101;-1588.631,-3492.366;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomExpressionNode;81;-2564.076,24.29174;Float;False;#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)$	clip(SplatWeight == 0.0f ? -1 : 1)@$#endif;1;True;1;True;SplatWeight;FLOAT;0;In;;SplatClip;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1595.393,-3054.836;Float;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;251;-1543.399,-1920.005;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;145;-3599.271,1424.545;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;250;-1709.788,-1601.526;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-1552.537,-2213.198;Float;False;Constant;_Float4;Float 4;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-1695.769,-2135.31;Float;False;Property;_Smoothness2;Smoothness2;14;1;[HideInInspector];Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;195;-2581.598,2621.971;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-2581.784,142.0611;Float;False;Constant;_Float3;Float 3;9;0;Create;True;0;0;False;0;0.001;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;291;-1550.676,-983.1463;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1613.245,-3782.366;Float;False;Constant;_Float2;Float 2;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;151;-1569.642,-2730.819;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-1738.625,-2976.947;Float;False;Property;_Smoothness1;Smoothness1;13;1;[HideInInspector];Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-1750.877,-3295.055;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1756.477,-3704.478;Float;False;Property;_Smoothness0;Smoothness0;12;1;[HideInInspector];Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;-2569.949,2879.641;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;252;-1410.744,-1624.466;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;60;-1429.315,-1345.214;Float;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;173;-2564.158,2264.378;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;56;-1404.922,-2209.47;Float;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;-2567.257,1371.304;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;49;-1465.63,-3778.638;Float;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;203;-2348.187,3742.83;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;290;-1389.747,-726.2874;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;140;-2334.477,1064.931;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;201;-2383.934,2864.053;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;103;-1439.936,-3322.934;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;197;-2569.524,3113.556;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;200;-2536.279,4059.438;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;174;-2350.29,1875.515;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;83;-2274.291,63.4366;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;52;-1447.778,-3051.108;Float;False;FLOAT4;4;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;1;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;152;-1411.131,-2445.568;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;84;-2149.786,-97.93913;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-1229.433,-754.1982;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;175;-2174.764,2231.958;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1231.047,-2466.277;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1210.109,-3337.879;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;141;-2159.208,1351.927;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-509.8241,537.7855;Float;False;Property;_Metallic0;Metallic0;16;2;[HideInInspector];[Gamma];Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-492.156,822.6334;Float;False;Property;_Metallic3;Metallic3;19;2;[HideInInspector];[Gamma];Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;204;-2182.157,4034.644;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-508.1562,626.6337;Float;False;Property;_Metallic1;Metallic1;17;2;[HideInInspector];[Gamma];Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-503.1562,724.6337;Float;False;Property;_Metallic2;Metallic2;18;2;[HideInInspector];[Gamma];Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;202;-2217.985,3072.24;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1246.37,-1644.696;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1235.333,553.0692;Float;False;Property;_Normal_Scale;Normal_Scale;10;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SummedBlendNode;32;-775.3961,-643.3714;Float;False;5;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;70;-150.009,645.9119;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SummedBlendNode;33;-1235.333,361.0691;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;72;20.86172,514.8439;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;34;653.6402,217.7935;Float;False;FinalColor *= SurfaceOut.Alpha@;7;False;3;True;SurfaceIn;OBJECT;0;In;Input;True;SurfaceOut;OBJECT;0;In;SurfaceOutputStandard;True;FinalColor;OBJECT;0;InOut;fixed4;SplatmapFinalColor;False;True;4;0;FLOAT;0;False;1;OBJECT;0;False;2;OBJECT;0;False;3;OBJECT;0;False;2;FLOAT;0;OBJECT;4
Node;AmplifyShaderEditor.SwizzleNode;64;436.8857,572.3002;Float;False;FLOAT;3;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;85;-317.1631,993.9602;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;47;431.6495,781.2004;Float;False;v.tangent.xyz = cross ( v.normal, float3( 0, 0, 1 ) )@$v.tangent.w = -1@;1;True;0;CalculateTangents;True;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;11;-1011.333,361.0691;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;696.0403,374.5324;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Yeti/Environment/SH_Terrain_AddPass_TriPlanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;-99;True;Opaque;;Geometry;All;True;True;True;False;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;Hidden/TerrainEngine/Splatmap/Diffuse-AddPass;0;-1;-1;-1;1;IgnoreProjector=True;False;0;0;False;-1;-1;0;False;-1;3;Pragma;multi_compile_fog;Define;TERRAIN_SPLAT_ADDPASS;Define;TERRAIN_STANDARD_SHADER;2;decal:add;finalcolor:SplatmapFinalColor;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;107;0;106;0
WireConnection;107;1;105;0
WireConnection;108;0;107;0
WireConnection;110;0;108;0
WireConnection;110;1;109;0
WireConnection;111;0;108;0
WireConnection;111;1;110;0
WireConnection;95;0;80;0
WireConnection;97;0;78;2
WireConnection;97;1;78;3
WireConnection;157;0;154;1
WireConnection;157;1;154;3
WireConnection;112;0;111;0
WireConnection;98;0;89;1
WireConnection;98;1;89;3
WireConnection;264;0;263;2
WireConnection;264;1;263;3
WireConnection;241;0;244;0
WireConnection;242;0;245;0
WireConnection;279;0;278;2
WireConnection;279;1;278;3
WireConnection;240;0;243;0
WireConnection;155;0;153;2
WireConnection;155;1;153;3
WireConnection;254;0;253;1
WireConnection;254;1;253;3
WireConnection;269;0;268;1
WireConnection;269;1;268;3
WireConnection;223;0;222;2
WireConnection;223;1;222;3
WireConnection;280;0;279;0
WireConnection;280;1;242;0
WireConnection;159;0;157;0
WireConnection;159;1;240;0
WireConnection;212;0;207;1
WireConnection;212;1;207;3
WireConnection;258;0;255;1
WireConnection;258;1;255;2
WireConnection;99;0;92;1
WireConnection;99;1;92;2
WireConnection;209;0;208;2
WireConnection;209;1;208;3
WireConnection;256;0;254;0
WireConnection;256;1;241;0
WireConnection;273;0;270;1
WireConnection;273;1;270;2
WireConnection;271;0;269;0
WireConnection;271;1;242;0
WireConnection;88;0;98;0
WireConnection;88;1;95;0
WireConnection;265;0;264;0
WireConnection;265;1;241;0
WireConnection;161;0;158;1
WireConnection;161;1;158;2
WireConnection;79;0;97;0
WireConnection;79;1;95;0
WireConnection;162;0;168;0
WireConnection;257;0;261;0
WireConnection;123;0;121;1
WireConnection;123;1;121;3
WireConnection;115;0;112;0
WireConnection;160;0;155;0
WireConnection;160;1;240;0
WireConnection;180;0;177;2
WireConnection;180;1;177;3
WireConnection;226;0;221;1
WireConnection;226;1;221;3
WireConnection;272;0;276;0
WireConnection;178;0;176;1
WireConnection;178;1;176;3
WireConnection;126;0;119;2
WireConnection;126;1;119;3
WireConnection;131;0;126;0
WireConnection;131;1;136;0
WireConnection;124;0;120;1
WireConnection;124;1;120;2
WireConnection;185;0;179;0
WireConnection;185;1;178;0
WireConnection;292;0;272;0
WireConnection;132;0;136;0
WireConnection;132;1;123;0
WireConnection;133;0;122;0
WireConnection;228;0;223;0
WireConnection;228;1;225;0
WireConnection;285;1;271;0
WireConnection;182;0;189;0
WireConnection;146;1;159;0
WireConnection;117;0;115;0
WireConnection;165;0;161;0
WireConnection;165;1;240;0
WireConnection;116;0;115;2
WireConnection;183;0;181;1
WireConnection;183;1;181;2
WireConnection;283;0;257;0
WireConnection;3;1;79;0
WireConnection;91;0;99;0
WireConnection;91;1;95;0
WireConnection;215;0;210;0
WireConnection;167;0;162;0
WireConnection;217;0;213;1
WireConnection;217;1;213;2
WireConnection;259;0;258;0
WireConnection;259;1;241;0
WireConnection;231;0;227;1
WireConnection;231;1;227;2
WireConnection;214;0;209;0
WireConnection;214;1;211;0
WireConnection;184;0;180;0
WireConnection;184;1;179;0
WireConnection;274;0;273;0
WireConnection;274;1;242;0
WireConnection;246;1;256;0
WireConnection;230;0;225;0
WireConnection;230;1;226;0
WireConnection;4;1;160;0
WireConnection;5;1;280;0
WireConnection;216;0;211;0
WireConnection;216;1;212;0
WireConnection;86;1;88;0
WireConnection;229;0;224;0
WireConnection;6;1;265;0
WireConnection;144;0;133;0
WireConnection;9;1;214;0
WireConnection;166;0;162;2
WireConnection;293;0;272;2
WireConnection;142;1;132;0
WireConnection;194;1;230;0
WireConnection;248;0;6;0
WireConnection;248;1;283;0
WireConnection;284;0;257;2
WireConnection;235;0;215;0
WireConnection;286;1;274;0
WireConnection;249;0;246;0
WireConnection;249;1;257;1
WireConnection;238;0;229;0
WireConnection;219;0;211;0
WireConnection;219;1;217;0
WireConnection;130;0;136;0
WireConnection;130;1;124;0
WireConnection;67;0;20;0
WireConnection;67;1;66;0
WireConnection;287;0;5;0
WireConnection;287;1;292;0
WireConnection;288;0;285;0
WireConnection;288;1;272;1
WireConnection;247;1;259;0
WireConnection;8;1;184;0
WireConnection;190;0;182;0
WireConnection;169;1;185;0
WireConnection;102;0;86;0
WireConnection;102;1;115;1
WireConnection;147;1;165;0
WireConnection;94;1;91;0
WireConnection;10;1;228;0
WireConnection;100;0;3;0
WireConnection;100;1;117;0
WireConnection;193;1;216;0
WireConnection;233;0;225;0
WireConnection;233;1;231;0
WireConnection;187;0;179;0
WireConnection;187;1;183;0
WireConnection;118;0;116;0
WireConnection;7;1;131;0
WireConnection;148;0;4;0
WireConnection;148;1;167;0
WireConnection;149;0;146;0
WireConnection;149;1;162;1
WireConnection;172;0;169;0
WireConnection;172;1;182;1
WireConnection;289;0;286;0
WireConnection;289;1;293;0
WireConnection;170;1;187;0
WireConnection;206;1;219;0
WireConnection;198;0;10;0
WireConnection;198;1;238;0
WireConnection;171;0;8;0
WireConnection;171;1;190;0
WireConnection;199;0;194;0
WireConnection;199;1;229;1
WireConnection;143;1;130;0
WireConnection;236;0;215;2
WireConnection;137;0;142;0
WireConnection;137;1;133;1
WireConnection;138;0;7;0
WireConnection;138;1;144;0
WireConnection;150;0;147;0
WireConnection;150;1;166;0
WireConnection;205;1;233;0
WireConnection;237;0;229;2
WireConnection;191;0;182;2
WireConnection;101;0;100;0
WireConnection;101;1;102;0
WireConnection;81;0;67;0
WireConnection;81;1;67;0
WireConnection;251;0;248;0
WireConnection;251;1;249;0
WireConnection;145;0;133;2
WireConnection;250;0;247;0
WireConnection;250;1;284;0
WireConnection;195;0;9;0
WireConnection;195;1;235;0
WireConnection;291;0;287;0
WireConnection;291;1;288;0
WireConnection;151;0;148;0
WireConnection;151;1;149;0
WireConnection;104;0;94;0
WireConnection;104;1;118;0
WireConnection;196;0;193;0
WireConnection;196;1;215;1
WireConnection;252;0;251;0
WireConnection;252;1;250;0
WireConnection;60;0;61;0
WireConnection;60;1;61;0
WireConnection;60;2;61;0
WireConnection;60;3;63;0
WireConnection;173;0;170;0
WireConnection;173;1;191;0
WireConnection;56;0;57;0
WireConnection;56;1;57;0
WireConnection;56;2;57;0
WireConnection;56;3;59;0
WireConnection;139;0;143;0
WireConnection;139;1;145;0
WireConnection;49;0;50;0
WireConnection;49;1;50;0
WireConnection;49;2;50;0
WireConnection;49;3;48;0
WireConnection;203;0;198;0
WireConnection;203;1;199;0
WireConnection;290;0;291;0
WireConnection;290;1;289;0
WireConnection;140;0;138;0
WireConnection;140;1;137;0
WireConnection;201;0;195;0
WireConnection;201;1;196;0
WireConnection;103;0;101;0
WireConnection;103;1;104;0
WireConnection;197;0;206;0
WireConnection;197;1;236;0
WireConnection;200;0;205;0
WireConnection;200;1;237;0
WireConnection;174;0;171;0
WireConnection;174;1;172;0
WireConnection;83;0;81;0
WireConnection;83;1;82;0
WireConnection;52;0;53;0
WireConnection;52;1;53;0
WireConnection;52;2;53;0
WireConnection;52;3;55;0
WireConnection;152;0;151;0
WireConnection;152;1;150;0
WireConnection;84;0;20;0
WireConnection;84;1;83;0
WireConnection;62;0;60;0
WireConnection;62;1;290;0
WireConnection;175;0;174;0
WireConnection;175;1;173;0
WireConnection;54;0;52;0
WireConnection;54;1;152;0
WireConnection;51;0;49;0
WireConnection;51;1;103;0
WireConnection;141;0;140;0
WireConnection;141;1;139;0
WireConnection;204;0;203;0
WireConnection;204;1;200;0
WireConnection;202;0;201;0
WireConnection;202;1;197;0
WireConnection;58;0;56;0
WireConnection;58;1;252;0
WireConnection;32;0;84;0
WireConnection;32;1;51;0
WireConnection;32;2;54;0
WireConnection;32;3;58;0
WireConnection;32;4;62;0
WireConnection;70;0;74;0
WireConnection;70;1;71;0
WireConnection;70;2;75;0
WireConnection;70;3;69;0
WireConnection;33;0;84;0
WireConnection;33;1;141;0
WireConnection;33;2;175;0
WireConnection;33;3;202;0
WireConnection;33;4;204;0
WireConnection;72;0;84;0
WireConnection;72;1;70;0
WireConnection;64;0;32;0
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
//CHKSM=D4899DBDAD4174817D18E4E2EE5CFD9881917C7B