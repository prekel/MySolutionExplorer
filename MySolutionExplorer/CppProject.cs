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
		[XmlIgnore]
		private FileInfo VS2017Proj { get { return base.VSProjFiles[0]; } set { base.VSProjFiles[0] = value; } }
		[XmlIgnore]
		private XmlDocument VS2017ProjXml;
		[XmlIgnore]
		private FileInfo VS2010Proj { get { return base.VSProjFiles[1]; } set { base.VSProjFiles[1] = value; } }
		[XmlIgnore]
		private XmlDocument VS2010ProjXml;

		public CppProject() : base(2)
		{

		}

		public CppProject(string path) : base(path, 2)
		{

		}

		protected override void FindProjectFiles()
		{
			foreach (var i in Dir.GetFiles())
			{
				VS2017Proj = CheckProjectFile(MyEnum.VCXProj, MyEnum.VS2017, i, VS2017Proj);
				VS2010Proj = CheckProjectFile(MyEnum.VCXProj, MyEnum.VS2010, i, VS2010Proj);
			}
		}
		
		private void LoadProjects()
		{
			if (VS2017Proj == null || VS2010Proj == null)
			{
				FindProjectFiles();
			}

			VS2017ProjXml = LoadProject(VS2017Proj);
			VS2010ProjXml = LoadProject(VS2010Proj); 
		}
		
		public new void CreateFiles()
		{
			base.CreateFiles();

			CreateProj(MyEnum.VS2017, VS2017Proj);
			CreateProj(MyEnum.VS2010, VS2010Proj);

			CodeFile = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCpp);
			CodeFile = Solution.RenameFile(CodeFile, CodeFileName);

			LoadProjects();
			ReformVSProjXml(VS2017ProjXml, VS2017Proj);
			ReformVSProjXml(VS2010ProjXml, VS2010Proj);

			FindFiles();
			FindProjectFiles();
		}
	}
}
