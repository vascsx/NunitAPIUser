using System;
using System.IO;
using System.Net;
using System.Text.Json;
using NUnit.Framework;
using RestSharp;
using System.Threading.Tasks;
using NunitAPIUser.src.TestData;
using NunitAPIUser.src.Helpers;

namespace NunitAPIUser.src.Tests
{
    public class RegisterUrl
    {
        public required string registerUrl { get; set; }
    }

    [TestFixture]
    public class RegistrationTests : IDisposable
    {
        private readonly string registerUrl;
        private RestClient client;

        public RegistrationTests()
        {
            var RegisterUrl = LoadRegisterUrl();
            registerUrl = RegisterUrl.registerUrl ?? throw new InvalidOperationException("API URL is not set in RegisterUrl.json.");
        }

        private RegisterUrl LoadRegisterUrl()
        {
            var json = File.ReadAllText("appsettings.json");
            var RegisterUrl = JsonSerializer.Deserialize<RegisterUrl>(json);
            if (RegisterUrl == null)
            {
                throw new InvalidOperationException("Failed to load RegisterUrl.json.");
            }
            return RegisterUrl;
        }

        [SetUp]
        public void Setup()
        {
            InitializeClient();
        }

        [TearDown]
        public void TearDown()
        {
            DisposeClient();
        }

        public void Dispose()
        {
            DisposeClient();
        }

        private void InitializeClient()
        {
            client = new RestClient(registerUrl);
        }

        private void DisposeClient()
        {
            client?.Dispose();
        }

        private RestRequest InitializeRequest(object newUser)
        {
            var request = new RestRequest(registerUrl, Method.Post);
            request.AddJsonBody(newUser);
            return request;
        }

        private async Task<RestResponse> ExecuteRequestAsync(object newUser)
        {
            var request = InitializeRequest(newUser);
            return await client.ExecuteAsync(request);
        }

        [TestCaseSource(typeof(RegisterTestData), nameof(RegisterTestData.TestCases))]
        public async Task ShouldReturnExpectedResultRegister(string fullName, string email, string password, HttpStatusCode expectedStatusCode, string expectedMessage)
        {
            var newUser = new   
            {
                fullName,
                email,
                password
            };

            var response = await ExecuteRequestAsync(newUser);

            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
            Assert.That(response.Content != null && response.Content.Contains(expectedMessage), Is.True, $"Expected message: {expectedMessage}, but got: {response.Content}");

            if (expectedStatusCode == HttpStatusCode.OK && expectedMessage == "Usuário cadastrado com sucesso!")
            {
                bool userExists = DatabaseHelper.UserExists(email);
                Assert.That(userExists, Is.True, "The user should exist in the database after successful registration.");
            }
        }
    }
}
