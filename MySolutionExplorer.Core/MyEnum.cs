﻿// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolutionExplorer.Core
{
    /// <summary>
    /// Перечисление со строковыми константами
    /// </summary>
    public struct MyEnum
    {
        public const string Cpp = ".cpp";
        public const string CSharp = ".cs";
        public const string Python = ".py";
        public const string Java = ".java";

        public const string CppNoDot = "cpp";
        public const string CSharpNoDot = "cs";
        public const string PythonNoDot = "py";
        public const string JavaNoDot = "java";

        public const string VCXProj = ".vcxproj";
        public const string CSProj = ".csproj";
        public const string PyProj = ".pyproj";
        public const string Ipr = ".ipr";
        public const string Iws = ".iws";
        public const string Iml = ".iml";

        public const string Input = "input.txt";
        public const string Output = "output.txt";

        public const string Trash = @"\Trash\";
        public const string Slash = @"\";
        public const string Src = @"\src\";

        public const string VS2017 = "[vs2017]";
        public const string VS2010 = "[vs2010]";
        public const string SharpDevelop = "[#develop]";

        public const string Txt = ".txt";
        public const string Config = ".config";

        public const string TemplateProj = "example 0000. Template ";
        public const string TemplateCode = "Task_example0000";

        public const string TemplateCSharpProj = "example 0000. Template [cs]";
        public const string TemplateCSharp = "Task_example0000.cs";

        public const string AppName = "MySolutionExplorer";

        public const string CppSuff = "[cpp]";
        public const string CSharpSuff = "[cs]";
        public const string PySuff = "[py]";
        public const string JavaSuff = "[java]";

        public const string TemplateCppProj = "example 0000. Template [cpp]";
        public const string TemplateCpp = "Task_example0000.cpp";

        public const string TemplatePyProj = "example 0000. Template [py]";
        public const string TemplatePy = "Task_example0000.py";

        public const string TemplateJavaProj = "example 0000. Template [java]";
        public const string TemplateJava = "Task_example0000.java";
    }
}
