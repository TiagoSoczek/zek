using Xunit;
using Zek.Shared;

namespace Zek.UnitTests.Shared
{
    public class ResultTests
    {
        [Fact]
        public void FromBool()
        {
            Result resultOk = true;
            Result resultFail = false;

            Assert.True(resultOk.IsSuccess);
            Assert.True(resultFail.IsFailure);
        }

        [Fact]
        public void ToBool()
        {
            bool success = Result.Success;
            bool fail = Result.NotFound;

            Assert.True(success);
            Assert.False(fail);
        }

        [Fact]
        public void Fail()
        {
            var fail = Result.Fail<Foo>(string.Empty);

            Assert.True(fail.IsFailure);
        }

        [Fact]
        public void ToResult()
        {
            var value = new Foo();
            var resultOk = Result.Ok(value);
            var resultFail = Result<Foo>.NotFound;

            Assert.True(resultOk.IsSuccess);
            Assert.Equal(value, resultOk.Value);

            Assert.True(resultFail.IsFailure);

            Result resultSimpleOk = resultOk;
            Result resultsimpleFail = resultFail;

            Assert.True(resultSimpleOk.IsSuccess);
            Assert.True(resultSimpleOk.HasValue);
            Assert.Equal(value, resultSimpleOk.Value);

            Assert.True(resultsimpleFail.IsFailure);
            Assert.False(resultsimpleFail.HasValue);
        }

        [Fact]
        public void FromResult()
        {
            var value = new Foo();
            var resultOk = Result.Ok(value);
            var resultFail = Result<Foo>.NotFound;

            Result resultSimpleOk = resultOk;
            Result resultSimpleFail = resultFail;

            Assert.True(resultSimpleOk.IsSuccess);
            Assert.True(resultSimpleOk.HasValue);
            Assert.Equal(value, resultSimpleOk.Value);

            Assert.True(resultSimpleFail.IsFailure);
            Assert.False(resultSimpleFail.HasValue);

            Result<Foo> resultOfSimpleOk = resultSimpleOk;
            Result<Foo> resultOfSimpleFail = resultSimpleFail;

            Assert.True(resultOfSimpleOk.IsSuccess);
            Assert.Equal(value, resultOfSimpleOk.Value);

            Assert.True(resultOfSimpleFail.IsFailure);
        }

        [Fact]
        public void FromT()
        {
            var value = new Foo();
            Foo valueNull = null;

            Result<Foo> resultOk = value;
            Result<Foo> resultFail = valueNull;

            Assert.True(resultOk.IsSuccess);
            Assert.True(resultOk);
            Assert.Equal(value, resultOk.Value);

            Assert.True(resultFail.IsFailure);
            Assert.Equal(ResultCode.NotFound, resultFail.Code);
        }

        public class Foo
        {
        }
    }
}