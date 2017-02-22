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
	public abstract class VSProject : Project
	{
		//[XmlIgnore]
		//protected List<FileInfo> VSProjFiles;
		//[XmlIgnore]
		//protected List<XmlDocument> VSProjXml;

		protected List<XmlProjectFile> XmlProjectFiles;

		protected VSProject()
		{

		}

		protected VSProject(string path) : base(path)
		{

		}

		protected VSProject(int n)
		{
			//VSProjFiles = new List<FileInfo>(n);
			//VSProjXml = new List<XmlDocument>(n);
			XmlProjectFiles = new List<XmlProjectFile>(n);
			for (var i = 0; i < n; i++)
			{
				//VSProjFiles.Add(null);
				//VSProjXml.Add(null);
				XmlProjectFiles.Add(null);
			}
		}

		protected VSProject(string path, int n) : base(path)
		{
			//VSProjFiles = new List<FileInfo>(n);
			//VSProjXml = new List<XmlDocument>(n);
			XmlProjectFiles = new List<XmlProjectFile>(n);
			for (var i = 0; i < n; i++)
			{
				//VSProjFiles.Add(null);
				//VSProjXml.Add(null);
				XmlProjectFiles.Add(null);
			}
		}

		protected override void FindProjectFiles()
		{
			foreach (var i in Dir.GetFiles())
			{
				for (var j = 0; j < XmlProjectFiles.Count; j++)
				{
					if (CheckProjectFile(i, XmlProjectFiles[j]))
					{
						XmlProjectFiles[j].File = i;
						AllowedFiles.Add(i.FullName);
						break;
					}
				}
			}
		}

		protected bool CheckProjectFile(FileInfo file, XmlProjectFile proj)
		{
			if (file.Extension == proj.Extension && file.Name.Contains(proj.Suff))
			{
				return true;
			}
			return false;
		}


		//protected FileInfo CheckProjectFile(string ex, string suff, FileInfo file, FileInfo proj)
		//{
		//	if (file.Extension == ex && file.Name.Contains(suff))
		//	{
		//		var ret = new FileInfo(Dir.FullName + MyEnum.Slash + file);
		//		AllowedFiles.Add(ret.FullName);
		//		return ret;
		//	}
		//	return proj;
		//}

		protected void LoadProjects()
		{
			for (var i = 0; i < XmlProjectFiles.Count; i++)
			{
				XmlProjectFiles[i].Xml = new XmlDocument();
				XmlProjectFiles[i].Xml.Load(XmlProjectFiles[i].File.FullName);
			}
		}

		protected XmlDocument LoadProject(FileInfo file)
		{
			var xml = new XmlDocument();
			xml.Load(file.FullName);
			return xml;
		}

		protected void CreateProjects()
		{
			for (var i = 0; i < XmlProjectFiles.Count; i++)
			{
				CreateProj(XmlProjectFiles[i]);
			}
		}

		protected void CreateProj(XmlProjectFile proj)
		{
			proj.File = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCppProj + proj.Suff + MyEnum.VCXProj);
			proj.File = Solution.RenameFile(proj.File, Name + proj.Suff + MyEnum.VCXProj);
		}

		protected void CreateProj(string suff, FileInfo file)
		{
		}

		protected void ReformRootNamespace(XmlDocument xml)
		{
			xml.DocumentElement["PropertyGroup"]["RootNamespace"].FirstChild.Value = RootNamespace;
		}

		protected void ReformCodeFileName(XmlDocument xml)
		{
			xml.DocumentElement["ItemGroup"].FirstChild.Attributes[0].Value = CodeFileName;
			//foreach (XmlElement i in xml.DocumentElement)
			//{
			//	if (i.Name == "ItemGroup" && i.FirstChild.Name == "ClCompile")
			//	{
			//		i.FirstChild.Attributes[0].Value = CodeFileName;
			//	}
			//}
		}

		protected void ReformVSProjects()
		{
			for (var i = 0; i < XmlProjectFiles.Count; i++)
			{
				ReformVSProjXml(XmlProjectFiles[i]);
			}
		}

		protected abstract void ReformVSProjXml(XmlProjectFile proj);
		//{
		//	ReformRootNamespace(proj.Xml);
		//	ReformCodeFileName(proj.Xml);
		//	proj.Xml.Save(proj.File.FullName);
		//}

		protected void ReformVSProjXml(XmlDocument xml, FileInfo file)
		{
			ReformRootNamespace(xml);
			ReformCodeFileName(xml);
			xml.Save(file.FullName);
		}
	}
}
