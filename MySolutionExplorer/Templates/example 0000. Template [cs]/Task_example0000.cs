using System;
using System.IO;
using System.Collections.Generic;

public class Task
{
	public static void Main()
	{
		#region Settings

		StreamReader In;
		StreamWriter Out;
		try
		{
			System.Threading.Thread.CurrentThread.CurrentCulture =
				System.Globalization.CultureInfo.InvariantCulture;
		}
		finally
		{
#if file
			Directory.SetCurrentDirectory(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.FullName);
			In = new StreamReader("input.txt");
			Out = new StreamWriter("output.txt");
#else
			In = new StreamReader(Console.OpenStandardInput());
			Out = new StreamWriter(Console.OpenStandardOutput());
#endif
		}

		#endregion

		var s = In.ReadLine().Split();
		var a = int.Parse(s[0]);
		var b = int.Parse(s[1]);
		Out.WriteLine(a + b);

		Out.Close();
	}
}
