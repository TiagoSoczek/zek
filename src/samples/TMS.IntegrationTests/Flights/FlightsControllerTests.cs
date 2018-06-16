using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace TMS.IntegrationTests.Flights
{
    public class FlightsControllerTests : IntegrationTest
    {
        [Fact]
        public async Task SearchFlights()
        {
            var response = await Client.GetAsync("flights?origin=CWB&destination=CGH&departureDate=06-30-2018").ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}