
namespace EvidentInstruction.Service.Infrastructures
{  
    public enum HeaderType
    {
        HEADER, //
        QUERY, // реализовать 1 шаг (для body) . можно в ввиже extension . !!!! параметры вызовы, котоырй используются в url (url?id=1 и т.д.) квери тоже самое, но можно не писать id  и т.д. 
        BODY // только переменная . 1. только одно (проверять на дубли)/ (чаще всего json & xml)  //проверка какого типа есть ее можно использовать
    }
}
