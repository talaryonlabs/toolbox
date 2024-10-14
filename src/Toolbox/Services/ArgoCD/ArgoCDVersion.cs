namespace Talaryon.Toolbox.Services.ArgoCD;

    /// <summary>
    /// VersionMessage represents version of the Argo CD API server
    /// </summary>
    public class ArgoCDVersion
    {
        /// <summary>
        /// Gets or sets the build date of the Argo CD API server.
        /// </summary>
        public string? BuildDate { get; set; }

        /// <summary>
        /// Gets or sets the compiler used to build the Argo CD API server.
        /// </summary>
        public string? Compiler { get; set; }

        /// <summary>
        /// Gets or sets the extra build information.
        /// </summary>
        public string? ExtraBuildInfo { get; set; }

        /// <summary>
        /// Gets or sets the Git commit hash of the build.
        /// </summary>
        public string? GitCommit { get; set; }

        /// <summary>
        /// Gets or sets the Git tag of the build.
        /// </summary>
        public string? GitTag { get; set; }

        /// <summary>
        /// Gets or sets the Git tree state.
        /// </summary>
        public string? GitTreeState { get; set; }

        /// <summary>
        /// Gets or sets the Go version used to build the server.
        /// </summary>
        public string? GoVersion { get; set; }

        /// <summary>
        /// Gets or sets the Helm version used in the build.
        /// </summary>
        public string? HelmVersion { get; set; }

        /// <summary>
        /// Gets or sets the Jsonnet version used in the build.
        /// </summary>
        public string JsonnetVersion { get; set; }

        /// <summary>
        /// Gets or sets the Kubectl version used in the build.
        /// </summary>
        public string KubectlVersion { get; set; }

        /// <summary>
        /// Gets or sets the Kustomize version used in the build.
        /// </summary>
        public string KustomizeVersion { get; set; }

        /// <summary>
        /// Gets or sets the platform for which the server was built.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the version of the Argo CD API server.
        /// </summary>
        public string Version { get; set; }
    }