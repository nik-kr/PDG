extends Node

# onready var GS = get_node("/root/GlobalSingletone")
onready var level_node

var LevelNode = preload("res://Node/Level/Level.tscn")
var Level : int

func _physics_process(delta):
	$GUI/Control/Debug/FPS.text = "FPS: " + str(Engine.get_frames_per_second())
	$GUI/Control/Bar/HPBar.max_value = float(GlobalSingletone.MaxHealthPoint);
	$GUI/Control/Bar/HPBar.value = float(GlobalSingletone.HealthPoint);

func _ready():
	Level = GlobalSingletone.Level
	GlobalSingletone.HPBar = $GUI/Control/Bar/HPBar
	GlobalSingletone.GUI = $GUI
	level_node = LevelNode.instance();
	$".".add_child(level_node)
	# gen_dungeon()
	

func update_level():
	Level = GlobalSingletone.Level
	$GUI/Control/Label.text = "Level: " + str(Level)

func gen_dungeon():
	# update_level()
	# var prev_level
	# if $".".has_node("Level"):
	# 	prev_level = $".".get_node("Level")
	# 	prev_level.queue_free()
	# # if prev_level:
	level_node = LevelNode.instance()
	add_child(level_node)
	# level_node.LEVEL = Level
	# GlobalSingletone.LevelNode = level_node
