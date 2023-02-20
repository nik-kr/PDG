extends Node2D

var Room = preload("res://Node/Room/Room.tscn")
onready var Map = $TileMap

var draw_room_outline = true
var draw_astar_line = true
export var debug_mode : bool = false

var Player

var start_room = null
var end_room = null
var play_mode = false
var player = null

export(int, 1, 100) var LEVEL = 1

export var min_size = 4
export var max_size = 10
export var title_size = 32
export var num_rooms = 45
export var hspread = 400
export var cull = 0.5

var tile_size = 16

var path

onready var GS = get_node("/root/GlobalSingletone")

func _ready():	
	if GS.Character == "Warrior":
		Player = GS._PacWarrior
	randomize()
	make_rooms()

func find_start_room():
	var min_x = INF
	var r
	for room in $Rooms.get_children():
		if room.position.x < min_x:
			start_room = room
			min_x = room.position.x 
			r = room
	r.RoomType = "START_ROOM"
	r.FillRoom(title_size)
	
func find_end_room():
	var max_x = -INF
	var r
	for room in $Rooms.get_children():
		if room.position.x > max_x:
			end_room = room
			max_x = room.position.x
			r = room
	r.RoomType = "END_ROOM"
	r.FillRoom(title_size)




func make_rooms():
	for i in range(num_rooms):
		var pos = Vector2(rand_range(-hspread, hspread), rand_range(-hspread, hspread)/2)
		var r = Room.instance()
		var w = min_size + randi() % (max_size - min_size)
		var h = min_size + randi() % (max_size - min_size)
		r.make_room(pos, Vector2(w, h) * title_size)
		$Rooms.add_child(r)
	
	yield(get_tree().create_timer(1.1), "timeout")
	
	# cull rooms
	var room_positions = []
	for room in $Rooms.get_children():
		if randf() < cull:
			$Rooms.remove_child(room)
			room.queue_free()
			
		else:
			room.mode = RigidBody2D.MODE_STATIC
			room_positions.append(Vector3(room.position.x, room.position.y, 0))
	yield(get_tree(), 'idle_frame')
	# generate spanning tree (path)
	path = find_mst(room_positions)
	
	make_map()
	
	draw_room_outline = false
	draw_astar_line = false
	
	find_start_room()
	
	$Camera.visible = false
	player = Player.instance()
	player.set_name("Player")
	add_child(player)
	player.position = start_room.position
	play_mode = true
	
	find_end_room()
	
	fill_rooms()
	
	

	
func fill_rooms():
	for r in $Rooms.get_children():
		if r.RoomType == "":
			r.RoomType = NFunc.w_choise(RoomFilling.RoomType, RoomFilling.Weight)
			r.FillRoom(tile_size)
	pass
	
func _draw():
	if draw_room_outline or debug_mode:
		for room in $Rooms.get_children():
			draw_rect(Rect2(room.position - room.size, room.size*2),
					  Color(0, 1, 0), false)
	if draw_astar_line or debug_mode:
		if path:
			for p in path.get_points():
				for c in path.get_point_connections(p):
					var pp = path.get_point_position(p)
					var cp = path.get_point_position(c)
					draw_line(Vector2(pp.x, pp.y),
							  Vector2(cp.x, cp.y),
							  Color(1, 1, 0, 1), 15, true)
				

func _physics_process(delta):
	update()

func _input(event):
	if event.is_action_pressed('debug_mode'):
		if debug_mode:
			debug_mode = false
			if player:
				player.speed = 200
				player.get_node("Collision").disabled = false
			GS.HPBar.percent_visible = false
			$"../GUI/Control/Debug".visible = false
		else:
			debug_mode = true
			if player:
				player.speed = 1200
				player.get_node("Collision").disabled = true
			GS.HPBar.percent_visible = true
			$"../GUI/Control/Debug".visible = true
	
	
	

func find_mst(nodes):
	# Prim's algorithm
	# Given an array of positions (nodes), generates a minimum
	# spanning tree
	# Returns an AStar object

	# Initialize the AStar and add the first point
	var path = AStar.new()
	path.add_point(path.get_available_point_id(), nodes.pop_front())

	# Repeat until no more nodes remain
	while nodes:
		var min_dist = INF  # Minimum distance found so far
		var min_p = null  # Position of that node
		var p = null  # Current position
		# Loop through the points in the path
		for p1 in path.get_points():
			p1 = path.get_point_position(p1)
			# Loop through the remaining nodes in the given array
			for p2 in nodes:
				# If the node is closer, make it the closest
				if p1.distance_to(p2) < min_dist:
					min_dist = p1.distance_to(p2)
					min_p = p2
					p = p1
		# Insert the resulting node into the path and add
		# its connection
		var n = path.get_available_point_id()
		path.add_point(n, min_p)
		if int(rand_range(0, 1)) == 1:
			var rn = rand_range(0, len(nodes))
			var rp = nodes[rn]
			path.add_point(rp.get_available_point_id(), rp)
			pass
		path.connect_points(path.get_closest_point(p), n)
		# Remove the node from the array so it isn't visited again
		nodes.erase(min_p)
	return path

func make_map():
	# Creates a TileMap from the generated rooms & path
	Map.clear()

	# Fill TileMaps
	var full_rect = Rect2()
	for room in $Rooms.get_children():
		var r = Rect2(room.position-room.size,
					room.get_node("CollisionShape2D").shape.extents*2)
		room.get_node("CollisionShape2D").queue_free()
		
		full_rect = full_rect.merge(r)
		#Fill Room
#		room.FillRoom(tile_size, )
		#Create map
		
	var topleft = Map.world_to_map(full_rect.position)
	var bottomright = Map.world_to_map(full_rect.end)
	for x in range(topleft.x, bottomright.x):
		for y in range(topleft.y, bottomright.y):
			Map.set_cell(x, y, 1)

	# Carve rooms and corridors
	var corridors = []  # One corridor per connection
	for room in $Rooms.get_children():
		var s = (room.size / tile_size).floor()
		var pos = Map.world_to_map(room.position)
		var ul = (room.position/tile_size).floor() - s
		for x in range(2, s.x * 2-1):
			for y in range(2, s.y * 2-1):
				Map.set_cell(ul.x+x, ul.y+y, 0)
		Map.update_bitmask_region()
		# Carve corridors
		var p = path.get_closest_point(Vector3(room.position.x,
									room.position.y, 0))
		for conn in path.get_point_connections(p):
			if not conn in corridors:
				var start = Map.world_to_map(Vector2(path.get_point_position(p).x, path.get_point_position(p).y))
				var end = Map.world_to_map(Vector2(path.get_point_position(conn).x, path.get_point_position(conn).y))
				carve_path(start, end)
		corridors.append(p)
		
func carve_path(pos1, pos2):
	# Carves a path between two points
	var x_diff = sign(pos2.x - pos1.x)
	var y_diff = sign(pos2.y - pos1.y)
	if x_diff == 0: x_diff = pow(-1.0, randi() % 2)
	if y_diff == 0: y_diff = pow(-1.0, randi() % 2)
	# Carve either x/y or y/x
	var x_y = pos1
	var y_x = pos2
	if (randi() % 2) > 0:
		x_y = pos2
		y_x = pos1
	for i in range(4):
		for x in range(pos1.x, pos2.x, x_diff):
				Map.set_cell(x, x_y.y, 0)
				Map.set_cell(x, x_y.y+y_diff+i-2, 0)
		for y in range(pos1.y, pos2.y, y_diff):
			Map.set_cell(y_x.x, y, 0)
			Map.set_cell(y_x.x+x_diff+i-2, y, 0)
#	for x in range(pos1.x, pos2.x, x_diff):
#		Map.set_cell(x, x_y.y, 0)
#		Map.set_cell(x, x_y.y+y_diff, 0)  # widen the corridor
#	for x in range(pos1.x, pos2.x, x_diff):
#		Map.set_cell(x, x_y.y, 0)
#		Map.set_cell(x, x_y.y+y_diff+1, 0)  # widen the corridor
#	for x in range(pos1.x, pos2.x, x_diff):
#		Map.set_cell(x, x_y.y, 0)
#		Map.set_cell(x, x_y.y+y_diff-1, 0)  # widen the corridor
#	for x in range(pos1.x, pos2.x, x_diff):
#		Map.set_cell(x, x_y.y, 0)
#		Map.set_cell(x, x_y.y+y_diff-2, 0)  # widen the corridor

#	for y in range(pos1.y, pos2.y, y_diff):
#		Map.set_cell(y_x.x, y, 0)
#		Map.set_cell(y_x.x+x_diff, y, 0)  # widen the corridor
#	for y in range(pos1.y, pos2.y, y_diff):
#		Map.set_cell(y_x.x, y, 0)
#		Map.set_cell(y_x.x+x_diff+1, y, 0)  # widen the corridor
#	for y in range(pos1.y, pos2.y, y_diff):
#		Map.set_cell(y_x.x, y, 0)
#		Map.set_cell(y_x.x+x_diff-1, y, 0)  # widen the corridor
#	for y in range(pos1.y, pos2.y, y_diff):
#		Map.set_cell(y_x.x, y, 0)
#		Map.set_cell(y_x.x+x_diff+2, y, 0)  # widen the corridor
	Map.update_bitmask_region()
