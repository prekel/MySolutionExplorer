// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolutionExplorer
{
	/// <summary>
	/// Перечисление со строковыми константами
	/// </summary>
	public struct MyEnum
	{
		public const string Cpp = ".cpp", CSharp = ".cs", Python = ".py";
		public const string VCXProj = ".vcxproj", CSProj = ".csproj", PyProj = ".pyproj";
		public const string Input = "input.txt", Output = "output.txt";
		public const string Trash = @"\Trash\", Slash = @"\";
		public const string TemplateCppProj = "example 0000. Template [cpp]", TemplateCpp = "Task_example0000.cpp";
		public const string VS2017 = "[vs2017]", VS2010 = "[vs2010]", SharpDevelop = "[#develop]";
		public const string Txt = ".txt", Config = ".config";
		public const string TemplateCSharpProj = "example 0000. Template [cs]", TemplateCSharp = "Task_example0000.cs";
		public const string AppName = "MySolutionExplorer";
		public const string CppSuff = "[cpp]", CSharpSuff = "[cs]";
	}
}
