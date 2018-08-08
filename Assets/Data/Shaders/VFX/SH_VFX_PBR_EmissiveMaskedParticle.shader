// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/VFX/SH_VFX_PBR_EmissiveMaskedParticle"
{
	Properties
	{
		[HDR]_Emissive_Strength("Emissive_Strength", Float) = 0
		_Albedo_Alpha("Albedo_Alpha", 2D) = "white" {}
		[Normal]_Normal("Normal", 2D) = "white" {}
		[Gamma]_Metallic_Smoothness_EmissiveMask("Metallic_Smoothness_EmissiveMask", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Albedo_Alpha;
		uniform float4 _Albedo_Alpha_ST;
		uniform sampler2D _Metallic_Smoothness_EmissiveMask;
		uniform float4 _Metallic_Smoothness_EmissiveMask_ST;
		uniform float _Emissive_Strength;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_Albedo_Alpha = i.uv_texcoord * _Albedo_Alpha_ST.xy + _Albedo_Alpha_ST.zw;
			float4 tex2DNode1 = tex2D( _Albedo_Alpha, uv_Albedo_Alpha );
			o.Albedo = tex2DNode1.rgb;
			float2 uv_Metallic_Smoothness_EmissiveMask = i.uv_texcoord * _Metallic_Smoothness_EmissiveMask_ST.xy + _Metallic_Smoothness_EmissiveMask_ST.zw;
			float4 tex2DNode2 = tex2D( _Metallic_Smoothness_EmissiveMask, uv_Metallic_Smoothness_EmissiveMask );
			o.Emission = ( ( tex2DNode2.b * i.vertexColor ) * _Emissive_Strength ).rgb;
			o.Metallic = tex2DNode2.r;
			o.Smoothness = tex2DNode2.g;
			o.Alpha = 1;
			clip( tex2DNode1.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
2758.4;316;1523;830;1418.049;232.8291;1.3;True;False
Node;AmplifyShaderEditor.SamplerNode;2;-777.3992,274.9002;Float;True;Property;_Metallic_Smoothness_EmissiveMask;Metallic_Smoothness_EmissiveMask;3;1;[Gamma];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;6;-618.5488,486.7213;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-405.3492,347.6211;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-424.8488,553.6708;Float;False;Property;_Emissive_Strength;Emissive_Strength;0;1;[HDR];Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-767.1995,-114.4;Float;True;Property;_Albedo_Alpha;Albedo_Alpha;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-769.8995,86.59999;Float;True;Property;_Normal;Normal;2;1;[Normal];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-222.049,346.971;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Yeti/VFX/SH_VFX_PBR_EmissiveMaskedParticle;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Opaque;;AlphaTest;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;4;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;2;3
WireConnection;8;1;6;0
WireConnection;9;0;8;0
WireConnection;9;1;10;0
WireConnection;0;0;1;0
WireConnection;0;1;3;0
WireConnection;0;2;9;0
WireConnection;0;3;2;1
WireConnection;0;4;2;2
WireConnection;0;10;1;4
ASEEND*/
//CHKSM=43297E22169056B1596B8FA7D043C3C9DB6C766C