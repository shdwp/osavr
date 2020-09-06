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
