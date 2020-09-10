import random
from ArrayGen import *

def create_array(length, minval, maxval):
    array = []
    for i in range(length):
        array.append(random.randrange(minval,maxval))
    return array

def insertion_sort(arr):
    for i in range(1,len(arr)):
        j = i-1
        next = arr[i]

        while (arr[j]>next) and (j>=0):
            arr[j+1] = arr[j]
            j = j-1 
        arr[j+1] = next
    #print(arr)
    return arr

def bubble_sort(arr):
    right = len(arr)-1

    for iteration in range(right,0,-1):
        for idx in range(iteration):
            if arr[idx]>arr[idx+1]:
                arr[idx], arr[idx+1] = arr[idx+1], arr[idx]
    return arr 

def merge_sort(arr):
    if len(arr)>1:
        mid = len(arr)//2
        right = arr[:mid]
        left = arr[mid:]

        merge_sort(left)
        merge_sort(right)

        a = 0
        b = 0
        c = 0

        while a < len(left) and b < len(right):
            if left[a] < right[b]:
                arr[c] = left[a]
                a = a+1
            else:
                arr[c] = right[b]
                b = b+1
            c = c + 1
            
        while a < len(left):
                arr[c] = left[a]
                a = a+1
                c = c+1

        while b < len(right):
                arr[c] = right[b]
                b = b+1
                c = c+1
        
    return arr

def shell_sort(arr):
    mid = len(arr)//2

    while mid > 0:
        for i in range(mid,len(arr)):
            temp = arr[i]
            j = i

            while j >= mid and arr[j-mid]> temp:
                arr[j] = arr[j-mid]
                j = j-mid
            arr[j] = temp
            mid = mid//2
        return arr



arr = ArrayGen()
x = arr.create_random(10,0,100)
print("x = {}".format(x))
y = insertion_sort(x)
z = bubble_sort(x)
v = merge_sort(x)
w = shell_sort(x)

print(y)
print(z)
print(v)
print(w)

# def move_zeros_left(arr):
#     count = 0
#     start = 0
#     end = int(len(arr)-1)
#     print("starting array = {}".format(arr))
#     print("end of array = {}".format(end))
#     print("iterate through {} items".format(range(end)))

#     #k = window

#     for i in range(end):  # how to work left to right??
#         print("iteration {}".format(i))
#         print("arr[{}] = {}".format(end, arr[end]))
#         if arr[end] != 0:
#             print("beginning value for end pointer = {}".format(end))
#             arr[start] = arr[end]
#             start += 1
#             count += 1
#             end -= 1
#             print("count = {}".format(count))
#             print("end value at end = {}".format(end))
#             print(arr)
#         else:
#             end -= 1
#             print("count = {}".format(count))
#             print("end = {}".format(end))

#     print("We need {} zeroes to be populated starting at {}".format((len(arr))-count, count))

#     for j in range(count, len(arr)):
#         print ("im in second loop!")
#         arr[j] = 0

#     return arr

# def boats(data, max_weight, max_passenger):
#     start = 0 
#     end = len(data) -1

#     boats = 0

#     while(start <= end):
#         if start == end:
#             boats +=1
#         if data[start] + data[start+max_passenger] < max_weight:
            
#             boats += 1

#print(move_zeros_left(x))
