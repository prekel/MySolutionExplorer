using System;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Список множителей, v4
/// </summary>
public class FactorList : Dictionary<int, int>
{
	public FactorList()
	{
	}

	public FactorList(IEnumerable<int> a)
	{
		foreach (var i in a)
		{
			if (ContainsKey(i)) this[i]++;
			else Add(i, 1);
		}
	}

	public FactorList(int n)
	{
		while (n % 2 == 0)
		{
			n = n / 2;
			if (ContainsKey(2)) this[2]++;
			else Add(2, 1);
		}
		var b = 3;
		var c = (int) Math.Sqrt(n) + 1;
		while (b < c)
		{
			if (n % b == 0)
			{
				if (n / b * b - n == 0)
				{
					if (ContainsKey(b)) this[b]++;
					else Add(b, 1);
					n = n / b;
					c = (int) Math.Sqrt(n) + 1;
				}
				else
					b += 2;
			}
			else
				b += 2;
		}
		if (ContainsKey(n)) this[n]++;
		else Add(n, 1);
	}

	public static FactorList operator *(FactorList a, FactorList b)
	{
		var c = new FactorList();
		foreach (var i in a)
		{
			if (b.ContainsKey(i.Key)) c[i.Key] = i.Value + b[i.Key];
			else c.Add(i.Key, i.Value);
		}
		foreach (var i in b)
		{
			if (!a.ContainsKey(i.Key)) c.Add(i.Key, i.Value);
		}
		return c;
	}

	public static FactorList operator /(FactorList a, FactorList b)
	{
		var c = new FactorList();
		foreach (var i in a)
		{
			if (b.ContainsKey(i.Key)) c[i.Key] = i.Value - b[i.Key];
			else c[i.Key] = i.Value;
		}
		return c;
	}

	public int ToInt()
	{
		var c = 1;
		foreach (var i in this)
		{
			for (var j = 0; j < i.Value; j++)
				c *= i.Key;
		}
		return c;
	}

	public long ToLong()
	{
		var c = 1L;
		foreach (var i in this)
		{
			for (var j = 0; j < i.Value; j++)
				c *= i.Key;
		}
		return c;
	}

	public BigInt ToBigInt()
	{
		var c = new BigInt(1);
		foreach (var i in this)
		{
			for (var j = 0; j < i.Value; j++)
				c *= i.Key;
		}
		return c;
	}

	public override string ToString()
	{
		var s = "";
		foreach (var i in this)
		{
			s += i + " ";
		}
		s += "= " + ToLong();
		return s;
	}
}

/// <summary>
/// Комбинаторика, v3
/// </summary>
public class Combine
{
	/// <summary>
	/// Факториал из N
	/// </summary>
	/// <param name="n"> N </param>
	/// <returns> Список множителей </returns>
	public static FactorList Fact(int n)
	{
		var a = new FactorList { { 1, 1 } };
		for (var i = 2; i <= n; i++)
		{
			a *= new FactorList(i);
		}
		return a;
	}

	/// <summary>
	/// Число размечений из N по K
	/// </summary>
	/// <param name="n"> N </param>
	/// <param name="k"> K </param>
	/// <returns> Список множителей </returns>
	public static FactorList A(int n, int k)
	{
		return Fact(n) / Fact(n - k);
	}

	/// <summary>
	/// Число сочетаний из N по K
	/// </summary>
	/// <param name="n"> N </param>
	/// <param name="k"> K </param>
	/// <returns> Список множителей </returns>
	public static FactorList C(int n, int k)
	{
		return A(n, k) / Fact(k);
	}

	/// <summary>
	/// Число повторяющихся перестановок
	/// </summary>
	/// <param name="n"> N </param>
	/// <param name="a"> Коллекция значений n1, n2, ..., nN </param>
	/// <returns> Список множителей </returns>
	public static FactorList P(int n, IEnumerable<int> a)
	{
		var c = new FactorList();
		foreach (var i in a)
		{
			c *= Fact(i);
		}
		return Fact(n) / c;
	}

	public static FactorList Sqr(FactorList a)
	{
		return a * a;
	}
}

/// <summary>
/// Длинное целое, v4
/// </summary>
public class BigInt
{
	private int[] a = new int[1111];
	private int sz = 1, bs = 10, sg = 1;

	public int Size
	{
		get { return sz; }
		set { sz = value; }
	}

	public int Base
	{
		get { return bs; }
		set { bs = value; }
	}

	public int Sign
	{
		get { return sg; }
		set { sg = value; }
	}

	public int this[int i]
	{
		get { return a[i]; }
		set { a[i] = value; }
	}

	public BigInt(string s)
	{
		sz = s.Length;
		for (var i = 0; i < sz; i++)
		{
			a[i] = s[sz - i - 1] - '0';
		}
	}

	public BigInt()
	{
	}

	public BigInt(BigInt A)
	{
		Size = A.Size;
		Base = A.Base;
		Sign = A.Sign;
		for (var i = 0; i < A.Size; i++)
		{
			a[i] = A[i];
		}
	}

	public BigInt(int n, int v)
	{
		Size = n;
		if (v == 0) return;
		for (var i = 0; i < n; i++)
		{
			a[i] = v;
		}
	}

	public BigInt(int n)
	{
		var s = n.ToString();
		sz = s.Length;
		for (var i = 0; i < sz; i++)
		{
			a[i] = s[sz - i - 1] - '0';
		}
	}

	public override string ToString()
	{
		var s = "";
		if (Sign == -1)
			s += '-';
		for (var i = sz - 1; i >= 0; i--)
		{
			s += a[i].ToString();
		}
		return s;
	}

	public static void Swap(ref BigInt A, ref BigInt B)
	{
		var C = A;
		A = B;
		B = C;
	}

	public static BigInt Sum(BigInt A, BigInt B)
	{
		var C = new BigInt(Math.Max(A.Size, B.Size), 0);
		for (var i = 0; i < C.Size; i++)
		{
			C[i] += A[i] + B[i];
			if (C[i] <= C.Base - 1) continue;
			C[i] = C[i] - C.Base;
			C[i + 1] = 1;
		}
		if (C[C.Size] > 0)
		{
			C.Size++;
		}
		return C;
	}

	public static BigInt operator +(BigInt A, BigInt B)
	{
		return Sum(A, B);
	}

	public static BigInt Sub(BigInt A, BigInt B)
	{
		var C = new BigInt(A);
		var D = new BigInt(B);
		if (A < B)
		{
			Swap(ref C, ref D);
			C.Sign = -1;
		}
		for (var i = 0; i < C.Size; i++)
		{
			C[i] += C.Base - D[i];
			if (C[i] < C.Base) C[i + 1]--;
			else C[i] -= C.Base;
		}
		while (C[C.Size - 1] == 0 && C.Size > 1)
		{
			C.Size--;
		}
		return C;
	}

	public static BigInt operator -(BigInt A, BigInt B)
	{
		return Sub(A, B);
	}

	public static int Comp(BigInt A, BigInt B)
	{
		if (A.Size < B.Size)
			return -1;
		if (A.Size > B.Size)
			return 1;
		for (var i = A.Size - 1; i >= 0; i--)
		{
			if (A[i] < B[i])
				return -1;
			if (A[i] > B[i])
				return 1;
		}
		return 0;
	}

	public static bool operator >(BigInt A, BigInt B)
	{
		return Comp(A, B) == 1;
	}

	public static bool operator <(BigInt A, BigInt B)
	{
		return Comp(A, B) == -1;
	}

	public static BigInt Mult(BigInt A, int b)
	{
		var C = new BigInt(A.Size, 0);
		if (b == 0)
		{
			C.Size = 1;
			return C;
		}
		var p = 0;
		for (var i = 0; i < A.Size; i++)
		{
			C[i] = A[i] * b + p;
			p = C[i] / C.Base;
			C[i] = C[i] % C.Base;
		}
		while (p > 0)
		{
			C[++C.Size - 1] = p % C.Base;
			p = p / C.Base;
		}
		return C;
	}

	public static BigInt operator *(BigInt A, int b)
	{
		return Mult(A, b);
	}
}

public class Task
{
	public static void Main()
	{
		StreamReader In;
		StreamWriter Out;
		//try { In = new StreamReader("input.txt"); Out = new StreamWriter("output.txt"); } catch {
		In = new StreamReader(Console.OpenStandardInput());
		Out = new StreamWriter(Console.OpenStandardOutput()); //}

		//var v1 = new FactorList(12);
		//var v2 = new FactorList(315);
		//var v3 = new FactorList(new[] {2, 3, 3, 3, 5, 5, 5});
		//var v4 = Combine.Fact(5);
		//var v5 = Combine.C(25, 13);
		//var v6 = v1 * v2;
		//var v7 = v6 / v1;
		//var v8 = v5.ToLong();

		var s = In.ReadLine().Split();
		var n = int.Parse(s[0]);
		var k = int.Parse(s[1]);

		if (k > n)
			Out.WriteLine(0);
		else
			Out.Write((Combine.Sqr(Combine.Fact(n)) / Combine.Fact(k) / Combine.Sqr(Combine.Fact(n - k))).ToInt());

		Out.Close();
	}
}