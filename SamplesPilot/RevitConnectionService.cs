using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ascon.Pilot.SDK;
using Newtonsoft.Json;
using PilotSamples.Settings;
using System.ComponentModel.Composition;

namespace PilotSamples
{
    [Export(typeof(IDataPlugin))]
    class RevitConnectionService : IDataPlugin, IRevitConnectionService, IObserver<KeyValuePair<string, string>>
    {
        private static SettingsModel _personalSettings;
        private static IObjectModifier _modifier;
        private static IPersonalSettings _settings;
        private static IObjectsRepository _repository;

        [ImportingConstructor]
        public RevitConnectionService(IObjectsRepository repository, IObjectModifier modifier, IPersonalSettings settings)
        {
            _modifier = modifier;
            _repository = repository;
            _settings = settings;
            _settings.SubscribeSetting(SettingsKey.Key).Subscribe(this);
        }

        public RevitConnectionService()
        {
        }

        public void CreateMessageObject(MessageResponse response)
        {
            if (_personalSettings == null)
                return;
            if (!Guid.TryParse(_personalSettings.ParentGuid, out Guid parentGuid))
                return;

            var objectType = _repository.GetType(_personalSettings.ObjectType);
            var objectAttribut = _personalSettings.ObjectAttributes;

            _modifier.CreateById(Guid.NewGuid(), parentGuid, objectType).SetAttribute(objectAttribut, response.Message);
            _modifier.Apply();
        }

        public void OnNext(KeyValuePair<string, string> value)
        {
            if (value.Key == SettingsKey.Key && value.Value != null)
            {
                _personalSettings = JsonConvert.DeserializeObject<SettingsModel>(value.Value);
            }
        }
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }
    }
}
