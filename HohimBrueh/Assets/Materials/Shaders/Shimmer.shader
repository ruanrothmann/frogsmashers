// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.33 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.33;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:1,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:0,qpre:3,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-2827-RGB;n:type:ShaderForge.SFN_SceneColor,id:2827,x:32992,y:32642,varname:node_2827,prsc:2|UVIN-9991-OUT;n:type:ShaderForge.SFN_Append,id:5964,x:33051,y:33122,varname:node_5964,prsc:2|A-3967-OUT,B-909-OUT;n:type:ShaderForge.SFN_ScreenPos,id:5250,x:32367,y:32826,varname:node_5250,prsc:2,sctp:2;n:type:ShaderForge.SFN_Add,id:9991,x:32856,y:32949,varname:node_9991,prsc:2|A-5250-UVOUT,B-5964-OUT;n:type:ShaderForge.SFN_Multiply,id:213,x:32565,y:33228,varname:node_213,prsc:2|A-5799-OUT,B-4829-R;n:type:ShaderForge.SFN_Multiply,id:8349,x:32565,y:33386,varname:node_8349,prsc:2|A-4829-G,B-6696-OUT;n:type:ShaderForge.SFN_ScreenParameters,id:8090,x:31930,y:33410,varname:node_8090,prsc:2;n:type:ShaderForge.SFN_Reciprocal,id:5799,x:32149,y:33312,varname:node_5799,prsc:2|IN-8090-PXW;n:type:ShaderForge.SFN_Reciprocal,id:6696,x:32149,y:33433,varname:node_6696,prsc:2|IN-8090-PXH;n:type:ShaderForge.SFN_Tex2d,id:4829,x:32495,y:32658,ptovrint:False,ptlb:Noise1,ptin:_Noise1,varname:node_4829,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b053b1a9cc7eba04d92eea86f438be04,ntxv:3,isnm:True|UVIN-2527-OUT;n:type:ShaderForge.SFN_Tex2d,id:9264,x:32712,y:32585,ptovrint:False,ptlb:Noise2,ptin:_Noise2,varname:_Noise2,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:af36616b3443f2d47906d34740c5f24b,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Time,id:1530,x:31719,y:33162,varname:node_1530,prsc:2;n:type:ShaderForge.SFN_Append,id:5805,x:32118,y:33065,varname:node_5805,prsc:2|A-7817-OUT,B-3928-OUT;n:type:ShaderForge.SFN_TexCoord,id:8712,x:31783,y:32861,varname:node_8712,prsc:2,uv:0;n:type:ShaderForge.SFN_Add,id:2527,x:32218,y:32576,varname:node_2527,prsc:2|A-8712-UVOUT,B-5805-OUT;n:type:ShaderForge.SFN_ValueProperty,id:694,x:32133,y:32480,ptovrint:False,ptlb:ySpeed,ptin:_ySpeed,varname:_WarpAmount_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:3971,x:31946,y:32526,ptovrint:False,ptlb:xSpeed,ptin:_xSpeed,varname:_WarpAmount_copy_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:7817,x:32007,y:32927,varname:node_7817,prsc:2|A-1530-TSL,B-3971-OUT;n:type:ShaderForge.SFN_Multiply,id:3928,x:31981,y:33190,varname:node_3928,prsc:2|A-694-OUT,B-1530-TSL;n:type:ShaderForge.SFN_ValueProperty,id:4587,x:32890,y:33466,ptovrint:False,ptlb:xAmount,ptin:_xAmount,varname:node_4587,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:278,x:32929,y:33614,ptovrint:False,ptlb:yAmount,ptin:_yAmount,varname:_xAmount_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:3967,x:33185,y:33503,varname:node_3967,prsc:2|A-4587-OUT,B-213-OUT;n:type:ShaderForge.SFN_Multiply,id:909,x:33070,y:33283,varname:node_909,prsc:2|A-278-OUT,B-8349-OUT;proporder:4829-3971-694-4587-278;pass:END;sub:END;*/

Shader "Shader Forge/Shimmer" {
    Properties {
        _Noise1 ("Noise1", 2D) = "bump" {}
        _xSpeed ("xSpeed", Float ) = 1
        _ySpeed ("ySpeed", Float ) = 1
        _xAmount ("xAmount", Float ) = 0
        _yAmount ("yAmount", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Opaque"
            "DisableBatching"="True"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        GrabPass{ "Refraction" }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D Refraction;
            uniform float4 _TimeEditor;
            uniform sampler2D _Noise1; uniform float4 _Noise1_ST;
            uniform float _ySpeed;
            uniform float _xSpeed;
            uniform float _xAmount;
            uniform float _yAmount;
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
                float4 sceneColor = tex2D(Refraction, sceneUVs);
////// Lighting:
////// Emissive:
                float4 node_1530 = _Time + _TimeEditor;
                float2 node_2527 = (i.uv0+float2((node_1530.r*_xSpeed),(_ySpeed*node_1530.r)));
                float3 _Noise1_var = UnpackNormal(tex2D(_Noise1,TRANSFORM_TEX(node_2527, _Noise1)));
                float3 emissive = tex2D( Refraction, (sceneUVs.rg+float2((_xAmount*((1.0 / _ScreenParams.r)*_Noise1_var.r)),(_yAmount*(_Noise1_var.g*(1.0 / _ScreenParams.g)))))).rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
