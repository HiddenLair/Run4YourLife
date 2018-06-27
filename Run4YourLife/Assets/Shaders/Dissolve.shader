// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:867,x:33595,y:33060,varname:node_867,prsc:2|diff-8164-OUT,spec-9554-OUT,normal-2470-RGB,emission-3160-OUT,amdfl-8622-OUT,clip-4786-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:8870,x:32601,y:33109,ptovrint:False,ptlb:Ramp,ptin:_Ramp,varname:node_8870,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:61a759b0593c6124b80f97cd037802bb,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:2357,x:32754,y:33043,varname:node_2357,prsc:2,tex:61a759b0593c6124b80f97cd037802bb,ntxv:0,isnm:False|UVIN-534-OUT,TEX-8870-TEX;n:type:ShaderForge.SFN_Append,id:534,x:32614,y:32932,varname:node_534,prsc:2|A-4053-OUT,B-3353-OUT;n:type:ShaderForge.SFN_Vector1,id:3353,x:32421,y:33033,varname:node_3353,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2d,id:3241,x:31641,y:33086,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_3241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b395866dd69988647886ca45a7033d56,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:4754,x:31119,y:32920,ptovrint:False,ptlb:Dissolve amout,ptin:_Dissolveamout,varname:node_4754,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_RemapRange,id:3705,x:31689,y:32881,varname:node_3705,prsc:2,frmn:0,frmx:1,tomn:-0.6,tomx:0.6|IN-6029-OUT;n:type:ShaderForge.SFN_Add,id:4786,x:31844,y:33073,varname:node_4786,prsc:2|A-3705-OUT,B-3241-R;n:type:ShaderForge.SFN_OneMinus,id:6029,x:31499,y:32881,varname:node_6029,prsc:2|IN-4754-OUT;n:type:ShaderForge.SFN_RemapRange,id:5501,x:32019,y:32914,varname:node_5501,prsc:2,frmn:0,frmx:1,tomn:-4,tomx:4|IN-4786-OUT;n:type:ShaderForge.SFN_Clamp01,id:2735,x:32221,y:32896,varname:node_2735,prsc:2|IN-5501-OUT;n:type:ShaderForge.SFN_OneMinus,id:4053,x:32416,y:32884,varname:node_4053,prsc:2|IN-2735-OUT;n:type:ShaderForge.SFN_Tex2d,id:9155,x:33502,y:32583,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_9155,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:57ccefeba1aad964ab5aaa18f7672877,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Slider,id:9554,x:33120,y:32187,ptovrint:False,ptlb:Smoothness,ptin:_Smoothness,varname:node_9554,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Tex2d,id:2470,x:33120,y:32289,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_2470,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:620ca9d03cebc7e428aa23723950870c,ntxv:3,isnm:True;n:type:ShaderForge.SFN_VertexColor,id:4303,x:33502,y:32754,varname:node_4303,prsc:2;n:type:ShaderForge.SFN_Multiply,id:860,x:33709,y:32729,varname:node_860,prsc:2|A-9155-RGB,B-4303-RGB;n:type:ShaderForge.SFN_AmbientLight,id:5730,x:32767,y:33200,varname:node_5730,prsc:2;n:type:ShaderForge.SFN_Multiply,id:248,x:32963,y:33150,varname:node_248,prsc:2|A-2357-RGB,B-5730-RGB;n:type:ShaderForge.SFN_LightAttenuation,id:1228,x:32964,y:33768,varname:node_1228,prsc:2;n:type:ShaderForge.SFN_LightColor,id:3989,x:32964,y:33644,varname:node_3989,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8622,x:33166,y:33695,varname:node_8622,prsc:2|A-3989-RGB,B-1228-OUT,C-8461-OUT;n:type:ShaderForge.SFN_LightVector,id:6554,x:32894,y:33352,varname:node_6554,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:7380,x:32894,y:33475,prsc:2,pt:True;n:type:ShaderForge.SFN_Dot,id:4748,x:33070,y:33399,varname:node_4748,prsc:2,dt:1|A-6554-OUT,B-7380-OUT;n:type:ShaderForge.SFN_Multiply,id:8461,x:33293,y:33308,varname:node_8461,prsc:2|A-9155-RGB,B-4748-OUT;n:type:ShaderForge.SFN_Color,id:6913,x:33519,y:32405,ptovrint:False,ptlb:Diffuse Color,ptin:_DiffuseColor,varname:node_6913,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:8164,x:34017,y:32606,varname:node_8164,prsc:2|A-6913-RGB,B-9155-RGB;n:type:ShaderForge.SFN_Tex2d,id:7457,x:32387,y:32356,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_7457,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False|UVIN-2065-UVOUT;n:type:ShaderForge.SFN_Multiply,id:6342,x:32872,y:32659,varname:node_6342,prsc:2|A-1634-RGB,B-7457-R,C-3373-OUT,D-8560-OUT;n:type:ShaderForge.SFN_Color,id:1634,x:32661,y:32146,ptovrint:False,ptlb:EmisionColor,ptin:_EmisionColor,varname:node_1634,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:3373,x:32103,y:32538,ptovrint:False,ptlb:EmissionForce,ptin:_EmissionForce,varname:node_3373,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Add,id:3160,x:33217,y:33062,varname:node_3160,prsc:2|A-6342-OUT,B-248-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:8560,x:32532,y:32755,ptovrint:False,ptlb:UseEmision,ptin:_UseEmision,varname:node_8560,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True;n:type:ShaderForge.SFN_TexCoord,id:2065,x:32147,y:32230,varname:node_2065,prsc:2,uv:0,uaff:False;proporder:4754-9554-9155-6913-2470-3241-8870-8560-7457-1634-3373;pass:END;sub:END;*/

Shader "Custom/Dissolve" {
    Properties {
        _Dissolveamout ("Dissolve amout", Range(0, 1)) = 0
        _Smoothness ("Smoothness", Range(0, 1)) = 0
        _Diffuse ("Diffuse", 2D) = "black" {}
        _DiffuseColor ("Diffuse Color", Color) = (1,1,1,1)
        _Normal ("Normal", 2D) = "bump" {}
        _Noise ("Noise", 2D) = "white" {}
        _Ramp ("Ramp", 2D) = "black" {}
        [MaterialToggle] _UseEmision ("UseEmision", Float ) = 1
        _Emission ("Emission", 2D) = "black" {}
        [HDR]_EmisionColor ("EmisionColor", Color) = (1,0,0,1)
        _EmissionForce ("EmissionForce", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Dissolveamout;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _Smoothness;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float4 _DiffuseColor;
            uniform sampler2D _Emission; uniform float4 _Emission_ST;
            uniform float4 _EmisionColor;
            uniform float _EmissionForce;
            uniform fixed _UseEmision;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise));
                float node_4786 = (((1.0 - _Dissolveamout)*1.2+-0.6)+_Noise_var.r);
                clip(node_4786 - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_Smoothness,_Smoothness,_Smoothness);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                indirectDiffuse += (_LightColor0.rgb*attenuation*(_Diffuse_var.rgb*max(0,dot(lightDirection,normalDirection)))); // Diffuse Ambient Light
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuseColor = (_DiffuseColor.rgb*_Diffuse_var.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 _Emission_var = tex2D(_Emission,TRANSFORM_TEX(i.uv0, _Emission));
                float2 node_534 = float2((1.0 - saturate((node_4786*8.0+-4.0))),0.0);
                float4 node_2357 = tex2D(_Ramp,TRANSFORM_TEX(node_534, _Ramp));
                float3 emissive = ((_EmisionColor.rgb*_Emission_var.r*_EmissionForce*_UseEmision)+(node_2357.rgb*UNITY_LIGHTMODEL_AMBIENT.rgb));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Dissolveamout;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _Smoothness;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform float4 _DiffuseColor;
            uniform sampler2D _Emission; uniform float4 _Emission_ST;
            uniform float4 _EmisionColor;
            uniform float _EmissionForce;
            uniform fixed _UseEmision;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise));
                float node_4786 = (((1.0 - _Dissolveamout)*1.2+-0.6)+_Noise_var.r);
                clip(node_4786 - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_Smoothness,_Smoothness,_Smoothness);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 diffuseColor = (_DiffuseColor.rgb*_Diffuse_var.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Dissolveamout;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float2 uv1 : TEXCOORD2;
                float2 uv2 : TEXCOORD3;
                float4 posWorld : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise));
                float node_4786 = (((1.0 - _Dissolveamout)*1.2+-0.6)+_Noise_var.r);
                clip(node_4786 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _Dissolveamout;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float _Smoothness;
            uniform float4 _DiffuseColor;
            uniform sampler2D _Emission; uniform float4 _Emission_ST;
            uniform float4 _EmisionColor;
            uniform float _EmissionForce;
            uniform fixed _UseEmision;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : SV_Target {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 _Emission_var = tex2D(_Emission,TRANSFORM_TEX(i.uv0, _Emission));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise));
                float node_4786 = (((1.0 - _Dissolveamout)*1.2+-0.6)+_Noise_var.r);
                float2 node_534 = float2((1.0 - saturate((node_4786*8.0+-4.0))),0.0);
                float4 node_2357 = tex2D(_Ramp,TRANSFORM_TEX(node_534, _Ramp));
                o.Emission = ((_EmisionColor.rgb*_Emission_var.r*_EmissionForce*_UseEmision)+(node_2357.rgb*UNITY_LIGHTMODEL_AMBIENT.rgb));
                
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 diffColor = (_DiffuseColor.rgb*_Diffuse_var.rgb);
                float3 specColor = float3(_Smoothness,_Smoothness,_Smoothness);
                o.Albedo = diffColor + specColor * 0.125; // No gloss connected. Assume it's 0.5
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
