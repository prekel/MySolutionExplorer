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
	/// Xml-файл проекта
	/// </summary>
	public class XmlProjectFile
	{
		public FileInfo File { get; set; }
		public XmlDocument Xml { get; set; }
		public Project Parent { get; set; }
		public string Suff { get; set; }
		public string Extension { get; set; }
	}
}