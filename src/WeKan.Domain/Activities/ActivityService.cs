using System;
using System.Collections.Generic;

namespace WeKan.Domain.Activities
{
    public interface IActivityService
    {
        void CalculateOrders(IEnumerable<Activity> activities);
    }

    public class ActivityService : IActivityService
    {
        /// <summary>
        /// Takes a collection of activities and changes the order property
        /// of each card to match its position in the collection
        /// </summary>
        /// <param name="activities"></param>
        public void CalculateOrders(IEnumerable<Activity> activities)
        {
            if (activities is null) throw new ArgumentNullException(nameof(activities));

            var order = 0;
            foreach (var activity in activities)
            {
                activity.ChangeOrder(order);
                order += 1;
            }
        }
    }
}
