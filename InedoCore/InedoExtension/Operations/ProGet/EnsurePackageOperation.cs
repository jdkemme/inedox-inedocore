﻿using System.ComponentModel;
using System.Threading.Tasks;
using Inedo.Documentation;
using Inedo.Extensibility;
using Inedo.Extensibility.Operations;
using Inedo.Extensions.Configurations.ProGet;

namespace Inedo.Extensions.Operations.ProGet
{
    [DisplayName("Ensure Package")]
    [Description("Ensures that the contents of a ProGet package are in the specified directory.")]
    [ScriptAlias("Ensure-Package")]
    [ScriptNamespace(Namespaces.ProGet)]
    [Tag("proget")]
    public sealed partial class EnsurePackageOperation : EnsureOperation<ProGetPackageConfiguration>
    {
        private volatile OperationProgress progress = null;

        public override OperationProgress GetProgress() => this.progress;

        public override Task ConfigureAsync(IOperationExecutionContext context) => PackageDeployer.DeployAsync(context, this.Template, this, "Ensure-Package", true, p => this.progress = p);

        protected override ExtendedRichDescription GetDescription(IOperationConfiguration config)
        {
            object[] versionText;
            if (string.IsNullOrWhiteSpace(config[nameof(ProGetPackageConfiguration.PackageVersion)]))
                versionText = new object[] { new Hilite("latest version") };
            else
                versionText = new object[] { "version ", new Hilite(config[nameof(ProGetPackageConfiguration.PackageVersion)]) };

            return new ExtendedRichDescription(
                new RichDescription(
                    "Ensure universal package contents of ",
                    versionText,
                    " of ",
                    new Hilite(config[nameof(ProGetPackageConfiguration.PackageName)])
                ),
                new RichDescription(
                    "are present in ",
                    new DirectoryHilite(config[nameof(ProGetPackageConfiguration.TargetDirectory)])
                )
            );
        }
    }
}
