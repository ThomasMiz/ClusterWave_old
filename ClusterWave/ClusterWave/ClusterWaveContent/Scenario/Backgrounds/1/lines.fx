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
	//output.Position = mul(mul(float4(input.Position.x + sin(time + input.Position.x) * 15, input.Position.y + sin(time + input.Position.y) * 15, input.Position.z, input.Position.w), View), Projection);


    return output;
}

float4 ColorPS(VertexShaderOutput input) : COLOR0
{
	//input.Pos.x += sin(time+input.Pos.x * 3) * 0.2;
	//input.Pos.y += sin(time + input.Pos.y * 3 + 3.14159265) * 0.2;
	float H = frac((input.Pos.x + input.Pos.y - time * 0.1) * 5);
	float4 color = float4(abs(H * 6 - 3) - 1, 2 - abs(H * 6 - 2), 2 - abs(H * 6 - 4), input.Pos.a);
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
