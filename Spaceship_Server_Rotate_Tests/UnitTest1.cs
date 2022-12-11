using System;
using Xunit;
using Spaceship__Server;
using Moq;


namespace Spaceship_Server_Rotate_Tests
{
    public class FractionTest
    {
        [Fact]
        public void RightSum()
        {
            Fraction a = new Fraction(1, 5);
            Fraction b = new Fraction(1, 6);
            Fraction c = a + b;

            Assert.Equal(c, new Fraction(11, 30));

        }
    }

    public class RotateTest
    {
        [Fact]
        public void MoqRotate()
        {
            Mock<IRotatable> Spaceship = new();
            Spaceship.SetupGet<Fraction[]>(m => m.angle).Returns(new Fraction[2] { new Fraction(1, 4), new Fraction(0, 0) });
            Spaceship.SetupGet<Fraction[]>(m => m.angle_velocity).Returns(new Fraction[2] { new Fraction(1, 2), new Fraction(0, 0) });
            RotateCommand rc = new RotateCommand(Spaceship.Object);
            rc.Execute();
            Assert.Equal(Spaceship.Object.angle[0], new Fraction(3, 4));
        }
        [Fact]
        public void CantReadAngle()
        {
            bool didThrow = false;
            Mock<IRotatable> Spaceship = new();
            Spaceship.SetupGet<Fraction[]>(m => m.angle).Throws<Exception>().Verifiable();
            RotateCommand rc = new RotateCommand(Spaceship.Object);
            try
            {
                rc.Execute();
            }
            catch(Exception)
            {
                didThrow = true;
            }
            Assert.True(didThrow);
        }
        [Fact]
        public void CantReadAngleVelocity()
        {
            bool didThrow = false;
            Mock<IRotatable> Spaceship = new();
            Spaceship.SetupGet<Fraction[]>(m => m.angle_velocity).Throws<Exception>().Verifiable();
            RotateCommand rc = new RotateCommand(Spaceship.Object);
            try
            {
                rc.Execute();
            }
            catch (Exception)
            {
                didThrow = true;
            }
            Assert.True(didThrow);
        }
        [Fact]
        public void CantChangeAngle()
        {
            bool didThrow = false;
            Mock<IRotatable> Spaceship = new();
            Spaceship.SetupGet<Fraction[]>(m => m.angle).Returns(new Fraction[2] { new Fraction(1, 4), new Fraction(0, 0) });
            Spaceship.SetupGet<Fraction[]>(m => m.angle_velocity).Returns(new Fraction[2] { new Fraction(1, 2), new Fraction(0, 0) });
            Spaceship.SetupSet<Fraction[]>(m => m.angle = It.IsAny<Fraction[]>()).Throws<Exception>().Verifiable();
            RotateCommand rc = new RotateCommand(Spaceship.Object);
            try
            {
                rc.Execute();
            }
            catch (Exception)
            {
                didThrow = true;
            }
            Assert.True(didThrow);
        }
        [Fact]
        public void EmptyAngles()
        {
            bool didThrow = false;
            Mock<IRotatable> Spaceship = new();
            RotateCommand rc = new RotateCommand(Spaceship.Object);
            try
            {
                rc.Execute();
            }
            catch (Exception)
            {
                didThrow = true;
            }
            Assert.True(didThrow);
        }
    }
}