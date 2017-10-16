float4x4 View;
float4x4 Projection;
float time;

texture colors;
sampler samp = sampler_state{ Texture = colors; };
float2 size;

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

VertexShaderOutput VS(VertexShaderInput input)
{
	VertexShaderOutput output;

	output.Position = mul(mul(input.Position, View), Projection);
	output.Coords = input.Position * 0.3;

	return output;
}

float4 PS(VertexShaderOutput input) : COLOR0
{
	float4 c = float4(size * min(sqrt(input.Coords), sqrt(1 - input.Coords)), 0, 0);
	float4 d = float4(0, 0, 0, 0);
	float4 e;
	[unroll]
	for (int i = 0; i < 4; i++)
	{
		e = floor(c);
		d += abs(sin(e * e.yxyx + e * time)) / 10;
		c *= 0.3;
	}
	d.x += d.y;
	return float4(tex2D(samp, d.xy).xyz * 0.6, 1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader = compile ps_3_0 PS();
	}
}
