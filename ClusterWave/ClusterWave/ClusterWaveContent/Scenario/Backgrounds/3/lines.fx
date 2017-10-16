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
	
	output.Pos = float4(input.Pos.rg / size, input.Pos.b, input.Pos.a) * 70;
	output.Position = mul(mul(input.Position, View), Projection);

    return output;
}

float4 ColorPS(VertexShaderOutput input) : COLOR0
{
	return float4(lerp(float3(0, 1, 1), float3(1, 0, 1), sin(input.Pos.x + time)*0.5 + 0.5), 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VS();
        PixelShader = compile ps_2_0 ColorPS();
    }
}
