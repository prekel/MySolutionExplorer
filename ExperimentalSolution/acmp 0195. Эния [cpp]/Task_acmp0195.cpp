#include <iostream>
#include <cstdio>

using namespace std;

int n, a, b;

int main()
{
#ifdef file
	freopen("input.txt", "r", stdin);
	freopen("output.txt", "w", stdout);
#endif

	cin >> n >> a >> b;
	cout << 2 * a * b * n;

	return 0;
}
