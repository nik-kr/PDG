extends Control

var can_change_key = false
var change_key_name = ""
var changedButton

onready var GS = $"/root/GlobalSingletone"

func _ready():
	GS.LoadCfg()
	
	$VBoxContainer/SoundMixer/HSplitContainer/SoundSlider.value = GS.masterVolume
	$VBoxContainer/Difficulty/HSplitContainer/DifficultySlider.value = GS.difficult

func _input(event):
	if event is InputEventKey:
		if can_change_key and change_key_name != "":
			GS.changeKey(event, change_key_name)
			can_change_key = false
			changedButton.text = "Изменено"
#			change_key(event)

func change_key(event):
	#Delete key
	if !InputMap.get_action_list(change_key_name).empty():
		InputMap.action_erase_event(change_key_name, InputMap.get_action_list(change_key_name)[0])
	if InputMap.action_has_event(change_key_name, event):
		InputMap.action_erase_event(change_key_name, event)
	InputMap.action_add_event(change_key_name, event)

	can_change_key = false
	changedButton.text = "Изменено"
	pass

func _on_SoundSlider_value_changed(value):
	$VBoxContainer/SoundMixer/HSplitContainer/Label.text = str(int(value))
	GS.masterVolume = $VBoxContainer/SoundMixer/HSplitContainer/SoundSlider.value
	pass # Replace with function body.


func _on_DifficultySlider_value_changed(value):
	var variant = ["Легкая", "Нормальная", "Сложная"]
	$VBoxContainer/Difficulty/HSplitContainer/Label.text = variant[int(value)]
	GS.difficult = $VBoxContainer/Difficulty/HSplitContainer/DifficultySlider.value
	pass # Replace with function body.

#------    UP    ------#
func _on_chooseButtonUp_pressed():
	if !can_change_key:
		can_change_key = true
		change_key_name = "ui_up"
		changedButton = $VBoxContainer/uiUp/HSplitContainer/chooseButton
		changedButton.text = "<Изменяем...>"
	pass


func _on_chooseButtonDown_pressed():
	if !can_change_key:
		can_change_key = true
		change_key_name = "ui_down"
		changedButton = $VBoxContainer/uiDown/HSplitContainer/chooseButton
		changedButton.text = "<Изменяем...>"
	pass # Replace with function body.


func _on_chooseButtonRight_pressed():
	if !can_change_key:
		can_change_key = true
		change_key_name = "ui_right"
		changedButton = $VBoxContainer/uiRight/HSplitContainer/chooseButton
		changedButton.text = "<Изменяем...>"
	pass # Replace with function body.


func _on_chooseButtonLeft_pressed():
	if !can_change_key:
		can_change_key = true
		change_key_name = "ui_left"
		changedButton = $VBoxContainer/uiLeft/HSplitContainer/chooseButton
		changedButton.text = "<Изменяем...>"
	pass # Replace with function body.

func _on_BackToPrevious_pressed():
	GS.SaveCfg()
	get_tree().change_scene_to(load("res://Scene/MainMenu/MainMenu.tscn"))
