﻿using System;
using Moq;
using Zavolokas.Structures;
using Xunit;
using Shouldly;

namespace Zavolokas.ImageProcessing.Inpainting.UnitTests.GivenInpaintMapBuilder
{
    public class WhenSetInpaintArea
    {
        [Fact]
        public void Shoud_Throw_MapIsNotInitializedException_When_Called_Before_InitMap_Call()
        {
            var mock = new Mock<IArea2DMapBuilder>();
            var mapBuilder = mock.Object;

            var inpaintMapBuilder = new InpaintMapBuilder(mapBuilder);

            var inpaintArea = Area2D.Create(3, 3, 5, 5);
            Should.Throw<MapIsNotInitializedException>(()=>inpaintMapBuilder.SetInpaintArea(inpaintArea));
        }

        [Fact]
        public void Should_Throw_ArgumentNullException_When_InpaintArea_IsNull()
        {
            var mock = new Mock<IArea2DMapBuilder>();
            var mapBuilder = mock.Object;

            var inpaintMapBuilder = new InpaintMapBuilder(mapBuilder);

            Area2D inpaintArea = null;
            inpaintMapBuilder.InitNewMap(Area2D.Create(0, 0, 15, 15));
            Should.Throw<ArgumentNullException>(() => inpaintMapBuilder.SetInpaintArea(inpaintArea));
        }

        [Fact]
        public void Should_Throw_EmptyAreaException_When_InpaintArea_IsEmpty()
        {
            var mock = new Mock<IArea2DMapBuilder>();
            var mapBuilder = mock.Object;

            var inpaintMapBuilder = new InpaintMapBuilder(mapBuilder);

            Area2D inpaintArea = Area2D.Empty;
            inpaintMapBuilder.InitNewMap(Area2D.Create(0, 0, 15, 15));
            Should.Throw<EmptyAreaException>(()=>inpaintMapBuilder.SetInpaintArea(inpaintArea));
        }

        [Fact]
        public void Should_Delegate_SetInpaintArea_To_MapBuilder()
        {
            var mock = new Mock<IArea2DMapBuilder>();
            var mapBuilder = mock.Object;

            var inpaintMapBuilder = new InpaintMapBuilder(mapBuilder);

            var inpaintArea = Area2D.Create(3, 3, 5, 5);
            inpaintMapBuilder.InitNewMap(Area2D.Create(0, 0, 15, 15));
            inpaintMapBuilder.SetInpaintArea(inpaintArea);

            mock.Verify(b => b.SetIgnoredSourcedArea(It.Is<Area2D>(d => d.IsSameAs(inpaintArea))));
        }

        [Fact]
        public void InpaintArea_Should_Be_Adjusted_To_Source_Area()
        {
            var mock = new Mock<IArea2DMapBuilder>();
            var mapBuilder = mock.Object;

            var inpaintMapBuilder = new InpaintMapBuilder(mapBuilder);

            var inpaintArea = Area2D.Create(-3, -3, 10, 10);
            var imageArea = Area2D.Create(0, 0, 15, 15);
            inpaintMapBuilder.InitNewMap(imageArea);
            inpaintMapBuilder.SetInpaintArea(inpaintArea);

            var realArea = imageArea.Intersect(inpaintArea);

            mock.Verify(b => b.SetIgnoredSourcedArea(It.Is<Area2D>(d => d.IsSameAs(realArea))));
        }
    }
}
