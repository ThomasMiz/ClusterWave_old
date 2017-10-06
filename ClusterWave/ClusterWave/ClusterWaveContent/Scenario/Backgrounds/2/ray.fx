float4x4 View;
float4x4 Projection;
float time;
float2 lightPos;
float2 size;

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

float wave(float x){
	x = frac(x * 0.5) * 2;
	if (x < 1) return -4 * x * (x - 1);
	return (x - 1) * (x - 2) * 4;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	if (input.Coords.x > 0 && input.Coords.y > 0 && input.Coords.x < size.x && input.Coords.y < size.y)
	{ //if inside scenario bounds
		float2 coords = input.Coords;
		float n = 0;
		n += wave(time * 0.841 + coords.x * 0.312 + coords.y * 0.409) * 0.1;
		n += wave(time * 0.646 + coords.x * -0.468 + coords.y * 0.549) * 0.1;
		n += wave(time * 0.983 + coords.x * 0.765 + coords.y * -0.232) * 0.1;
		n += wave(time * 1.046 + coords.y * -0.441) * 0.1;
		n += wave(time * -0.718 + coords.y * 0.275) * 0.1;
		n += wave(time * 0.812 + coords.y * 0.915) * 0.1;

		n = n * 0.6 + 0.2;
		if (n < 0.1) n = 0.1;

		return float4(n, n, n, 1);
	}
	return float4(0, 0, 0, 0);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
