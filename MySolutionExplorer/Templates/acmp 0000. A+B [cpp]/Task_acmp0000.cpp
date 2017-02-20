#include <iostream>
#include <cstdio>

using namespace std;

int a, b;

int main()
{
#ifdef file
	freopen("input.txt", "r", stdin);
	freopen("output.txt", "w", stdout);
#endif

	cin >> a >> b;
	cout << a + b;

	return 0;
}
