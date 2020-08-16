void BlurSignal_float(Texture2D Tex, SamplerState SS, float2 UV, out float4 Out) {
    float col = 0.f;
    
    float step = 0.01f;
    float range = 0.025f;
    float max_dist = distance(float2(0.f, 0.f), float2(range, range));
    for (float x = -range; x < range; x += step) {
        for (float y = -range; y < range; y += step) {
            float4 c = Tex.Sample(SS, float2(UV.x + x, UV.y + y));
            float dist = distance(float2(x, y), float2(0.f, 0.f));
            
            if (c.r > 0) {
                col += dist * 0.05f;
            }
        }
    }
    
    Out = float4(col, col, col, 1.f);
}

void Colorize_float(float4 In, float4 Color, out float4 Out) {
    Out = Color * In.r;
}

float RadarScope_AzimuthOffset(float2 Pos, float Azimuth) {
    const float pi = 3.141592653589793238462;
    
    float2 dir = normalize(float2(Pos.x - 0.5f, Pos.y - 0.5f));
    float angle = acos(dot(normalize(float2(1.f, 0.f)), dir));
    
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

float RadarScope_ScanlineFade(float2 Pos, float Azimuth) {
    const float pi = 3.141592653589793238462;
    return clamp(pow(RadarScope_AzimuthOffset(Pos, Azimuth) / pi * 2, 3), 0.f, 1.f);
}

void RadarScope_RangeCircles_float(float2 UV, float Scanline_Azimuth, out float4 Out) {
    float dist = smoothstep(0.f, 0.5f, distance(float2(0.5f, 0.5f), UV));
    Out = float4(0.f, 0.f, 0.f, 1.f);
    
    for (float i = 0.1f; i < 1.f; i += 1.f / 5.f) {
        if (dist > i && dist < i + 0.02f) {
            Out.rgb = float3(1.f, 1.f, 1.f);
        }
    }
    
    Out.rgb *= RadarScope_ScanlineFade(UV, Scanline_Azimuth);
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

void RadarReturns_float(Texture2D Tex, SamplerState SS, float2 UV, out float4 Out) {

}