# How to run Selenium automation tests on Hypertest (using C# SpecFlow framework)

Download the concierge binary corresponding to the host operating system. It is recommended to download the binary in the project's Parent Directory.

* Mac: https://downloads.lambdatest.com/concierge/darwin/concierge
* Linux: https://downloads.lambdatest.com/concierge/linux/concierge
* Windows: https://downloads.lambdatest.com/concierge/windows/concierge.exe

[Note - The current project has concierge for macOS. Irrespective of the host OS, the concierge will auto-update whenever there is a new version on the server]

The project structure is as shown below:

```yaml
specflow-demo-sample
      |
      |--- Features (Contains the feature files)
              |
              | --- GoogleSearch.feature
              | --- LambdaTestSearch.feature
              | --- SeleniumPlayground.feature
              | --- ToDoApp.feature
      |--- Hooks (Contains the event bindings to perform additional automation logic)
              | --- Hooks.cs
      |--- Steps (Contains the step definitions that correspond to the feature files)
              | --- GoogleSearchSteps.cs
              | --- DuckDuckGoSearchSteps.cs
              | --- SeleniumPlaygroundSteps.cs
              | --- ToDoAppSteps.cs
      |--- dotnet-install.sh  (Windows - Shell script to install .NET SDK, including .NET CLI & shared runtime)
      |--- dotnet-install.ps1 (macOS - Shell script to install .NET SDK, including .NET CLI & shared runtime)
      |--- App.config (Application Configuration file containing settings specific to the app)
      |--- specflow_hypertest_matrix_sample.yaml
      |--- specflow_hypertest_autosplit_sample.yaml
```

## Running tests in SpecFlow using the Matrix strategy

Matrix YAML file (specflow_hypertest_matrix_sample.yaml) in the repo contains the following configuration:

```yaml
globalTimeout: 90
testSuiteTimeout: 90
testSuiteStep: 90
```

Global timeout, testSuite timeout, and testSuite timeout are set to 90 minutes.

The target platform is set to macOS

```yaml
 os: [mac]
```

A user-defined key *project* is set to the C#
 solution  (i.e. .sln). It can even be set to C# project (.csproj) instead of C# solution.

Hence, the matrix comprises of *os* and *project* keys, details of which are shown below:

```yaml
matrix:
  os: [mac]
  project: ["OnlySpecTest.sln"]
```

Content under the *pre* directive is the pre-condition that will be run before the tests are executed on Hypertest grid. The "dotnet install" script for macOS & Windows is downloaded and kept in the project root directory. The stable version of the scripts can be downloaded from [Microsoft Official Website](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script).

* [Bash - Linux/macOS](https://dot.net/v1/dotnet-install.sh)
* [PowerShell for Windows](https://dot.net/v1/dotnet-install.ps1)

Environment variables *LT_USERNAME* and *LT_ACCESS_KEY* are added under *env* in the YAML file.

```yaml
env:
 LT_USERNAME: ${ YOUR_LAMBDATEST_USERNAME()}
 LT_ACCESS_KEY: ${ YOUR_LAMBDATEST_ACCESS_KEY()}
```

However, this is an optional step and can be skipped from the *pre* directive. Once downloaded, we install the LTS release using the commands mentioned [here](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script#examples). We set the permissions of C# project to 777 (i.e. rwx).

```yaml
pre:
   - ./dotnet-install.sh --channel LTS
   - chmod +rwx OnlySpecTest.sln
```

The *testSuites* object contains a list of commands (that can be presented in an array). In the current YAML file, commands to be run for executing the tests are put in an array (with a '-' preceding each item). In the current YAML file, *dotnet test* command is used for executing the tests present in the *$project* key (i.e. "OnlySpecTest.sln")

```yaml
testSuites:
  - dotnet test $project
```

The [user_name and access_key of LambdaTest](https://accounts.lambdatest.com/detail/profile) is appended to the *concierge* command using the *--user* and *--key* command-line options. The CLI option *--config* is used for providing the custom Hypertest YAML file (e.g. specflow_hypertest_matrix_sample.yaml). Run the following command on the terminal to trigger the tests in C# project on the Hypertest grid.

```bash
./concierge --user "${ YOUR_LAMBDATEST_USERNAME()}" --key "${ YOUR_LAMBDATEST_ACCESS_KEY()}" --config specflow_hypertest_matrix_sample.yaml --verbose
```

Visit [Hypertest Automation Dashboard](https://automation.lambdatest.com/hypertest) to check the status of execution

## Running tests in SpecFlow using Auto-split execution

Matrix YAML file (specflow_hypertest_autosplit_sample.yaml) in the repo contains the following configuration:

```yaml
globalTimeout: 90
testSuiteTimeout: 90
testSuiteStep: 90
```

Global timeout, testSuite timeout, and testSuite timeout are set to 90 minutes.

The *runson* key determines the platform (or operating system) on which the tests would be executed. Here we have set the target OS as macOS.

```yaml
runson: mac
```

Auto-split is set to true in the YAML file.

```yaml
autosplit: true
```

Retry on failure is set to False and the concurrency (i.e. number of parallel sessions) is set to 1. If the test execution fails (at the first shot), further attempts for execution would not be made.

```yaml
retryOnFailure: false
maxRetries: 5
concurrency: 1
```

Content under the *pre* directive is the pre-condition that will be run before the tests are executed on Hypertest grid.
The "dotnet install" script for macOS & Windows is downloaded and kept in the project root directory. The stable version of the scripts are downloaded from [Microsoft Official Website](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script).

* [Bash - Linux/macOS](https://dot.net/v1/dotnet-install.sh)
* [PowerShell for Windows](https://dot.net/v1/dotnet-install.ps1)

Environment variables *LT_USERNAME* and *LT_ACCESS_KEY* are added under *env* in the YAML file.

```yaml
env:
 LT_USERNAME: ${ YOUR_LAMBDATEST_USERNAME()}
 ACCESS_KEY: ${ YOUR_LAMBDATEST_ACCESS_KEY()}
```

However, this is an optional step and can be skipped from the *pre* directive. Once downloaded, we install the LTS release using the commands mentioned [here](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script#examples). We set the permissions of C# solution to 777 (i.e. rwx).

```yaml
pre:
   - ./dotnet-install.sh --channel LTS
   - chmod +rwx OnlySpecTest.sln
```

The *testDiscoverer* contains the command that locates the C# solution (i.e. .sln). The output of the *testDiscoverer* command is passed in the *testRunnerCommand*

```bash
find . -type f -name "*.sln"
```

Running the above command on the terminal gives the following output:

* ./OnlySpecTest.sln

The [user_name and access_key of LambdaTest](https://accounts.lambdatest.com/detail/profile) is appended to the *concierge* command using the *--user* and *--key* command-line options. The CLI option *--config* is used for providing the custom Hypertest YAML file (e.g. specflow_hypertest_autosplit_sample.yaml). Run the following command on the terminal to trigger the tests in C# project on the Hypertest grid.

```bash
./concierge --user "${ YOUR_LAMBDATEST_USERNAME()}" --key "${ YOUR_LAMBDATEST_ACCESS_KEY()}" --config specflow_hypertest_autosplit_sample.yaml --verbose

Visit [Hypertest Automation Dashboard](https://automation.lambdatest.com/hypertest) to check the status of execution
