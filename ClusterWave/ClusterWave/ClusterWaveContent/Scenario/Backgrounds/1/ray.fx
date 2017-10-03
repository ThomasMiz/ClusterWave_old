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

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	if (input.Coords.x > 0 && input.Coords.y > 0 && input.Coords.x < size.x && input.Coords.y < size.y)
		return float4(0, 0, 0, 1); //if inside scenario bounds
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
