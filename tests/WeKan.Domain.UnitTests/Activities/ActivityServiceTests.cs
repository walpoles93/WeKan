using System;
using System.Collections.Generic;
using WeKan.Domain.Activities;
using Xunit;

namespace WeKan.Domain.UnitTests.Activities
{
    public class ActivityServiceTests
    {
        [Fact]
        public void CalculateOrders_EnumerableInstance_CallsChangeOrderFromSequence()
        {
            var service = new ActivityService();
            var activity1 = Activity.Create("activity1-title");
            var activity2 = Activity.Create("activity2-title");
            var activities = new List<Activity> { activity1, activity2 };

            service.CalculateOrders(activities);

            Assert.Equal(0, activity1.Order);
            Assert.Equal(1, activity2.Order);
        }

        [Fact]
        public void CalculateOrders_EnumerableNull_ThrowsNullArgumentException()
        {
            var service = new ActivityService();

            void action() => service.CalculateOrders(null);

            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
