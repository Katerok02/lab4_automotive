using Moq;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Project1;
using ClassLibrary1;

namespace TestProject1
{
    [TestFixture]
    public class OMDbApiClientTests
    {
        private OMDbApiClient _client;
        private Mock<IHttpClient> _httpClientMock;

        [SetUp]
        public void Setup()
        {
            _httpClientMock = new Mock<IHttpClient>();
            _client = new OMDbApiClient("a0e4a358", _httpClientMock.Object);
        }

        private void SetupHttpClientMock(string imdbId, string responseContent)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent)
            };
            _httpClientMock.Setup(client => client.GetAsync(It.Is<string>(url => url.Contains(imdbId))))
                .ReturnsAsync(responseMessage);
        }

        [Test]
        public async Task GetMovieByIdAsync_ReturnsExpectedMovieInfo()
        {
            var jsonResponse = JsonConvert.SerializeObject(new OMDbMovieResponse
            {
                Title = "Guardians of the Galaxy Vol. 2",
                Year = "2017"
            });
            SetupHttpClientMock("tt3896198", jsonResponse);

            var movieResponse = await _client.GetMovieByIdAsync("tt3896198");

            Assert.AreEqual("Guardians of the Galaxy Vol. 2", movieResponse.Title);
            Assert.AreEqual("2017", movieResponse.Year);
        }

        [Test]
        public async Task GetMovieByIdAsync_ReturnsExpectedMovieInfo2()
        {
            var jsonResponse = JsonConvert.SerializeObject(new OMDbMovieResponse
            {
                Title = "True Detective",
                Year = "2014–"
            });
            SetupHttpClientMock("tt2356777", jsonResponse);

            var movieResponse = await _client.GetMovieByIdAsync("tt2356777");

            Assert.AreEqual("True Detective", movieResponse.Title);
            Assert.AreEqual("2014–", movieResponse.Year);
        }

        [Test]
        public async Task GetMovieByIdAsync_ReturnsNotNull()
        {
            var jsonResponse = JsonConvert.SerializeObject(new OMDbMovieResponse
            {
                Title = "True Detective"
            });
            SetupHttpClientMock("tt2356777", jsonResponse);

            var movieResponse = await _client.GetMovieByIdAsync("tt2356777");

            Assert.NotNull(movieResponse.Title);
        }

        [Test]
        public async Task GetMovieByIdAsync_ReturnFamilyCategory()
        {
            var jsonResponse = JsonConvert.SerializeObject(new OMDbMovieResponse
            {
                Rated = "PG-13"
            });
            SetupHttpClientMock("tt3896198", jsonResponse);

            var respond = await _client.DefineMovieRating("tt3896198");

            Assert.AreEqual("Family film", respond);
        }

        [Test]
        public async Task GetMovieByIdAsync_ReturnRestrictedCategory()
        {
            var jsonResponse = JsonConvert.SerializeObject(new OMDbMovieResponse
            {
                Rated = "R"
            });
            SetupHttpClientMock("tt0387564", jsonResponse);

            var respond = await _client.DefineMovieRating("tt0387564");

            Assert.AreEqual("Restricted film", respond);
        }

        [Test]
        public async Task GetMovieByIdAsync_ReturnAllCategory()
        {
            var jsonResponse = JsonConvert.SerializeObject(new OMDbMovieResponse
            {
                Rated = "G"
            });
            SetupHttpClientMock("tt1049413", jsonResponse);

            var respond = await _client.DefineMovieRating("tt1049413");

            Assert.AreEqual("For all", respond);
        }
    }
}