n = int(input())
d0 = [1]
d1 = [1]
for i in range(1, n):
	d0.append(d0[-1] + d1[-1])
	d1.append(d0[-2])
	
print(d0[n - 1] + d1[n - 1])
