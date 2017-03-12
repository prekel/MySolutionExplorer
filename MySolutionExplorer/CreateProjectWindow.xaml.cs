// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MySolutionExplorer
{
	public class ProjectEventArgs : EventArgs
	{
		public Project p;
		public ProjectEventArgs(Project p)
		{
			this.p = p;
		}
	}

	/// <summary>
	/// Логика взаимодействия для CreateProjectWindow.xaml
	/// </summary>
	public partial class CreateProjectWindow : Window
	{
		public event EventHandler<ProjectEventArgs> Create;
		Solution s;

		public CreateProjectWindow()
		{
			InitializeComponent();
		}

		public CreateProjectWindow(Solution s) : this()
		{
			this.s = s;
		}

		public void CreateCppProject()
		{
			CppProject.Create(s, nameText.Text, siteText.Text, numberText.Text, s.Dir);
		}

		public void CreateCSharpProject()
		{
			CSharpProject.Create(s, nameText.Text, siteText.Text, numberText.Text, s.Dir);
		}

		private void createbutton_Click(object sender, RoutedEventArgs e)
		{
			var lang = ((TextBlock)langList.SelectedValue).Text;
			if (lang == "cpp")
			{
				CreateCppProject();
			}
			else if (lang == "cs")
			{
				CreateCSharpProject();
			}
			Create(this, new ProjectEventArgs(s[s.Count - 1]));
		}
	}
}
