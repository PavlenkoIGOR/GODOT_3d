shader_type spatial;

uniform vec3 myAlbedo : source_color;
uniform vec3 myAlbedoFresnel : source_color;
uniform float myMetallic : hint_range(0.0, 1.0) = 0;
uniform float myRoughness : hint_range(0.0, 1.0) = 0.02;
uniform sampler2D wave;
uniform sampler2D texture_normal;
uniform sampler2D texture_normal2;
uniform vec2 my_Wave_direction = vec2(2.0, 0.0);
uniform vec2 my_Wave_direction2 = vec2(0.0, 1.0);
uniform float myTime_Scale : hint_range(0.0, 0.2, 0.005) = 0.025;
uniform float noise_scale = 10.0;
uniform float height_scale = 0.15;

uniform vec4 color_deep : source_color;
uniform vec4 color_shallow : source_color;
uniform float beers_law = 2.0;
uniform float depth_offset = -0.75;

uniform float edge_scale = 0.1;
uniform float near = 1.0;
uniform float far = 100.0;
uniform vec3 edge_color : source_color;

uniform sampler2D DEPTH_TEXTURE : hint_depth_texture, filter_linear_mipmap;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

varying float height;
varying vec3 world_pos;

float Fresnel(float amount, vec3 normal, vec3 view)
{
	return pow((1.0 - clamp(dot(normalize(normal), normalize(view)), 0.0, 1.0)), amount);
}
float Edge(float depth){
	depth = 2.0 * depth - 1.0;
	return near * far / (far + depth * (near - far));
}
void vertex(){
	world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	height = texture(wave, world_pos.xz / noise_scale + TIME * myTime_Scale).r;
	VERTEX.y += height * height_scale;
}
void fragment()
{	
	float depth_texture = texture(DEPTH_TEXTURE, SCREEN_UV).r * 2.0 - 1.0;
	float depth = PROJECTION_MATRIX[3][2] / (depth_texture + PROJECTION_MATRIX[2][2]);
	float depth_blend = exp((depth + VERTEX.z + depth_offset) * -beers_law);
	depth_blend = clamp(1.0 - depth_blend, 0.0, 1.0);
	float depth_blend_power = clamp(pow(depth_blend, 2.5), 0.0, 1.0);
	
	vec3 screen_color = textureLod(SCREEN_TEXTURE, SCREEN_UV, depth_blend_power * 2.5).rgb;
	vec3 depth_color = mix(color_shallow.rgb, color_deep.rgb, depth_blend_power);
	vec3 color = mix(screen_color * depth_color, depth_color * 0.25, depth_blend_power * 0.5);
	
	float z_depth = Edge(texture(DEPTH_TEXTURE, SCREEN_UV).x);
	float z_pos = Edge(FRAGCOORD.z);
	float z_dif = z_depth - z_pos;
	
	vec2 time = (TIME * my_Wave_direction) * myTime_Scale;
	vec2 time2 = (TIME * my_Wave_direction2) * myTime_Scale;
	vec3 normal_blend = mix(texture(texture_normal, world_pos.xz + time).rgb, 
	texture(texture_normal2, world_pos.xz + time2).rgb, 0.5);
	float fresnel = Fresnel(5.0, NORMAL, VIEW);
	vec3 surface_color = mix(myAlbedo, myAlbedoFresnel, fresnel);
	
	vec3 depth_color_adj = mix(edge_color, color, step(edge_scale, z_dif));
	
	ALBEDO = clamp(surface_color + depth_color_adj, vec3(0.0), vec3(1.0));
	METALLIC = myMetallic;
	ROUGHNESS = myRoughness;
	NORMAL_MAP = normal_blend;
}