float4x4 World;
float4x4 Proj;

texture colors;
sampler samp = sampler_state{ Texture = colors; };
float time;
float2 size;

float2 points[5];

struct VertexShaderInput
{
	float4 Pos : POSITION0;
	float4 Color : COLOR0;
	float2 Coords : TEXCOORD0;
};

struct PixelShaderInput
{
	float4 Pos : POSITION0;
	float4 Color : COLOR0;
	float2 Coords : TEXCOORD0;
};

PixelShaderInput VertexShaderFunction(VertexShaderInput input)
{
	PixelShaderInput ret;
	ret.Pos = mul(mul(input.Pos, World), Proj);//mul(mul(mul(input.Pos, World), View), Proj);
	ret.Color = input.Color;
	ret.Coords = input.Coords;
	return ret;
}

float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
	float2 coords = input.Coords * size;
	float2 fp = coords;
	
	for (int i = 0; i < 5; i++){
		float dis = 1.5 * distance(fp, points[i]);
		float wav = sin(dis * 0.1 + time) * 0.5 + 0.5;
		coords.xy += wav * 100.0 * sin(dis * 0.1 + time * 2.5);
	}
	coords /= size;
	return tex2D(samp, coords);
}

technique Technique1
{
    pass Pass1
    {
		VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
