extends Node


#Комната-Магазин
#Комната Стартовая
#Комната Конечная
#Обычная Комната
#Комната-Спаунер
#Комната-Босса

var RoomType = ["SHOP_ROOM", "DEFAULT_ROOM", "SPAUNER_ROOM", "BOSS_ROOM"]
var Weight = [0.05, 0.80, 0.15, 0.05]


var room_items = {
	"test_sprite" : preload("res://Node/Test/test_sprite.tscn"),
	"ladder" : preload("res://Node/Ladder/Ladder.tscn"),
}


func FillRoom(ROOM, p1, p2, room_type, tile_size):
	if room_type == "END_ROOM":
		var ladder = room_items.ladder.instance()
		ROOM.add_child(ladder)
		ladder.global_position = Vector2((p1.x+p2.x)/2, (p1.y + p2.y) / 2)
	if GlobalSingletone.Level <=5:
		var items = preload("res://Scripts/Levels/1-5lvl.gd").new()
		if room_type == "DEFAULT_ROOM":
			for i in range(p1.x, p2.x, tile_size):
				for j in range(p1.y, p2.y, tile_size):
					var _item
					var to_place
					# _item = NFunc.w_choise(items.Items, items.Weights)
					if _item:
						to_place = items.items.get(_item).instance()
						ROOM.add_child(to_place)
						to_place.global_position = Vector2(int(i), int(j))
			
