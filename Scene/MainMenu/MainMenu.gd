extends Control

onready var GS = $"/root/GlobalSingletone"

var GameScene = preload("res://Scene/GameScene/Game.tscn")
var SettingMenu = preload("res://Node/GUI/GameSettings.tscn")


func _ready():
	GS.LoadCfg()
	$LRecord.text = "Рекордный уровень: " + str(GS.gameRecord)

func _on_NewGame_pressed():
	print("New Game!")
	$CenterContainer.visible = false
	$AnimationPlayer.play("LoadScreen")



func _on_AnimationPlayer_animation_finished(anim_name):
	get_tree().change_scene_to(GameScene)


func _on_NewGame2_pressed():
	get_tree().change_scene_to(SettingMenu)	


func _on_Exit_pressed():
	GS.SaveCfg()
	get_tree().quit();
	pass # Replace with function body.
