float4x4 View;
float4x4 Projection;
float time;

texture tex;
sampler samp = sampler_state{ Texture = tex; };

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

float wave(float x){
	x = frac(x * 0.5) * 2;
	if (x < 1) return -2 * x * (x - 1) + 0.5;
	return (x - 1) * (x - 2) * 2 + 0.5;
}

float4 PS(VertexShaderOutput input) : COLOR0
{
	float2 coords = input.Coords * 10;
	coords.xy += time;
	//coords.x += sin(time + coords.y * 0.25) * 0.5;
	//coords.y += cos(time + coords.x * 0.25) * 0.5;
	coords.x += wave((time + coords.y * 0.25) / 3.14) - 0.5;
	coords.y += wave((time + coords.x * 0.25) / 3.14) - 0.5;
	return tex2D(samp, coords);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VS();
		PixelShader = compile ps_2_0 PS();
	}
}
