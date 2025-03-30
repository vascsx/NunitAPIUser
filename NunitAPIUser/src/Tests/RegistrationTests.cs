using System;
using System.IO;
using System.Net;
using System.Text.Json;
using NUnit.Framework;
using RestSharp;
using System.Threading.Tasks;
using NunitAPIUser.src.TestData;

namespace NunitAPIUser.src.Tests
{
    public class AppSettings
    {
        public string ApiUrl { get; set; }
    }

    [TestFixture]
    public class RegistrationTests : IDisposable
    {
        private readonly string apiUrl;
        private RestClient client;

        public RegistrationTests()
        {
            var appSettings = LoadAppSettings();
            apiUrl = appSettings.ApiUrl ?? throw new InvalidOperationException("API URL is not set in appsettings.json.");
        }

        private AppSettings LoadAppSettings()
        {
            var json = File.ReadAllText("appsettings.json");
            return JsonSerializer.Deserialize<AppSettings>(json);
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
            client = new RestClient(apiUrl);
        }

        private void DisposeClient()
        {
            client?.Dispose();
        }

        private RestRequest InitializeRequest(object newUser)
        {
            var request = new RestRequest(apiUrl, Method.Post);
            request.AddJsonBody(newUser);
            return request;
        }

        private async Task<RestResponse> ExecuteRequestAsync(object newUser)
        {
            var request = InitializeRequest(newUser);
            return await client.ExecuteAsync(request);
        }

        [TestCaseSource(typeof(UserRegistrationTestData), nameof(UserRegistrationTestData.TestCases))]
        public async Task ShouldReturnExpectedResult(string fullName, string email, string password, HttpStatusCode expectedStatusCode, string expectedMessage)
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
        }
    }
}
