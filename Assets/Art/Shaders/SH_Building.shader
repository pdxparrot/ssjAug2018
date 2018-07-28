// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Yeti/SH_Building"
{
	Properties
	{
		[NoScaleOffset]_Mask1("Mask1", 2D) = "white" {}
		[NoScaleOffset]_Material1_Albedo("Material1_Albedo", 2D) = "white" {}
		_Material1_TintColor("Material1_TintColor", Color) = (1,1,1,1)
		_Material1_TintStrength("Material1_TintStrength", Range( 0 , 1)) = 0
		[NoScaleOffset][Normal]_Material1_Normal("Material1_Normal", 2D) = "white" {}
		[NoScaleOffset]_Material1_Roughness("Material1_Roughness", 2D) = "white" {}
		[NoScaleOffset]_Material1_Metallic("Material1_Metallic", 2D) = "white" {}
		_Material1_Tiling("Material1_Tiling", Vector) = (1,1,0,0)
		_Material1_Offset("Material1_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Material2_Albedo("Material2_Albedo", 2D) = "white" {}
		_Material2_TintColor("Material2_TintColor", Color) = (1,1,1,1)
		_Material2_TintStrength("Material2_TintStrength", Range( 0 , 1)) = 0
		[NoScaleOffset][Normal]_Material2_Normal("Material2_Normal", 2D) = "white" {}
		[NoScaleOffset]_Material2_Roughness("Material2_Roughness", 2D) = "white" {}
		[NoScaleOffset]_Material2_Metallic("Material2_Metallic", 2D) = "white" {}
		_Material2_Tiling("Material2_Tiling", Vector) = (1,1,0,0)
		_Material2_Offset("Material2_Offset", Vector) = (0,0,0,0)
		[NoScaleOffset]_Material3_Albedo("Material3_Albedo", 2D) = "white" {}
		_Material3_TintColor("Material3_TintColor", Color) = (1,1,1,1)
		_Material3_TintStrength("Material3_TintStrength", Range( 0 , 1)) = 0
		[NoScaleOffset][Normal]_Material3_Normal("Material3_Normal", 2D) = "white" {}
		[NoScaleOffset]_Material3_Roughness("Material3_Roughness", 2D) = "white" {}
		[NoScaleOffset]_Material3_Metallic("Material3_Metallic", 2D) = "white" {}
		_Material3_Tiling("Material3_Tiling", Vector) = (1,1,0,0)
		_Material3_Offset("Material3_Offset", Vector) = (0,0,0,0)
		[HDR]_Window_Color("Window_Color", Color) = (1,1,1,0)
		_Window_EmissiveStrength("Window_EmissiveStrength", Float) = 0
		[HideInInspector][NoScaleOffset]_Window_Normal("Window_Normal", 2D) = "white" {}
		_Window_Roughness("Window_Roughness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Mask1;
		uniform sampler2D _Material1_Normal;
		uniform float2 _Material1_Tiling;
		uniform float2 _Material1_Offset;
		uniform sampler2D _Material2_Normal;
		uniform float2 _Material2_Tiling;
		uniform float2 _Material2_Offset;
		uniform sampler2D _Material3_Normal;
		uniform float2 _Material3_Tiling;
		uniform float2 _Material3_Offset;
		uniform sampler2D _Window_Normal;
		uniform sampler2D _Material1_Albedo;
		uniform float4 _Material1_TintColor;
		uniform float _Material1_TintStrength;
		uniform sampler2D _Material2_Albedo;
		uniform float4 _Material2_TintColor;
		uniform float _Material2_TintStrength;
		uniform sampler2D _Material3_Albedo;
		uniform float4 _Material3_TintColor;
		uniform float _Material3_TintStrength;
		uniform float4 _Window_Color;
		uniform float _Window_EmissiveStrength;
		uniform sampler2D _Material1_Metallic;
		uniform sampler2D _Material2_Metallic;
		uniform sampler2D _Material3_Metallic;
		uniform sampler2D _Material1_Roughness;
		uniform sampler2D _Material2_Roughness;
		uniform sampler2D _Material3_Roughness;
		uniform float _Window_Roughness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Mask113 = i.uv_texcoord;
			float4 tex2DNode13 = tex2D( _Mask1, uv_Mask113 );
			float2 uv_TexCoord10 = i.uv_texcoord * _Material1_Tiling + _Material1_Offset;
			float2 uv_TexCoord36 = i.uv_texcoord * _Material2_Tiling + _Material2_Offset;
			float2 uv_TexCoord58 = i.uv_texcoord * _Material3_Tiling + _Material3_Offset;
			float2 uv_Window_Normal87 = i.uv_texcoord;
			o.Normal = ( ( ( ( tex2DNode13.r * tex2D( _Material1_Normal, uv_TexCoord10 ) ) + ( tex2DNode13.g * tex2D( _Material2_Normal, uv_TexCoord36 ) ) ) + ( tex2DNode13.b * tex2D( _Material3_Normal, uv_TexCoord58 ) ) ) + ( tex2DNode13.a * tex2D( _Window_Normal, uv_Window_Normal87 ) ) ).rgb;
			float4 tex2DNode3 = tex2D( _Material1_Albedo, uv_TexCoord10 );
			float4 blendOpSrc30 = _Material1_TintColor;
			float4 blendOpDest30 = tex2DNode3;
			float4 lerpResult31 = lerp( tex2DNode3 , ( saturate( (( blendOpDest30 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest30 - 0.5 ) ) * ( 1.0 - blendOpSrc30 ) ) : ( 2.0 * blendOpDest30 * blendOpSrc30 ) ) )) , _Material1_TintStrength);
			float4 tex2DNode39 = tex2D( _Material2_Albedo, uv_TexCoord36 );
			float4 blendOpSrc40 = _Material2_TintColor;
			float4 blendOpDest40 = tex2DNode39;
			float4 lerpResult46 = lerp( tex2DNode39 , ( saturate( (( blendOpDest40 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest40 - 0.5 ) ) * ( 1.0 - blendOpSrc40 ) ) : ( 2.0 * blendOpDest40 * blendOpSrc40 ) ) )) , _Material2_TintStrength);
			float4 tex2DNode59 = tex2D( _Material3_Albedo, uv_TexCoord58 );
			float4 blendOpSrc60 = _Material3_TintColor;
			float4 blendOpDest60 = tex2DNode59;
			float4 lerpResult62 = lerp( tex2DNode59 , ( saturate( (( blendOpDest60 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest60 - 0.5 ) ) * ( 1.0 - blendOpSrc60 ) ) : ( 2.0 * blendOpDest60 * blendOpSrc60 ) ) )) , _Material3_TintStrength);
			o.Albedo = ( ( ( ( tex2DNode13.r * lerpResult31 ) + ( tex2DNode13.g * lerpResult46 ) ) + ( tex2DNode13.b * lerpResult62 ) ) + ( tex2DNode13.a * _Window_Color ) ).rgb;
			o.Emission = ( tex2DNode13.a * ( _Window_Color * _Window_EmissiveStrength ) ).rgb;
			o.Metallic = ( ( ( ( tex2DNode13.r * tex2D( _Material1_Metallic, uv_TexCoord10 ) ) + ( tex2DNode13.g * tex2D( _Material2_Metallic, uv_TexCoord36 ) ) ) + ( tex2DNode13.b * tex2D( _Material3_Metallic, uv_TexCoord58 ) ) ) + ( tex2DNode13.a * 0.0 ) ).r;
			o.Smoothness = ( ( ( ( tex2DNode13.r * tex2D( _Material1_Roughness, uv_TexCoord10 ) ) + ( tex2DNode13.g * tex2D( _Material2_Roughness, uv_TexCoord36 ) ) ) + ( tex2DNode13.b * tex2D( _Material3_Roughness, uv_TexCoord58 ) ) ) + ( tex2DNode13.a * _Window_Roughness ) ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1907.2;96;845;725;2218.712;-3228.933;1.341875;True;True
Node;AmplifyShaderEditor.CommentaryNode;28;-2216.216,-280.2991;Float;False;1687.403;1246.22;Material1;19;3;7;8;5;1;2;10;11;12;9;6;20;25;26;15;24;30;32;31;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;33;-2203.221,994.5939;Float;False;1687.403;1246.22;Material2;19;52;51;50;49;48;47;46;45;44;43;42;41;40;39;38;37;36;35;34;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;11;-2166.216,372.0286;Float;False;Property;_Material1_Tiling;Material1_Tiling;7;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;12;-2166.216,498.5248;Float;False;Property;_Material1_Offset;Material1_Offset;8;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;57;-2195.008,2293.513;Float;False;1687.403;1246.22;Material3;19;76;75;74;73;72;71;70;69;68;67;66;65;64;63;62;61;60;59;58;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;35;-2153.221,1646.922;Float;False;Property;_Material2_Tiling;Material2_Tiling;15;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;34;-2153.221,1773.418;Float;False;Property;_Material2_Offset;Material2_Offset;16;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;37;-1801.765,1239.907;Float;True;Property;_Material2_Albedo;Material2_Albedo;9;1;[NoScaleOffset];Create;True;0;0;False;0;5b36aef0cf5f6b54f92328797885aae9;5b36aef0cf5f6b54f92328797885aae9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.Vector2Node;73;-2145.008,2945.841;Float;False;Property;_Material3_Tiling;Material3_Tiling;23;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;74;-2145.008,3072.337;Float;False;Property;_Material3_Offset;Material3_Offset;24;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1894.164,391.0901;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1814.762,-34.9864;Float;True;Property;_Material1_Albedo;Material1_Albedo;1;1;[NoScaleOffset];Create;True;0;0;False;0;5b36aef0cf5f6b54f92328797885aae9;5b36aef0cf5f6b54f92328797885aae9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-1881.167,1665.984;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;71;-1793.551,2538.826;Float;True;Property;_Material3_Albedo;Material3_Albedo;17;1;[NoScaleOffset];Create;True;0;0;False;0;5b36aef0cf5f6b54f92328797885aae9;5b36aef0cf5f6b54f92328797885aae9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ColorNode;38;-1395.462,1049.127;Float;False;Property;_Material2_TintColor;Material2_TintColor;10;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-1502.437,38.02408;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;39;-1489.44,1312.917;Float;True;Property;_TextureSample5;Texture Sample 5;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-1872.953,2964.903;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;20;-1408.458,-225.7663;Float;False;Property;_Material1_TintColor;Material1_TintColor;2;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-1421.666,1222.687;Float;False;Property;_Material2_TintStrength;Material2_TintStrength;11;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;69;-1387.248,2348.046;Float;False;Property;_Material3_TintColor;Material3_TintColor;18;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;-1434.662,-52.20638;Float;False;Property;_Material1_TintStrength;Material1_TintStrength;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;59;-1481.226,2611.836;Float;True;Property;_TextureSample9;Texture Sample 9;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;44;-1797.992,1790.747;Float;True;Property;_Material2_Roughness;Material2_Roughness;13;1;[NoScaleOffset];Create;True;0;0;False;0;5b36aef0cf5f6b54f92328797885aae9;5b36aef0cf5f6b54f92328797885aae9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.BlendOpsNode;30;-1111.242,-222.4394;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;14;-2950.503,2387.156;Float;True;Property;_Mask1;Mask1;0;1;[NoScaleOffset];Create;True;0;0;False;0;e151bab8f799fe541b2218a36b0220d6;e151bab8f799fe541b2218a36b0220d6;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;43;-1804.44,1454.059;Float;True;Property;_Material2_Normal;Material2_Normal;12;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;ab6d98ca5dbe17b43b6f9942284717dd;ab6d98ca5dbe17b43b6f9942284717dd;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;5;-1810.989,515.8536;Float;True;Property;_Material1_Roughness;Material1_Roughness;5;1;[NoScaleOffset];Create;True;0;0;False;0;5b36aef0cf5f6b54f92328797885aae9;5b36aef0cf5f6b54f92328797885aae9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.BlendOpsNode;40;-1098.246,1052.454;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;6;-1802.324,735.921;Float;True;Property;_Material1_Metallic;Material1_Metallic;6;1;[NoScaleOffset];Create;True;0;0;False;0;5b36aef0cf5f6b54f92328797885aae9;5b36aef0cf5f6b54f92328797885aae9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1817.436,179.1662;Float;True;Property;_Material1_Normal;Material1_Normal;4;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;ab6d98ca5dbe17b43b6f9942284717dd;ab6d98ca5dbe17b43b6f9942284717dd;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;41;-1789.327,2010.814;Float;True;Property;_Material2_Metallic;Material2_Metallic;14;1;[NoScaleOffset];Create;True;0;0;False;0;5b36aef0cf5f6b54f92328797885aae9;5b36aef0cf5f6b54f92328797885aae9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.LerpOp;46;-943.0163,1326.512;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;45;-1493.016,1979.624;Float;True;Property;_TextureSample6;Texture Sample 6;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-2685.786,2384.927;Float;True;Property;_TextureSample4;Texture Sample 4;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;47;-1498.214,1756.09;Float;True;Property;_TextureSample7;Texture Sample 7;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;75;-1789.778,3089.666;Float;True;Property;_Material3_Roughness;Material3_Roughness;21;1;[NoScaleOffset];Create;True;0;0;False;0;5b36aef0cf5f6b54f92328797885aae9;5b36aef0cf5f6b54f92328797885aae9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;72;-1796.226,2752.978;Float;True;Property;_Material3_Normal;Material3_Normal;20;2;[NoScaleOffset];[Normal];Create;True;0;0;False;0;ab6d98ca5dbe17b43b6f9942284717dd;ab6d98ca5dbe17b43b6f9942284717dd;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;8;-1511.21,481.1963;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;76;-1781.113,3309.733;Float;True;Property;_Material3_Metallic;Material3_Metallic;22;1;[NoScaleOffset];Create;True;0;0;False;0;5b36aef0cf5f6b54f92328797885aae9;5b36aef0cf5f6b54f92328797885aae9;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;7;-1502.547,259.3951;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-956.012,51.61835;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;48;-1489.55,1534.288;Float;True;Property;_TextureSample8;Texture Sample 8;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;60;-1090.032,2351.373;Float;False;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-1413.452,2521.606;Float;False;Property;_Material3_TintStrength;Material3_TintStrength;19;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-1506.012,704.7307;Float;True;Property;_TextureSample3;Texture Sample 3;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-704.0773,366.7334;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;63;-1490,3055.009;Float;True;Property;_TextureSample11;Texture Sample 11;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;64;-1481.336,2833.207;Float;True;Property;_TextureSample12;Texture Sample 12;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-693.6147,120.636;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-681.368,1515.957;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-691.0814,1641.627;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-682.815,1814.823;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-694.3639,241.0638;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-680.6189,1395.529;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;62;-934.8027,2625.431;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;61;-1484.802,3278.543;Float;True;Property;_TextureSample10;Texture Sample 10;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-695.8108,539.9293;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-165.5486,1915.873;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-1788.802,3787.331;Float;False;Property;_Window_EmissiveStrength;Window_EmissiveStrength;26;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1221.463,4093;Float;False;Property;_Window_Roughness;Window_Roughness;28;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-682.8679,2940.546;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-162.3222,1715.839;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;81;-1727.861,3610.322;Float;False;Property;_Window_Color;Window_Color;25;1;[HDR];Create;True;0;0;False;0;1,1,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;87;-1702.312,3938.84;Float;True;Property;_Window_Normal;Window_Normal;27;2;[HideInInspector];[NoScaleOffset];Create;True;0;0;False;0;ab6d98ca5dbe17b43b6f9942284717dd;ab6d98ca5dbe17b43b6f9942284717dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;97;-967.9966,4263.9;Float;False;Constant;_Window_Metallic;Window_Metallic;29;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-674.6014,3113.742;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-673.1544,2814.876;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-194.5857,1502.899;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-210.7163,1289.96;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-672.4053,2694.448;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-641.5442,4245.67;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;218.6739,2356.297;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-674.4472,3588.378;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-680.603,4075.231;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;213.6794,2524.978;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;77;219.4639,1970.094;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-679.8538,3914.722;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;223.273,2167.729;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-1401.625,3770.37;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;113;389.3557,3581.495;Float;False;876.0587;301.7654;LOD Dither;4;107;111;110;112;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DitheringNode;112;1039.014,3633.254;Float;False;0;2;0;FLOAT;0;False;1;SAMPLER2D;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LODFadeNode;107;440.692,3638.317;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;111;439.3556,3768.66;Float;False;Constant;_FullOpacity;FullOpacity;30;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;110;731.2715,3631.495;Float;False;Property;_Keyword0;Keyword 0;29;0;Fetch;True;0;0;False;0;0;0;0;True;LOD_FADE_CROSSFADE;Toggle;2;Key0;Key1;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;566.0865,2808.019;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-675.706,3737.84;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;90;579.4318,3004.86;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;91;566.4818,3130.39;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;88;566.0865,2647.876;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1315.737,2905.879;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Yeti/SH_Building;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;36;0;35;0
WireConnection;36;1;34;0
WireConnection;3;0;1;0
WireConnection;3;1;10;0
WireConnection;39;0;37;0
WireConnection;39;1;36;0
WireConnection;58;0;73;0
WireConnection;58;1;74;0
WireConnection;59;0;71;0
WireConnection;59;1;58;0
WireConnection;30;0;20;0
WireConnection;30;1;3;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;46;0;39;0
WireConnection;46;1;40;0
WireConnection;46;2;42;0
WireConnection;45;0;41;0
WireConnection;45;1;36;0
WireConnection;13;0;14;0
WireConnection;47;0;44;0
WireConnection;47;1;36;0
WireConnection;8;0;5;0
WireConnection;8;1;10;0
WireConnection;7;0;2;0
WireConnection;7;1;10;0
WireConnection;31;0;3;0
WireConnection;31;1;30;0
WireConnection;31;2;32;0
WireConnection;48;0;43;0
WireConnection;48;1;36;0
WireConnection;60;0;69;0
WireConnection;60;1;59;0
WireConnection;9;0;6;0
WireConnection;9;1;10;0
WireConnection;25;0;13;1
WireConnection;25;1;8;0
WireConnection;63;0;75;0
WireConnection;63;1;58;0
WireConnection;64;0;72;0
WireConnection;64;1;58;0
WireConnection;15;0;13;1
WireConnection;15;1;31;0
WireConnection;49;0;13;2
WireConnection;49;1;48;0
WireConnection;50;0;13;2
WireConnection;50;1;47;0
WireConnection;52;0;13;2
WireConnection;52;1;45;0
WireConnection;24;0;13;1
WireConnection;24;1;7;0
WireConnection;51;0;13;2
WireConnection;51;1;46;0
WireConnection;62;0;59;0
WireConnection;62;1;60;0
WireConnection;62;2;70;0
WireConnection;61;0;76;0
WireConnection;61;1;58;0
WireConnection;26;0;13;1
WireConnection;26;1;9;0
WireConnection;56;0;26;0
WireConnection;56;1;52;0
WireConnection;66;0;13;3
WireConnection;66;1;63;0
WireConnection;55;0;25;0
WireConnection;55;1;50;0
WireConnection;68;0;13;3
WireConnection;68;1;61;0
WireConnection;65;0;13;3
WireConnection;65;1;64;0
WireConnection;54;0;24;0
WireConnection;54;1;49;0
WireConnection;53;0;15;0
WireConnection;53;1;51;0
WireConnection;67;0;13;3
WireConnection;67;1;62;0
WireConnection;95;0;13;4
WireConnection;95;1;97;0
WireConnection;79;0;55;0
WireConnection;79;1;66;0
WireConnection;93;0;13;4
WireConnection;93;1;81;0
WireConnection;94;0;13;4
WireConnection;94;1;96;0
WireConnection;80;0;56;0
WireConnection;80;1;68;0
WireConnection;77;0;53;0
WireConnection;77;1;67;0
WireConnection;92;0;13;4
WireConnection;92;1;87;0
WireConnection;78;0;54;0
WireConnection;78;1;65;0
WireConnection;82;0;81;0
WireConnection;82;1;84;0
WireConnection;112;0;110;0
WireConnection;110;1;107;1
WireConnection;110;0;111;0
WireConnection;89;0;78;0
WireConnection;89;1;92;0
WireConnection;85;0;13;4
WireConnection;85;1;82;0
WireConnection;90;0;79;0
WireConnection;90;1;94;0
WireConnection;91;0;80;0
WireConnection;91;1;95;0
WireConnection;88;0;77;0
WireConnection;88;1;93;0
WireConnection;0;0;88;0
WireConnection;0;1;89;0
WireConnection;0;2;85;0
WireConnection;0;3;91;0
WireConnection;0;4;90;0
ASEEND*/
//CHKSM=2AD60E3CADEE3A28BB01E0F6EC29D5B5324622AC