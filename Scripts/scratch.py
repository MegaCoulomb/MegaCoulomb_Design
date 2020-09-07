# array random array generator for exercises
import math


class ArrayGen:

    def __init__(self):
        self.array = []

    # def create_random(self, length, minval, maxval):
    #     for i in range(length):
    #         self.array =
    #
    #     return self.array

    def create_descending(self, length: int, startval: int):
        for i in range(startval, length):
            self.array[i] = i
            return self.array


    # def create_ascending(self, length, maxval, minval):
    #     for i in range(length):
    #         pass
