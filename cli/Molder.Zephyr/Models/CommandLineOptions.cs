using CommandLine;

namespace Molder.Zephyr.Models
{
    public class CommandLineOptions
    {
        [Value(index: 0, Required = true, HelpText = "Путь к файлу с результатами прогона.", Default = null)]
        public string Path { get; set; }

        [Option(shortName: 'u', longName: "url", Required = true, HelpText = "Адрес к API Jira Zephyr.", Default = null)]
        public string Url { get; set; }
        
        [Option(shortName: 'v', longName: "version", Required = false, HelpText = "Версия API.", Default = "latest")]
        public string Version { get; set; }

        [Option(shortName: 'l', longName: "login", Required = false, HelpText = "Логин пользователя.", Default = null)]
        public string Login { get; set; }
        
        [Option(shortName: 'p', longName: "password", Required = false, HelpText = "Пароль пользователя.", Default = null)]
        public string Password { get; set; }
        
        [Option(shortName: 'r', longName: "release", Required = true, HelpText = "Релиз для прогона.", Default = null)]
        public string Release { get; set; }
        
        [Option(longName: "project", Required = true, HelpText = "Проект.", Default = null)]
        public string Project { get; set; }
        
        [Option(shortName: 'c', longName: "cycle", Required = true, HelpText = "Цикл для прогона.", Default = null)]
        public string TestCycle { get; set; }
    }
}