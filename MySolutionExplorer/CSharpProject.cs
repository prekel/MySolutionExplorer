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
	public class CSharpProject : VSProject
	{
		[XmlIgnore]
		private XmlProjectFile VS2017ProjectFile { get { return XmlProjectFiles[0]; } set { XmlProjectFiles[0] = value; } }
		//[XmlIgnore]
		//private XmlProjectFile VS2010ProjectFile { get { return XmlProjectFiles[1]; } set { XmlProjectFiles[1] = value; } }

		private void Init()
		{
			VS2017ProjectFile = new XmlProjectFile
			{
				Suff = MyEnum.VS2017,
				Parent = this,
				Extension = MyEnum.CSProj
			};
			//VS2010ProjectFile = new XmlProjectFile
			//{
			//	Suff = MyEnum.VS2010,
			//	Parent = this,
			//	Extension = MyEnum.VCXProj
			//};
		}

		public CSharpProject() : base(1)
		{

		}

		public CSharpProject(string path) : base(path, 1)
		{

		}

		protected override void ReformVSProjXml(XmlProjectFile proj)
		{
			throw new NotImplementedException();
		}
	}
}
