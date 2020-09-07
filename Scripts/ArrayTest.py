import scratch


def move_zeros_left(arr):
    count = 0
    start = 0
    end = int(len(arr)-1)
    print("starting array = {}".format(arr))
    print("end of array = {}".format(end))
    print("iterate through {} items".format(range(end)))

    #k = window

    for i in range(end):  # how to work left to right??
        print("iteration {}".format(i))
        print("arr[{}] = {}".format(end, arr[end]))
        if arr[end] != 0:
            print("beginning value for end pointer = {}".format(end))
            arr[start] = arr[end]
            start += 1
            count += 1
            end -= 1
            print("count = {}".format(count))
            print("end value at end = {}".format(end))
            print(arr)
        else:
            end -= 1
            print("count = {}".format(count))
            print("end = {}".format(end))

    print("We need {} zeroes to be populated starting at {}".format((len(arr))-count, count))

    for j in range(count, len(arr)):
        print ("im in second loop!")
        arr[j] = 0

    return arr


array = [0, 10, 5, 5, 10, 0, 25]

print(move_zeros_left(array))
