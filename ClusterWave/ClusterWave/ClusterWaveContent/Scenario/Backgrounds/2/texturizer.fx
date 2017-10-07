float4x4 View;
float4x4 Projection;
float time;

texture tex;
sampler samp = sampler_state{ Texture = tex; };

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 Coords : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
	float2 Coords : TEXCOORD0;
};

VertexShaderOutput VS(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = mul(mul(input.Position, View), Projection);

	output.Color = input.Color;
	output.Coords = input.Position * 0.3;

    return output;
}

float wave(float x){
	x = frac(x * 0.5) * 2;
	if (x < 1) return -2 * x * (x - 1) + 0.5;
	return (x - 1) * (x - 2) * 2 + 0.5;
}

float4 PS(VertexShaderOutput input) : COLOR0
{
	float2 c = input.Coords;
	c.x = c.x * 7.61 + time * 39.612;
	c.y = c.y * 7.61 + time * 74.512;
	float4 f = tex2D(samp, c);
		f.xyz *= 0.5;
	f.xyz -= 0.25;
	return f;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 PS();
    }
}
