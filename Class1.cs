using System;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace xunitcore
{
  public class Tests
  {
    [Fact]
    public void footest()
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
          .AddEnvironmentVariables()
          .Build();

      var service = new ServiceCollection();
      service.AddOptions();
      service.Configure<MongoDbSettings>(configuration.GetSection("MongoDbAzure"));

      service.AddSingleton<IFoo, Foo>();

      var foo = service.BuildServiceProvider().GetService<IFoo>();

      Assert.Equal("foobob", foo.Message());
    }
  }

  public interface IFoo
  {
    string Message();
  }

  public class Foo : IFoo
  {
    private readonly MongoDbSettings settings;

    public Foo(IOptions<MongoDbSettings> settings)
    {
      this.settings = settings.Value;
    }

    public string Message() => settings.ConnectionString;

  }

  public class MongoDbSettings
  {
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
  }
}
