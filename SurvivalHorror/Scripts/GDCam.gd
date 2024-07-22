extends Node

@export var playerTrans :Node3D
@export var cam :Node3D

@export var sens : float
@export var _clamp : float
var firstPerson : bool
func _unhandled_input(_event):

	
	if firstPerson:
		if(InputManager.controlTypeInt == 0):
			rotateCam(InputManager.cameraInput)

func _process(_delta):
	if firstPerson:
		if(InputManager.controlTypeInt == 1):
			rotateCam(InputManager.cameraInput)

		
func rotateCam(input:Vector2):
	playerTrans.rotate_y(-input.x * sens)
	cam.rotate_x(-input.y * sens)
	cam.rotation.x = clamp(cam.rotation.x,deg_to_rad(-_clamp),deg_to_rad(_clamp))

			
	


