// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/VFX/EmissiveFresnel_MaskPan"
{
	Properties
	{
		_EmissiveColor("Emissive Color", Color) = (0,1,0.9693696,0)
		_OpacityMaximum("Opacity Maximum", Float) = 1
		_Fresnel_Power("Fresnel_Power", Float) = 1
		_Fresnel_Scale("Fresnel_Scale", Float) = 1
		[Toggle(_BOTTOMFADE_ON)] _BottomFade("Bottom Fade", Float) = 0
		_BottomeFade_Power("BottomeFade_Power", Float) = 1
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		[Toggle(_MASK_INVERT_ON)] _Mask_Invert("Mask_Invert", Float) = 0
		_Mask_Tiling("Mask_Tiling", Vector) = (1,1,0,0)
		_Mask_Speed("Mask_Speed", Vector) = (0,0,0,0)
		_Mask_Tiling2("Mask_Tiling2", Vector) = (1,1,0,0)
		_Mask_Speed2("Mask_Speed2", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _BOTTOMFADE_ON
		#pragma shader_feature _MASK_INVERT_ON
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float3 worldNormal;
			float3 viewDir;
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float4 _EmissiveColor;
		uniform float _Fresnel_Power;
		uniform float _Fresnel_Scale;
		uniform sampler2D _Mask;
		uniform float2 _Mask_Speed;
		uniform float2 _Mask_Tiling;
		uniform float2 _Mask_Speed2;
		uniform float2 _Mask_Tiling2;
		uniform float _BottomeFade_Power;
		uniform float _OpacityMaximum;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Emission = _EmissiveColor.rgb;
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 normalizeResult39 = normalize( i.viewDir );
			float dotResult38 = dot( ase_vertexNormal , normalizeResult39 );
			float2 uv_TexCoord29 = i.uv_texcoord * _Mask_Tiling;
			float2 panner22 = ( 1.0 * _Time.y * _Mask_Speed + uv_TexCoord29);
			float2 uv_TexCoord49 = i.uv_texcoord * _Mask_Tiling2;
			float2 panner50 = ( 1.0 * _Time.y * _Mask_Speed2 + uv_TexCoord49);
			float4 temp_output_46_0 = ( tex2D( _Mask, panner22 ) * tex2D( _Mask, panner50 ) );
			#ifdef _MASK_INVERT_ON
				float4 staticSwitch31 = ( 1.0 - temp_output_46_0 );
			#else
				float4 staticSwitch31 = temp_output_46_0;
			#endif
			float4 temp_output_52_0 = ( ( pow( ( 1.0 - saturate( dotResult38 ) ) , _Fresnel_Power ) * _Fresnel_Scale ) * staticSwitch31 );
			#ifdef _BOTTOMFADE_ON
				float4 staticSwitch44 = ( temp_output_52_0 * pow( ( 1.0 - i.uv_texcoord.y ) , _BottomeFade_Power ) );
			#else
				float4 staticSwitch44 = temp_output_52_0;
			#endif
			o.Alpha = saturate( ( ( staticSwitch44 * _OpacityMaximum ) * i.vertexColor.a ) ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1845.6;96;907;725;2063.275;494.4327;1.879339;True;False
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;35;-2122.258,30.84714;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector2Node;27;-2648.742,318.6846;Float;False;Property;_Mask_Tiling;Mask_Tiling;8;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;47;-2655.284,586.9743;Float;False;Property;_Mask_Tiling2;Mask_Tiling2;10;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;48;-2413.059,703.074;Float;False;Property;_Mask_Speed2;Mask_Speed2;11;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;49;-2466.605,570.1073;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;23;-2406.517,434.7843;Float;False;Property;_Mask_Speed;Mask_Speed;9;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.NormalVertexDataNode;34;-2124.868,-112.4886;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;39;-1932.127,41.24414;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-2460.063,301.8176;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;18;-2180.948,185.4266;Float;True;Property;_Mask;Mask;6;1;[NoScaleOffset];Create;True;0;0;False;0;e151bab8f799fe541b2218a36b0220d6;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;22;-2182.248,412.6288;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;50;-2188.79,680.9185;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;38;-1745.825,-108.7275;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;20;-1939.081,184.8963;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;40;-1602.904,-114.4095;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;45;-1938.771,426.1296;Float;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-1453.189,-25.31792;Float;False;Property;_Fresnel_Power;Fresnel_Power;2;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1588.511,194.1107;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;41;-1444.398,-118.6129;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1433.263,428.8563;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;30;-1299.695,266.748;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1216.148,3.219462;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;3;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;42;-1232.171,-118.6441;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1080.44,-118.2677;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1257.264,572.8563;Float;False;Property;_BottomeFade_Power;BottomeFade_Power;5;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;11;-1177.263,476.8564;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;31;-1140.604,188.3164;Float;False;Property;_Mask_Invert;Mask_Invert;7;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;14;-985.2625,476.8564;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-845.4642,168.9738;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-725.9515,280.673;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;44;-575.8746,166.1335;Float;False;Property;_BottomFade;Bottom Fade;4;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-539.5794,289.9808;Float;False;Property;_OpacityMaximum;Opacity Maximum;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;7;-266.3592,326.9409;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-214.3267,172.841;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-66.22752,172.1705;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;17;74.44968,170.4879;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;4;-14.16397,-12.49624;Float;False;Property;_EmissiveColor;Emissive Color;0;0;Create;True;0;0;False;0;0,1,0.9693696,0;0,1,0.9693696,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;6;272.1968,14.19525;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Yeti/VFX/EmissiveFresnel_MaskPan;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;True;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;49;0;47;0
WireConnection;39;0;35;0
WireConnection;29;0;27;0
WireConnection;22;0;29;0
WireConnection;22;2;23;0
WireConnection;50;0;49;0
WireConnection;50;2;48;0
WireConnection;38;0;34;0
WireConnection;38;1;39;0
WireConnection;20;0;18;0
WireConnection;20;1;22;0
WireConnection;40;0;38;0
WireConnection;45;0;18;0
WireConnection;45;1;50;0
WireConnection;46;0;20;0
WireConnection;46;1;45;0
WireConnection;41;0;40;0
WireConnection;30;0;46;0
WireConnection;42;0;41;0
WireConnection;42;1;3;0
WireConnection;43;0;42;0
WireConnection;43;1;2;0
WireConnection;11;0;9;2
WireConnection;31;1;46;0
WireConnection;31;0;30;0
WireConnection;14;0;11;0
WireConnection;14;1;15;0
WireConnection;52;0;43;0
WireConnection;52;1;31;0
WireConnection;10;0;52;0
WireConnection;10;1;14;0
WireConnection;44;1;52;0
WireConnection;44;0;10;0
WireConnection;32;0;44;0
WireConnection;32;1;33;0
WireConnection;8;0;32;0
WireConnection;8;1;7;4
WireConnection;17;0;8;0
WireConnection;6;2;4;0
WireConnection;6;9;17;0
ASEEND*/
//CHKSM=A3605A6B74A326B5D7B4D544B19A6C2DF5D8D756