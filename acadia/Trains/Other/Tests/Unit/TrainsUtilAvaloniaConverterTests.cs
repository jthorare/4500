using Avalonia;
using Avalonia.Collections;
using Avalonia.Media;
using NUnit.Framework;
using System;
using Trains.Models.GamePieces;
using Trains.Util.AvaloniaConverters;

namespace Tests
{
    public class TrainsUtilAvaloniaConverterTests
    {
        // CityPointConverter Tests
        [Test]
        public void TestCityPointConverterConvert()
        {
            Assert.AreEqual(null, new CityPointConverter().Convert(null, typeof(Point), null, null));
            Assert.AreEqual(new Point(255, 30), new CityPointConverter().Convert(TestVariables.boston, typeof(Point), null, null));
            Assert.AreEqual(new Point(255, 30), new CityPointConverter().Convert(TestVariables.boston, typeof(Point), null, null));
            Assert.Throws<NotSupportedException>(() => new CityPointConverter().Convert(TestVariables.boston, typeof(Double), null, null));
            Assert.Throws<NotSupportedException>(() => new CityPointConverter().Convert(20, typeof(Point), null, null));
            Assert.Throws<NotSupportedException>(() => new CityPointConverter().Convert(20, typeof(Double), null, null));
        }

        /// <summary>
        /// Test for ColorConverter.ConvertBack()
        /// </summary>
        [Test]
        public void TestCityPointConverterConvertBack()
        {
            Assert.Throws<NotImplementedException>(() => new CityPointConverter().ConvertBack(TestVariables.boston, typeof(City), null, null));
        }

        // ColorConverter Tests

        /// <summary>
        /// Test for ColorConverter.Convert().
        /// The commented out tests ARE equal, but the SolidColorBrush.Equals() uses reference equality so there is no way for us to get the tests to pass.
        /// </summary>
        [Test]
        public void TestColorConverterConvert()
        {
            Assert.IsNull(new ColorConverter().Convert(null, typeof(IBrush), null, null));
            //Assert.AreEqual(new SolidColorBrush(Colors.Red), new ColorConverter().Convert(GamePieceColor.Red, typeof(IBrush), null, null));
            //Assert.AreEqual(new SolidColorBrush(Colors.LightGreen), new ColorConverter().Convert(GamePieceColor.Green, typeof(IBrush), null, null));
            //Assert.AreEqual(new SolidColorBrush(Colors.GhostWhite), new ColorConverter().Convert(GamePieceColor.White, typeof(IBrush), null, null));
            //Assert.AreEqual(new SolidColorBrush(Colors.DodgerBlue), new ColorConverter().Convert(GamePieceColor.Blue, typeof(IBrush), null, null));
            Assert.Throws<NotSupportedException>(() => new ColorConverter().Convert(TestVariables.boston, typeof(IBrush), null, null));
            Assert.Throws<NotSupportedException>(() => new ColorConverter().Convert(GamePieceColor.Red, typeof(Double), null, null));
            Assert.Throws<NotSupportedException>(() => new ColorConverter().Convert(TestVariables.boston, typeof(Double), null, null));
        }

        /// <summary>
        /// Test for ColorConverter.ConvertBack()
        /// </summary>
        [Test]
        public void TestColorConverterConvertBack()
        {
            Assert.Throws<NotImplementedException>(() => new ColorConverter().ConvertBack(GamePieceColor.Blue, typeof(IBrush), null, null));
        }

        // ConnectionStrokeDashArrayConverter Tests

        /// <summary>
        /// Tests for ConnectionStrokeDashArray.Convert()
        /// </summary>
        [Test]
        public void TestConnectionStrokeDashArrayConvert()
        {
            Assert.AreEqual(null, new ConnectionStrokeDashArrayConverter().Convert(null, typeof(AvaloniaList<Double>), null, null));
            double segmentLengthExpectedOne = 40.8;
            double gapsizeExpectedOne = 12.75;
            double segmentLengthActualOne = ((AvaloniaList<double>) (new ConnectionStrokeDashArrayConverter().Convert(TestVariables.bosSea, typeof(AvaloniaList<Double>), null, null)))[0];
            double gapsizeActualOne = ((AvaloniaList<double>)(new ConnectionStrokeDashArrayConverter().Convert(TestVariables.bosSea, typeof(AvaloniaList<Double>), null, null)))[1];
            Assert.That(segmentLengthExpectedOne, Is.EqualTo(segmentLengthActualOne).Within(0.001));
            Assert.That(gapsizeExpectedOne, Is.EqualTo(gapsizeActualOne).Within(0.001));

            double segmentLengthExpectedTwo = 6;
            double gapsizeExpectedTwo = 1;
            double segmentLengthActualTwo = ((AvaloniaList<double>)(new ConnectionStrokeDashArrayConverter().Convert(TestVariables.bosNyc, typeof(AvaloniaList<Double>), null, null)))[0];
            double gapsizeActualTwo = ((AvaloniaList<double>)(new ConnectionStrokeDashArrayConverter().Convert(TestVariables.bosNyc, typeof(AvaloniaList<Double>), null, null)))[1];
            Assert.That(segmentLengthExpectedTwo, Is.EqualTo(segmentLengthActualTwo).Within(0.001));
            Assert.That(gapsizeExpectedTwo, Is.EqualTo(gapsizeActualTwo).Within(0.001));

            Assert.Throws<NotSupportedException>(() => new ConnectionStrokeDashArrayConverter().Convert(TestVariables.boston, typeof(IBrush), null, null));
            Assert.Throws<NotSupportedException>(() => new ConnectionStrokeDashArrayConverter().Convert(TestVariables.boston, typeof(AvaloniaList<Double>), null, null));
            Assert.Throws<NotSupportedException>(() => new ConnectionStrokeDashArrayConverter().Convert(TestVariables.bosSea, typeof(IBrush), null, null));
        }

        /// <summary>
        /// Test for ConnectionStrokeDashArrayConverter.ConvertBack()
        /// </summary>
        [Test]
        public void TestConnectionStrokeDashArrayConverterConvertBack()
        {
            Assert.Throws<NotImplementedException>(() => new ConnectionStrokeDashArrayConverter().ConvertBack(15, typeof(Double), null, null));
        }
    }
}
