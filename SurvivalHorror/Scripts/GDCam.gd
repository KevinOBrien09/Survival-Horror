extends Node

@export var playerTrans :Node3D
@export var cam :Node3D
@export var pivot :Node3D
@export var sens : float
@export var _clamp : float
var firstPerson : bool
func _input(event):
	if event is InputEventMouseMotion:
		
		if firstPerson:
			playerTrans.rotate_y(-event.relative.x * sens)
			cam.rotate_x(-event.relative.y * sens)
			cam.rotation.x = clamp(cam.rotation.x,deg_to_rad(-_clamp),deg_to_rad(_clamp))

			
	


