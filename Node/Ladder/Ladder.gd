extends StaticBody2D



func _on_Ladder_body_entered(body):
	if(body.get("NodeType") == "Player"):
		GlobalSingletone.Level += 1
		# get_tree().root.get_node("Game").gen_dungeon()	
		


func _on_Area2D_body_entered(body):
	pass # Replace with function body.
