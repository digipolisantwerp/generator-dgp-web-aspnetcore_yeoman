using System;

namespace StarterKit.Utilities.Configs
{
    public class LoggingFileTargetConfiguration
    {
        public string Name { get; set; }
		public string Path { get; set; }
        public string FileName { get; set; }
        public string Level { get; set; }
        public string Layout { get; set; }
    }
}