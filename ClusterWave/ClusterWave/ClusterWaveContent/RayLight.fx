float4x4 View;
float4x4 Projection;
float2 lightPos;

struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
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

    output.Position = mul(mul(pos, View), Projection);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return float4(0, 0, 0, 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
