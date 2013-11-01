using Dynamo.Services;
using Dynamo.UI.Prompts;
using Dynamo.Utilities;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Dynamo.Services
{
    public class UsageReportingManager : NotificationObject
    {
        #region Private
        private static UsageReportingManager instance;
        #endregion

        #region Properties
        public static UsageReportingAgreementPrompt UsageReportingPrompt { get; set; }

        public static UsageReportingManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new UsageReportingManager();
                return instance;
            }
        }

        public bool IsUsageReportingApproved
        {
            get
            {
                return dynSettings.Controller.PreferenceSettings.IsUsageReportingApproved;
            }
            private set
            {
                dynSettings.Controller.PreferenceSettings.IsUsageReportingApproved = value;
                RaisePropertyChanged("IsUsageReportingApproved");
                
                // Call PreferenceSettings to save
                dynSettings.Controller.PreferenceSettings.Save();
            }
        }

        public bool FirstRun
        {
            get
            {
                return dynSettings.Controller.PreferenceSettings.IsFirstRun;
            }
            private set
            {
                dynSettings.Controller.PreferenceSettings.IsFirstRun = value;
                RaisePropertyChanged("FirstRun");
            }
        }
        #endregion

        public void CheckFirstRun()
        {
            // First run of Dynamo
            if (dynSettings.Controller.PreferenceSettings.IsFirstRun)
            {
                this.FirstRun = false;

                if (!dynSettings.Controller.Testing)
                    ShowUsageReportingPrompt();
            }
        }

        public void SetUsageReportingAgreement(bool approved)
        {
            IsUsageReportingApproved = approved;
        }

        public void ToggleUsageReportingAgreement()
        {
            bool resultOption = !IsUsageReportingApproved;

            // If toggling to approve usage reporting, show agreement consent
            if (resultOption)
                ShowUsageReportingPrompt();
            else
                IsUsageReportingApproved = false;
        }

        public void ShowUsageReportingPrompt()
        {
            UsageReportingPrompt = new UsageReportingAgreementPrompt();
            if (null != Application.Current)
                UsageReportingPrompt.Owner = Application.Current.MainWindow;

            UsageReportingPrompt.ShowDialog();
            
        }
    }
}
