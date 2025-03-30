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
    public class EditUserUrl
    {
        public required string editUserUrl { get; set; }
    }

    [TestFixture]
    public class EditUserTests : IDisposable
    {
        private readonly string baseEditUserUrl;
        private RestClient client;

        public EditUserTests()
        {
            var appSettings = LoadAppSettings();
            baseEditUserUrl = appSettings.editUserUrl ?? throw new InvalidOperationException("API URL is not set in appsettings.json.");
        }

        private EditUserUrl LoadAppSettings()
        {
            var json = File.ReadAllText("appsettings.json");
            var appSettings = JsonSerializer.Deserialize<EditUserUrl>(json);
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
            client = new RestClient(baseEditUserUrl);
        }

        private void DisposeClient()
        {
            client?.Dispose();
        }

        private RestRequest InitializeRequest(string id, object editUser)
        {
            var requestUrl = $"{baseEditUserUrl}/{id}";
            var request = new RestRequest(requestUrl, Method.Put);
            request.AddJsonBody(editUser);
            return request;
        }

        private async Task<RestResponse> ExecuteRequestAsync(string id, object editUser)
        {
            var request = InitializeRequest(id, editUser);
            return await client.ExecuteAsync(request);
        }

        [TestCaseSource(typeof(EditUserTestData), nameof(EditUserTestData.TestCases))]
        public async Task ShouldReturnExpectedResultEditUser(string id, string fullName, string email, string password, HttpStatusCode expectedStatusCode, string expectedMessage)
        {
            var editUser = new
            {
                fullName,
                email,
                password
            };
            var response = await ExecuteRequestAsync(id, editUser);

            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
            Assert.That(response.Content != null && response.Content.Contains(expectedMessage), Is.True, $"Expected message: {expectedMessage}, but got: {response.Content}");
        }
    }
}
