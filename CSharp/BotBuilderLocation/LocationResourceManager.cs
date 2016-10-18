namespace Microsoft.Bot.Builder.Location
{
    using System;
    using System.Reflection;
    using System.Resources;
    using Resources;

    [Serializable]
    internal class LocationResourceManager
    {
        private readonly ResourceManager resourceManager;

        internal LocationResourceManager(Assembly resourceAssembly = null, string resourceName = null)
        {
            if (resourceAssembly == null || resourceName == null)
            {
                resourceAssembly = typeof(LocationSelectionDialog).Assembly;
                resourceName = typeof(Strings).FullName;
            }

            this.resourceManager = new ResourceManager(resourceName, resourceAssembly);
        }

        internal string GetResource(string name)
        {
            return this.resourceManager.GetString(name) ??
                   Strings.ResourceManager.GetString(name);
        }
    }
}
