extends Control

var GameScene = preload("res://Scene/GameScene/Game.tscn")

var Character



func _ready():
	$WarriorContainer/BackButton.visible = false
	$WarriorContainer/ApplyButton.visible = false
	$AnimationTree/ScreenLoad.visible = false
	pass

func _on_TextureButton_pressed():
	$WarriorAnim.play("ChooseCharacter")
	$WarriorContainer/TextureButton.visible = false
	pass


func _on_BackButton_pressed():
	$WarriorAnim.play_backwards("ChooseCharacter")
	$WarriorContainer/TextureButton.visible = true
	pass # Replace with function body.


func _on_ApplyButton_pressed():
	$AnimationTree.play("ScreenLoad")
	GlobalSingletone.ChangeCharacter("Warrior")
	pass # Replace with function body.


func _on_AnimationTree_animation_finished(anim_name):
	get_tree().change_scene_to(GameScene)
	pass # Replace with function body.
