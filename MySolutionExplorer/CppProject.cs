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
	[Serializable]
	public class CppProject : VSProject
	{
		//[XmlIgnore]
		//private FileInfo VS2017Proj { get { return VSProjFiles[0]; } set { VSProjFiles[0] = value; } }
		//[XmlIgnore]
		//private XmlDocument VS2017ProjXml;
		//[XmlIgnore]
		//private FileInfo VS2010Proj { get { return VSProjFiles[1]; } set { VSProjFiles[1] = value; } }
		//[XmlIgnore]
		//private XmlDocument VS2010ProjXml;

		[XmlIgnore]
		private XmlProjectFile VS2017ProjectFile { get { return XmlProjectFiles[0]; } set { XmlProjectFiles[0] = value; } }
		[XmlIgnore]
		private XmlProjectFile VS2010ProjectFile { get { return XmlProjectFiles[1]; } set { XmlProjectFiles[1] = value; } }

		private void Init()
		{
			VS2017ProjectFile = new XmlProjectFile
			{
				Suff = MyEnum.VS2017,
				Parent = this,
				Extension = MyEnum.VCXProj
			};
			VS2010ProjectFile = new XmlProjectFile
			{
				Suff = MyEnum.VS2010,
				Parent = this,
				Extension = MyEnum.VCXProj
			};
		}

		public CppProject() : base(2)
		{
			Init();
		}

		public CppProject(string path) : base(path, 2)
		{
			Init();
		}

		//protected override void FindProjectFiles()
		//{
		//	foreach (var i in Dir.GetFiles())
		//	{
		//		VS2017Proj = CheckProjectFile(MyEnum.VCXProj, MyEnum.VS2017, i, VS2017Proj);
		//		VS2010Proj = CheckProjectFile(MyEnum.VCXProj, MyEnum.VS2010, i, VS2010Proj);
		//	}
		//}

		//private void LoadProjects()
		//{
		//	if (VS2017Proj == null || VS2010Proj == null)
		//	{
		//		FindProjectFiles();
		//	}

		//	LoadProjects();
		//	VS2017ProjXml = LoadProject(VS2017Proj);
		//	VS2010ProjXml = LoadProject(VS2010Proj);
		//}

		public new void CreateFiles()
		{
			base.CreateFiles();

			CreateProjects();
			//CreateProj(MyEnum.VS2017, VS2017Proj);
			//CreateProj(MyEnum.VS2010, VS2010Proj);

			CodeFile = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCpp);
			CodeFile = Solution.RenameFile(CodeFile, CodeFileName);

			LoadProjects();
			ReformVSProjects();

			FindFiles();
			FindProjectFiles();
		}

		protected override void ReformVSProjXml(XmlProjectFile proj)
		{
			ReformRootNamespace(proj.Xml);
			ReformCodeFileName(proj.Xml);
			proj.Xml.Save(proj.File.FullName);
		}
	}
}
