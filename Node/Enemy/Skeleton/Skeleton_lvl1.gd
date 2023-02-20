extends KinematicBody2D

export var p1 : Vector2
export var p2 : Vector2

export var SPEED = 150
export var DETECTION_RADIUS = 80

var entered : bool = false

var player: KinematicBody2D

func _physics_process(delta):
	if entered:
		var velocity = global_position.direction_to(player.global_position)
		move_and_collide(velocity * SPEED * delta)
	else:
		var velocity = Vector2(0, 0)
		move_and_collide(velocity * SPEED * delta)
		
	pass

func _on_DetectionZone_body_entered(body : Node):
	if body.is_in_group("player"):
		player = body
		entered = true


func _on_DetectionZone_body_exited(body):
	if body.is_in_group("player"):
		entered = false
	pass
