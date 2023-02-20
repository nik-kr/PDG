extends RigidBody2D

export var size : Vector2

export var RoomType : String = ""

var RoomPosition1 : Vector2
var RoomPosition2 : Vector2

func make_room(_pos : Vector2, _size : Vector2):
	position = _pos
	size = _size
	var s = RectangleShape2D.new()
	s.custom_solver_bias = 0.75
	s.extents = size
	$CollisionShape2D.shape = s

#func _process(delta):
#	update()

func FillRoom(tile_size : int):
	var p1 = Vector2()
	var p2 = Vector2()
	
	p1.x = floor((position.x - $CollisionShape2D.shape.extents.x) / tile_size) * tile_size + tile_size * 4
	p2.x = floor((position.x + $CollisionShape2D.shape.extents.x) / tile_size) * tile_size - tile_size * 4
	p1.y = floor((position.y - $CollisionShape2D.shape.extents.y) / tile_size) * tile_size + tile_size * 4
	p2.y = floor((position.y + $CollisionShape2D.shape.extents.y) / tile_size) * tile_size - tile_size * 4
	
	# RoomFilling.FillRoom(self, p1, p2, RoomType, tile_size)
	
	pass
