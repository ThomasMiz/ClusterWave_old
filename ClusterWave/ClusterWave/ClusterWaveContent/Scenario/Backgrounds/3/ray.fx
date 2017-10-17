float4x4 View;
float4x4 Projection;
float time;
float2 lightPos;
float2 size;

texture noise;
sampler samp = sampler_state { Texture = noise; };

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 Coords : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4 pos = input.Position;
		if (pos.z != 0){
			pos.xy -= lightPos;
			pos.xy *= 9999;
			pos.xy += lightPos;
		}

	output.Coords = pos;
	output.Position = mul(mul(pos, View), Projection);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	if (input.Coords.x > 0 && input.Coords.y > 0 && input.Coords.x < size.x && input.Coords.y < size.y)
	{ //if inside scenario bounds

		float2 c = input.Coords;
		c = mad(c, float2(0.125, 7.61), time * float2(57.1512, 14.512));
		float4 f = tex2D(samp, c);

		f.xyz *= 0.0625;
		float tmp = input.Coords.x - lightPos.x;
		f.rg *= sin(input.Coords.y * 64 - tmp * tmp + floor(input.Coords.x)) * 0.2 + 0.6;
		return f;
	}
	return float4(0, 0, 0, 0);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
