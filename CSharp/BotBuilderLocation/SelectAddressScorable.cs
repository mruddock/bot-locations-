using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Internals.Scorables;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Builder.Location
{
    internal class SelectAddressScorable : IScorable<IMessageActivity, double>
    {
        private readonly IDialogStack stack;
        private readonly List<Bing.Location> locations;

        public SelectAddressScorable(IDialogStack stack, List<Bing.Location> locations)
        {
            SetField.NotNull(out this.stack, nameof(stack), stack);
            SetField.NotNull(out this.locations, nameof(locations), locations);
        }

        public Task<object> PrepareAsync(IMessageActivity item, CancellationToken token)
        {
            int value;
            if (Int32.TryParse(item.Text, out value) && value > 0 && value <= this.locations.Count)
            {
               return Task.FromResult<object>(value - 1);
            }

            return Task.FromResult<object>(null);
        }

        public bool HasScore(IMessageActivity item, object state)
        {
            return state != null;
        }

        public double GetScore(IMessageActivity item, object state)
        {
            return 1;
        }

        public Task PostAsync(IMessageActivity item, object state, CancellationToken token)
        {
            int index = (int) state;
            this.stack.Done(PlaceExtensions.FromLocation(this.locations[index]));

            return Task.FromResult(0);
        }
    }
}
