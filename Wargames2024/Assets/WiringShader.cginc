#ifndef WIRINGSHADER
#define WIRINGSHADER

struct WiringShaderInput
{

};

void Process_float(
    in float4 In,
    in float BlueMultiplier,
    in float WhiteMultiplier,
    in float Time,
    out float4 Out)
{
    float multiplier = 5;

    // WhiteMultiplier += ((sin(Time * 3.14) + 1) / 2) * multiplier;
    BlueMultiplier += ((sin(Time * 3.14) + 1) / 2) * multiplier;

    [flatten] if (In.x > 0.5f)
    {
        Out = In * WhiteMultiplier;
    }
    else
    {
        Out = In * BlueMultiplier;
    }
}


#endif
