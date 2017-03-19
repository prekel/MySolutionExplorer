In = open('input.txt', 'r')
Out = open('output.txt', 'w')

s = In.read().split()
a = int(s[0])
b = int(s[1])

print(a + b, file = Out)
