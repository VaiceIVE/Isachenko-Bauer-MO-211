using Hwdtech;
using Spaceship__Server;
using Moq;

namespace Spaceship.Collision.Tests;

public class CollisionTests
{   
    [Fact]
    public void CollisionTrue()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Check.Collision", (object [] args) => 
        {
            CollisionSolver solver= new();
            return solver.Solve2D(args);
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Strategies.SolveTree", (object[] args) =>
        {
            TreeDesicion td = new();
            return (object)td.Decide(args);
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IUObject.IMovable", (object [] args) => 
        {
            MovableAdapter adp = new(args);
            return  (object)adp;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CalculateDeltas", (object[] args) => {

            IMovable _obj1 = Hwdtech.IoC.Resolve<IMovable>("Adapters.IUObject.IMovable",args[0]);
            IMovable _obj2 = Hwdtech.IoC.Resolve<IMovable>("Adapters.IUObject.IMovable",args[1]);
            
            List<int> deltalist = new List<int>();

            int dimension = 0;

            foreach(int coord in _obj1.Position.coords)
            {
                deltalist.Add(coord - _obj2.Position[dimension]);
                dimension++;
            }
            
            dimension = 0;

            foreach(int coord in _obj1.Speed.coords)
            {
                deltalist.Add(coord - _obj2.Speed[dimension]);
                dimension++;
            }

            return deltalist;
        }).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Solution.Tree", (object[] args) => {

            Dictionary<int, object> branch0 = new(){{1, new List<int>(){1, 2}}, {3, new List<int>(){3, 4, 8}}, {5, new List<int>(){6, 8, 9}}, {6, new List<int>(){9}}};

            Dictionary<int, object> branch1 = new(){{2, new List<int>(){4, 5}}, {5, new List<int>(){5, 2, 3}}, {6, new List<int>(){7, 8, 10}}, {7, new List<int>(){8}}};

            Dictionary<int, object> branch2 = new(){{3, new List<int>(){7, 6}}, {6, new List<int>(){6, 8}}, {5, new List<int>(){8, 9}}, {8, new List<int>(){4}}};

            Dictionary<int, object> branch3 = new(){{4, new List<int>(){8, 1}}, {7, new List<int>(){7, 4}}, {5, new List<int>(){10}}, {9, new List<int>(){11}}};


            Dictionary<int, object> branch4 = new(){{1, branch0}, {2, branch1}};

            Dictionary<int, object> branch5 = new(){{8, branch2}, {4, branch3}};

            Dictionary<int, object> branch6 = new(){{16, branch0}, {0, branch3}};

            Dictionary<int, object> branch7 = new(){{4, branch1}, {5, branch2}};


            Dictionary<int, object> tree = new(){{12, branch6}, {1, branch4}};


            return tree;
        }).Execute();
        Mock<IUObject> _obj1 = new();
        _obj1.Setup(o => o.get_property("Velocity")).Returns(new Vector(3, 4));
        _obj1.Setup(o => o.get_property("Position")).Returns(new Vector(1, 2));

        Mock<IUObject> _obj2 = new();
        _obj2.Setup(o => o.get_property("Velocity")).Returns(new Vector(4, 5));
        _obj2.Setup(o => o.get_property("Position")).Returns(new Vector(2, 3));

        Assert.Throws<Exception>(() => {Hwdtech.IoC.Resolve<bool>("Check.Collision", _obj2.Object, _obj1.Object);});
    }

    [Fact]
     public void CollisionFalse()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Check.Collision", (object [] args) => 
        {
            CollisionSolver solver= new();
            return solver.Solve2D(args);
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Strategies.SolveTree", (object[] args) =>
        {
            TreeDesicion td = new();
            return (object)td.Decide(args);
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IUObject.IMovable", (object [] args) => 
        {
            MovableAdapter adp = new(args);
            return  (object)adp;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CalculateDeltas", (object[] args) => {

            IMovable _obj1 = Hwdtech.IoC.Resolve<IMovable>("Adapters.IUObject.IMovable",args[0]);
            IMovable _obj2 = Hwdtech.IoC.Resolve<IMovable>("Adapters.IUObject.IMovable",args[1]);
            
            List<int> deltalist = new List<int>();

            int dimension = 0;

            foreach(int coord in _obj1.Position.coords)
            {
                deltalist.Add(coord - _obj2.Position[dimension]);
                dimension++;
            }
            
            dimension = 0;

            foreach(int coord in _obj1.Speed.coords)
            {
                deltalist.Add(coord - _obj2.Speed[dimension]);
                dimension++;
            }

            return deltalist;
        }).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Solution.Tree", (object[] args) => {

            Dictionary<int, object> branch0 = new(){{1, new List<int>(){1, 2}}, {3, new List<int>(){3, 4, 8}}, {5, new List<int>(){6, 8, 9}}, {6, new List<int>(){9}}};

            Dictionary<int, object> branch1 = new(){{2, new List<int>(){4, 5}}, {5, new List<int>(){5, 2, 3}}, {6, new List<int>(){7, 8, 10}}, {7, new List<int>(){8}}};

            Dictionary<int, object> branch2 = new(){{3, new List<int>(){7, 6}}, {6, new List<int>(){6, 8}}, {5, new List<int>(){8, 9}}, {8, new List<int>(){4}}};

            Dictionary<int, object> branch3 = new(){{4, new List<int>(){8, 1}}, {7, new List<int>(){7, 4}}, {5, new List<int>(){10}}, {9, new List<int>(){11}}};


            Dictionary<int, object> branch4 = new(){{1, branch0}, {2, branch1}};

            Dictionary<int, object> branch5 = new(){{8, branch2}, {4, branch3}};

            Dictionary<int, object> branch6 = new(){{16, branch0}, {0, branch3}};

            Dictionary<int, object> branch7 = new(){{4, branch1}, {5, branch2}};


            Dictionary<int, object> tree = new(){{12, branch6}, {1, branch4}};


            return tree;
        }).Execute();
        Mock<IUObject> _obj1 = new();
        _obj1.Setup(o => o.get_property("Velocity")).Returns(new Vector(1, 1));
        _obj1.Setup(o => o.get_property("Position")).Returns(new Vector(2, 1));

        Mock<IUObject> _obj2 = new();
        _obj2.Setup(o => o.get_property("Velocity")).Returns(new Vector(4, 5));
        _obj2.Setup(o => o.get_property("Position")).Returns(new Vector(2, 6));

        Assert.False(Hwdtech.IoC.Resolve<bool>("Check.Collision", _obj2.Object, _obj1.Object));
    }
    [Fact]
    public void CollisionFalseEmptyTree()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Check.Collision", (object [] args) => 
        {
            CollisionSolver solver= new();
            return solver.Solve2D(args);
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Strategies.SolveTree", (object[] args) =>
        {
            TreeDesicion td = new();
            return (object)td.Decide(args);
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IUObject.IMovable", (object [] args) => 
        {
            MovableAdapter adp = new(args);
            return  (object)adp;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CalculateDeltas", (object[] args) => {

            IMovable _obj1 = Hwdtech.IoC.Resolve<IMovable>("Adapters.IUObject.IMovable",args[0]);
            IMovable _obj2 = Hwdtech.IoC.Resolve<IMovable>("Adapters.IUObject.IMovable",args[1]);
            
            List<int> deltalist = new List<int>();

            int dimension = 0;

            foreach(int coord in _obj1.Position.coords)
            {
                deltalist.Add(coord - _obj2.Position[dimension]);
                dimension++;
            }
            
            dimension = 0;

            foreach(int coord in _obj1.Speed.coords)
            {
                deltalist.Add(coord - _obj2.Speed[dimension]);
                dimension++;
            }

            return deltalist;
        }).Execute();
        
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Solution.Tree", (object[] args) => {
 
            List<int> branch = new List<int>();

            Dictionary<int, object> tree = new(){{12, branch}, {1, branch}};

            return tree;
        }).Execute();
        Mock<IUObject> _obj1 = new();
        _obj1.Setup(o => o.get_property("Velocity")).Returns(new Vector(1, 1));
        _obj1.Setup(o => o.get_property("Position")).Returns(new Vector(1, 1));

        Mock<IUObject> _obj2 = new();
        _obj2.Setup(o => o.get_property("Velocity")).Returns(new Vector(4, 5));
        _obj2.Setup(o => o.get_property("Position")).Returns(new Vector(2, 6));

        Assert.False(Hwdtech.IoC.Resolve<bool>("Check.Collision", _obj2.Object, _obj1.Object));
    }
}
