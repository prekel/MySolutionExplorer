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

		protected HashSet<string> AllowedFiles = new HashSet<string>();

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
				if (i.Name == MyEnum.Input)
				{
					InputFile = i;
					AllowedFiles.Add(i.FullName);
				}
				if (i.Name == MyEnum.Output)
				{
					OutputFile = i;
					AllowedFiles.Add(i.FullName);
				}
				if (i.Extension == MyEnum.Cpp || i.Extension == MyEnum.CSharp || i.Extension == MyEnum.Python)
				{
					CodeFile = i;
					AllowedFiles.Add(i.FullName);
				}
			}
		}
		public void Clean()
		{
			
			foreach (var i in Dir.GetFiles())
			{
				if (!AllowedFiles.Contains(i.FullName))
				{
					Directory.CreateDirectory(ParentSolution.Dir + MyEnum.Trash + FullName);
					Directory.Move(i.FullName, ParentSolution.Dir + MyEnum.Trash + FullName + MyEnum.Slash + i.Name);
				}
			}
		}
	}
}
