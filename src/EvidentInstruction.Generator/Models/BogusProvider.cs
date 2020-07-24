using Bogus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvidentInstruction.Generator.Models
{
    public class BogusProvider : IBogusProvider
    {
        private readonly Faker faker;
        private string digits = "0123456789";
        private string rusChars = "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮйцукенгшщзхъфывапролджэячсмитьбю";
        public BogusProvider(Faker faker)
        {
            this.faker = faker;
        }
        public string Guid()
        {
            return faker.Random.Guid().ToString();
        }
        public DateTime Between(DateTime? start = null, DateTime? end = null)
        {
            start = start ?? new DateTime(1900, 01, 01);
            end = end ?? new DateTime(2100, 12, 31);
            return faker.Date.Between((DateTime)start, (DateTime)end);
        }
        public string Numbers(int len)
        {
            return faker.Random.String2(len, digits);
        }
        //в формате  (###)###-####
        public string Phone(string format)
        {
            return faker.Phone.PhoneNumber(format);
        }
        public string String(int len)
        {
            return faker.Random.String2(len, rusChars + digits);
        }
        public string Chars(int len)
        {
            return faker.Random.String2(len, rusChars);
        }

    }
}
