namespace EvidentInstruction.Service.Models
{
    public static class Message
    {
        public static string CreateMessage(RequestInfo request)
        {
            string result = $"Сервисс с именем \"{request.Name}\" был вызван со следующими параметрами \"{request}\" ";
            return result;
        }
        public static string CreateMessage(ResponseInfo response)
        {
            string result = $"Получен следующий ответ сервиса {response}";
            return result;
        }
    }
}
