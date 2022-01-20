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
      |
      yaml
       |
       |--- specflow_hypertest_matrix_sample.yaml
       |--- specflow_hypertest_autosplit_sample.yaml
```

Before running the tests, it is required to declare the environment variables *LT_USERNAME* and *LT_ACCESS_KEY* on the terminal from where Concierge is triggered for test execution. You can find details about LambdaTest User Name and Access Key in [LambdaTest Profile](https://accounts.lambdatest.com/detail/profile) page.

*<b> For macOS </b>*
```bash
$ export LT_USERNAME = LT_USERNAME
$ export LT_ACCESS_KEY = LT_ACCESS_KEY
```
*<b> For Windows </b>*
```bash
$ set LT_USERNAME = LT_USERNAME
$ set LT_ACCESS_KEY = LT_ACCESS_KEY
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
  feature: ["GoogleSearchLT", "DuckDuckGoLTBlog", "SeleniumPlayground", "TodoApp"]
```

Content under the *pre* directive is the pre-condition that will be run before the tests are executed on Hypertest grid. The "dotnet install" script for macOS & Windows is downloaded and kept in the project root directory. The stable version of the scripts can be downloaded from [Microsoft Official Website](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script).

* [Bash - Linux/macOS](https://dot.net/v1/dotnet-install.sh)
* [PowerShell for Windows](https://dot.net/v1/dotnet-install.ps1)

However, this is an optional step and can be skipped from the *pre* directive. Once downloaded, we install the LTS release using the commands mentioned [here](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script#examples). We set the permissions of C# project to 777 (i.e. rwx).

```yaml
pre:
   - ./dotnet-install.sh --channel LTS
   - chmod +rwx OnlySpecTest.sln
```

The *testSuites* object contains a list of commands (that can be presented in an array). In the current YAML file, commands to be run for executing the tests are put in an array (with a '-' preceding each item). In the current YAML file, *dotnet test* command is used for executing the tests present in the *$project* key (i.e. "OnlySpecTest.sln"). The *Feature* names are read from the feature files located in the 'Features' folder (i.e. "GoogleSearchLT", "DuckDuckGoLTBlog", "SeleniumPlayground", "TodoApp").

```yaml
testSuites:
  - dotnet test $project --filter Name~$feature
```

The CLI option *--config* is used for providing the custom Hypertest YAML file (e.g. specflow_hypertest_matrix_sample.yaml). Run the following command on the terminal to trigger the tests in C# project on the Hypertest grid.

```bash
./concierge --config yaml/specflow_hypertest_matrix_sample.yaml --verbose
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

Retry on failure is set to true. Even on failure of test execution, an attempt of re-execution is made till the time the tests pass or the maximum number of retries (i.e. *maxRetries*) have elapsed. The *concurrency* is set to 4 i.e. 4 VMs will be spawned in parallel for running the *features* specified in the respective files.

```yaml
retryOnFailure: true
maxRetries: 5
concurrency: 4
```

The *runson* key determines the platform (or operating system) on which the tests would be executed. Here we have set the target OS as macOS.

```yaml
runson: mac
```

Auto-split is set to true in the YAML file.

```yaml
autosplit: true
```

Content under the *pre* directive is the pre-condition that will be run before the tests are executed on Hypertest grid.
The "dotnet install" script for macOS & Windows is downloaded and kept in the project root directory. The stable version of the scripts are downloaded from [Microsoft Official Website](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script).

* [Bash - Linux/macOS](https://dot.net/v1/dotnet-install.sh)
* [PowerShell for Windows](https://dot.net/v1/dotnet-install.ps1)

However, this is an optional step and can be skipped from the *pre* directive. Once downloaded, we install the LTS release using the commands mentioned [here](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script#examples). We set the permissions of C# solution to 777 (i.e. rwx).

```yaml
pre:
   - ./dotnet-install.sh --channel LTS
   - chmod +rwx OnlySpecTest.sln
```

The *testDiscoverer* contains the command that locates the C# solution (i.e. .sln). The output of the *testDiscoverer* command is passed in the *testRunnerCommand*

```bash
grep -rni 'Features' -e 'Feature:' | sed 's/.*://'
```

Running the above command on the terminal gives the following output:

* GoogleSearchLT
* TodoApp
* DuckDuckGoLTBlog
* SeleniumPlayground

The CLI option *--config* is used for providing the custom Hypertest YAML file (e.g. yaml/specflow_hypertest_autosplit_sample.yaml). Run the following command on the terminal to trigger the tests in C# project on the Hypertest grid.

```bash
./concierge --config yaml/specflow_hypertest_autosplit_sample.yaml --verbose

Visit [Hypertest Automation Dashboard](https://automation.lambdatest.com/hypertest) to check the status of execution
