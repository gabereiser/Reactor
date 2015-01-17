﻿in vec2 out_texcoords;
in vec4 out_color;

uniform sampler2D diffuse;

out vec4 color;

void main(){
    vec4 t = texture(diffuse, out_texcoords);
	color = out_color * t;

}
