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
		public FileInfo VSLastProj { get; set; }
		[XmlIgnore]
		private XmlDocument VSLastProjXml = new XmlDocument();

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
				if (i.Extension == MyEnum.VCXProj)
				{
					VSLastProj = new FileInfo(Dir.FullName + MyEnum.Slash + i);
					AllowedFiles.Add(VSLastProj.FullName);
					VSLastProjXml.Load(VSLastProj.FullName);
				}
			}
		}
	}
}
