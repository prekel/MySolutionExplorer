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
	/// Проект
	/// </summary>
	[Serializable]
	public abstract class Project
	{
		private DirectoryInfo _dir;

		/// <summary>
		/// Директория проекта
		/// </summary>
		[XmlIgnore]
		public DirectoryInfo Dir
		{
			get => _dir ?? (_dir = new DirectoryInfo(ParentSolution.Dir + MyEnum.Slash + Name));
			set => _dir = value;
		}

		/// <summary>
		/// Файл с кодом
		/// </summary>
		[XmlIgnore]
		public FileInfo CodeFile { get; set; }

		/// <summary>
		/// Файл ввода (input.txt)
		/// </summary>
		[XmlIgnore]
		public FileInfo InputFile { get; set; }

		/// <summary>
		/// Файл вывода (output.txt)
		/// </summary>
		[XmlIgnore]
		public FileInfo OutputFile { get; set; }

		/// <summary>
		/// Решение
		/// </summary>
		[XmlIgnore]
		public Solution ParentSolution { get; set; }

		/// <summary>
		/// Разрешенные файлы
		/// Кандидат к удалению
		/// </summary>
		protected HashSet<string> AllowedFiles = new HashSet<string>();

		/// <summary>
		/// Разрешенные расширения в проекте
		/// </summary>
		protected HashSet<string> AllowedExtension = new HashSet<string>();

		/// <summary>
		/// Номер (ID) задачи
		/// </summary>
		public int Number { get; set; }
		/// <summary>
		/// Сайт задачи
		/// </summary>
		public string Site { get; set; }
		/// <summary>
		/// Язык
		/// </summary>
		public string Lang { get; set; }
		/// <summary>
		/// Название задачи
		/// </summary>
		public string TaskName { get; set; }

		/// <summary>
		/// Директория решения
		/// </summary>
		[XmlIgnore]
		public string Path
		{
			get => Dir.FullName;
			set => Dir = new DirectoryInfo(value);
		}

		/// <summary>
		/// Имя проекта (Имя задачи вместе с сайтом, номером и языком)
		/// </summary>
		public string Name
		{
			get => $"{Site} {Number:D4}. {TaskName} [{Lang}]";
			set
			{
				if (Number != 0 && Site != "" && Lang != "" && TaskName != "") return;
				var r = new System.Text.RegularExpressions.Regex("([a-z]+) ([0-9a-zA-Z]{4}). ([0-9А-Яа-яЁёA-Za-z- ]+)");
				var m = r.Match(value);
				Site = m.Groups[1].Value;
				Number = int.Parse(m.Groups[2].Value);
				TaskName = m.Groups[3].Value;
				TaskName = TaskName.Substring(0, TaskName.Length - 1);
			}
		}

		/// <summary>
		/// Пространство имён по умолчанию (кандидат к переносу в другой класс)
		/// </summary>
		public string RootNamespace => $"{Site}_{Number:D4}";

		/// <summary>
		/// Имя кодового файла (кандидат на изменение публичности)
		/// </summary>
		public string CodeFileName => $"Task_{Site}{Number:D4}.{Lang}";

		protected Project()
		{
		}

		protected Project(string path) => Path = path;

		/// <summary>
		/// Поиск файлов ввода-вывода и кода
		/// </summary>
		public void FindFiles()
		{
			FindProjectFiles();
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
				if (i.Extension == MyEnum.Cpp || i.Extension == MyEnum.CSharp || i.Extension == MyEnum.Python || i.Extension == MyEnum.Java)
				{
					CodeFile = i;
					AllowedFiles.Add(i.FullName);
				}
			}
		}

		/// <summary>
		/// Поиск файлов проекта
		/// </summary>
		protected abstract void FindProjectFiles();

		/// <summary>
		/// Очистка
		/// </summary>
		public void Clean()
		{
			FindFiles();
			foreach (var i in Dir.GetFiles())
			{
				if (AllowedFiles.Contains(i.FullName)) continue;
				Directory.CreateDirectory(ParentSolution.Dir + MyEnum.Trash + Name);
				Directory.Move(i.FullName, ParentSolution.Dir + MyEnum.Trash + Name + MyEnum.Slash + i.Name);
			}
		}

		/// <summary>
		/// Кандидат на перенос или удаление или переименование
		/// </summary>
		public abstract void CreateFiles();

		/// <summary>
		/// Копирование из Templates в папку с проектом
		/// </summary>
		/// <param name="templatename">Имя шаблонного проекта</param>
		public void CreateFiles(string templatename)
		{
			if (Dir.Exists)
			{
				throw new Exception("Папка с проектом уже есть");
			}
			try
			{
				Solution.DirectoryCopy(@"Templates\" + templatename, Dir.FullName, true);
			}
			catch
			{
				// ignored
			}
		}

		public void Delete()
		{
			ParentSolution.Remove(this);
			Dir.Delete(true);
		}

		public abstract void Rename();
    }
}