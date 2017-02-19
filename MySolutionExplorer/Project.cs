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
	public class Project
	{
		[XmlIgnore]
		public DirectoryInfo Dir { get; set; }
		[XmlIgnore]
		public FileInfo CodeFile { get; set; }
		[XmlIgnore]
		public FileInfo InputFile { get; set; }
		[XmlIgnore]
		public FileInfo OutputFile { get; set; }
		[XmlIgnore]
		public Solution ParentSolution { get; set; }

		public int Number { get; set; }
		public string Site { get; set; }
		public string Lang { get; set; }
		public string Name { get; set; }

		[XmlIgnore]
		public string Path
		{
			get
			{
				return Dir.FullName;
			}
			set
			{
				Dir = new DirectoryInfo(value);
			}
		}

		public string FullName
		{
			get
			{
				return String.Format("{0} {1:D4}. {2} [{3}]", Site, Number, Name, Lang);
			}
			set
			{
				if (Number == 0 || Site == "" || Lang == "" || Name == "")
				{
					throw new NotImplementedException();
				}
			}
		}
		public string RootNamespace
		{
			get
			{
				return String.Format("{0}_{1:D4}", Site, Number);
			}
		}

		public Project()
		{

		}

		public Project(string path)
		{
			Path = path;
		}

		public void FindFiles()
		{
			foreach (var i in Dir.GetFiles())
			{
				if (i.Name == "input.txt")
				{
					InputFile = i;
				}
				if (i.Name == "output.txt")
				{
					OutputFile = i;
				}
				if (i.Extension == ".cpp" || i.Extension == ".cs" || i.Extension == ".py")
				{
					CodeFile = i;
				}
			}
		}
	}
}
