using System.Collections.Generic;
using System.Net;
using NUnit.Framework;

namespace NunitAPIUser.src.TestData
{
    public static class EditUserTestData
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("1", "Anderson Teste", "2", "testevasc@gmail.com", HttpStatusCode.BadRequest, "E-mail já cadastrado.").SetName("UserNotFound");
            }
        }
    }
}
