extends Node


func w_choise(variant : Array, weights : Array):
	var mass = 0
	for w in weights:
		mass += w
	var _variant = rand_range(0, mass)
	var _tmp = 0
	for i in range(len(weights)):
		_tmp += weights[i]
		if(_tmp) >= _variant:
			return variant[i]
	pass
