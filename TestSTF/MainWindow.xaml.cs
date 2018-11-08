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
using StringToFormulaTest;

namespace TestSTF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StringToFormula stf = new StringToFormula();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCalculateResult_Click(object sender, RoutedEventArgs e)
        {
            string exp = stringField.Text;

            double result = stf.EvaluateExpression(exp);
            resultField.Text = result.ToString();
        }
    }
}
