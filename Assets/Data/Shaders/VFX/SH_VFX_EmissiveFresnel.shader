// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/VFX/EmissiveFresnel"
{
	Properties
	{
		_EmissiveColor("Emissive Color", Color) = (0,1,0.9693696,0)
		_Fresnel_Power("Fresnel_Power", Float) = 1
		_Fresnel_Scale("Fresnel_Scale", Float) = 1
		[NoScaleOffset]_WPO_Noise("WPO_Noise", 2D) = "white" {}
		_WPO_Scale("WPO_Scale", Range( 0 , 1)) = 0
		_WPO_Tiling("WPO_Tiling", Vector) = (1,1,0,0)
		_WPO_Speed("WPO_Speed", Vector) = (0,0,0,0)
		_WPO_Fade("WPO_Fade", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float4 vertexColor : COLOR;
			float3 worldNormal;
			float3 viewDir;
		};

		uniform sampler2D _WPO_Noise;
		uniform float2 _WPO_Speed;
		uniform float2 _WPO_Tiling;
		uniform float _WPO_Scale;
		uniform float _WPO_Fade;
		uniform float4 _EmissiveColor;
		uniform float _Fresnel_Power;
		uniform float _Fresnel_Scale;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float3 temp_cast_0 = (0.0).xxx;
			float2 uv_TexCoord23 = v.texcoord.xy * _WPO_Tiling;
			float2 panner22 = ( 1.0 * _Time.y * _WPO_Speed + uv_TexCoord23);
			float3 lerpResult34 = lerp( temp_cast_0 , (tex2Dlod( _WPO_Noise, float4( panner22, 0, 0.0) )).rgb , _WPO_Scale);
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			v.vertex.xyz += ( ( ase_vertexNormal * lerpResult34 ) * saturate( ( (ase_worldNormal).y * _WPO_Fade ) ) );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Emission = _EmissiveColor.rgb;
			float3 ase_worldNormal = i.worldNormal;
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float3 normalizeResult11 = normalize( i.viewDir );
			float dotResult12 = dot( ase_vertexNormal , normalizeResult11 );
			o.Alpha = ( i.vertexColor.a * ( pow( ( 1.0 - saturate( dotResult12 ) ) , _Fresnel_Power ) * _Fresnel_Scale ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1673.6;96;1079;725;1888.824;-352.5706;1.786672;True;False
Node;AmplifyShaderEditor.Vector2Node;24;-1975.89,759.8839;Float;False;Property;_WPO_Tiling;WPO_Tiling;5;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;9;-1581.972,399.1906;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalVertexDataNode;10;-1580.12,249.1621;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-1793.964,757.7931;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;25;-1760.506,897.8962;Float;False;Property;_WPO_Speed;WPO_Speed;6;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.NormalizeNode;11;-1387.379,402.8948;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;12;-1201.077,252.9232;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;22;-1534.667,764.0659;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;20;-1576.722,565.4061;Float;True;Property;_WPO_Noise;WPO_Noise;3;1;[NoScaleOffset];Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;21;-1308.828,665.7843;Float;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;13;-1065.674,264.1553;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;42;-1375.722,1065.504;Float;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;44;-1170.582,1061.879;Float;False;False;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1096.07,868.8757;Float;False;Property;_WPO_Scale;WPO_Scale;4;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-966.6907,755.1161;Float;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1024.074,1256.627;Float;False;Property;_WPO_Fade;WPO_Fade;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-903.0414,351.8555;Float;False;Property;_Fresnel_Power;Fresnel_Power;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;29;-1017.776,668.3154;Float;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;15;-907.1677,259.9519;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-688.5126,363.1657;Float;False;Property;_Fresnel_Scale;Fresnel_Scale;2;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-822.1803,1065.454;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;19;-787.4635,505.8172;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;34;-771.5264,674.4064;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;17;-694.9402,259.9207;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-556.9557,506.7057;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-499.9856,258.4176;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;45;-602.4213,1052.946;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;7;-537.7682,62.64068;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-206.501,209.3763;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-350.6629,21.76969;Float;False;Property;_EmissiveColor;Emissive Color;0;0;Create;True;0;0;False;0;0,1,0.9693696,0;0,1,0.9693696,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-338.8954,503.7643;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;6;-3.252533,29.2728;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Yeti/VFX/EmissiveFresnel;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;True;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;24;0
WireConnection;11;0;9;0
WireConnection;12;0;10;0
WireConnection;12;1;11;0
WireConnection;22;0;23;0
WireConnection;22;2;25;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;13;0;12;0
WireConnection;44;0;42;0
WireConnection;29;0;21;0
WireConnection;15;0;13;0
WireConnection;46;0;44;0
WireConnection;46;1;47;0
WireConnection;34;0;35;0
WireConnection;34;1;29;0
WireConnection;34;2;27;0
WireConnection;17;0;15;0
WireConnection;17;1;3;0
WireConnection;28;0;19;0
WireConnection;28;1;34;0
WireConnection;18;0;17;0
WireConnection;18;1;2;0
WireConnection;45;0;46;0
WireConnection;8;0;7;4
WireConnection;8;1;18;0
WireConnection;41;0;28;0
WireConnection;41;1;45;0
WireConnection;6;2;4;0
WireConnection;6;9;8;0
WireConnection;6;11;41;0
ASEEND*/
//CHKSM=6636D92EE087EE3FDE7BD71A51B2FA9E8467DBD7