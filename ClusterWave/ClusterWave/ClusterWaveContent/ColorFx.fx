float4x4 World;
float4x4 View;
float4x4 Projection;

float time;

texture colors;
sampler samp = sampler_state{ Texture = colors; };

float alpha;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	//float4 Color : COLOR0;
	float2 Coords : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	//float4 Color : COLOR0;
	float2 Coords : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	//output.Color = input.Color;
	
	output.Coords.x = ((output.Position.x + 1.0) / 2.0) * 7.5;
	output.Coords.y = ((output.Position.y + 1.0) / 2.0) * 7.5;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 col = tex2D(samp, input.Coords);
	col.a = alpha;
	return col;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
