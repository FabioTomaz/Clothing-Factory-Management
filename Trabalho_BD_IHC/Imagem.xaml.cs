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
using System.Windows.Shapes;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for Imagem.xaml
    /// </summary>
    public partial class Imagem : Window
    {
        public Imagem(BitmapImage image)
        {
            InitializeComponent();
            this.image.Source = image;
        }
    }
}
