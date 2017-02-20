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
		private FileInfo VSLastProj { get; set; }
		[XmlIgnore]
		private XmlDocument VSLastProjXml;
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

		private override void FindProjectFiles()
		{
			foreach (var i in Dir.GetFiles())
			{
				if (i.Extension == MyEnum.VCXProj && i.Name.Contains(MyEnum.VSLast))
				{
					VSLastProj = new FileInfo(Dir.FullName + MyEnum.Slash + i);
					AllowedFiles.Add(VSLastProj.FullName);
				}
				if (i.Extension == MyEnum.VCXProj && i.Name.Contains(MyEnum.VS2010))
				{
					VS2010Proj = new FileInfo(Dir.FullName + MyEnum.Slash + i);
					AllowedFiles.Add(VS2010Proj.FullName);
				}
			}
		}

		private void LoadProjects()
		{
			if (VSLastProj == null)
			{
				FindProjectFiles();
			}
			VSLastProjXml = new XmlDocument();
			VSLastProjXml.Load(VSLastProj.FullName);
			if (VS2010Proj == null)
			{
				FindProjectFiles();
			}
			VS2010ProjXml = new XmlDocument();
			VS2010ProjXml.Load(VS2010Proj.FullName);
		}

		public new void CreateFiles()
		{
			base.CreateFiles();

			VSLastProj = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCppProj + MyEnum.VSLast + MyEnum.VCXProj);
			VSLastProj = Solution.RenameFile(VSLastProj, FullName + MyEnum.VSLast + MyEnum.VCXProj);

			VS2010Proj = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCppProj + MyEnum.VS2010 + MyEnum.VCXProj);
			VS2010Proj = Solution.RenameFile(VS2010Proj, FullName + MyEnum.VS2010 + MyEnum.VCXProj);

			CodeFile = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCpp);
			CodeFile = Solution.RenameFile(CodeFile, CodeFileName);

			LoadProjects();
			ReformVSLastProjXml();
			ReformVS2010ProjXml();

			FindFiles();
			FindProjectFiles();
		}

		private void ReformVSLastProjXml()
		{
			VSLastProjXml.DocumentElement["PropertyGroup"]["RootNamespace"].InnerText = RootNamespace;
			foreach (XmlElement i in VSLastProjXml.DocumentElement)
			{
				if (i.Name == "ItemGroup" && i.FirstChild.Name == "ClCompile")
				{
					i.FirstChild.Attributes[0].Value = CodeFileName;
				}
			}
			VSLastProjXml.Save(VSLastProj.FullName);
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
