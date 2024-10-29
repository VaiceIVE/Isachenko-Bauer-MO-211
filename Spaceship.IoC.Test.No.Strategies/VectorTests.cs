using Hwdtech;
using Spaceship__Server;
using Moq;

namespace Spaceship.IoC.Test.No.Strategies;

using System;


public class VectorTests
{
    [Fact]
    public void VectorDefault()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(2, 5);

        Assert.Equal(new Vector(7, 17), vec1 + vec2);
        
    }

        [Fact]
    public void VectorEqual()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(5, 12);

        Assert.True(vec1 == vec2);
        
    }

            [Fact]
    public void VectorUnEqual()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(5, 13);

        Assert.False(vec1 == vec2);
        
    }

    [Fact]
    public void VectorUnEqualSize()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(5, 12, 4);

        Assert.False(vec1 == vec2);
        
    }

    [Fact]
    public void VectorUnEqualFalse()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(5, 12);

        Assert.False(vec1 != vec2);
        
    }

    [Fact]
    public void VectorUnEqualTrue()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(5, 13);

        Assert.True(vec1 != vec2);
        
    }

    [Fact]
    public void VectorSubtraction()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(5, 12);

        Assert.Equal(new Vector(0, 0), vec1 - vec2);
        
    }

    [Fact]
    public void VectorAdditionWrongSize()
    {
        Vector vec1 = new Vector(5, 12, 0);

        Vector vec2 = new Vector(5, 12);

        Assert.Throws<ArgumentException>(() => {var vec3 = vec1 + vec2;});
    }

    [Fact]
    public void VectorAdditionWrongSizeSecond()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(5, 12, 0);

        Assert.Throws<ArgumentException>(() => {var vec3 = vec1 + vec2;});
    }

    [Fact]
    public void VectorSubWrongSize()
    {
        Vector vec1 = new Vector(5, 12, 0);

        Vector vec2 = new Vector(5, 12);

        Assert.Throws<ArgumentException>(() => {var vec3 = vec1 - vec2;});
    }

    [Fact]
    public void VectorSubWrongSizeSecond()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(5, 12, 0);

        Assert.Throws<ArgumentException>(() => {var vec3 = vec1 - vec2;});
    }

    [Fact]
    public void VectorStringify()
    {
        Vector vec1 = new Vector(5, 12);

        Assert.Equal("Vector(5, 12)", vec1.ToString());
    }

    [Fact]
    public void VectorHash()
    {
        Vector vec1 = new Vector(5, 12);

        var hc = vec1.GetHashCode();

        Assert.Equal(hc, vec1.GetHashCode());
    }

    [Fact]
    public void VectorEquals()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(5, 12);

        Assert.True(vec1.Equals(vec2));
    }

    [Fact]
    public void VectorEqualsFalse()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(5, 123);

        Assert.False(vec1.Equals(vec2));
    }

    [Fact]
    public void VectorEqualsFalseSize()
    {
        Vector vec1 = new Vector(5, 12);

        Vector vec2 = new Vector(5, 12, 3);

        Assert.False(vec1.Equals(vec2));
    }

    [Fact]
    public void VectorEqualsFalseSizeFirst()
    {
        Vector vec1 = new Vector(5, 12, 3);

        Vector vec2 = new Vector(5, 12);

        Assert.False(vec1.Equals(vec2));
    }

    [Fact]
    public void VectorNonEqualsNonVector()
    {
        Vector vec1 = new Vector(5, 12);

        int vec2 = 5;

        Assert.False(vec1.Equals((object)vec2));
    }

    [Fact]
    public void VectorPositioning()
    {
        Vector vec1 = new Vector(5, 12);

        vec1[0] = 7;

        Assert.Equal(new Vector(7, 12), vec1);

        Assert.Equal(7, vec1[0]);
    }

    [Fact]
    public void VectorReplacement()
    {
        Vector vec1 = new Vector(5, 12);

        Assert.Throws<ArgumentException>(() => {var val = vec1[3];});

        Assert.Throws<ArgumentException>(() => {vec1[3] = 5;});
    }

}
