using Zek.IntegrationTests;

namespace TMS.IntegrationTests
{
    public class IntegrationTest : BaseIntegrationTest<Startup>
    {
        public IntegrationTest() : base(@"..\..\..\..\TMS")
        {
        }
    }
}