// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MySolutionExplorer.Core
{
    /// <summary>
    /// Проект IntelliJ IDEA file-based
    /// </summary>
    [Serializable]
    public abstract class IprProject : XmlProject
    {
        [XmlIgnore]
        protected XmlProjectFile IprFile
        {
            get => XmlProjectFiles[0];
            set => XmlProjectFiles[0] = value;
        }

        [XmlIgnore]
        protected XmlProjectFile IwsFile
        {
            get => XmlProjectFiles[1];
            set => XmlProjectFiles[1] = value;
        }

        [XmlIgnore]
        protected XmlProjectFile ImlFile
        {
            get => XmlProjectFiles[2];
            set => XmlProjectFiles[2] = value;
        }

        protected IprProject() : base(3) => Init();

        protected IprProject(string path) : base(path, 3) => Init();

        private void Init()
        {
            IprFile = new XmlProjectFile
            {
                //Suff = MyEnum.,
                Parent = this,
                Extension = MyEnum.Ipr
            };
            IwsFile = new XmlProjectFile
            {
                //Suff = MyEnum.SharpDevelop,
                Parent = this,
                Extension = MyEnum.Iws
            };
            ImlFile = new XmlProjectFile
            {
                //Suff = MyEnum.SharpDevelop,
                Parent = this,
                Extension = MyEnum.Iml
            };
        }

        protected IprProject(int n) : base(n)
        {
            Init();
        }

        protected IprProject(string path, int n) : base(path, n)
        {
            Init();
        }

        protected void ReformFiles()
        {
            ReformIpr();
            ReformIws();
            ReformIml();
        }

        protected void Replace(XmlProjectFile file)
        {
            string s;
            using (var sr = new StreamReader(file.File.OpenRead()))
            {
                s = sr.ReadToEnd();
            }
            s = s.Replace($"{MyEnum.TemplateProj}[{Lang}]", Name);
            s = s.Replace($"{MyEnum.TemplateCode}.{Lang}", CodeFileName);
            using (var sw = new StreamWriter(file.File.OpenWrite()))
            {
                sw.Write(s);
            }
        }

        protected void ReformIpr()
        {
            Replace(IprFile);
        }

        protected void ReformIws()
        {
            Replace(IwsFile);
        }

        protected void ReformIml()
        {
            Replace(ImlFile);
        }
    }
}
