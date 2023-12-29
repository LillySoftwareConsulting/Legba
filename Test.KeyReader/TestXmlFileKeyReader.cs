using KeyReader;
using Shouldly;
using System.Xml;

namespace Test.KeyReader;

public class TestXmlFileKeyReader
{
    private const string TEST_FILE_NAME = "./TestFiles/config.xml";

    [Fact]
    public void Test_Success_ApiKey()
    {
        var reader = new XmlFileKeyReader(TEST_FILE_NAME, "root/keys/openAi_ApiKey");

        reader.GetKey().ShouldBe("YOUR_OPENAI_API_KEY");
    }

    [Fact]
    public void Test_Success_OrgId()
    {
        var reader = new XmlFileKeyReader(TEST_FILE_NAME, "root/keys/openAi_OrgId");

        reader.GetKey().ShouldBe("YOUR_OPENAI_ORG_ID");
    }

    [Fact]
    public void Test_Failure_FileNotFound()
    {
        Should.Throw<FileNotFoundException>(() => 
            new XmlFileKeyReader("badfile.xml", "root/keys/openAi_ApiKey"));
    }

    [Fact]
    public void Test_Failure_XPathNotFound()
    {
        Should.Throw<XmlException>(() => 
            new XmlFileKeyReader(TEST_FILE_NAME, "root/keys/openAi_BadKey"));
    }
}