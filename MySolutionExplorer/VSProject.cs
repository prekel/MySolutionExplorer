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
	public class VSProject : Project
	{
		[XmlIgnore]
		protected List<FileInfo> VSProjFiles;
		[XmlIgnore]
		protected List<XmlDocument> VSProjXml;

		protected VSProject()
		{

		}

		protected VSProject(string path) : base(path)
		{

		}

		protected VSProject(int n)
		{
			VSProjFiles = new List<FileInfo>(n);
			VSProjXml = new List<XmlDocument>(n);
			for (var i = 0; i < n; i++)
			{
				VSProjFiles.Add(null);
				VSProjXml.Add(null);
			}
		}

		protected VSProject(string path, int n) : base(path)
		{
			VSProjFiles = new List<FileInfo>(n);
			VSProjXml = new List<XmlDocument>(n);
			for (var i = 0; i < n; i++)
			{
				VSProjFiles.Add(null);
				VSProjXml.Add(null);
			}
		}

		protected FileInfo CheckProjectFile(string ex, string suff, FileInfo file, FileInfo proj)
		{
			if (file.Extension == ex && file.Name.Contains(suff))
			{
				var ret = new FileInfo(Dir.FullName + MyEnum.Slash + file);
				AllowedFiles.Add(ret.FullName);
				return ret;
			}
			return proj;
		}

		protected XmlDocument LoadProject(FileInfo file)
		{
			var xml = new XmlDocument();
			xml.Load(file.FullName);
			return xml;
		}

		protected void CreateProj(string suff, FileInfo file)
		{
			file = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCppProj + suff + MyEnum.VCXProj);
			file = Solution.RenameFile(file, Name + suff + MyEnum.VCXProj);
		}

		protected void ReformRootNamespace(XmlDocument xml)
		{
			xml.DocumentElement["PropertyGroup"]["RootNamespace"].InnerText = RootNamespace;
		}

		protected void ReformCodeFileName(XmlDocument xml)
		{
			foreach (XmlElement i in xml.DocumentElement)
			{
				if (i.Name == "ItemGroup" && i.FirstChild.Name == "ClCompile")
				{
					i.FirstChild.Attributes[0].Value = CodeFileName;
				}
			}
		}

		protected void ReformVSProjXml(XmlDocument xml, FileInfo file)
		{
			ReformRootNamespace(xml);
			ReformCodeFileName(xml);
			xml.Save(file.FullName);
		}

		protected override void FindProjectFiles()
		{
			throw new NotImplementedException();
		}
	}
}
