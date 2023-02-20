extends Control

var PersonChoose = preload("res://Scene/MainMenu/PersonChoose.tscn")


func _on_NewGame_pressed():
	$CenterContainer.visible = false
	$AnimationPlayer.play("LoadScreen")



func _on_AnimationPlayer_animation_finished(anim_name):
	get_tree().change_scene_to(PersonChoose)