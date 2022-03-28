<img height="100" alt="hypertest_logo" src="https://user-images.githubusercontent.com/1688653/159473714-384e60ba-d830-435e-a33f-730df3c3ebc6.png">

HyperExecute is a smart test orchestration platform to run end-to-end Selenium tests at the fastest speed possible. HyperExecute lets you achieve an accelerated time to market by providing a test infrastructure that offers optimal speed, test orchestration, and detailed execution logs.

The overall experience helps teams test code and fix issues at a much faster pace. HyperExecute is configured using a YAML file. Instead of moving the Hub close to you, HyperExecute brings the test scripts close to the Hub!

* <b>HyperExecute HomePage</b>: https://www.lambdatest.com/hyperexecute
* <b>Lambdatest HomePage</b>: https://www.lambdatest.com
* <b>LambdaTest Support</b>: [support@lambdatest.com](mailto:support@lambdatest.com)

To know more about how HyperExecute does intelligent Test Orchestration, do check out [HyperExecute Getting Started Guide](https://www.lambdatest.com/support/docs/getting-started-with-hyperexecute/)

# How to run Selenium automation tests on HyperExecute (using SpecFlow framework)

* [Pre-requisites](#pre-requisites)
   - [Download Concierge](#download-concierge)
   - [Configure Environment Variables](#configure-environment-variables)

* [Matrix Execution with SpecFlow](#matrix-execution-with-specflow)
   - [Core](#core)
   - [Pre Steps and Dependency Caching](#pre-steps-and-dependency-caching)
   - [Post Steps](#post-steps)
   - [Artifacts Management](#artifacts-management)
   - [Test Execution](#test-execution)

* [Auto-Split Execution with SpecFlow](#auto-split-execution-with-specflow)
   - [Core](#core-1)
   - [Pre Steps and Dependency Caching](#pre-steps-and-dependency-caching-1)
   - [Post Steps](#post-steps-1)
   - [Artifacts Management](#artifacts-management-1)
   - [Test Execution](#test-execution-1)

* [Secrets Management](#secrets-management)
* [Navigation in Automation Dashboard](#navigation-in-automation-dashboard)

# Pre-requisites

Before using HyperExecute, you have to download Concierge CLI corresponding to the host OS. Along with it, you also need to export the environment variables *LT_USERNAME* and *LT_ACCESS_KEY* that are available in the [LambdaTest Profile](https://accounts.lambdatest.com/detail/profile) page.

## Download Concierge

Concierge is a CLI for interacting and running the tests on the HyperExecute Grid. Concierge provides a host of other useful features that accelerate test execution. In order to trigger tests using Concierge, you need to download the Concierge binary corresponding to the platform (or OS) from where the tests are triggered:

Also, it is recommended to download the binary in the project's parent directory. Shown below is the location from where you can download the Concierge binary:

* Mac: https://downloads.lambdatest.com/concierge/darwin/concierge
* Linux: https://downloads.lambdatest.com/concierge/linux/concierge
* Windows: https://downloads.lambdatest.com/concierge/windows/concierge.exe

## Configure Environment Variables

Before the tests are run, please set the environment variables LT_USERNAME & LT_ACCESS_KEY from the terminal. The account details are available on your [LambdaTest Profile](https://accounts.lambdatest.com/detail/profile) page.

For macOS:

```bash
export LT_USERNAME=LT_USERNAME
export LT_ACCESS_KEY=LT_ACCESS_KEY
```

For Linux:

```bash
export LT_USERNAME=LT_USERNAME
export LT_ACCESS_KEY=LT_ACCESS_KEY
```

For Windows:

```bash
set LT_USERNAME=LT_USERNAME
set LT_ACCESS_KEY=LT_ACCESS_KEY
```

The project structure is as shown below:

```yaml
specflow-demo-sample
      |
      |--- Features (Contains the feature files)
              |
              | --- BingSearch.feature
              | --- LambdaTestSearch.feature
              | --- SeleniumPlayground.feature
              | --- ToDoApp.feature
      |--- Hooks (Contains the event bindings to perform additional automation logic)
              | --- Hooks.cs
      |--- Steps (Contains the step definitions that correspond to the feature files)
              | --- BingSearchSteps.cs
              | --- DuckDuckGoSearchSteps.cs
              | --- SeleniumPlaygroundSteps.cs
              | --- ToDoAppSteps.cs
      |--- App.config (Application Configuration file containing settings specific to the app)
      |
      yaml
       |
       |--- specflow_hyperexecute_matrix_sample.yaml
       |--- specflow_hyperexecute_autosplit_sample.yaml
```

# Matrix Execution with SpecFlow

Matrix-based test execution is used for running the same tests across different test (or input) combinations. The Matrix directive in HyperExecute YAML file is a *key:value* pair where value is an array of strings.

Also, the *key:value* pairs are opaque strings for HyperExecute. For more information about matrix multiplexing, check out the [Matrix Getting Started Guide](https://www.lambdatest.com/support/docs/getting-started-with-hyperexecute/#matrix-based-build-multiplexing)

### Core

In the current example, matrix YAML file (*yaml/specflow_hyperexecute_matrix_sample.yaml*) in the repo contains the following configuration:

```yaml
globalTimeout: 90
testSuiteTimeout: 90
testSuiteStep: 90
```

Global timeout, testSuite timeout, and testSuite timeout are set to 90 minutes.
 
The target platform is set to Windows. Please set the *[runson]* key to *[mac]* if the tests have to be executed on the macOS platform.

```yaml
runson: win
```

The *matrix* constitutes of the following entries - *project* and *scenario*. This is because parallel execution will be achieved at the *scenario* level.

```yaml
matrix:
  project: ["OnlySpecTest.sln"]
  #Parallel execution at feature level
  scenario: ["BingSearch_1", "BingSearch_2", "BingSearch_3",
             "LambdaTestBlogSearch_1", "LambdaTestBlogSearch_2", "LambdaTestBlogSearch_3",
             "SeleniumPlayground_1", "SeleniumPlayground_2", "SeleniumPlayground_3",
             "ToDoApp_1", "ToDoApp_2", ToDoApp_3]
```

The *testSuites* object contains a list of commands (that can be presented in an array). In the current YAML file, commands for executing the tests are put in an array (with a '-' preceding each item). The [*dotnet test*](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test) command is used to run tests located in the current project. In the current project, parallel execution is achieved at the *scenario* level.

Please refer to [Executing specific Scenarios in Build pipeline](https://docs.specflow.org/projects/specflow/en/latest/Execution/Executing-Specific-Scenarios.html) for more information on filtering the test execution based on *Category*

```yaml
testSuites:
  - dotnet test $project --filter "(Category=$scenario)"
```

### Pre Steps and Dependency Caching

Dependency caching is enabled in the YAML file to ensure that the package dependencies are not downloaded in subsequent runs. The first step is to set the Key used to cache directories.

```yaml
cacheKey: '{{ checksum "packages.txt" }}'
```

Set the array of files & directories to be cached. Separate folders are created for downloading global-packages, http-cache, and plugins-cache. Pleas refer to [Configuring NuGet CLI environment variables](https://docs.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-environment-variables) to know more about overriding settings in NuGet.Config files.


```yaml
NUGET_PACKAGES: 'C:\nuget_global_cache'
NUGET_HTTP_CACHE_PATH: 'C:\nuget_http_cache'
NUGET_PLUGINS_CACHE_PATH: 'C:\nuget_plugins_cache'
```

Steps (or commands) that must run before the test execution are listed in the *pre* run step. In the example, the necessary NuGet packages are fetched using the [*dotnet list package*](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-list-package) command. All the local packages are cleared using the *nuget locals all -clear* command, post which the entire project is built from scratch using the *dotnet build -c Release* command.

```yaml
pre:
 # https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-list-package
 - dotnet list $project package > packages.txt
 - nuget locals all -clear
 - dotnet build -c Release
```

### Post Steps

Steps (or commands) that need to run after the test execution are listed in the *post* step. In the example, we *cat* the contents of *yaml/specflow_hyperexecute_matrix_sample.yaml*

```yaml
post:
  - cat yaml/specflow_hyperexecute_matrix_sample.yaml
```

### Artifacts Management

The *mergeArtifacts* directive (which is by default *false*) is set to *true* for merging the artifacts and combining artifacts generated under each task.

The *uploadArtefacts* directive informs HyperExecute to upload artifacts [files, reports, etc.] generated after task completion. In the example, *path* consists of a regex for parsing the directories (i.e. *Report/* and *Screenshots/*) that contain the test reports and execution screenshots respectively.

```yaml
mergeArtifacts: true

uploadArtefacts:
 - name: Execution_Report
   path:
    - Report/**
 - name: Execution_Screenshots
   path:
    - Screenshots/**/**
```

HyperExecute also facilitates the provision to download the artifacts on your local machine. To download the artifacts, click on Artifacts button corresponding to the associated TestID.

<img width="1425" alt="specflow_matrix_artefacts_1" src="https://user-images.githubusercontent.com/1688653/160473330-28d7d2fd-b0fc-42df-a8c6-0fd2f9a37e1f.png">

Now, you can download the artifacts by clicking on the Download button as shown below:

<img width="1425" alt="specflow_matrix_artefacts_2" src="https://user-images.githubusercontent.com/1688653/160473349-a85482f2-642e-47d2-aa84-f85c569c9791.png">

## Test Execution

The CLI option *--config* is used for providing the custom HyperExecute YAML file (i.e. *yaml/specflow_hyperexecute_matrix_sample.yaml*). Run the following command on the terminal to trigger the tests in C# files on the HyperExecute grid. The *--download-artifacts* option is used to inform HyperExecute to download the artifacts for the job. The *--force-clean-artifacts* option force cleans any existing artifacts for the project.

```bash
./concierge --config yaml/specflow_hyperexecute_matrix_sample.yaml --force-clean-artifacts --download-artifacts
```

Visit [HyperExecute Automation Dashboard](https://automation.lambdatest.com/hypertest) to check the status of execution:

<img width="1414" alt="specflow_matrix_execution" src="https://user-images.githubusercontent.com/1688653/160473330-28d7d2fd-b0fc-42df-a8c6-0fd2f9a37e1f.png">

Shown below is the execution screenshot when the YAML file is triggered from the terminal:

<img width="1413" alt="specflow_cli1_execution" src="https://user-images.githubusercontent.com/1688653/159754931-84db9e68-02d8-4518-982d-779febe8b105.png">

<img width="1101" alt="specflow_cli2_execution" src="https://user-images.githubusercontent.com/1688653/159754953-17f5b229-7679-4ee8-8ac9-6005ff6788c9.png">

## Auto-Split Execution with SpecFlow

Auto-split execution mechanism lets you run tests at predefined concurrency and distribute the tests over the available infrastructure. Concurrency can be achieved at different levels - file, module, test suite, test, scenario, etc.

For more information about auto-split execution, check out the [Auto-Split Getting Started Guide](https://www.lambdatest.com/support/docs/getting-started-with-hyperexecute/#smart-auto-test-splitting)

### Core

Auto-split YAML file (*yaml/specflow_hyperexecute_autosplit_sample.yaml*) in the repo contains the following configuration:

```yaml
globalTimeout: 90
testSuiteTimeout: 90
testSuiteStep: 90
```

Global timeout, testSuite timeout, and testSuite timeout are set to 90 minutes.
 
The *runson* key determines the platform (or operating system) on which the tests are executed. Here we have set the target OS as Windows.

```yaml
runson: win
```

Auto-split is set to true in the YAML file.

```yaml
 autosplit: true
```

*retryOnFailure* is set to true, instructing HyperExecute to retry failed command(s). The retry operation is carried out till the number of retries mentioned in *maxRetries* are exhausted or the command execution results in a *Pass*. In addition, the concurrency (i.e. number of parallel sessions) is set to 25.

```yaml
retryOnFailure: true
maxRetries: 5
concurrency: 25
```

## Pre Steps and Dependency Caching

Dependency caching is enabled in the YAML file to ensure that the package dependencies are not downloaded in subsequent runs. The first step is to set the Key used to cache directories.

```yaml
cacheKey: '{{ checksum "packages.txt" }}'
```

Set the array of files & directories to be cached. Separate folders are created for downloading global-packages, http-cache, and plugins-cache. Pleas refer to [Configuring NuGet CLI environment variables](https://docs.microsoft.com/en-us/nuget/reference/cli-reference/cli-ref-environment-variables) to know more about overriding settings in NuGet.Config files.


```yaml
NUGET_PACKAGES: 'C:\nuget_global_cache'
NUGET_HTTP_CACHE_PATH: 'C:\nuget_http_cache'
NUGET_PLUGINS_CACHE_PATH: 'C:\nuget_plugins_cache'
```

## Post Steps

The *post* directive contains a list of commands that run as a part of post-test execution. Here, the contents of *yaml/specflow_hyperexecute_autosplit_sample.yaml* are read using the *cat* command as a part of the post step.

```yaml
post:
  - cat yaml/specflow_hyperexecute_autosplit_sample.yaml
```

The *testDiscovery* directive contains the command that gives details of the mode of execution, along with detailing the command that is used for test execution. Here, we are fetching the list of test methods that would be further passed in the *testRunnerCommand*

```yaml
testDiscovery:
  type: raw
  mode: static
  command: grep -rni 'Features' -e '@' --include=\*.feature | sed 's/.*@//'
```

Running the above command on the terminal will give a list of scenarios present in the *feature* files:

* BingSearch_1
* BingSearch_2
* BingSearch_3
* ToDoApp_1
* ToDoApp_2
* ToDoApp_3
* LambdaTestBlogSearch_1
* LambdaTestBlogSearch_2
* LambdaTestBlogSearch_3
* SeleniumPlayground_1
* SeleniumPlayground_2
* SeleniumPlayground_3

The *testRunnerCommand* contains the command that is used for triggering the test. The output fetched from the *testDiscoverer* command acts as an input to the *testRunner* command.

```yaml
testRunnerCommand: dotnet test --filter "(Category=$test)"
```

### Artifacts Management

The *mergeArtifacts* directive (which is by default *false*) is set to *true* for merging the artifacts and combining artifacts generated under each task.

The *uploadArtefacts* directive informs HyperExecute to upload artifacts [files, reports, etc.] generated after task completion. In the example, *path* consists of a regex for parsing the directories (i.e. *Report/* and *Screenshots/*) that contain the test reports and execution screenshots respectively.

```yaml
mergeArtifacts: true

uploadArtefacts:
 - name: Execution_Report
   path:
    - Report/**
 - name: Execution_Screenshots
   path:
    - Screenshots/**/**
```

HyperExecute also facilitates the provision to download the artifacts on your local machine. To download the artifacts, click on Artifacts button corresponding to the associated TestID.

<img width="1425" alt="specflow_autosplit_artefacts_1" src="https://user-images.githubusercontent.com/1688653/160473354-e54e10c4-c519-46a7-9695-2d7def0c2ab1.png">

Now, you can download the artifacts by clicking on the Download button as shown below:

<img width="1425" alt="specflow_autosplit_artefacts_2" src="https://user-images.githubusercontent.com/1688653/160473358-0709e451-258b-4da7-a834-c58245b9f397.png">

### Test Execution

The CLI option *--config* is used for providing the custom HyperExecute YAML file (i.e. *yaml/specflow_hyperexecute_autosplit_sample.yaml*). Run the following command on the terminal to trigger the tests in C# files on the HyperExecute grid. The *--download-artifacts* option is used to inform HyperExecute to download the artifacts for the job. The *--force-clean-artifacts* option force cleans any existing artifacts for the project.

```bash
./concierge --config yaml/specflow_hyperexecute_autosplit_sample.yaml --force-clean-artifacts --download-artifacts
```

Visit [HyperExecute Automation Dashboard](https://automation.lambdatest.com/hypertest) to check the status of execution

<img width="1414" alt="specflow_autosplit_execution" src="https://user-images.githubusercontent.com/1688653/160473354-e54e10c4-c519-46a7-9695-2d7def0c2ab1.png">

Shown below is the execution screenshot when the YAML file is triggered from the terminal:

<img width="1412" alt="specflow_autosplit_cli1_execution" src="https://user-images.githubusercontent.com/1688653/159755221-7b0c58b0-1612-41c7-808c-ca2e7e912d05.png">

<img width="1408" alt="specflow_autosplit_cli2_execution" src="https://user-images.githubusercontent.com/1688653/159755192-8050b20e-3ef4-46ac-9184-ab7ac8b78177.png">

## Secrets Management

In case you want to use any secret keys in the YAML file, the same can be set by clicking on the *Secrets* button the dashboard.

<img width="703" alt="specflow_secrets_key_1" src="https://user-images.githubusercontent.com/1688653/152540968-90e4e8bc-3eb4-4259-856b-5e513cbd19b5.png">

Now create a *secret* key that you can use in the HyperExecute YAML file.

<img width="359" alt="secrets_management_1" src="https://user-images.githubusercontent.com/1688653/153250877-e58445d1-2735-409a-970d-14253991c69e.png">

All you need to do is create an environment variable that uses the secret key:

```yaml
env:
  PAT: ${{ .secrets.testKey }}
```

## Navigation in Automation Dashboard

HyperExecute lets you navigate from/to *Test Logs* in Automation Dashboard from/to *HyperExecute Logs*. You also get relevant get relevant Selenium test details like video, network log, commands, Exceptions & more in the Dashboard. Effortlessly navigate from the automation dashboard to HyperExecute logs (and vice-versa) to get more details of the test execution.

Shown below is the HyperExecute Automation dashboard which also lists the tests that were executed as a part of the test suite:

<img width="1429" alt="specflow_hypertest_automation_dashboard" src="https://user-images.githubusercontent.com/1688653/160473330-28d7d2fd-b0fc-42df-a8c6-0fd2f9a37e1f.png">

Here is a screenshot that lists the automation test that was executed on the HyperExecute grid:

<img width="1429" alt="specflow_testing_automation_dashboard" src="https://user-images.githubusercontent.com/1688653/159754043-caf4ed6f-7f5f-4092-b611-60cca69f0d37.png">

## We are here to help you :)
* LambdaTest Support: [support@lambdatest.com](mailto:support@lambdatest.com)
* HyperExecute HomePage: https://www.lambdatest.com/support/docs/getting-started-with-hyperexecute/
* Lambdatest HomePage: https://www.lambdatest.com

## License
Licensed under the [MIT license](LICENSE).
