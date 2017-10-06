float4x4 View;
float4x4 Projection;
float time;
float2 size;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Pos : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Pos : TEXCOORD0;
};

VertexShaderOutput VS(VertexShaderInput input)
{
    VertexShaderOutput output;
	
	output.Pos = float4(input.Pos.rg / size, input.Pos.b, input.Pos.a);
    output.Position = mul(mul(input.Position, View), Projection);

    return output;
}

float4 ColorPS(VertexShaderOutput input) : COLOR0
{
	float v = frac((input.Pos.x + input.Pos.y - time * 0.1) * 5);
	float4 color = float4(v,v,v,1);
	return color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 ColorPS();
    }
}
