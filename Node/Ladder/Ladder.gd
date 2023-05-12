extends StaticBody2D

onready var GS = $"/root/GlobalSingletone"

func _on_Ladder_body_entered(body):
	pass
	
	
func _on_Area2D_body_entered(body):
	if(body.get("NodeType") == "Player"):
		GS.level += 1
		get_tree().root.get_node("Game").UpdateLevel()	
