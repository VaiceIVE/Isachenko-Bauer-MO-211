namespace Spaceship.IoC.Test.No.Strategies;

using Hwdtech;
using Spaceship__Server;
using Moq;

public class TreeBuilderTests
{
    [Fact]
    public void BuildTree()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "BuildTree", (object[] args) =>
        {
            return new TreeBuilder(args).tree;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Check.Collision", (object[] args) =>
        {
            CollisionSolver solver = new();
            return solver.Solve2D(args);
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Strategies.SolveTree", (object[] args) =>
        {
            TreeDesicion td = new();
            return (object)td.Decide(args);
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IUObject.IMovable", (object[] args) =>
        {
            MovableAdapter adp = new(args);
            return (object)adp;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CalculateDeltas", (object[] args) => {

            IMovable _obj1 = Hwdtech.IoC.Resolve<IMovable>("Adapters.IUObject.IMovable", args[0]);
            IMovable _obj2 = Hwdtech.IoC.Resolve<IMovable>("Adapters.IUObject.IMovable", args[1]);

            List<int> deltalist = new List<int>();

            int dimension = 0;

            foreach (int coord in _obj1.Position.coords)
            {
                deltalist.Add(coord - _obj2.Position[dimension]);
                dimension++;
            }

            dimension = 0;

            foreach (int coord in _obj1.Speed.coords)
            {
                deltalist.Add(coord - _obj2.Speed[dimension]);
                dimension++;
            }

            return deltalist;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Solution.Tree", (object[] args) => {
            return Hwdtech.IoC.Resolve<Dictionary<int, object>>("BuildTree", new List<string>() { "1 1 1 1", "1 1 1 2", "1 1 2 2", "1 1 2 1", "1 1 2 4" });
        }).Execute();

        Mock<IUObject> _obj1 = new();
        _obj1.Setup(o => o.get_property("Velocity")).Returns(new Vector(3, 4));
        _obj1.Setup(o => o.get_property("Position")).Returns(new Vector(1, 2));

        Mock<IUObject> _obj2 = new();
        _obj2.Setup(o => o.get_property("Velocity")).Returns(new Vector(5, 5));
        _obj2.Setup(o => o.get_property("Position")).Returns(new Vector(2, 3));

        Assert.Throws<Exception>(() => { Hwdtech.IoC.Resolve<bool>("Check.Collision", _obj2.Object, _obj1.Object); });

    }
}
