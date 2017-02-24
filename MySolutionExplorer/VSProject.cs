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
	/// Проект Visual Studio
	/// </summary>
	[Serializable]
	public abstract class VSProject : Project
	{
		protected List<XmlProjectFile> XmlProjectFiles;

		protected VSProject()
		{
		}

		protected VSProject(string path) : base(path)
		{
		}

		private void Init(int n)
		{
			XmlProjectFiles = new List<XmlProjectFile>(n);
			for (var i = 0; i < n; i++)
			{
				XmlProjectFiles.Add(null);
			}
		}

		protected VSProject(int n)
		{
			Init(n);
		}


		protected VSProject(string path, int n) : base(path)
		{
			Init(n);
		}

		protected override void FindProjectFiles()
		{
			foreach (var i in Dir.GetFiles())
			{
				foreach (XmlProjectFile j in XmlProjectFiles)
				{
					if (CheckProjectFile(i, j))
					{
						j.File = i;
						AllowedFiles.Add(i.FullName);
						break;
					}
				}
			}
		}

		protected bool CheckProjectFile(FileInfo file, XmlProjectFile proj)
		{
			return file.Extension == proj.Extension && file.Name.Contains(proj.Suff);
		}

		protected void LoadProjects()
		{
			foreach (XmlProjectFile i in XmlProjectFiles)
			{
				i.Xml = new XmlDocument();
				i.Xml.Load(i.File.FullName);
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
			foreach (XmlProjectFile i in XmlProjectFiles)
			{
				CreateProj(i);
			}
		}

		protected void CreateProj(XmlProjectFile proj)
		{
			proj.File = Solution.RenameFile(proj.File, Name + proj.Suff + proj.Extension);
		}

		protected void CreateProj(string suff, FileInfo file)
		{
		}

		protected void ReformRootNamespace(XmlDocument xml)
		{
			xml.DocumentElement["PropertyGroup"]["RootNamespace"].FirstChild.Value = RootNamespace;
		}

		protected void ReformAssemblyName(XmlDocument xml)
		{
			xml.DocumentElement["PropertyGroup"]["AssemblyName"].FirstChild.Value = Name;
		}

		protected void ReformCodeFileName(XmlDocument xml)
		{
			xml.DocumentElement["ItemGroup"].FirstChild.Attributes[0].Value = CodeFileName;
		}

		protected void ReformVSProjects()
		{
			foreach (XmlProjectFile i in XmlProjectFiles)
			{
				ReformVSProjXml(i);
			}
		}

		protected abstract void ReformVSProjXml(XmlProjectFile proj);

		protected void ReformVSProjXml(XmlDocument xml, FileInfo file)
		{
			ReformRootNamespace(xml);
			ReformCodeFileName(xml);
			xml.Save(file.FullName);
		}
	}
}