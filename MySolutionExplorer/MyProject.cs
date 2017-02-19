using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace MySolutionExplorer
{
	public class Project
	{
		public DirectoryInfo Dir { get; set; }
		public DirectoryInfo CodeFile { get; set; }
		public DirectoryInfo InputFile { get; set; }
		public DirectoryInfo OutputFile { get; set; }

		public int Number { get; set; }
		public string Site { get; set; }
		public string Lang { get; set; }
		public string Name { get; set; }

		public string FullName
		{
			get
			{
				return String.Format("{0} {1:D4}. {2} [{3}]", Site, Number, Name, Lang);
			}
		}
		public string RootNamespace
		{
			get
			{
				return String.Format("{0}_{1:D4}", Site, Number);
			}
		}
		public XmlDocument VSLastProj;

		public Project()
		{

		}

		public Project(string path)
		{
			Dir = new DirectoryInfo(path).Parent;
		}
	}
}
