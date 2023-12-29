using KeyReader;
using Shouldly;

namespace Test.KeyReader;

public class TestJsonFileKeyReader
{
    private const string TEST_FILE_NAME = "./TestFiles/appsettings.json";

    [Fact]
    public void Test_Success_ApiKey()
    {
        var reader = new JsonFileKeyReader(TEST_FILE_NAME, "keys:openAi_ApiKey");

        reader.GetKey().ShouldBe("YOUR_OPENAI_API_KEY");
    }

    [Fact]
    public void Test_Success_OrgId()
    {
        var reader = new JsonFileKeyReader(TEST_FILE_NAME, "keys:openAi_OrgId");

        reader.GetKey().ShouldBe("YOUR_OPENAI_ORG_ID");
    }

    [Fact]
    public void Test_Failure_FileNotFound()
    {
        Should.Throw<FileNotFoundException>(() => 
            new JsonFileKeyReader("badfile.json", "keys:openAi_ApiKey"));
    }
}