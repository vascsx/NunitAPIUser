using System.Collections.Generic;
using System.Net;
using NUnit.Framework;

namespace NunitAPIUser.src.TestData
{
    public static class RegisterTestData
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                //yield return new TestCaseData("Anderson Teste", "testevasconcelos1@gmail.com", "testesucess", HttpStatusCode.OK, "Usuário cadastrado com sucesso!").SetName("RegistrationWithAllFieldsCorrectlyFilled");
                yield return new TestCaseData("Anderson Teste", "testevasc@gmail.com", "testesucess", HttpStatusCode.BadRequest, "E-mail já cadastrado.").SetName("EmailAlreadyRegistered");
                yield return new TestCaseData("", "teste@gmail.com", "testees", HttpStatusCode.BadRequest, "O campo 'FullName' é obrigatório.").SetName("FullNameIsRequired");
                yield return new TestCaseData("Anderson Vasc", "", "testees", HttpStatusCode.BadRequest, "O campo 'Email' é obrigatório.").SetName("EmailIsRequired");
                yield return new TestCaseData("Anderson Vasc", "teste@gmail.com", "", HttpStatusCode.BadRequest, "O campo 'Password' é obrigatório.").SetName("PasswordIsRequired");
                yield return new TestCaseData("", "", "", HttpStatusCode.BadRequest, "Nenhum campo foi preenchido!").SetName("AllFieldsAreRequired");
                yield return new TestCaseData("Anderson1", "teste@gmail.com", "senhaessss", HttpStatusCode.BadRequest, "O campo 'FullName' deve conter apenas letras").SetName("FullNameMustContainOnlyLetters");
                yield return new TestCaseData("Anderson", "testegmail.com", "testeyes", HttpStatusCode.BadRequest, "O campo 'Email' não é válido.").SetName("InvalidEmailFormat1");
                yield return new TestCaseData("Anderson", "teste@gmailcom", "testeees", HttpStatusCode.BadRequest, "O campo 'Email' não é válido.").SetName("InvalidEmailFormat2");
                yield return new TestCaseData("Anderson", "teste@gmail.com", "teese", HttpStatusCode.BadRequest, "A senha deve ter no mínimo 6 caracteres.").SetName("PasswordTooShort");
            }
        }
    }
}
