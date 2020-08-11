using System;
using EvidentInstruction.Helpers;

namespace EvidentInstruction.Service.Models
{
    public static class Message
    {
        public static string CreateMessage(RequestInfo request)
        {
            string headersResult =Converter.DictToString(request.ServiceAttribute.Headers);
            string paramsResult = Converter.DictToString(request.ServiceAttribute.Parameters);
            string result = $"Имя сервиса: {request.Name} {Environment.NewLine}Метод запроса: {request.Method}{Environment.NewLine}Адрес: {request.Url}{Environment.NewLine}Параметры запроса: {paramsResult}{Environment.NewLine}Заголовки запроса: {headersResult}{Environment.NewLine}Таймаут запроса: {request.ServiceAttribute.Timeout} ";
            return result;
        }
        public static string CreateMessage(ResponseInfo response)
        {
            string result = $"Код статуса: { response.StatusCode}{ Environment.NewLine}Тело ответа: { response.Content}{ Environment.NewLine}Заголовки ответа: { response.Headers}";
            return result;
        }
    }
}
