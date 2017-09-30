float4x4 World;
float4x4 View;
float4x4 Projection;
float value;

texture shield;
sampler samp = sampler_state{ Texture = shield; };

texture colors;
sampler colorsamp = sampler_state{ Texture = colors; };

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 Coords : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 Coords : TEXCOORD0;
	float2 Other : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.Coords = input.Coords;

	output.Other.x = ((output.Position.x + 1.0) / 2.0) * 7.5;
	output.Other.y = ((output.Position.y + 1.0) / 2.0) * 7.5;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return lerp(tex2D(colorsamp, input.Other), tex2D(samp, input.Coords), value);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
