using System.Collections.Generic;
using System.Net;
using NUnit.Framework;

namespace NunitAPIUser.src.TestData
{
    public static class LoginTestData
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("testevasconcelos@gmail.com", "testesucess", HttpStatusCode.OK, "Bem-vindo Anderson Teste!").SetName("LoginWithValidCredentials");
                yield return new TestCaseData("", "testesucess", HttpStatusCode.BadRequest, "O campo 'Email' é obrigatório.").SetName("LoginWithEmptyEmail");
                yield return new TestCaseData("testevasconcelos@gmail.com", "", HttpStatusCode.BadRequest, "O campo 'Password' é obrigatório.").SetName("LoginWithEmptyPassword");
                yield return new TestCaseData("", "", HttpStatusCode.BadRequest, "Nenhum campo foi preenchido!").SetName("LoginWithEmptyEmailAndPassword");
                yield return new TestCaseData("testevasconcelos@gmail.com", "ssssssss", HttpStatusCode.Unauthorized, "Email ou senha inválidos.").SetName("LoginWithInvalidPassword");
                yield return new TestCaseData("testevasconcelos", "testesucess", HttpStatusCode.Unauthorized, "Email ou senha inválidos.").SetName("LoginWithInvalidEmail");
            }
        }
    }
}