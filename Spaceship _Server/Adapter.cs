namespace Spaceship__Server
{
        public class MovableAdapter : IMovable
    {
        public Vector Speed{get;}
        public Vector Position{get; set;}
        public MovableAdapter(object[] obj)
        {
            IUObject _obj = (IUObject) obj[0];
            this.Speed = (Vector) _obj.get_property("Velocity");
            this.Position = (Vector) _obj.get_property("Position");
        }
    }
}
