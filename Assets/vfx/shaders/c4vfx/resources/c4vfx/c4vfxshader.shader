Shader "LoveEngine/C4VFXShader" {
	Properties {
		[Toggle(DOUBLE_FACE_ON)] _DoubleFaceOn ("Double Face", Float) = 0
		[Toggle] _BackColorOn ("Back Color On?", Float) = 0
		[HDR] _BackColor ("Back Color", Vector) = (1,1,1,1)
		_BackColorScale ("BackColorScale", Range(0, 10)) = 1
		_SkewX ("Skew Horizontal", Float) = 0
		_SkewY ("Skew Vertical", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 2
		[Toggle] _ZWriteMode ("ZWriteMode", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcMode ("Src Mode", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstMode ("Dst Mode", Float) = 10
		_MainTex ("Main Texture", 2D) = "white" {}
		_MainTex_IsClampX ("Is Main Tex X Clamp Mode?", Float) = 0
		_MainTex_IsClampY ("Is Main Tex Y Clamp Mode?", Float) = 0
		[Enum(LoveEngine.Editor.UVSetId)] _MainUVId ("Main UV Set Id", Float) = 0
		_MainUVPivotX ("Main UV Pivot X", Float) = 0
		_MainUVPivotY ("Main UV Pivot Y", Float) = 0
		[Enum(LoveEngine.Editor.ColorChannel)] _MainColorChannel ("Main Color Channel", Float) = 0
		[Enum(LoveEngine.Editor.AlphaChannel)] _MainAlphaChannel ("Main Alpha Channel", Float) = 0
		[HDR] _MainColor ("Main Color", Vector) = (1,1,1,1)
		_MainColorScale ("MainColorScale", Range(0, 10)) = 1
		[Toggle(MAIN_SEQUENCE_ON)] _MainSequenceOn ("Main Frame Sequence On", Float) = 0
		_MainSequenceGridX ("Main Frame Sequence Grid X Size", Float) = 1
		_MainSequenceGridY ("Main Frame Sequence Grid Y Size", Float) = 1
		_MainSequenceGridStart ("Main Frame Sequence Start Index", Float) = 1
		_MainSequenceGridCount ("Main Frame Sequence Count (0 for All)", Float) = 0
		_MainSequenceTime ("Main Frame Sequence Play Time", Vector) = (1000,1,1,0)
		[Toggle(MAIN_POLAR_ON)] _MainPolarOn ("Main Use Polar", Float) = 0
		_MainUVSpeed ("Main UV Speed", Vector) = (0,0,0,0)
		_MainUVRotateOffsetX ("Main UV Rotate Center Offset X", Float) = 0
		_MainUVRotateOffsetY ("Main UV Rotate Center Offset Y", Float) = 0
		[Toggle] _MainUVSoloRotate ("Rotate tile-solo(1) or as a whole(0)?", Float) = 0
		[Toggle(MAIN_DISTORTION_ON)] _MainDistortionOn ("Main Distortion On", Float) = 0
		_MainDistortionIntensity ("Main Distortion Intensity", Float) = 1
		[Toggle(MASK_ON)] _MaskOn ("Mask ON", Float) = 0
		_MaskTex ("Mask Texture", 2D) = "white" {}
		_MaskTex_IsClampX ("Is Mask Tex X Clamp Mode?", Float) = 0
		_MaskTex_IsClampY ("Is Mask Tex Y Clamp Mode?", Float) = 0
		[Enum(LoveEngine.Editor.UVSetId)] _MaskUVId ("Mask UV Set Id", Float) = 0
		_MaskUVPivotX ("Mask UV Pivot X", Float) = 0
		_MaskUVPivotY ("Mask UV Pivot Y", Float) = 0
		[Toggle] _SpaceMaskOn ("Use As World Space Mask?", Float) = 0
		[Toggle] _MaskRevertOn ("Mask Revert On?", Float) = 0
		_MaskColorMode ("Mask Color Mode", Vector) = (0,0,0,0)
		[HDR] _MaskColor ("Mask Color", Vector) = (1,1,1,1)
		_MaskColorScale ("MaskColorScale", Range(0, 10)) = 1
		[Toggle(MASK_SEQUENCE_ON)] _MaskSequenceOn ("Mask Frame Sequence On", Float) = 0
		_MaskSequenceGridX ("Mask Frame Sequence Grid X Size", Float) = 1
		_MaskSequenceGridY ("Mask Frame Sequence Grid Y Size", Float) = 1
		_MaskSequenceGridStart ("Mask Frame Sequence Start Index", Float) = 1
		_MaskSequenceGridCount ("Mask Frame Sequence Count (0 for All)", Float) = 0
		_MaskSequenceTime ("Mask Frame Sequence Play Time", Vector) = (1000,1,1,0)
		[Toggle(MASK_POLAR_ON)] _MaskPolarOn ("Mask Use Polar", Float) = 0
		_MaskUVSpeed ("Mask UV Speed", Vector) = (0,0,0,0)
		_MaskUVRotateOffsetX ("Mask UV Rotate Center Offset X", Float) = 0
		_MaskUVRotateOffsetY ("Mask UV Rotate Center Offset Y", Float) = 0
		[Toggle] _MaskUVSoloRotate ("Rotate tile-solo(1) or as a whole(0)?", Float) = 0
		[Toggle(MASK_DISTORTION_ON)] _MaskDistortionOn ("Mask Distortion On", Float) = 0
		_MaskDistortionIntensity ("Mask Distortion Intensity", Float) = 1
		[Space(10)] [Toggle(MASK2_ON)] _Mask2On ("Mask2 ON", Float) = 0
		_Mask2Tex_IsClampX ("Is Mask2 Tex X Clamp Mode?", Float) = 0
		_Mask2Tex_IsClampY ("Is Mask2 Tex Y Clamp Mode?", Float) = 0
		_Mask2Tex ("Mask2 Texture", 2D) = "white" {}
		[Enum(LoveEngine.Editor.UVSetId)] _Mask2UVId ("Mask2 UV Set Id", Float) = 0
		_Mask2UVPivotX ("Mask2 UV Pivot X", Float) = 0
		_Mask2UVPivotY ("Mask2 UV Pivot Y", Float) = 0
		[Toggle] _SpaceMask2On ("Use As World Space Mask?", Float) = 0
		[Toggle] _Mask2RevertOn ("Mask2 Revert On?", Float) = 0
		_Mask2ColorMode ("Mask2 Color Mode", Vector) = (0,0,0,0)
		[HDR] _Mask2Color ("Mask2 Color", Vector) = (1,1,1,1)
		_Mask2ColorScale ("Mask2ColorScale", Range(0, 10)) = 1
		[Toggle(MASK2_SEQUENCE_ON)] _Mask2SequenceOn ("Mask2 Frame Sequence On", Float) = 0
		_Mask2SequenceGridX ("Mask2 Frame Sequence Grid X Size", Float) = 1
		_Mask2SequenceGridY ("Mask2 Frame Sequence Grid Y Size", Float) = 1
		_Mask2SequenceGridStart ("Mask2 Frame Sequence Start Index", Float) = 1
		_Mask2SequenceGridCount ("Mask2 Frame Sequence Count (0 for All)", Float) = 0
		_Mask2SequenceTime ("Mask2 Frame Sequence Play Time", Vector) = (1000,1,1,0)
		[Toggle(MASK2_POLAR_ON)] _Mask2PolarOn ("Mask2 Use Polar", Float) = 0
		_Mask2UVSpeed ("Mask2 UV Speed", Vector) = (0,0,0,0)
		_Mask2UVRotateOffsetX ("Mask2 UV Rotate Center Offset X", Float) = 0
		_Mask2UVRotateOffsetY ("Mask2 UV Rotate Center Offset Y", Float) = 0
		[Toggle] _Mask2UVSoloRotate ("Rotate tile-solo(1) or as a whole(0)?", Float) = 0
		[Toggle(MASK2_DISTORTION_ON)] _Mask2DistortionOn ("Mask2 Distortion On", Float) = 0
		_Mask2DistortionIntensity ("Mask2 Distortion Intensity", Float) = 1
		[Space(10)] [Toggle(MASK3_ON)] _Mask3On ("Mask3 ON", Float) = 0
		_Mask3Tex_IsClampX ("Is Mask3 X Clamp Mode?", Float) = 0
		_Mask3Tex_IsClampY ("Is Mask3 Y Clamp Mode?", Float) = 0
		_Mask3Tex ("Mask3 Texture", 2D) = "white" {}
		[Enum(LoveEngine.Editor.UVSetId)] _Mask3UVId ("Mask3 UV Set Id", Float) = 0
		_Mask3UVPivotX ("Mask3 UV Pivot X", Float) = 0
		_Mask3UVPivotY ("Mask3 UV Pivot Y", Float) = 0
		[Toggle] _SpaceMask3On ("Use As World Space Mask?", Float) = 0
		[Toggle] _Mask3RevertOn ("Mask3 Revert On?", Float) = 0
		_Mask3ColorMode ("Mask3 Color Mode", Vector) = (0,0,0,0)
		[HDR] _Mask3Color ("Mask3 Color", Vector) = (1,1,1,1)
		_Mask3ColorScale ("Mask3ColorScale", Range(0, 10)) = 1
		[Toggle(MASK3_SEQUENCE_ON)] _Mask3SequenceOn ("Mask3 Frame Sequence On", Float) = 0
		_Mask3SequenceGridX ("Mask3 Frame Sequence Grid X Size", Float) = 1
		_Mask3SequenceGridY ("Mask3 Frame Sequence Grid Y Size", Float) = 1
		_Mask3SequenceGridStart ("Mask3 Frame Sequence Start Index", Float) = 1
		_Mask3SequenceGridCount ("Mask3 Frame Sequence Count (0 for All)", Float) = 0
		_Mask3SequenceTime ("Mask3 Frame Sequence Play Time", Vector) = (1000,1,1,0)
		[Toggle(MASK3_POLAR_ON)] _Mask3PolarOn ("Mask3 Use Polar", Float) = 0
		_Mask3UVSpeed ("Mask3 UV Speed", Vector) = (0,0,0,0)
		_Mask3UVRotateOffsetX ("Mask3 UV Rotate Center Offset X", Float) = 0
		_Mask3UVRotateOffsetY ("Mask3 UV Rotate Center Offset Y", Float) = 0
		[Toggle] _Mask3UVSoloRotate ("Rotate tile-solo(1) or as a whole(0)?", Float) = 0
		[Toggle(MASK3_DISTORTION_ON)] _Mask3DistortionOn ("Mask3 Distortion On", Float) = 0
		_Mask3DistortionIntensity ("Mask3 Distortion Intensity", Float) = 1
		[Space(10)] [Toggle(MASK4_ON)] _Mask4On ("Mask4 ON", Float) = 0
		_Mask4Tex_IsClampX ("Is Mask4 Tex X Clamp Mode?", Float) = 0
		_Mask4Tex_IsClampY ("Is Mask4 Tex Y Clamp Mode?", Float) = 0
		_Mask4Tex ("Mask4 Texture", 2D) = "white" {}
		[Enum(LoveEngine.Editor.UVSetId)] _Mask4UVId ("Mask4 UV Set Id", Float) = 0
		_Mask4UVPivotX ("Mask4 UV Pivot X", Float) = 0
		_Mask4UVPivotY ("Mask4 UV Pivot Y", Float) = 0
		[Toggle] _SpaceMask4On ("Use As World Space Mask?", Float) = 0
		[Toggle] _Mask4RevertOn ("Mask4 Revert On?", Float) = 0
		_Mask4ColorMode ("Mask4 Color Mode", Vector) = (0,0,0,0)
		[HDR] _Mask4Color ("Mask4 Color", Vector) = (1,1,1,1)
		_Mask4ColorScale ("Mask4ColorScale", Range(0, 10)) = 1
		[Toggle(MASK4_SEQUENCE_ON)] _Mask4SequenceOn ("Mask4 Frame Sequence On", Float) = 0
		_Mask4SequenceGridX ("Mask4 Frame Sequence Grid X Size", Float) = 1
		_Mask4SequenceGridY ("Mask4 Frame Sequence Grid Y Size", Float) = 1
		_Mask4SequenceGridStart ("Mask4 Frame Sequence Start Index", Float) = 1
		_Mask4SequenceGridCount ("Mask4 Frame Sequence Count (0 for All)", Float) = 0
		_Mask4SequenceTime ("Mask4 Frame Sequence Play Time", Vector) = (1000,1,1,0)
		[Toggle(MASK4_POLAR_ON)] _Mask4PolarOn ("Mask4 Use Polar", Float) = 0
		_Mask4UVSpeed ("Mask4 UV Speed", Vector) = (0,0,0,0)
		_Mask4UVRotateOffsetX ("Mask4 UV Rotate Center Offset X", Float) = 0
		_Mask4UVRotateOffsetY ("Mask4 UV Rotate Center Offset Y", Float) = 0
		[Toggle] _Mask4UVSoloRotate ("Rotate tile-solo(1) or as a whole(0)?", Float) = 0
		[Toggle(MASK4_DISTORTION_ON)] _Mask4DistortionOn ("Mask4 Distortion On", Float) = 0
		_Mask4DistortionIntensity ("Mask4 Distortion Intensity", Float) = 1
		[Toggle(DISTORTION_ON)] _DistortionOn ("Distortion On?", Float) = 0
		_DistortionTex ("Distortion Texture", 2D) = "white" {}
		[Enum(LoveEngine.Editor.UVSetId)] _DistortionUVId ("Distortion UV Set Id", Float) = 0
		_DistortionUVPivotX ("Distortion UV Pivot X", Float) = 0
		_DistortionUVPivotY ("Distortion UV Pivot Y", Float) = 0
		[Toggle(DISTORTION_POLAR_ON)] _DistortionPolarOn ("Distortion Use Polar", Float) = 0
		_DistortionUVSpeed ("Distortion UV Speed", Vector) = (0,0,0,0)
		_DistortionUVRotateOffsetX ("Distortion UV Rotate Center Offset X", Float) = 0
		_DistortionUVRotateOffsetY ("Distortion UV Rotate Center Offset Y", Float) = 0
		[Toggle] _DistortionUVSoloRotate ("Rotate tile-solo(1) or as a whole(0)?", Float) = 0
		_DistortionIntensity ("Distortion Intensity", Float) = 1
		[Space(10)] [Toggle(DISTORTION2_ON)] _Distortion2On ("Distortion2 On?", Float) = 0
		_Distortion2Tex ("Distortion2 Texture", 2D) = "white" {}
		[Enum(LoveEngine.Editor.UVSetId)] _Distortion2UVId ("Distortion2 UV Set Id", Float) = 0
		_Distortion2UVPivotX ("Distortion2 UV Pivot X", Float) = 0
		_Distortion2UVPivotY ("Distortion2 UV Pivot Y", Float) = 0
		[Toggle(DISTORTION2_POLAR_ON)] _Distortion2PolarOn ("Distortion2 Use Polar", Float) = 0
		_Distortion2UVSpeed ("Distortion2 UV Speed", Vector) = (0,0,0,0)
		_Distortion2UVRotateOffsetX ("Distortion2 UV Rotate Center Offset X", Float) = 0
		_Distortion2UVRotateOffsetY ("Distortion2 UV Rotate Center Offset Y", Float) = 0
		[Toggle] _Distortion2UVSoloRotate ("Rotate tile-solo(1) or as a whole(0)?", Float) = 0
		_Distortion2Intensity ("Distortion2 Intensity", Float) = 1
		[Toggle(DISSOLVE_ON)] _DissolveOn ("Dissolve On?", Float) = 0
		_DissolveTex ("Dissolve Tex", 2D) = "" {}
		_DissolveTex_IsClampX ("Is Dissiove Tex X Clamp Mode?", Float) = 0
		_DissolveTex_IsClampY ("Is Dissiove Tex Y Clamp Mode?", Float) = 0
		[Enum(LoveEngine.Editor.UVSetId)] _DissolveUVId ("Dissolve UV Set Id", Float) = 0
		_DissolveUVPivotX ("Dissolve UV Pivot X", Float) = 0
		_DissolveUVPivotY ("Dissolve UV Pivot Y", Float) = 0
		[Toggle(DISSOLVE_POLAR_ON)] _DissolvePolarOn ("Dissolve Use Polar", Float) = 0
		_DissolveUVSpeed ("Dissolve UV Speed", Vector) = (0,0,0,0)
		[Toggle(DISSOLVE_DISTORTION_ON)] _DissolveDistortionOn ("Dissolve Distortion On", Float) = 0
		_DissolveDistortionIntensity ("Dissolve Distortion Intensity", Float) = 1
		[Toggle] _UseCustomThreshold ("Use Particle's CustomeData.x as Threshold?", Float) = 0
		_DissolveThreshold ("Dissolve Threshold", Range(0.0001, 1)) = 0.5
		_DissolvePower ("Dissolve Power", Range(0, 10)) = 1
		[Toggle(DISSOLVE_UV_DIR_ON)] _DissolveUVDirOn ("Dissolve UV Direction On?", Float) = 0
		_DissolveUVDirX ("Dissolve UV Direction X", Float) = 0
		_DissolveUVDirY ("Dissolve UV Direction Y", Float) = 0
		[Toggle(DISSOLVE_WORLD_DIR_ON)] _DissolveWorldDirOn ("Dissolve World Direction On?", Float) = 0
		_DissolveWorldDir ("Dissolve World Direction", Vector) = (0,0,0,1)
		[Toggle(DISSOLVE_EDGE_ON)] _DissolveEdgeOn ("Dissolve Edge On?", Float) = 0
		[HDR] _DissolveEdgeColor ("Edge Color", Vector) = (1,0,0,1)
		_DissolveEdgeColorScale ("Edge Color Scale", Range(0, 10)) = 1
		[Space(10)] [Toggle(DISSOLVE_EDGE_LEVELS_ON)] _DissolveEdgeLevelsOn ("Edge Levels On?", Float) = 0
		_DissolveEdgeLevelsTex ("Edge Levels Tex", 2D) = "white" {}
		_DissolveEdgeLevelsUVSpeed ("Dissolve Edge Levels UV Speed", Vector) = (0,0,0,0)
		[Space(10)] _DissolveEdgeStart ("Edge Start", Range(0, 1)) = 0
		_DissolveEdgeWidth ("Edge Width", Range(0, 1)) = 0.01
		[Toggle(MODEL_POSITION_MOVE_ON)] _ModelPositionMoveOn ("Model Position Move On?", Float) = 0
		_ModelPositionMoveTex ("Model Position Move  Texture", 2D) = "black" {}
		[Enum(LoveEngine.Editor.UVSetId)] _ModelPositionMoveUVId ("Model Position Move UV Set Id", Float) = 0
		_ModelPositionMoveUVPivotX ("ModePosition Move UV Pivot X", Float) = 0
		_ModelPositionMoveUVPivotY ("ModePosition Move UV Pivot Y", Float) = 0
		[Toggle(MODEL_POSITION_MOVE_POLAR_ON)] _ModelPositionMovePolarOn ("ModelPositionMove Use Polar", Float) = 0
		_ModelPositionMoveIntensity ("Model Posotion Move Intensity", Float) = 1
		_ModelPostionUVSpeed ("Model Postion UV Speed", Vector) = (0,0,0,0)
		[Toggle(MODEL_POSITION_DISTORTION_ON)] _ModelPositionDistortionOn ("Model Position Distortion On", Float) = 0
		_ModelPositionDistortionIntensity ("Dissolve Position Distortion Intensity", Float) = 1
		[Toggle(FRESNEL_ON)] _FresnelOn ("Fresnel On?", Float) = 0
		[Toggle] _FresnelTransparentOn ("Fresnel Transparent On?", Float) = 0
		_FresnelColor ("Fresnel Color", Vector) = (1,1,1,1)
		_FresnelColorScale ("Fresnel Color Scale", Range(0, 10)) = 1
		_FresnelPower ("Fresnel Power", Range(0, 1)) = 0.5
		[Toggle(FLOWLIGHT_ON)] _FlowLightOn ("FlowLight On?", Float) = 0
		[NoScaleOffset] _FlowLightTex ("FlowLight indicator Texture", 2D) = "" {}
		_FlowLightColor ("FlowLight Color", Vector) = (1,1,1,50)
		_FlowLightPos ("FlowLight Position", Range(-1, 1)) = 0
		_FlowLightOffset ("FlowLight Offset", Range(-1, 1)) = 0
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Unlit/Transparent"
	//CustomEditor "LoveEngine.Editor.C4VFXInspector"
}