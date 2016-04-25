using System;

namespace ScriptConfig.Sample
{
    public class AppConfiguration
    {
        public int Number { get; set; }

        public string Text { get; set; }
    }

    public enum DataTarget
    {
        Test,
        Production
    }

    public class MyAppConfig
    {
        public DataTarget Target { get; set; }
        public Uri AppUrl { get; set; }
        public int CacheTime { get; set; }
    }
}