shader_type canvas_item;


// TilingAndOffsetUVAnimated
vec2 _tilingNoffsetAnimatedFunc(vec2 _tilingNoffset_uv, float _tilingNoffset_time, vec2 _tilingNoffset_offset_vect){
	return vec2(mod((_tilingNoffset_uv.x + _tilingNoffset_offset_vect.x * _tilingNoffset_time), 1.0),
				mod((_tilingNoffset_uv.y + _tilingNoffset_offset_vect.y * _tilingNoffset_time), 1.0));
}

void vertex() {
// Output:0

}

void fragment() {
// Input:3
	vec3 n_out3p0 = vec3(UV, 0.0);

// Scalar:15
	float n_out15p0 = 0.500000;

// Input:14
	float n_out14p0 = TIME;

// TilingAndOffsetUVAnimated:13
	vec3 n_out13p0;
	{
		n_out13p0.xy += _tilingNoffsetAnimatedFunc(n_out3p0.xy, n_out14p0, vec3(n_out15p0).xy);
	}

// Texture:5
	vec3 n_out5p0;
	float n_out5p1;
	{
		vec4 _tex_read = texture(TEXTURE, n_out13p0.xy);
		n_out5p0 = _tex_read.rgb;
		n_out5p1 = _tex_read.a;
	}

// Output:0
	COLOR.rgb = n_out5p0;
	COLOR.a = n_out5p1;

}

void light() {
// Output:0

}
