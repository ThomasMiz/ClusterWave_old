float4x4 Proj;

float2 pix;
texture tex;
sampler samp = sampler_state { Texture = tex; };

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
	output.Coords = input.Coords;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 color = 0.01234568 * tex2D(samp, input.Coords + pix * float2(-2, -2));
	color += 0.02469136 * tex2D(samp, input.Coords + pix * float2(-1, -2));
	color += 0.03703704 * tex2D(samp, input.Coords + pix * float2(0, -2));
	color += 0.02469136 * tex2D(samp, input.Coords + pix * float2(1, -2));
	color += 0.01234568 * tex2D(samp, input.Coords + pix * float2(2, -2));
	color += 0.02469136 * tex2D(samp, input.Coords + pix * float2(-2, -1));
	color += 0.04938272 * tex2D(samp, input.Coords + pix * float2(-1, -1));
	color += 0.07407407 * tex2D(samp, input.Coords + pix * float2(0, -1));
	color += 0.04938272 * tex2D(samp, input.Coords + pix * float2(1, -1));
	color += 0.02469136 * tex2D(samp, input.Coords + pix * float2(2, -1));
	color += 0.03703704 * tex2D(samp, input.Coords + pix * float2(-2, 0));
	color += 0.07407407 * tex2D(samp, input.Coords + pix * float2(-1, 0));
	color += 0.1111111 * tex2D(samp, input.Coords + pix * float2(0, 0));
	color += 0.07407407 * tex2D(samp, input.Coords + pix * float2(1, 0));
	color += 0.03703704 * tex2D(samp, input.Coords + pix * float2(2, 0));
	color += 0.02469136 * tex2D(samp, input.Coords + pix * float2(-2, 1));
	color += 0.04938272 * tex2D(samp, input.Coords + pix * float2(-1, 1));
	color += 0.07407407 * tex2D(samp, input.Coords + pix * float2(0, 1));
	color += 0.04938272 * tex2D(samp, input.Coords + pix * float2(1, 1));
	color += 0.02469136 * tex2D(samp, input.Coords + pix * float2(2, 1));
	color += 0.01234568 * tex2D(samp, input.Coords + pix * float2(-2, 2));
	color += 0.02469136 * tex2D(samp, input.Coords + pix * float2(-1, 2));
	color += 0.03703704 * tex2D(samp, input.Coords + pix * float2(0, 2));
	color += 0.02469136 * tex2D(samp, input.Coords + pix * float2(1, 2));
	color += 0.01234568 * tex2D(samp, input.Coords + pix * float2(2, 2));

	return color;

	//return tex2D(samp, input.Coords);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
