using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dynamo.UI.Prompts
{
    /// <summary>
    /// Interaction logic for UsageReportingAgreementPrompt.xaml
    /// </summary>
    public partial class UsageReportingAgreementPrompt : Window
    {
        public bool IsUsageReportingApproved { get; private set; }

        public UsageReportingAgreementPrompt()
        {
            InitializeComponent();
            this.IsUsageReportingApproved = false;
        }

        private void OnContinueClick(object sender, RoutedEventArgs e)
        {
            if (acceptCheck.IsChecked.HasValue)
                this.IsUsageReportingApproved = acceptCheck.IsChecked.Value;
            Close();
        }
    }
}
