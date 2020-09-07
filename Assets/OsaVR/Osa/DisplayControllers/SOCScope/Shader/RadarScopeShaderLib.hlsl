float RadarScope_AzimuthOffset(float2 Pos, float Azimuth)
{
    const float pi = 3.141592653589793238462;

    float2 dir = normalize(float2(Pos.x - 0.5f, Pos.y - 0.5f));
    float angle = acos(dot(normalize(float2(-1.f, 0.f)), dir));
    
    if (dir.y > 0)
    {
        angle = pi * 2 - angle;
    }

    angle = angle + Azimuth + pi * 2.5;
    if (angle > pi * 2)
    {
        float a = ceil(angle / (pi * 2));
        angle = angle - (a * pi * 2);
    }

    return abs(angle);
}

float DistanceToLine(float2 start, float2 end, float2 pos)
{
    float uv_start_dist = abs(distance(start, pos));
    float uv_end_dist = abs(distance(end, pos));
    float total_dist = abs(distance(start, end));

    if (uv_start_dist >= total_dist || uv_end_dist >= total_dist)
    {
        return -1.f;
    }
    else
    {
        float p = (total_dist + uv_start_dist + uv_end_dist) / 2;
        return 2 / total_dist * sqrt(p * (p - uv_start_dist) * (p - uv_end_dist) * (p - total_dist));
    }
}

void RadarScope_ScanlineFade_float(float4 In, float2 Pos, float Azimuth, out float4 Out)
{
    const float pi = 3.141592653589793238462;
    float factor = 3;
    Out = In * smoothstep(0.f, pow(pi * 2, factor), pow(RadarScope_AzimuthOffset(Pos, Azimuth), factor));
}

void RadarScope_RangeCircles_float(float2 UV, float RangeMarkerMargin, float RangeMarkerCount, float SOCAzimuth, out float4 Out)
{
    float dist = abs(distance(float2(0.5f, 0.5f), UV)) * 2;
    
    Out = float4(0.f, 0.f, 0.f, 1.f);

    float spacing = 1.f / (RangeMarkerCount);
    for (float i = RangeMarkerMargin; i <= 1.f; i += spacing)
    {
        float delta = abs(dist - i);
        float comp = lerp(1.f, 0.f, smoothstep(0.001f, 0.007f, delta));

        Out.rgb += float3(comp, comp, comp);
    }

    float2 pos = float2(0.5 + 0.5 * sin(SOCAzimuth), 0.5f + 0.5 * cos(SOCAzimuth));
    float soc_dist = DistanceToLine(float2(0.5, 0.5), pos, UV);

    if (soc_dist >= 0.f)
    {
        Out.rgb *= lerp(1.f, 0.1f, smoothstep(0.001f, 0.04f, soc_dist));
    }
    else
    {
        Out.rgb *= 0.1f;
    }
}

void SOCLine_float(float2 UV, float Azimuth, out float4 Out)
{
    float2 center = float2(0.5f, 0.5f);
    float2 pos = float2(center.x + 0.5 * sin(Azimuth), center.y + 0.5 * cos(Azimuth));

    float uv_start_dist = abs(distance(center, UV));
    float uv_end_dist = abs(distance(pos, UV));
    float total_dist = abs(distance(center, pos));

    if (uv_start_dist >= total_dist || uv_end_dist >= total_dist)
    {
        Out = float4(0.f, 0.f, 0.f, 1.f);
    }
    else
    {
        float p = (total_dist + uv_start_dist + uv_end_dist) / 2;
        float h = 2 / total_dist * sqrt(p * (p - uv_start_dist) * (p - uv_end_dist) * (p - total_dist));

        float r = lerp(1.f, 0.f, smoothstep(0.0015f, 0.002f, h));
        Out = float4(r, r, r, 1.f);
    }
}

void SSCLine_float(float2 UV, float Azimuth, float Distance, out float4 Out)
{
    float2 center = float2(0.5f, 0.5f);
    float2 pos = float2(center.x + 0.5 * sin(Azimuth), center.y + 0.5 * cos(Azimuth));

    float uv_start_dist = abs(distance(center, UV));
    float uv_end_dist = abs(distance(pos, UV));
    float total_dist = abs(distance(center, pos));

    if (uv_start_dist >= total_dist || uv_end_dist >= total_dist || abs(uv_start_dist - Distance * 0.5f) < 0.01f)
    {
        Out = float4(0.f, 0.f, 0.f, 1.f);
    }
    else
    {
        float p = (total_dist + uv_start_dist + uv_end_dist) / 2;
        float h = 2 / total_dist * sqrt(p * (p - uv_start_dist) * (p - uv_end_dist) * (p - total_dist));

        float r = lerp(1.f, 0.f, smoothstep(0.001f, 0.004f, h));
        Out = float4(r, r, r, 1.f);
    }
}
