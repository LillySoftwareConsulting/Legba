using KeyReader;
using Shouldly;

namespace Test.KeyReader;

public class TestJsonStringKeyReader
{
    [Fact]
    public void Test_Success_TopLevel()
    {
        string json = @"{ ""OPENAI_API_KEY"": ""test-key"" }";

        var reader = new JsonStringKeyReader(json, "OPENAI_API_KEY");

        reader.GetKey().ShouldBe("test-key");
    }

    [Fact]
    public void Test_Success_SecondLevel_Colon()
    {
        string json = 
            @"{ ""keys"": {
                    ""openAi_ApiKey"": ""test-key"",
                    ""openAi_OrganizationId"": ""test-org-id""
                }
            }";

        var reader = new JsonStringKeyReader(json, "keys:openAi_ApiKey");

        reader.GetKey().ShouldBe("test-key");
    }

    [Fact]
    public void Test_Success_SecondLevel_Period()
    {
        string json =
            @"{ ""keys"": {
                    ""openAi_ApiKey"": ""test-key"",
                    ""openAi_OrganizationId"": ""test-org-id""
                }
            }";

        var reader = new JsonStringKeyReader(json, "keys.openAi_OrganizationId");

        reader.GetKey().ShouldBe("test-org-id");
    }

    [Fact]
    public void Test_InvalidJson()
    {
        string json = @"{ ""OPENAI_API_KEY"": ""test-key"" ";

        Assert.ThrowsAny<Exception>(() => 
            new JsonStringKeyReader(json, "OPENAI_API_KEY"));
    }

    [Fact]
    public void Test_MissingKey()
    {
        string json = @"{ ""OPENAI_API_KEY"": ""test-key"" }";

        Assert.Throws<InvalidOperationException>(() =>
            new JsonStringKeyReader(json, "MISSING_KEY"));
    }
}