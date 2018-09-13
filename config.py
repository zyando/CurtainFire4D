# -*- coding: utf-8 -*-

import sys
sys.path.append(PYTHON_LIB_DIRECTORY)

import clr
clr.AddReference("CurtainFire4D")
clr.AddReference("VecMath")
clr.AddReference("System.Windows.Forms")

def init_shottype():
	from CurtainFire4D.Entities import *
	from System import Array
	
	return Array[ShotType]([
		ShotTypeBillboard("S", "Resource\\Textures\\Sphere.png", 1),
	])
