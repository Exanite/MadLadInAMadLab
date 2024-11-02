#ifndef SPRITE_GLOW_SHADER
#define SPRITE_GLOW_SHADER

void Process_float(
    in float4 In,
    in float Time,
    in float PreMultiplier,
    in float PulseMultiplier,
    in float PostMultiplier,
    out float4 Out)
{
    float multiplier = PreMultiplier;
    multiplier *= 1 + ((sin(Time * 3.14) + 1) / 2) * PulseMultiplier;;
    multiplier *= PostMultiplier;

    Out = In * multiplier;
}

#endif
