float4x4 Proj;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 Coords : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 Coords : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = mul(input.Position, Proj);
	output.Coords = input.Coords * 6.283185 * 4;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return 0.6 * float4(sin(input.Coords.x + input.Coords.y * 0.5)*0.5+0.5, sin(input.Coords.y + input.Coords.x * 0.5), 1, 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
