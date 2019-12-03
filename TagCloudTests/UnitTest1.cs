using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter circularCloud;
        private readonly Point center = new Point(800, 600);
        private List<Rectangle> rectangles;

        [SetUp]
        public void SetUp()
        {
            var roundPositionGenerator = new RoundSpiralPositionGenerator();
            circularCloud = new CircularCloudLayouter(roundPositionGenerator);
            rectangles = new List<Rectangle>();
        }

        [Test]
        public void ThrowException_WhenSizeHaveNegativeNumber()
        {
            var rectangleSize = new Size(-1, 100);
            Action act = () => circularCloud.PutNextRectangle(rectangleSize);
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void FirstRectangle_MustBeNearCenter()
        {
            var rectangleSize = new Size(100, 100);
            var rectangle = circularCloud.PutNextRectangle(rectangleSize);
            rectangles.Add(rectangle);
            var deltaX = Math.Abs(rectangle.X - center.X);
            var deltaY = Math.Abs(rectangle.Y - center.Y);
            deltaX.Should().BeLessThan(100);
            deltaY.Should().BeLessThan(100);
        }

        [TestCase(2)]
        [TestCase(20)]
        [TestCase(200)]
        public void Rectangles_Should_NotIntersectWithPrevious(int countRectangles)
        {
            var rectangleSize = new Size(100, 100);
            for (var i = 0; i < countRectangles; i++)
            {
                var rect = circularCloud.PutNextRectangle(rectangleSize);
                rectangles.Any(previousRectangle => rect.IntersectsWith(previousRectangle)).Should().Be(false);
                rectangles.Add(rect);
            }
        }

        [Test]
        public void Rectangles_Should_BeInCircle()
        {
            var rectangleSize = new Size(123, 112);
            for (var i = 0; i < 100; i++)
            {
                rectangles.Add(circularCloud.PutNextRectangle(rectangleSize));
            }
            var maxY = rectangles.Max(rect => rect.Bottom);
            var minY = rectangles.Min(rect => rect.Top);
            var maxX = rectangles.Max(rect => rect.Right);
            var minX = rectangles.Min(rect => rect.Left);
            Math.Abs((maxY - minY) - (maxX - minX)).Should().BeLessThan(100 * (100 / 10));
        }
    }
}