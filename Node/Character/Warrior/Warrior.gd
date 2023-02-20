extends KinematicBody2D

var NodeType = "Player"

var speed = 200  # speed in pixels/sec
var velocity = Vector2.ZERO



onready var footprint = preload("res://Node/Character/Footprint.tscn")

func _ready():
	pass

func get_input():
	velocity = Vector2.ZERO
	if Input.is_action_pressed('ui_right'):
		velocity.x += 1
	if Input.is_action_pressed('ui_left'):
		velocity.x -= 1
	if Input.is_action_pressed('ui_down'):
		velocity.y += 1
	if Input.is_action_pressed('ui_up'):
		velocity.y -= 1
	# Make sure diagonal movement isn't faster
	velocity = velocity.normalized() * speed

func _input(event):
	pass

func _physics_process(delta):
	get_input()
	velocity = move_and_slide(velocity)
	get_tree().get_nodes_in_group("DebugMenu")[0].get_node("PlayerPosition").text = (
		"PLAYER POSITION\nX:" + str(global_position.x) + "\nY: " + str(global_position.y))

func _on_footstepTimer_timeout():
	var f = footprint.instance()
	f.position = position
	get_parent().add_child(f)
	pass # Replace with function body.
