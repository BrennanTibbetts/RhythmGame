float4 Gradient(float2 uv)
{
    return lerp(color1, color2, uv.x);
}