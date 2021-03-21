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
    /// Проект Java
    /// </summary>
    [Serializable]
    public class JavaProject : IprProject
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        private void Init()
        {
            Lang = "cpp";
        }

        public JavaProject() : base(3) => Init();

        public JavaProject(string path) : base(path, 3) => Init();

        public override void CreateFiles()
        {
            CreateFiles(MyEnum.TemplateJavaProj);

            FindProjectFiles();

            CreateProjects();

            CodeFile = new FileInfo(Dir + MyEnum.Src + MyEnum.TemplateJava);
            CodeFile = Solution.RenameFile(CodeFile, CodeFileName);

            LoadProjects();
            ReformAll();

            FindFiles();
            FindProjectFiles();
        }

        /// <summary>
        /// Кандидат к удалению
        /// </summary>
        public static void Create(Solution s, string text1, string text2, string text3, DirectoryInfo dir)
        {
            throw new NotImplementedException();
        }
    }
}
