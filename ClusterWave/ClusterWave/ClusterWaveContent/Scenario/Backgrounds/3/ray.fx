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

float rand(float2 n) {
	return frac(cos(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
}

float noise(float2 n) {
	const float2 d = float2(0.0, 1.0);
	float2 b = floor(n), f = smoothstep(float2(0, 0), float2(1, 1), frac(n));
	return lerp(lerp(rand(b), rand(b + d.yx), f.x), lerp(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
}

float fbm(float2 n) {
	float total = 0.0, amplitude = 1.0;
	for (int i = 0; i < 4; i++) {
		total += noise(n) * amplitude;
		n += n;
		amplitude *= 0.5;
	}
	return total;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	if (input.Coords.x > 0 && input.Coords.y > 0 && input.Coords.x < size.x && input.Coords.y < size.y)
	{
		return float4(0.1, 0, 0.1, 1);
		//return float4(lerp(float3(0, 0, 1), float3(1, 0, 1), sin(input.Coords.x + time)*0.5+0.5), 1);

		/*const float3 c1 = float3(0.5, 0.0, 0.1);
		const float3 c2 = float3(0.9, 0.0, 0.0);
		const float3 c3 = float3(0.2, 0.0, 0.0);
		const float3 c4 = float3(1.0, 0.9, 0.0);
		const float3 c5 = float3(0.1, 0.1, 0.1);
		const float3 c6 = float3(0.9, 0.9, 0.9);

		float2 speed = float2(0.7, 0.4);
		float shift = 1.6;
		float alpha = 1.0;

		float2 p = input.Coords * 8.0 / size.xx;
		float q = fbm(p - time * 0.1);

		float2 r = float2(fbm(p + q + time * speed.x - p.x - p.y), fbm(p + q - time * speed.y));
		float3 c = lerp(c1, c2, fbm(p + r)) + lerp(c3, c4, r.x) - lerp(c5, c6, r.y);
		return float4(c * cos(shift * input.Coords.y / size.y), alpha);*/

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
