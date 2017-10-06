float4x4 View;
float4x4 Projection;
float time;

texture tex;
sampler samp = sampler_state{ Texture = tex; };

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 Coord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 Coord : TEXCOORD0;
};

VertexShaderOutput VS(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = mul(mul(input.Position, View), Projection);

	output.Color = input.Color;
	output.Coord = input.Position * 0.3;

    return output;
}

float wave(float x){
	x = frac(x * 0.5) * 2;
	if (x < 1) return -2 * x * (x - 1) + 0.5;
	return (x - 1) * (x - 2) * 2 + 0.5;
}

float4 PS(VertexShaderOutput input) : COLOR0
{
	float2 coords = input.Coord * 1;
	coords.xy += time * 0.1;
	return tex2D(samp, coords) * input.Color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS();
    }
}
