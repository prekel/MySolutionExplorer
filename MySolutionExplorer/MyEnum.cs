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
		public const string Cpp = ".cpp";
		public const string CSharp = ".cs";
		public const string Python = ".py";

		public const string VCXProj = ".vcxproj";
		public const string CSProj = ".csproj";
		public const string PyProj = ".pyproj";

		public const string Input = "input.txt";
		public const string Output = "output.txt";

		public const string Trash = @"\Trash\";
		public const string Slash = @"\";

		public const string TemplateCppProj = "example 0000. Template [cpp]";
		public const string TemplateCpp = "Task_example0000.cpp";

		public const string VS2017 = "[vs2017]";
		public const string VS2010 = "[vs2010]";
		public const string SharpDevelop = "[#develop]";

		public const string Txt = ".txt";
		public const string Config = ".config";

		public const string TemplateCSharpProj = "example 0000. Template [cs]";
		public const string TemplateCSharp = "Task_example0000.cs";

		public const string AppName = "MySolutionExplorer";

		public const string CppSuff = "[cpp]";
		public const string CSharpSuff = "[cs]";
	}
}
