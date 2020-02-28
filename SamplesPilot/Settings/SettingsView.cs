using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ascon.Pilot.SDK;
using Newtonsoft.Json;

namespace PilotSamples.Settings
{
    class SettingsView
    {
        private readonly ISettingValueProvider _setting;
        public SettingsModel Model { get; set; }

        public SettingsView(ISettingValueProvider setting)
        {
            _setting = setting;
            Model = GetSettings();
        }

        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new Command(Save));
            }
        }

        public SettingsModel GetSettings()
        {
            var value = _setting.GetValue();
            if (value == null)
                return new SettingsModel();
            else
                return JsonConvert.DeserializeObject<SettingsModel>(value);
        }

        public void Save()
        {
            _setting.SetValue(JsonConvert.SerializeObject(Model, Formatting.Indented));
        }

    }
}
