﻿using Desktop_task.Services.DataDb;
using Desktop_task.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Desktop_task
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(FinanceDbContext financeDb)
        {

                InitializeComponent();
                MainViewModel viewModel = new MainViewModel(financeDb);
                DataContext = viewModel;

        }
    }
}
