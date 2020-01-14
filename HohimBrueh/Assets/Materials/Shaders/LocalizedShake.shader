// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.33 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.33;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:0,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:1,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-2827-RGB;n:type:ShaderForge.SFN_Tex2d,id:5801,x:32211,y:32562,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_5801,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_SceneColor,id:2827,x:32992,y:32642,varname:node_2827,prsc:2|UVIN-9991-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9411,x:32302,y:33136,ptovrint:False,ptlb:xOffset,ptin:_xOffset,varname:node_9411,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:5557,x:32327,y:33310,ptovrint:False,ptlb:yOffset,ptin:_yOffset,varname:node_5557,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Append,id:5964,x:32840,y:33129,varname:node_5964,prsc:2|A-213-OUT,B-8349-OUT;n:type:ShaderForge.SFN_ScreenPos,id:5250,x:32225,y:32901,varname:node_5250,prsc:2,sctp:2;n:type:ShaderForge.SFN_Add,id:9991,x:32674,y:32800,varname:node_9991,prsc:2|A-5250-UVOUT,B-5964-OUT;n:type:ShaderForge.SFN_Multiply,id:213,x:32674,y:33047,varname:node_213,prsc:2|A-5799-OUT,B-9411-OUT,C-5801-A;n:type:ShaderForge.SFN_Multiply,id:8349,x:32619,y:33210,varname:node_8349,prsc:2|A-5557-OUT,B-6696-OUT,C-5801-A;n:type:ShaderForge.SFN_ScreenParameters,id:8090,x:32548,y:33456,varname:node_8090,prsc:2;n:type:ShaderForge.SFN_Reciprocal,id:5799,x:32700,y:33391,varname:node_5799,prsc:2|IN-8090-PXW;n:type:ShaderForge.SFN_Reciprocal,id:6696,x:32837,y:33465,varname:node_6696,prsc:2|IN-8090-PXH;proporder:5801-9411-5557;pass:END;sub:END;*/

Shader "Shader Forge/LocalizedShake" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _xOffset ("xOffset", Float ) = 0
        _yOffset ("yOffset", Float ) = 0
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent+1"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _xOffset;
            uniform float _yOffset;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 emissive = tex2D( _GrabTexture, (sceneUVs.rg+float2(((1.0 / _ScreenParams.r)*_xOffset*_MainTex_var.a),(_yOffset*(1.0 / _ScreenParams.g)*_MainTex_var.a)))).rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
