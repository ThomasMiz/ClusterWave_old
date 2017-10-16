float4x4 Proj;
float2 pix;
float time;

texture prev;
sampler samp = sampler_state { Texture = prev; };

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
	output.Coords = input.Coords + pix * 0.5;

	return output;
}

float rand(float2 pos){
	pos = (pos - .5) * 50.0 - .5;
	return frac(81.583*time*pos.x+13758.0*pos.y+frac(pos.x*pos.x*pos.y*pos.y));
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	//return float4(0, 0, 0.1, 1);
	float rot = rand(input.Coords) * 6.283185;
	float2 oc = float2(cos(rot), sin(rot)) * pix + input.Coords;
	float4 other = tex2D(samp, oc);

	return other;
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}
