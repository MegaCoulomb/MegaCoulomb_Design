# array random array generator for exercises
import random
from typing import List

class ArrayGen:        

   def create_random(self, length, minval, maxval) -> List[int]:
         array = []
         for i in range(length):
            array.append(random.randrange(minval, maxval))
    
         return array
         
   def insertion_sort(arr):
      for i in range(1,len(arr)):
         j = i-1

         while (arr[j]>arr[i]) and (j>=0):
            arr[i] = arr[j]
            j = j-1
         
      return arr
