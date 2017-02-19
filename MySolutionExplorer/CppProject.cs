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
		public XmlDocument VSLastProj;

		public CppProject()
		{

		}

		public CppProject (string path) : base(path)
		{

		}

		public void FindProjectFiles()
		{
			foreach (var i in Dir.GetFiles())
			{
				if (i.Extension == ".vcxproj")
				{
					VSLastProj = new XmlDocument();
					VSLastProj.Load(Dir.FullName + @"\" + i);
				}
			}
		}
	}
}
