extends KinematicBody2D

var speed = 200  # speed in pixels/sec
var velocity = Vector2.ZERO

var Strenght
var HealthPoint
var Manna
var Stamina
var Lucky
var Charisma
var Intelect
var Damage
var Secrecy

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
	if event is InputEventMouseMotion:
		$Camera2D/Label.text = str(get_global_mouse_position())

func _physics_process(delta):
	get_input()
	velocity = move_and_slide(velocity)
