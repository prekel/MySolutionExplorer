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
	public class CSharpProject : VSProject
	{
		[XmlIgnore]
		private FileInfo VS2017Proj { get { return VSProjFiles[0]; } set { VSProjFiles[0] = value; } }
		[XmlIgnore]
		private XmlDocument VS2017ProjXml;

		public CSharpProject() : base(1)
		{

		}

		public CSharpProject(string path) : base(path, 1)
		{

		}
	}
}
