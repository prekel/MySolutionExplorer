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
	[XmlInclude(typeof(Project))]
	[XmlInclude(typeof(CppProject))]
	public class Solution : List<Project>
	{
		[XmlIgnore]
		public DirectoryInfo Dir;

		public Solution(string path)
		{
			Dir = new DirectoryInfo(path);
		}
	}
}
