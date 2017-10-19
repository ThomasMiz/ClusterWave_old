float4x4 World;

float time;
float2 pos;
float2 size;
float scale;

float2 redOff;
float2 greenOff;
float2 blueOff;

float multiply;

texture tex;
sampler samp = sampler_state{ Texture = tex; };

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 Coords : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 Coords : TEXCOORD0;
	float2 Pos : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	output.Coords = input.Coords;
	output.Position = mul(input.Position, World);
	output.Pos = input.Coords * size;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float2 Coords = input.Coords;
	float dis = 0.75*multiply*scale*distance(input.Pos, pos);
	float mult = 2.0 - dis;
	if (mult > 0)
	{
		float wav = sin(mad(time, -2, dis * 16.0));
		float2 lel = normalize(input.Pos - pos);
		Coords.xy += lel * wav * mult / 2.0 * 0.002;
		float cd = mult * 0.005;

		float4 me = tex2D(samp, Coords);

		return lerp(me, float4(tex2D(samp, Coords + cd * redOff).r, tex2D(samp, Coords + cd * greenOff).g, tex2D(samp, Coords + cd * blueOff).b, 1), 0.2);
	}
	return tex2D(samp, Coords);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
