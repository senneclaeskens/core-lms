﻿using CoreLMS.Application.Services;
using CoreLMS.Core.Entities;
using CoreLMS.Core.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreLMS.Tests.Services
{
    public class CourseServiceTests
    {
        private readonly Mock<IAppDbContext> appDbContextMock;
        private readonly ICourseService subject;

        public CourseServiceTests()
        {
            this.appDbContextMock = new Mock<IAppDbContext>();

            this.subject = new CourseService(this.appDbContextMock.Object);
        }

        [Fact]
        public async Task ItGetsAListOfCourses()
        {
            // given (arrange)
            List<Course> databaseCourses = new List<Course>();
            databaseCourses.Add(new Course()
            {
                Id = 1,
                Name = "Course #1"
            });

            databaseCourses.Add(new Course()
            {
                Id = 2,
                Name = "Course #2"
            });

            // TODO Cloning objects
            this.appDbContextMock.Setup(db =>
                db.SelectCoursesAsync())
                    .ReturnsAsync(databaseCourses.ToList());

            // when (act)
            List<Course> actualCourses = await subject.GetCoursesAsync();

            // then (assert)
            // 1. Actual list of courses == expected courses
            // 2. DB was hit once (and no more)
            actualCourses.Should().BeEquivalentTo(databaseCourses);
            appDbContextMock.Verify(db => db.SelectCoursesAsync(), Times.Once);
            appDbContextMock.VerifyNoOtherCalls();
        }
    }
}