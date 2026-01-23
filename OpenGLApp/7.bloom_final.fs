#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D scene;
uniform sampler2D bloomBlur;
uniform sampler2D depthBuffer;
uniform bool bloom;
uniform float exposure;
uniform float blurDepth;

float LinearizeDepth(float depth);

void main()
{             
    const float gamma = 2.2;
    vec3 hdrColor = texture(scene, TexCoords).rgb;      
    vec3 bloomColor = texture(bloomBlur, TexCoords).rgb;
    float depth = texture(depthBuffer, TexCoords).r;
    //if(bloom)
    //hdrColor += bloomColor; // additive blending
    // tone mapping
    //vec3 result = vec3(1.0) - exp(-hdrColor * exposure);
    // also gamma correct while we're at it       

    vec3 result = mix(hdrColor, bloomColor, depth);
    if (depth > blurDepth)
        FragColor = vec4(vec3(result), 1.0);
    else
        FragColor = vec4(vec3(result), 1.0);

    //FragColor = vec4(vec3(depth), 1.0);
}

float LinearizeDepth(float depth)
{
    float far_plane = 1.0;
    float near_plane = 0.01;

    float z = depth * 2.0 - 1.0; // Back to NDC 
    return (2.0 * near_plane * far_plane) / (far_plane + near_plane - z * (far_plane - near_plane));	
}