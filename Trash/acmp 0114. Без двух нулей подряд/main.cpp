#include <iostream>
#include <cstdio>
#include <algorithm>
#include <ctime>

using namespace std;

int n, k, dk[11111], d0[11111], i;

int main()
{
    freopen("input.txt", "r", stdin);
    freopen("output.txt", "w", stdout);

    cin >> n >> k;

	dk[1] = k - 1;
	
	for (i = 2; i <= n; i++)
	{
		dk[i] = (dk[i - 1] + d0[i - 1]) * (k - 1);
		d0[i] = dk[i - 1];
	}

	cout << d0[n] + dk[n];

    return 0;
}
