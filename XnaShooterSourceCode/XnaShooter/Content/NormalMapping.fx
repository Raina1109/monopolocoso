// Project: XnaShooter, File: NormalMapping.fx
// Creation date: 01.11.2005 04:56
// Last modified: 03.12.2006 03:51
// Author: Benjamin Nitschke (abi@exdream.com) (c) 2005
// Note: For testing FX Composer from NVIDIA can be used

string description = "Normal mapping shaders for XnaShooter";

// Shader techniques in this file, all shaders usally work with vs/ps 1.1,
// but to make the file a little bit cleaner the ps1.1 shader was left out here.
// Please check out the full shader in the next chapters.
// Specular20         : Nicer effect for ps20, also required for 3DS max to show shader!

// Default variables, supported by the engine (may not be used here)
// If you don't need any global variable, just comment it out, this way
// the game engine does not have to set it!
float4x4 viewProj         : ViewProjection;
float4x4 world            : World;
float4x4 viewInverse      : ViewInverse;

float3 lightDir : Direction
<
	string UIName = "Light Direction";
	string Object = "DirectionalLight";
	string Space = "World";
> = {-0.65f, 0.65f, -0.39f}; // Normalized by app. FxComposer still uses inverted stuff :(

// The ambient, diffuse and specular colors are pre-multiplied with the light color!
float4 ambientColor : Ambient
<
	string UIName = "Ambient Color";
	string Space = "material";
> = {0.1f, 0.1f, 0.1f, 1.0f};
//> = {0.25f, 0.25f, 0.25f, 1.0f};

float4 diffuseColor : Diffuse
<
	string UIName = "Diffuse Color";
	string Space = "material";
> = {1.0f, 1.0f, 1.0f, 1.0f};

float4 specularColor : Specular
<
	string UIName = "Specular Color";
	string Space = "material";
> = {1.0f, 1.0f, 1.0f, 1.0f};

float shininess : SpecularPower
<
	string UIName = "Specular Power";
	string UIWidget = "slider";
	float UIMin = 1.0;
	float UIMax = 128.0;
	float UIStep = 1.0;
> = 16.0;

// Texture and samplers
texture diffuseTexture : Diffuse
<
	string UIName = "Diffuse Texture";
	string ResourceName = "asteroid4.dds";
>;
sampler diffuseTextureSampler = sampler_state
{
	Texture = <diffuseTexture>;
	AddressU  = Wrap;//Clamp;
	AddressV  = Wrap;//Clamp;
	AddressW  = Wrap;//Clamp;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

texture normalTexture : Diffuse
<
	string UIName = "Normal Texture";
	string ResourceName = "asteroid4Normal.dds";
>;
sampler normalTextureSampler = sampler_state
{
	Texture = <normalTexture>;
	AddressU  = Wrap;//Clamp;
	AddressV  = Wrap;//Clamp;
	AddressW  = Wrap;//Clamp;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

//----------------------------------------------------

// Vertex input structure (used for ALL techniques here!)
struct VertexInput
{
	float3 pos      : POSITION;
	float2 texCoord : TEXCOORD0;
	float3 normal   : NORMAL;
	float3 tangent	: TANGENT;
};

//----------------------------------------------------

// Common functions
float4 TransformPosition(float3 pos)//float4 pos)
{
	return mul(mul(float4(pos.xyz, 1), world), viewProj);
} // TransformPosition(.)

float3 GetWorldPos(float3 pos)
{
	return mul(float4(pos, 1), world).xyz;
} // GetWorldPos(.)

float3 GetCameraPos()
{
	return viewInverse[3].xyz;
} // GetCameraPos()

float3 CalcNormalVector(float3 nor)
{
	return normalize(mul(nor, (float3x3)world));//worldInverseTranspose));
} // CalcNormalVector(.)

// Get light direction
float3 GetLightDir()
{
	return lightDir;
} // GetLightDir()
	
float3x3 ComputeTangentMatrix(float3 tangent, float3 normal)
{
	// Compute the 3x3 tranform from tangent space to object space
	float3x3 worldToTangentSpace;
	worldToTangentSpace[0] =
		mul(cross(normal, tangent), world);
	worldToTangentSpace[1] = mul(tangent, world);
	worldToTangentSpace[2] = mul(normal, world);
	return worldToTangentSpace;
} // ComputeTangentMatrix(..)

//----------------------------------------------------

// vertex shader output structure
struct VertexOutput_Specular20
{
	float4 pos          : POSITION;
	float2 texCoord     : TEXCOORD0;
	float3 lightVec     : TEXCOORD1;
	float3 viewVec      : TEXCOORD2;
};

// Vertex shader function
VertexOutput_Specular20 VS_Specular20(VertexInput In)
{
	VertexOutput_Specular20 Out = (VertexOutput_Specular20) 0; 
	Out.pos = TransformPosition(In.pos);
	// We can duplicate texture coordinates for diffuse and normal map
	// in the pixel shader 2.0. For Pixel shader 1.1 we have to do that
	// in the vertex shader instead and pass it over.
	Out.texCoord = In.texCoord;

	// Compute the 3x3 tranform from tangent space to object space
	float3x3 worldToTangentSpace =
		ComputeTangentMatrix(In.tangent, In.normal);

	float3 worldEyePos = GetCameraPos();
	float3 worldVertPos = GetWorldPos(In.pos);

	// Transform light vector and pass it as a color (clamped from 0 to 1)
	// For ps_2_0 we don't need to clamp form 0 to 1
	Out.lightVec = normalize(mul(worldToTangentSpace, GetLightDir()));
	Out.viewVec = mul(worldToTangentSpace, worldEyePos - worldVertPos);

	// And pass everything to the pixel shader
	return Out;
} // VS_Specular20(.)

// Pixel shader function
float4 PS_Specular20(VertexOutput_Specular20 In) : COLOR
{
	// Grab texture data
	float4 diffuseTexture = tex2D(diffuseTextureSampler, In.texCoord);
	float3 normalVector = (2.0 * tex2D(normalTextureSampler, In.texCoord).agb) - 1.0;
	// Normalize normal to fix blocky errors
	normalVector = normalize(normalVector);

	// Additionally normalize the vectors
	float3 lightVector = In.lightVec;//not needed: normalize(In.lightVec);
	float3 viewVector = normalize(In.viewVec);
	// For ps_2_0 we don't need to unpack the vectors to -1 - 1

	// Compute the angle to the light
	float bump = saturate(dot(normalVector, lightVector));
	// Specular factor
	float3 reflect = normalize(2 * bump * normalVector - lightVector);
	float spec = pow(saturate(dot(reflect, viewVector)), shininess);
	//return spec;

	float4 ambDiffColor = ambientColor + bump * diffuseColor;
	return diffuseTexture * ambDiffColor +
		bump * spec * specularColor * diffuseTexture.a;
} // PS_Specular20(.)

// Techniques
technique Specular20
{
	pass P0
	{
		VertexShader = compile vs_1_1 VS_Specular20();
		PixelShader  = compile ps_2_0 PS_Specular20();
	} // pass P0
} // Specular20
