// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

 Shader "Custom/Water GrabPass" 
 {
     Properties 
     {        
         _Colour ("Water Colour", Color) = (1,1,1,1)
		 _RippleColour ("Ripple Colour", Color) = (1,1,1,1)

		 _MainTex ("Noise Texture", 2D) = "bump" {}
		 _RippleTex ("Noise Texture", 2D) = "bump" {}

		 _RippleThreshold ("Ripple Threshold",Range(0, 1)) = 0.5
		 _RippleWidth ("Ripple Width",Range(0, 500)) = 0.5
		 _RippleHeight ("Ripple Height",Range(0, 50)) = 0.5
		 _RippleSpeed ("Ripple Speed",Range(0, 2)) = 0.5
	
		 _ReflectionSkew ("Reflection Skew",Range(0, 1)) = 0.05
		 _ReflectionOffset ("Reflection Offset",Range(0, 0.2)) = 0.05

		 _DistortionScale ("Distortion Scale",  Range(0, 0.5)) = 0.05
         _DistortionAmount ("Distortion Amount", Range(0, 0.1)) = 0.05
		 _DistortionSpeed ("Distortion Speed",Range(0, 5)) = 0.5
		 _PixelSize ("Pixel Size",Range(0, 0.1)) = 0.5

		 [MaterialToggle] _UVDebug("Debug UVs", Float) = 0
     }
     
     SubShader
     {
         Tags {"Queue"="Transparent+2" "IgnoreProjector"="True" "RenderType"="Opaque" "DisableBatching" = "true"}
         ZWrite On Lighting Off Cull Off Fog { Mode Off } Blend One Zero
 
         GrabPass { "_GrabTexture" }
         
         Pass 
         {
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #include "UnityCG.cginc"
 
             sampler2D _GrabTexture;
             fixed4 _Colour;
			 fixed4 _RippleColour;
             sampler2D _MainTex;
			 sampler2D _RippleTex;
             float  _DistortionScale;
			 float  _DistortionAmount;
			 float  _DistortionSpeed;
			 float  _UVDebug;
			 float  _ReflectionSkew;
			 float  _ReflectionOffset;
			 float  _RippleThreshold;
			 float  _RippleSpeed;
			 float  _RippleWidth;
			 float  _RippleHeight;
			 float _PixelSize;
 
             struct vin
             {
                 float4 vertex : POSITION;
				 float2 rippleTexcoord : TEXCOORD1;
                 float2 texcoord : TEXCOORD0;
                 
             };
 
             struct v2f
             {
                 float4 vertex : POSITION;
                 float2 texcoord : TEXCOORD0;
				 float2 rippleTexcoord : TEXCOORD1;
                 float4 uvgrab : TEXCOORD2;
                     
             };

	
			 
 
             float4 _MainTex_ST;
			 float4 _RippleTex_ST;
 
             // Vertex function 
             v2f vert (vin v)
             {
                 v2f o;

                 o.vertex = UnityObjectToClipPos(v.vertex);
                 o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex) * _DistortionScale;
				 o.rippleTexcoord = TRANSFORM_TEX(v.rippleTexcoord, _RippleTex) * (1 / float2(_RippleWidth, _RippleHeight)) ;
 
 
             #if UNITY_UV_STARTS_AT_TOP
                 float scale = -1.0;
             #else
                 float scale = 1.0;
             #endif            
 
                 o.uvgrab.xy = (float2(o.vertex.x, (o.vertex.y) * scale) + o.vertex.w) * 0.5;
                 o.uvgrab.zw = o.vertex.zw;
 
                 float4 top = UnityObjectToClipPos(float4(0, 0.5, 0, 1));
                 top.xy /= top.w;

				float skew = 1 - (v.vertex.y +0.5);
                o.uvgrab.y = 1 -  (o.uvgrab.y + top.y) + (_WorldSpaceCameraPos.y  / _ScreenParams.y) - (skew * _ReflectionSkew) - _ReflectionOffset;

                 return o;
             }

			 half2 QuantizeVector(half2 coordinate)
			 {
				return half2(floor(coordinate.x / _PixelSize) * _PixelSize, floor(coordinate.y / _PixelSize) * _PixelSize);
			 }

 
             // Fragment function
             half4 frag (v2f i) : COLOR
             {        
                 
				 half2 rippleA = i.rippleTexcoord - (_Time.x * _RippleSpeed) + half2(0.5,0.3);
				 half2 rippleB = i.rippleTexcoord + (_Time.x * _RippleSpeed);
				
                 half2 distortion = UnpackNormal(tex2D(_MainTex, i.texcoord + (_Time.x * _DistortionSpeed))).rg;
				 half2 ripple = UnpackNormal((tex2D(_RippleTex, rippleA) + tex2D(_RippleTex, rippleB)) * 0.5).rg;	 	
                 
                 i.uvgrab.xy += distortion * _DistortionAmount;                    

				 if (_UVDebug > 0.5) 
				 {
					return fixed4(distortion.x, distortion.y, 1.0, 1.0);
					//return fixed4(ripple.x, ripple.y, 1.0, 1.0);
				 }

		


                 fixed3 colour = (tex2D( _GrabTexture, QuantizeVector(i.uvgrab)) * _Colour).rgb;   

				 if (length(ripple) > _RippleThreshold) 
				 {
					colour = lerp(colour, _RippleColour.rgb, _RippleColour.a);
				 }
				
				              
               return fixed4(colour.r, colour.g, colour.b ,1.0);
             }

	
         
             ENDCG
         } 
     }
 }