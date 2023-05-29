using Hwdtech;
using Spaceship__Server;
using Moq;

namespace Spaceship.IoC.Test.No.Strategies;

public class AdapterGeneratorTests
{
    [Fact]
    public void MainTestMovable()
    {
        Assert.Equal( "public class IMovableAdapter : IMovable\n{\n\t public Spaceship__Server.Vector Speed { get; }\n\t public Spaceship__Server.Vector Position { get;  set; }\n\t public IMovableAdapter (object obj)\n\t {\n\t \t this.Speed = Hwdtech.IoC.Resolve<Spaceship__Server.Vector>(\"IUObject.Property.Get\", obj, \"Speed\");\n\t \t this.Position = Hwdtech.IoC.Resolve<Spaceship__Server.Vector>(\"IUObject.Property.Get\", obj, \"Position\");\n\t }\n}", AdapterGenerator.Generate(typeof(IMovable))) ;
    }
}