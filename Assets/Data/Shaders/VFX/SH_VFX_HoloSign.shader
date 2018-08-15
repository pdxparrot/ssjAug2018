// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/VFX/HoloSign"
{
	Properties
	{
		[NoScaleOffset]_Sign_Texture("Sign_Texture", 2D) = "white" {}
		_Emission_Strength("Emission_Strength", Float) = 1
		_Opacity_Max("Opacity_Max", Range( 0 , 1)) = 1
		_Layer_Offset("Layer_Offset", Float) = 0
		_Layer_Direction("Layer_Direction", Vector) = (0,0,1,0)
		[NoScaleOffset]_Scanline_Texture("Scanline_Texture", 2D) = "white" {}
		_Scanline_Tiling("Scanline_Tiling", Vector) = (1,1,0,0)
		_Scanline_Speed("Scanline_Speed", Float) = 1
		_Scanline_Lift("Scanline_Lift", Float) = 0
		[NoScaleOffset]_Distortion_Texture("Distortion_Texture", 2D) = "white" {}
		_Distortion_Strength("Distortion_Strength", Float) = 0
		_Distortion_Speed("Distortion_Speed", Float) = 1
		_Distortion_Tiling("Distortion_Tiling", Vector) = (1,1,0,0)
		_Flicker_Speed("Flicker_Speed", Float) = 1
		_Flicker_Darkness("Flicker_Darkness", Float) = 0
		_Flicker_Brightness("Flicker_Brightness", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float3 _Layer_Direction;
		uniform float _Layer_Offset;
		uniform float _Emission_Strength;
		uniform float _Flicker_Speed;
		uniform float _Flicker_Darkness;
		uniform float _Flicker_Brightness;
		uniform sampler2D _Scanline_Texture;
		uniform float _Distortion_Strength;
		uniform sampler2D _Distortion_Texture;
		uniform float _Distortion_Speed;
		uniform float2 _Distortion_Tiling;
		uniform float2 _Scanline_Tiling;
		uniform float _Scanline_Speed;
		uniform float _Scanline_Lift;
		uniform sampler2D _Sign_Texture;
		uniform float _Opacity_Max;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( _Layer_Direction * ( ( _Layer_Offset * v.color.g ) + ( _Layer_Offset * ( 2.0 * v.color.b ) ) ) );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float clampResult83 = clamp( saturate( ( ( ( sin( ( _Time.y * _Flicker_Speed ) ) + 1.0 ) * 0.5 ) + ( ( sin( ( _Time.y * ( _Flicker_Speed * 0.4 ) ) ) + 1.0 ) * 0.5 ) ) ) , _Flicker_Darkness , _Flicker_Brightness );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float2 uv_Sign_Texture1 = i.uv_texcoord;
			float4 tex2DNode1 = tex2D( _Sign_Texture, uv_Sign_Texture1 );
			o.Emission = ( _Emission_Strength * ( clampResult83 * ( saturate( ( tex2D( _Scanline_Texture, ( ( ( _Distortion_Strength * tex2D( _Distortion_Texture, ( ( _Distortion_Speed * _Time.y ) + ( ase_vertex3Pos.y * _Distortion_Tiling ) ) ).g ) + ase_vertex3Pos.x ) + ( ( ase_vertex3Pos.y * _Scanline_Tiling ) + ( _Time.y * _Scanline_Speed ) ) ) ).r + _Scanline_Lift ) ) * tex2DNode1 ) ) ).rgb;
			o.Alpha = ( tex2DNode1.a * _Opacity_Max );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
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
2758.4;316.8;1523;829;4057.602;1554.261;3.07708;True;False
Node;AmplifyShaderEditor.CommentaryNode;87;-3787.954,-1182.1;Float;False;1132.182;549.4798;Distortion;10;61;62;67;64;66;63;65;56;51;55;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-3683.427,-1132.1;Float;False;Property;_Distortion_Speed;Distortion_Speed;11;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;67;-3737.954,-792.4205;Float;False;Property;_Distortion_Tiling;Distortion_Tiling;12;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;64;-3676.176,-1014.837;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;62;-3734.137,-936.7231;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-3512.355,-864.42;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-3456.734,-1033.646;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-3325.07,-897.2778;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;88;-2609.936,-1169.703;Float;False;1705.316;420.432;Flicker;15;79;71;77;74;72;69;75;70;76;80;84;81;83;78;89;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;86;-3150.186,-561.703;Float;False;1656.589;562.8593;Scanlines;13;42;39;41;37;40;43;44;52;60;35;47;46;45;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-2550.632,-1005.953;Float;False;Property;_Flicker_Speed;Flicker_Speed;13;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;51;-3187.582,-903.4138;Float;True;Property;_Distortion_Texture;Distortion_Texture;9;1;[NoScaleOffset];Create;True;0;0;False;0;003ce2ff8eb37274e953fa545e2df75d;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;56;-3118.433,-990.2898;Float;False;Property;_Distortion_Strength;Distortion_Strength;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-3100.186,-113.4437;Float;False;Property;_Scanline_Speed;Scanline_Speed;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;41;-3086.148,-215.2392;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;37;-3088.311,-353.5992;Float;False;Property;_Scanline_Tiling;Scanline_Tiling;6;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;78;-2559.936,-863.8703;Float;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;False;0;0.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;39;-3074.169,-499.8616;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-2820.573,-937.1008;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-2833.144,-358.8114;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-2850.609,-185.0737;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;71;-2535.025,-1097.884;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-2339.016,-914.0208;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-2144.743,-1090.946;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-2635.412,-310.188;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-2602.106,-511.703;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-2141.772,-941.0021;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;69;-1995.809,-1084.588;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-2310.329,-330.4373;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SinOpNode;75;-1992.838,-934.6436;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;70;-1855.068,-1084.008;Float;False;ConstantBiasScale;-1;;1;63208df05c83e8e49a48ffbdce2e43a0;0;3;3;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;76;-1852.096,-934.0637;Float;False;ConstantBiasScale;-1;;2;63208df05c83e8e49a48ffbdce2e43a0;0;3;3;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;35;-2164.489,-336.7904;Float;True;Property;_Scanline_Texture;Scanline_Texture;5;1;[NoScaleOffset];Create;True;0;0;False;0;7edd262399f9a7545baa809ffd39c9c2;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;47;-2036.243,-131.191;Float;False;Property;_Scanline_Lift;Scanline_Lift;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;34;-1110.544,473.2559;Float;False;970.7536;524.3414;Position Offset;8;25;33;20;7;17;24;28;32;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-1827.401,-149.8931;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;-1607.029,-1021.564;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;7;-1059.892,743.6969;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-2393.457,39.33511;Float;True;Property;_Sign_Texture;Sign_Texture;0;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;45;-1667.197,-149.3668;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-1388.935,-851.6801;Float;False;Property;_Flicker_Brightness;Flicker_Brightness;15;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1060.544,663.8985;Float;False;Property;_Layer_Offset;Layer_Offset;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-790.0798,865.3973;Float;False;2;2;0;FLOAT;2;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-1448.868,-939.1758;Float;False;Property;_Flicker_Darkness;Flicker_Darkness;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;81;-1447.446,-1021.564;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-1446.421,8.975028;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-657.5526,811.3036;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-790.0802,672.014;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;83;-1080.219,-1119.703;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;32;-565.5915,523.2559;Float;False;Property;_Layer_Direction;Layer_Direction;4;0;Create;True;0;0;False;0;0,0,1;0,0,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;3;-613.9741,-165.2189;Float;False;Property;_Emission_Strength;Emission_Strength;1;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-525.3879,205.4637;Float;False;Property;_Opacity_Max;Opacity_Max;2;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-940.9673,-12.61386;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-502.0313,673.3654;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-222.0078,126.555;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-304.5906,569.2355;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-367.0452,-54.03136;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Yeti/VFX/HoloSign;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;66;0;62;2
WireConnection;66;1;67;0
WireConnection;63;0;61;0
WireConnection;63;1;64;0
WireConnection;65;0;63;0
WireConnection;65;1;66;0
WireConnection;51;1;65;0
WireConnection;55;0;56;0
WireConnection;55;1;51;2
WireConnection;40;0;39;2
WireConnection;40;1;37;0
WireConnection;43;0;41;0
WireConnection;43;1;42;0
WireConnection;77;0;79;0
WireConnection;77;1;78;0
WireConnection;72;0;71;0
WireConnection;72;1;79;0
WireConnection;44;0;40;0
WireConnection;44;1;43;0
WireConnection;52;0;55;0
WireConnection;52;1;39;1
WireConnection;74;0;71;0
WireConnection;74;1;77;0
WireConnection;69;0;72;0
WireConnection;60;0;52;0
WireConnection;60;1;44;0
WireConnection;75;0;74;0
WireConnection;70;3;69;0
WireConnection;76;3;75;0
WireConnection;35;1;60;0
WireConnection;46;0;35;1
WireConnection;46;1;47;0
WireConnection;80;0;70;0
WireConnection;80;1;76;0
WireConnection;45;0;46;0
WireConnection;20;1;7;3
WireConnection;81;0;80;0
WireConnection;48;0;45;0
WireConnection;48;1;1;0
WireConnection;25;0;17;0
WireConnection;25;1;20;0
WireConnection;24;0;17;0
WireConnection;24;1;7;2
WireConnection;83;0;81;0
WireConnection;83;1;84;0
WireConnection;83;2;89;0
WireConnection;82;0;83;0
WireConnection;82;1;48;0
WireConnection;28;0;24;0
WireConnection;28;1;25;0
WireConnection;5;0;1;4
WireConnection;5;1;6;0
WireConnection;33;0;32;0
WireConnection;33;1;28;0
WireConnection;2;0;3;0
WireConnection;2;1;82;0
WireConnection;0;2;2;0
WireConnection;0;9;5;0
WireConnection;0;11;33;0
ASEEND*/
//CHKSM=1B38B13466E4F8351B9787B945335F19C2D1D0A9