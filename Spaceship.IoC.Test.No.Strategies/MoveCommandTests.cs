using Hwdtech;
using Spaceship__Server;
using Moq;

namespace Spaceship.IoC.Test.No.Strategies;

using System;


public class MoveComandTests
{
    [Fact]
    public void MoveCommandDefault()
    {

        Mock<IMovable> _obj = new();

        Vector vec1 = new Vector(-5, 2);

        Vector vec2 = new Vector(12, 5);

        _obj.Setup(o => o.Speed).Returns(vec1);

        _obj.Setup(o => o.Position).Returns(vec2);

        MoveCommand mc = new(_obj.Object);

        mc.Execute();

        Assert.Equal(new Vector(7, 7), _obj.Object.Position);
    }

    [Fact]
    public void MoveCommandException()
    {

        Mock<IMovable> _obj = new();

        Vector vec1 = new Vector(-5, 2);

        _obj.Setup(o => o.Speed).Returns(vec1);

        _obj.SetupGet<Vector>(o => o.Position).Throws<Exception>().Verifiable();

        MoveCommand mc = new(_obj.Object);

        Assert.Throws<Exception>(() => {mc.Execute();});
    }

    [Fact]
    public void MoveCommandImmovable()
    {

        Mock<IMovable> _obj = new();

        Vector vec1 = new Vector(-5, 2);

        _obj.SetupSet<Vector>(o => o.Position = It.IsAny<Vector>()).Throws<Exception>().Verifiable();

        MoveCommand mc = new(_obj.Object);

        Assert.Throws<NullReferenceException>(() => {mc.Execute();});
    }

    [Fact]
    public void MoveCommandSpeedUnreadable()
    {

        Mock<IMovable> _obj = new();

        Vector vec1 = new Vector(-5, 2);

        _obj.SetupGet<Vector>(o => o.Speed).Throws<Exception>().Verifiable();

        MoveCommand mc = new(_obj.Object);

        Assert.Throws<Exception>(() => {mc.Execute();});
    }

    [Fact]
    public void MoveCommandWrongSizes()
    {

        Mock<IMovable> _obj = new();

        Vector vec1 = new Vector(-5, 2, 4);

        Vector vec2 = new Vector(12, 5);

        _obj.Setup(o => o.Speed).Returns(vec1);

        _obj.Setup(o => o.Position).Returns(vec2);

        MoveCommand mc = new(_obj.Object);

        Assert.Throws<Exception>(() => {mc.Execute();});
    }
}
