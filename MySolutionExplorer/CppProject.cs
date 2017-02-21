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
	public class CppProject : Project
	{
		[XmlIgnore]
		private FileInfo VS2017Proj { get; set; }
		[XmlIgnore]
		private XmlDocument VS2017ProjXml;
		[XmlIgnore]
		private FileInfo VS2010Proj { get; set; }
		[XmlIgnore]
		private XmlDocument VS2010ProjXml;

		public CppProject()
		{

		}

		public CppProject(string path) : base(path)
		{

		}

		public void CheckProjectFile(string ex, string suff, FileInfo file, FileInfo proj)
		{
			if (file.Extension == ex && file.Name.Contains(suff))
			{
				proj = new FileInfo(Dir.FullName + MyEnum.Slash + file);
				AllowedFiles.Add(proj.FullName);
			}
		}

		protected override void FindProjectFiles()
		{
			foreach (var i in Dir.GetFiles())
			{
				CheckProjectFile(MyEnum.VCXProj, MyEnum.VS2017, i, VS2017Proj);
				CheckProjectFile(MyEnum.VCXProj, MyEnum.VS2010, i, VS2010Proj);
			}
		}

		public void LoadProject(FileInfo file, XmlDocument xml)
		{
			xml = new XmlDocument();
			xml.Load(file.FullName);
		}

		private void LoadProjects()
		{
			if (VS2017Proj == null || VS2010Proj == null)
			{
				FindProjectFiles();
			}

			LoadProject(VS2017Proj, VS2017ProjXml);
			LoadProject(VS2010Proj, VS2010ProjXml); 
		}

		public void CreateProj(string suff, FileInfo file)
		{
			file = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCppProj + suff + MyEnum.VCXProj);
			file = Solution.RenameFile(file, Name + suff + MyEnum.VCXProj);
		}

		public new void CreateFiles()
		{
			base.CreateFiles();

			CreateProj(MyEnum.VS2017, VS2017Proj);
			CreateProj(MyEnum.VS2010, VS2010Proj);

			CodeFile = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCpp);
			CodeFile = Solution.RenameFile(CodeFile, CodeFileName);

			LoadProjects();
			ReformVS2017ProjXml();
			ReformVS2010ProjXml();

			FindFiles();
			FindProjectFiles();
		}

		private void ReformVS2017ProjXml()
		{
			VS2017ProjXml.DocumentElement["PropertyGroup"]["RootNamespace"].InnerText = RootNamespace;
			foreach (XmlElement i in VS2017ProjXml.DocumentElement)
			{
				if (i.Name == "ItemGroup" && i.FirstChild.Name == "ClCompile")
				{
					i.FirstChild.Attributes[0].Value = CodeFileName;
				}
			}
			VS2017ProjXml.Save(VS2017Proj.FullName);
		}

		private void ReformVS2010ProjXml()
		{
			VS2010ProjXml.DocumentElement["PropertyGroup"]["RootNamespace"].InnerText = RootNamespace;
			foreach (XmlElement i in VS2010ProjXml.DocumentElement)
			{
				if (i.Name == "ItemGroup" && i.FirstChild.Name == "ClCompile")
				{
					i.FirstChild.Attributes[0].Value = CodeFileName;
				}
			}
			VS2010ProjXml.Save(VS2010Proj.FullName);
		}
	}
}
