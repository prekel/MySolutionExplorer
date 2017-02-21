using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Build.Construction;
using Microsoft.Build;
using Microsoft.Build.Exceptions;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Logging;
using System.Xml;
using System.Xml.Serialization;

namespace MySolutionExplorer
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Solution s;
		DirectoryInfo dir;
		FileInfo dirfile;

		public MainWindow()
		{
			InitializeComponent(); 
			//var s = SolutionFile.Parse(@"C:\Users\vladislav\OneDrive\Projects\MySolutionExplorer\ExperimentalSolution\ExperimentalSolution.sln");
			//s.ProjectsInOrder[0].Dependencies.
			//var p2 = new MyProject(dir.FullName + @"acmp 0156. Шахматы - 2 cs\acmp 0156. Шахматы - 2 cs.csproj");
			//var f = dir.GetFiles().
			//var d = dir.GetDirectories();

			//var p1 = new MyProject(dir.FullName + @"acmp 0114. Без двух нулей подряд\acmp 0114. Без двух нулей подряд.vcxproj");
			//var p = new MyProject(dir.FullName + @"acmp 0001. A+B [cpp]\acmp 0001. A+B [cpp].vcxproj");
			//p.VSLastProj.AddItem("txt", dir.FullName + @"acmp 0001. A+B [cpp]\input.txt");


			//var x = XmlReader.Create(dir + "textxml.xml");
			//var x = new XmlDocument();
			//x.Load(dir + "textxml.xml");
			//x.DocumentElement["PropertyGroup"]["RootNamespace"].InnerText = "acmp_0001";
			//x.Save(dir + "textxml.xml");


			//Directory.SetCurrentDirectory(dir.FullName);

			//var p = new CppProject(dir + @"acmp 0001. A+B [cpp]")
			//{
			//	Name = "A+B",
			//	Site = "acmp",
			//	Number = 1,
			//	Lang = "cpp"
			//};
			//p.FindFiles();
			//p.FindProjectFiles();

			//var p1 = new Project(dir + @"acmp 0156. Шахматы - 2 cs")
			//{
			//	Name = "Шахматы - 2",
			//	Site = "acmp",
			//	Number = 156,
			//	Lang = "cs"
			//};

			//var s = new Solution(dir + "ExperimentalSolution.mysln") { p };//, p1 };
			//s = Solution.Load(dir + "ExperimentalSolution.mysln");

			//var p2 = new CppProject(dir + @"acmp 0108. Неглухой телефон [cpp]")
			//{
			//	Name = "Неглухой телефон",
			//	Site = "acmp",
			//	Number = 108,
			//	Lang = "cpp"
			//};
			//s.Add(p2);
			//p2.CreateFiles();

			//s.Save();

			//s.DeleteTrash();
			//((CppProject)s[0]).FindProjectFiles();
			//s[0].Clean();

			//s.FindProjects();

			//var serializer = new XmlSerializer(typeof(Solution));

			//using (var fs = new StreamWriter(dir + "ExperimentalSolution.mysln"))
			//{
			//	serializer.Serialize(fs, s);
			//}

			//using (var fs = new FileStream(dir + "ExperimentalSolution.mysln", FileMode.OpenOrCreate))
			//{
			//	var s1 = (Solution)serializer.Deserialize(fs);
			//}

			//mainTable.ItemsSource = s;
			
		}

		private void loadButton_Click(object sender, RoutedEventArgs e)
		{
			s = Solution.Load(dirfile.FullName);
			mainTable.ItemsSource = s;
		}

		private void createButton_Click(object sender, RoutedEventArgs e)
		{
			if (s == null)
			{
				s = new Solution { DirSolution = new FileInfo(dirfile.FullName) };
			}
			var p = new CppProject
			{
				Name = nameText.Text,
				Site = siteText.Text,
				Number = int.Parse(numberText.Text),
				Lang = ((TextBlock)langList.SelectedValue).Text
			};
			p.Path = dir + MyEnum.Slash + p.FullName;
			p.CreateFiles();
			s.Add(p);
			mainTable.ItemsSource = null;
			mainTable.ItemsSource = s;
		}

		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			s.Save();
		}

		private void showButton_Click(object sender, RoutedEventArgs e)
		{
			var myDialog = new Microsoft.Win32.OpenFileDialog()
			{
				Filter = "MySLN|*.mysln|XML|*.xml|Все файлы|*.*",
				CheckFileExists = true
			};
			if (myDialog.ShowDialog() == true)
			{
				dirfile = new FileInfo(myDialog.FileName);
				dir = dirfile.Directory;
			}
		}
	}
}
