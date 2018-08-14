// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/VFX/Impact"
{
	Properties
	{
		[NoScaleOffset]_Texture("Texture", 2D) = "white" {}
		_Texture_Tiling("Texture_Tiling", Vector) = (1,1,0,0)
		_Texture_Speed("Texture_Speed", Vector) = (0,0,0,0)
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		[Toggle(_MASK_INVERT_ON)] _Mask_Invert("Mask_Invert", Float) = 0
		_Mask_Tiling("Mask_Tiling", Vector) = (1,1,0,0)
		_Mask_Speed("Mask_Speed", Vector) = (0,0,0,0)
		_Mask_Tiling2("Mask_Tiling2", Vector) = (1,1,0,0)
		_Mask_Speed2("Mask_Speed2", Vector) = (0,0,0,0)
		_Fresnel_Power("Fresnel_Power", Float) = 1
		_Fresnel_Scale("Fresnel_Scale", Float) = 1
		[Toggle(_BOTTOMFADE_ON)] _BottomFade("BottomFade", Float) = 0
		_BottomeFade_Power("BottomeFade_Power", Float) = 1
		_Opacity_Maximum("Opacity_Maximum", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _BOTTOMFADE_ON
		#pragma shader_feature _MASK_INVERT_ON
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float3 worldNormal;
			float3 viewDir;
		};

		uniform sampler2D _Texture;
		uniform float2 _Texture_Speed;
		uniform float2 _Texture_Tiling;
		uniform sampler2D _Mask;
		uniform float2 _Mask_Speed;
		uniform float2 _Mask_Tiling;
		uniform float2 _Mask_Speed2;
		uniform float2 _Mask_Tiling2;
		uniform float _BottomeFade_Power;
		uniform float _Opacity_Maximum;
		uniform float _Fresnel_Power;
		uniform float _Fresnel_Scale;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord41 = i.uv_texcoord * _Texture_Tiling;
			float2 panner42 = ( 1.0 * _Time.y * _Texture_Speed + uv_TexCoord41);
			o.Albedo = saturate( ( i.vertexColor + tex2D( _Texture, panner42 ) ) ).rgb;
			o.Smoothness = 0.0;
			float2 uv_TexCoord9 = i.uv_texcoord * _Mask_Tiling;
			float2 panner11 = ( 1.0 * _Time.y * _Mask_Speed + uv_TexCoord9);
			float2 uv_TexCoord5 = i.uv_texcoord * _Mask_Tiling2;
			float2 panner12 = ( 1.0 * _Time.y * _Mask_Speed2 + uv_TexCoord5);
			float4 temp_output_18_0 = ( tex2D( _Mask, panner11 ) * tex2D( _Mask, panner12 ) );
			#ifdef _MASK_INVERT_ON
				float4 staticSwitch27 = ( 1.0 - temp_output_18_0 );
			#else
				float4 staticSwitch27 = temp_output_18_0;
			#endif
			float4 temp_cast_1 = (( 1.0 - i.vertexColor.a )).xxxx;
			float4 temp_output_50_0 = ( staticSwitch27 - temp_cast_1 );
			float4 temp_cast_2 = (( 1.0 - i.vertexColor.a )).xxxx;
			#ifdef _BOTTOMFADE_ON
				float4 staticSwitch31 = ( temp_output_50_0 * pow( ( 1.0 - i.uv_texcoord.y ) , _BottomeFade_Power ) );
			#else
				float4 staticSwitch31 = temp_output_50_0;
			#endif
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 normalizeResult8 = normalize( i.viewDir );
			float dotResult13 = dot( ase_vertexNormal , normalizeResult8 );
			o.Alpha = saturate( ( ( staticSwitch31 * _Opacity_Maximum ) * ( pow( ( 1.0 - saturate( dotResult13 ) ) , _Fresnel_Power ) * _Fresnel_Scale ) ) ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
				float3 worldNormal : TEXCOORD3;
				half4 color : COLOR0;
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
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				surfIN.viewDir = worldViewDir;
				surfIN.worldNormal = IN.worldNormal;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
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
1921;23;1918;1016;2688.967;452.8181;1.363438;True;False
Node;AmplifyShaderEditor.CommentaryNode;49;-3703.011,114.9934;Float;False;1240.368;729.1779;Two Mask Pan;12;18;10;6;2;11;9;4;3;16;14;12;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;3;-3653.011,567.0717;Float;False;Property;_Mask_Tiling2;Mask_Tiling2;7;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;2;-3646.469,298.7817;Float;False;Property;_Mask_Tiling;Mask_Tiling;5;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-3457.79,281.9147;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;4;-3410.786,683.1714;Float;False;Property;_Mask_Speed2;Mask_Speed2;8;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;6;-3404.244,414.8813;Float;False;Property;_Mask_Speed;Mask_Speed;6;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-3464.332,550.2046;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;10;-3178.675,165.5237;Float;True;Property;_Mask;Mask;3;1;[NoScaleOffset];Create;True;0;0;False;0;e151bab8f799fe541b2218a36b0220d6;e151bab8f799fe541b2218a36b0220d6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;11;-3179.975,392.7259;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;12;-3186.517,661.0159;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;14;-2936.808,164.9934;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;45;-2406.127,687.1135;Float;False;1263.428;383.4912;Opacity Fresnel;10;1;7;8;13;15;19;23;24;22;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;16;-2936.498,406.2267;Float;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;48;-2405.981,347.2966;Float;False;678.0004;309.0002;V Fade;4;20;26;28;25;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;1;-2353.517,886.6044;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2605.979,174.2078;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalizeNode;8;-2165.36,887.1308;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;21;-2390.876,248.4511;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;7;-2356.127,739.3206;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-2355.982,397.2966;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;33;-2273.704,-300.0819;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-2179.985,541.2967;Float;False;Property;_BottomeFade_Power;BottomeFade_Power;12;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;26;-2099.987,445.2967;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;53;-2084.016,-207.397;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;51;-1944.487,126.9449;Float;False;224;183;Erode;1;50;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;27;-2231.785,170.0195;Float;False;Property;_Mask_Invert;Mask_Invert;4;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;47;-1583.427,-220.6908;Float;False;1045.728;370.1018;Texture Controls;5;38;42;41;43;44;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;13;-1977.085,741.1076;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;28;-1907.988,445.2967;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;15;-1834.164,741.348;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;43;-1533.427,-153.8238;Float;False;Property;_Texture_Tiling;Texture_Tiling;1;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;50;-1894.487,176.945;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-1344.749,-170.6908;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1664.875,255.7873;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;44;-1291.203,-37.7241;Float;False;Property;_Texture_Speed;Texture_Speed;2;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;17;-1708.138,828.4655;Float;False;Property;_Fresnel_Power;Fresnel_Power;9;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;19;-1675.658,737.1446;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;23;-1512.784,737.1135;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;42;-1064.96,-51.98312;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1510.58,833.3134;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;10;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1447.983,301.0537;Float;False;Property;_Opacity_Maximum;Opacity_Maximum;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;31;-1474.407,175.2322;Float;False;Property;_BottomFade;BottomFade;11;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1185.901,181.9397;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;38;-858.6995,-80.58897;Float;True;Property;_Texture;Texture;0;1;[NoScaleOffset];Create;True;0;0;False;0;bad2aad5cbac17b42ac57cd1b9c7d5f5;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;46;-466.5941,-241.9506;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1311.7,737.4898;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-991.1957,181.05;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-337.6503,-95.4156;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-213.4281,0.447386;Float;False;Constant;_Smoothness;Smoothness;14;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;40;-193.6205,-89.06138;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;36;-203.3326,186.3712;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;76.70289,-91.32429;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Yeti/VFX/Impact;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;2;0
WireConnection;5;0;3;0
WireConnection;11;0;9;0
WireConnection;11;2;6;0
WireConnection;12;0;5;0
WireConnection;12;2;4;0
WireConnection;14;0;10;0
WireConnection;14;1;11;0
WireConnection;16;0;10;0
WireConnection;16;1;12;0
WireConnection;18;0;14;0
WireConnection;18;1;16;0
WireConnection;8;0;1;0
WireConnection;21;0;18;0
WireConnection;26;0;20;2
WireConnection;53;0;33;4
WireConnection;27;1;18;0
WireConnection;27;0;21;0
WireConnection;13;0;7;0
WireConnection;13;1;8;0
WireConnection;28;0;26;0
WireConnection;28;1;25;0
WireConnection;15;0;13;0
WireConnection;50;0;27;0
WireConnection;50;1;53;0
WireConnection;41;0;43;0
WireConnection;30;0;50;0
WireConnection;30;1;28;0
WireConnection;19;0;15;0
WireConnection;23;0;19;0
WireConnection;23;1;17;0
WireConnection;42;0;41;0
WireConnection;42;2;44;0
WireConnection;31;1;50;0
WireConnection;31;0;30;0
WireConnection;34;0;31;0
WireConnection;34;1;32;0
WireConnection;38;1;42;0
WireConnection;46;0;33;0
WireConnection;24;0;23;0
WireConnection;24;1;22;0
WireConnection;35;0;34;0
WireConnection;35;1;24;0
WireConnection;39;0;46;0
WireConnection;39;1;38;0
WireConnection;40;0;39;0
WireConnection;36;0;35;0
WireConnection;0;0;40;0
WireConnection;0;4;54;0
WireConnection;0;9;36;0
ASEEND*/
//CHKSM=01E388E167E9FDE660F933A36CF85B5DFE85628D