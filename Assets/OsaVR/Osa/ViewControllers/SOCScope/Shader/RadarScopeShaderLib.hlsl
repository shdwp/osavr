float RadarScope_AzimuthOffset(float2 Pos, float Azimuth) {
    const float pi = 3.141592653589793238462;
    
    float2 dir = normalize(float2(Pos.x - 0.5f, Pos.y - 0.5f));
    float angle = acos(dot(normalize(float2(-1.f, 0.f)), dir));
    
    if (dir.y > 0) {
        angle = pi * 2 - angle;
    }
    
    angle = angle + Azimuth + pi * 2.5;
    if (angle > pi * 2) {
        float a = ceil(angle / (pi * 2));
        angle = angle - (a * pi * 2);
    }
    
    return abs(angle);
}

void RadarScope_ScanlineFade_float(float4 In, float2 Pos, float Azimuth, out float4 Out) {
    const float pi = 3.141592653589793238462;
    float factor = 3;
    Out = In * smoothstep(0.f, pow(pi * 2, factor), pow(RadarScope_AzimuthOffset(Pos, Azimuth), factor));
}

void RadarScope_RangeCircles_float(float2 UV, out float4 Out) {
    float dist = smoothstep(0.f, 0.5f, distance(float2(0.5f, 0.5f), UV));
    Out = float4(0.f, 0.f, 0.f, 1.f);
    
    for (float i = 0.1f; i < 1.f; i += 1.f / 5.f) {
        if (dist > i && dist < i + 0.01f) {
            Out.rgb = float3(1.f, 1.f, 1.f);
        }
    }
}

void RadarScope_Sightline_float(float2 UV, float Azimuth, float Distance, out float4 Out) {
    float dist = smoothstep(0.f, 0.5f, distance(float2(0.5f, 0.5f), UV));
    float rad = RadarScope_AzimuthOffset(UV, 0.f);
    float offset = abs(Azimuth - rad);
    
    if (dist > 0.1f && (dist < Distance || dist > Distance + 0.2f) && dist < 0.95f && offset < 0.005f / dist) {
        Out = float4(1.f, 1.f, 1.f, 1.f);
    } else {
        Out = float4(0.f, 0.f, 0.f, 1.f);
    }
}
