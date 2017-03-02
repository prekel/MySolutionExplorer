// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MySolutionExplorer
{
	/// <summary>
	/// Проект C#
	/// </summary>
	[Serializable]
	public class CSharpProject : VSProject
	{
		/// <summary>
		/// Файл .csproj версии 2017
		/// </summary>
		[XmlIgnore]
		private XmlProjectFile VS2017ProjectFile
		{
			get { return XmlProjectFiles[0]; }
			set { XmlProjectFiles[0] = value; }
		}
		/// <summary>
		/// Файл .csproj для SharpDevelop
		/// </summary>
		[XmlIgnore]
		private XmlProjectFile SharpDevelopProjectFile
		{
			get { return XmlProjectFiles[1]; }
			set { XmlProjectFiles[1] = value; }
		}

		/// <summary>
		/// Инициализация
		/// </summary>
		private void Init()
		{
			VS2017ProjectFile = new XmlProjectFile
			{
				Suff = MyEnum.VS2017,
				Parent = this,
				Extension = MyEnum.CSProj
			};
			SharpDevelopProjectFile = new XmlProjectFile
			{
				Suff = MyEnum.SharpDevelop,
				Parent = this,
				Extension = MyEnum.CSProj
			};
		}

		public CSharpProject() : base(2)
		{
			Init();
		}

		public CSharpProject(string path) : base(path, 2)
		{
			Init();
		}

		/// <summary>
		/// Создание файлов из шаблона, переименовывание, изменение имён внутри
		/// </summary>
		public override void CreateFiles()
		{
			CreateFiles(MyEnum.TemplateCSharpProj);

			FindProjectFiles();

			CreateProjects();

			CodeFile = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCSharp);
			CodeFile = Solution.RenameFile(CodeFile, CodeFileName);

			LoadProjects();
			ReformVSProjects();

			FindFiles();
			FindProjectFiles();
		}

		/// <summary>
		/// Действия при преобразовании шаблонного проекта
		/// </summary>
		/// <param name="proj">Xml-проект</param>
		protected override void ReformVSProjXml(XmlProjectFile proj)
		{
			ReformRootNamespace(proj.Xml);
			ReformAssemblyName(proj.Xml);
			ReformCodeFileName(proj.Xml);
			proj.Xml.Save(proj.File.FullName);
		}

		public static bool IsExistCodeFile(DirectoryInfo dir)
		{
			foreach (var i in dir.GetFiles())
			{
				if (i.Extension == MyEnum.CSharp)
				{
					return true;
				}
			}
			return false;
		}
	}
}