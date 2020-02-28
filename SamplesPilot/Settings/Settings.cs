using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Ascon.Pilot.SDK;
using System.Windows;

namespace PilotSamples.Settings
{
    [Export(typeof(ISettingsFeature))]
    class Settings : ISettingsFeature
    {
        private ISettingValueProvider _setting;

        public string Key => SettingsKey.Key;

        public string Title => "WCF Samples";

        public FrameworkElement Editor
        {
            get
            {
                var view = new SettingsView(_setting);
                var control = new SettingsControl()
                {
                    DataContext = view
                };

                return control;  
            }
        }

        public void SetValueProvider(ISettingValueProvider settingValueProvider)
        {
            _setting = settingValueProvider;
        }
    }
}
