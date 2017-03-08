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
		
		//[TestMethod]
		//public void CreateSolutionTest()
		//{
		//	var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
		//	var s = new Solution(dir.FullName + MyEnum.Slash + "TestSolution.mysln");
		//	//s.Save();
		//}

		//[TestMethod]
		//public void CreateCppProjectTest()
		//{
		//	var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
		//	var s = new Solution(dir.FullName);
		//	s.Save();
		//	CppProject.Create(s, "TestTask", "TestSite", "1234", dir);
		//}
	}
}
