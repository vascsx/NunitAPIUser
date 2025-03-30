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
    public class LoginUrl
    {
        public required string loginUrl { get; set; }
    }

    [TestFixture]
    public class LoginTests : IDisposable
    {
        private readonly string loginUrl;
        private RestClient client;

        public LoginTests()
        {
            var appSettings = LoadAppSettings();
            loginUrl = appSettings.loginUrl ?? throw new InvalidOperationException("API URL is not set in appsettings.json.");
        }

        private LoginUrl LoadAppSettings()
        {
            var json = File.ReadAllText("appsettings.json");
            var appSettings = JsonSerializer.Deserialize<LoginUrl>(json);
            if (appSettings == null)
            {
                throw new InvalidOperationException("Failed to load appsettings.json.");
            }
            return appSettings;
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
            client = new RestClient(loginUrl);
        }

        private void DisposeClient()
        {
            client?.Dispose();
        }

        private RestRequest InitializeRequest(object loginUser)
        {
            var request = new RestRequest(loginUrl, Method.Post);
            request.AddJsonBody(loginUser);
            return request;
        }

        private async Task<RestResponse> ExecuteRequestAsync(object loginUser)
        {
            var request = InitializeRequest(loginUser);
            return await client.ExecuteAsync(request);
        }

        [TestCaseSource(typeof(LoginTestData), nameof(LoginTestData.TestCases))]
        public async Task ShouldReturnExpectedResultLogin(string email, string password, HttpStatusCode expectedStatusCode, string expectedMessage)
        {
            var loginUser = new
            {
                email,
                password
            };

            var response = await ExecuteRequestAsync(loginUser);

            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
            Assert.That(response.Content != null && response.Content.Contains(expectedMessage), Is.True, $"Expected message: {expectedMessage}, but got: {response.Content}");

            if (expectedStatusCode == HttpStatusCode.OK && expectedMessage == "Bem-vindo Anderson Teste!")
            {
                bool userExists = DatabaseHelper.UserExists(email);
                Assert.That(userExists, Is.True, "The user should exist in the database after successful registration.");
            }
        }
    }
}
