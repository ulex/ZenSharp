using System;
using System.Collections.Generic;

using JetBrains.Application.Settings.Storage;
using JetBrains.Application.Settings.Storage.Persistence;
using JetBrains.Util;

namespace Github.Ulex.ZenSharp.Integration
{
    class Program
    {
        /// <summary>
        /// does not working
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //var stringBuilder = new StringBuilder();
            var dictionary = new Dictionary<string, object>() {{"fdsdf","fds"}, {"a", "fdsf"}};
            //var xmlReader = XmlReader.Create(@"C:\Users\aulitin\AppData\Roaming\JetBrains\ReSharper\vAny\GlobalSettingsStorage.My.DotSettings");
            ILogger mylogger = new MyLogger();
            //var aa = XmlWriter.Create(stringBuilder);
            //var resul = EntriesWriter.Run(dictionary, action => action(xmlReader), action => action(aa), mylogger, mylogger);
            var settingsStorageFlat = new SettingsStorageFlat("noname", mylogger);
            var storage = settingsStorageFlat as ISettingsStorageEntriesSerialization;
            var setstorag = settingsStorageFlat as ISettingsStorage;
            setstorag.Set(KeyPathComponents.Parse("C:\\out.txt"), null);
            storage.Load(SettingsStorageLoadFlag.FirstTime, () => dictionary);
            storage.Save(a => true);

        }
    }

    internal class MyLogger : ILogger
    {
        public void LogException(Exception ex, ExceptionOrigin origin)
        {
        }

        public void LogOrThrowException(Exception ex, ExceptionOrigin origin)
        {
        }

        public void LogMessage(string message, LoggingLevel level = LoggingLevel.NORMAL)
        {
        }

        public bool IsEnabled(LoggingLevel level)
        {
            return true;
        }
    }
}
