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
		public FileInfo VSLastProj { get; set; }
		[XmlIgnore]
		private XmlDocument VSLastProjXml;

		public CppProject()
		{

		}

		public CppProject(string path) : base(path)
		{

		}

		public void FindProjectFiles()
		{
			foreach (var i in Dir.GetFiles())
			{
				if (i.Extension == MyEnum.VCXProj)
				{
					VSLastProj = new FileInfo(Dir.FullName + MyEnum.Slash + i);
					AllowedFiles.Add(VSLastProj.FullName);
				}
			}
		}

		public void LoadProjects()
		{
			if (VSLastProj == null)
			{
				FindProjectFiles();
			}
			VSLastProjXml = new XmlDocument();
			VSLastProjXml.Load(VSLastProj.FullName);
		}

		public new void CreateFiles()
		{
			base.CreateFiles();
			VSLastProj = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateVSLastProj + MyEnum.VCXProj);
			VSLastProj = Solution.RenameFile(VSLastProj, FullName + MyEnum.VCXProj);
			CodeFile = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCpp);
			CodeFile = Solution.RenameFile(CodeFile, CodeFileName);

			LoadProjects();
			ReformVSLastProjXml();

			FindFiles();
			FindProjectFiles();
		}

		public void ReformVSLastProjXml()
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
	}
}
