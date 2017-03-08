using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySolutionExplorer;
using System.IO;

namespace MySolutionExplorer.Tests
{
	[TestClass]
	public class UnitTest
	{
		[TestMethod]
		public void TestTest1()
		{
			Assert.AreEqual(1, 1);
		}

		[TestMethod]
		public void TestTest2()
		{
			Assert.AreEqual(1, 1);
		}

		[TestMethod]
		public void TestTest3()
		{
			Assert.AreEqual(1, 1);
		}
		
		[TestMethod]
		public void CreateSolutionTest()
		{
			var s = new Solution();
		}

		[TestMethod]
		public void CreateCppProjectTest()
		{
			var dir = Directory.GetCurrentDirectory();
			var s = new Solution();
			var p = new CppProject
			{
				ParentSolution = s,
				TaskName = "Sample",
				Site = "asd",
				Number = 123,
				Lang = "cpp"
			};
			p.Path = dir + MyEnum.Slash + p.Name;
			p.CreateFiles();
			s.Add(p);
		}
	}
}
