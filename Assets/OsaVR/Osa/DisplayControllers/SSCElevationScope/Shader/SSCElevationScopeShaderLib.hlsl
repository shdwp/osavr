void ElevationBar_float(float2 UV, float Elevation, out float4 Out) {
    Out = float4(1.f, 1.f, 1.f, 1.f);

    float distance = abs(UV.y - Elevation);
    Out.rgb *= lerp(1.f, 0.f, smoothstep(0.01f, 0.05f, distance));
}
