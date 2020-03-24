import numpy as np
import random
import math
import matplotlib.pyplot as plt

pt_dict = {}
'''
Return the midpoint of two points based on their entry in the global point dictionary
Arguments:
	pt_pair tuple of tuples ((x,y), (x,y)):
		The two points to lookup
Return:
	midpoint (x,y):
		The midpoint of the two points
'''
def get_midpt(pt_pair):
	if(pt_pair not in pt_dict):
		return pt_dict[(pt_pair[1], pt_pair[0])]
	else:
		return pt_dict[pt_pair]

'''
Add a point pair and its corresponding midpoint to the global point dictionary
Arguments:
	pt_pair (tuple of tuples) ((x,y), (x,y)):
		a pair of points
	midpt (tuple) (x,y):
		a coordinate midpoint between the points in pt_pair

'''
def input_midpt(pt_pair, midpt):
	#the point is not currently in the global dictionary	
	if(pt_pair not in pt_dict):
		if((pt_pair[1], pt_pair[0]) not in pt_dict):
			pt_dict[pt_pair] = midpt
		else:
			pt_dict[(pt_pair[1], pt_pair[0])] = midpt
	#the point IS in the global, update the current entry
	else:
		pt_dict[pt_pair] = midpt
'''
check if the midpoint of two points has already been updated
Arguments:
	pt_pair ((x,y), (x,y)): pair of points
return: 
	boolean, True if the midpoint has been updated,
		 	 False if the midpoint doesn't exist, or hasn't been updated
'''
def is_updated(pt_pair):
	midpt = get_midpt(pt_pair)
	if(midpt == None):
		return false
	else:
		midx = (pt_pair[0][0]+pt_pair[1][0])/2
		midy = (pt_pair[0][1]+pt_pair[1][1])/2
		return not (midx, midy) == midpt

def update_midpt(pt_pair):
	if(get_midpt(pt_pair) == None):
		return
	else:
		midpt = get_midpt(pt_pair)

	pt1 = pt_pair[0]
	pt2 = pt_pair[1]
	length = math.sqrt((pt2[0]-pt1[0])**2 + (pt2[1]-pt1[1])**2)
	max_disp = length/(2.8*math.sqrt(2))
	y_disp = random.uniform(-1*max_disp, max_disp)
	input_midpt(pt_pair, (midpt[0], midpt[1] + y_disp))

def input_pts(pt_pair):
	pt1 = pt_pair[0]
	pt2 = pt_pair[1]
	midx = (pt2[0]+pt1[0])/2
	midy = (pt2[1]+pt1[1])/2

	input_midpt(pt_pair, (midx, midy))

def input_triangle(triangle):
	pt1 = triangle[0]
	pt2 = triangle[1]
	pt3 = triangle[2]
	input_pts((pt1, pt2))
	input_pts((pt1, pt3))
	input_pts((pt2, pt3))

'''
Update a triangle by creating new midpoints for each side
Arugments:
	triangle (tuple(tuple))  ((x,y), (x,y), (x,y)): 
		outer tuple holdes three sets of coordinates ()
		inner tuple holds an x and y coordinate (x,y)
Return:
	four triangeles, 
'''
def update_triangle(triangle):
	pt1 = triangle[0]
	pt2 = triangle[1]
	pt3 = triangle[2]

	if not is_updated((pt1, pt2)):
		update_midpt((pt1, pt2))
	if not is_updated((pt1, pt3)):
		update_midpt((pt1, pt3))
	if not is_updated((pt2, pt3)):
		update_midpt((pt2, pt3))

	#print(pt_dict)

	triangle1 = (pt1, get_midpt((pt1, pt2)), get_midpt((pt1, pt3)))
	triangle2 = (pt2, get_midpt((pt1, pt2)), get_midpt((pt2, pt3)))
	triangle3 = (pt3, get_midpt((pt1, pt3)), get_midpt((pt2, pt3)))
	triangle4 = (get_midpt((pt1, pt2)), get_midpt((pt2, pt3)), get_midpt((pt1, pt3)))
	
	input_triangle(triangle1)
	input_triangle(triangle2)
	input_triangle(triangle3)
	input_triangle(triangle4)

	#print(pt_dict)

	return triangle1, triangle2, triangle3, triangle4	
'''
input: list of triangles [((x,y), (x,y), (x,y)),....]
list represents all triangles
each out tuple consists of three tuples of coordinates to designate a triangle
each inner tuple consists of an x and y cooridnate

iteration (int): the current recursive iteration, counts down to zero
'''
def fractal(input, iteration):
	if(iteration == 0):
		return input

	output = []
	for triangle in input:
		t1, t2, t3, t4 = update_triangle(triangle)
		output.append(t1)
		output.append(t2)
		output.append(t3)
		output.append(t4)

	#print(output)

	return fractal(output, iteration-1)

def plot_fractal(fractal):
	pts = {}
	for triangle in fractal:
		if((triangle[0], triangle[1]) not in pts):
			pts[(triangle[0], triangle[1])] = True
		if((triangle[0], triangle[2]) not in pts):
			pts[(triangle[0], triangle[2])] = True
		if((triangle[1], triangle[2]) not in pts):
			pts[(triangle[1], triangle[2])] = True
	
	plt.axes()
	for pt_pair in pts:
		pt1 = pt_pair[0]
		pt2 = pt_pair[1]
		line = plt.Line2D((pt1[0], pt2[0]), (pt1[1], pt2[1]), lw=.5)
		plt.gca().add_line(line)
	plt.axis('scaled')
	plt.show()		

triangle = [(0,0), (4,2), (2,0)]
input_triangle(triangle)
output = fractal([triangle], 1)
plot_fractal(output)

