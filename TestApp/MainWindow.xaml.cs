﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AlertBarWpf;
using MahApps.Metro.IconPacks;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AlertBarManager.Alert("Default",
                AlertBuilder.Build()
                    .WithTitle("TestTitle")
                    .WithMessage("Hello")
                    .WithAction("Klick mich", () => MessageBox.Show("Hier ist die Antwort: 42"), true)
                    //.AsStickyInfo()
                    .WithBackground(Colors.Pink));
        }
    }
}
