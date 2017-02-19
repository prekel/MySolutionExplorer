using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace MySolutionExplorer
{
	[Serializable]
	[XmlRoot("Solution")]
	[XmlInclude(typeof(Project))]
	[XmlInclude(typeof(CppProject))]
	public class Solution : List<Project>
	{
		[XmlIgnore]
		public FileInfo DirSolution;
		[XmlIgnore]
		public DirectoryInfo Dir;

		public Solution()
		{

		}

		public Solution(string path)
		{
			DirSolution = new FileInfo(path);
			Dir = DirSolution.Directory;
		}

		public void FindProjects()
		{
			foreach (var i in Dir.GetDirectories())
			{
				if (i.Name.Contains("[cpp]"))
				{
					Add(new CppProject(i.FullName));
				}
			}
		}

		public new void Add(Project item)
		{
			base.Add(item);
			item.ParentSolution = this;
		}

		public void DeleteTrash()
		{
			try
			{
				Directory.Delete(Dir + MyEnum.Trash, true);
			}
			catch (Exception e)
			{
				//ignored
			}
			//Directory.CreateDirectory(Dir + MyEnum.Trash);
		}
	}
}
