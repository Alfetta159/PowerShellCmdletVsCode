# Creating PowerShell CmdLets in VSCode

Refer to: [Creating PowerShell CmdLets in VSCode](https://dev.to/alfetta159/creating-powershell-cmdlets-in-vscode-3gap)

Nothing has ever made Linux so exciting for me than Microsoft's recent push to make many of their development tools and frameworks cross-platform. With the exception of ordering on-line, checking email, research.., oh, and a few social media sites, I'm just a coder. Not a gamer, not a TikToker, just a coder.

## Is This Article about Linux?
Now I mentioned Linux. This article isn't about Linux, but it's not about Windows either. It's about writing PowerShell CmdLets in VSCode. And PowerShell, .NET, and VSCode all work across those two operating systems.
## But Why?
Why do we need PowerShell when Linux normally relies on Bash? Because if you're an enterprise coder like me, it's nice to be able to rely on familiar and stalwart platforms, languages and tools like Windows, .NET, C# and VisualStudio/Code to create APIs, SDKs and CmdLets that help other programmers including those who might be scripting in JavaScript or PowerShell. In other words, not .NET. Notice that I didn't use the term low-code? JS, PS and whatever else can be very sophisticated, and I'm looking for cross-platform capability.
## What?
Recently I wanted to unify a lot of our emailing that was happening from various applications in various ways. We want to move to the cloud, and while some of our new apps have, others haven't. We also wanted to have basic endpoints that would handle the vast majority of our standard emails. We wanted email templates that were more about just notifying the correct folks that more information was available to them and show them where to log on to see the important and private stuff all the while being careful to not display this proprietary information in the email itself. After all, you don't want to see a subject line saying something like, "That test you took came back positive!" in an email from your HMO, do you?
We rely on [SendGrid](https://sendgrid.com/) as a cloud SMTP provider, but creating a nice looking email in HTML takes a bit of prep in their API. So we created a template that would handle input like recipients: to, cc, bcc and then sections of the email: subject, what the email is about, why you're receiving it, instructions on what to do, and then even an email link that would take all of that information and send it to our internal help desk system in a plain-text email in case the recipient had a problem. Then this was hosted as an Azure Function project.
So now that JavaScript people and anyone who wants to talk directly to our API thru HTTPS can by posting ordinary information in form of strings without any styling metadata.
Then we created a .NET-base SDK allowing the .NET folks to access the same Azure Functions but without all that mucking about with HTTP clients, URI concatenation or POST body encoding.
## But What about the Others?
The others? You know, folks who have to maintain systems that might run on Linux or are simple console apps that are triggered by scheduled tasks in Windows or are actually Powershell scripts or modules. Wouldn't it be nice to be able to just install a cmdlet and start sending shiny, branded, HTML-based emails that have corporate logos and convenient links for those not so tech savvy?
## Then How? (Finally!)
First, install [PowerShell](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.2) and [VSCode](https://code.visualstudio.com/) on your system. If you're really installing VSCode for the first time, then you'll probably need [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) as well.
Then you'll need a project. This should be a class library. You can base it in .NET Framework or .NET Standard, but I use the latest .NET. I swore off .NET Framework some time ago, and this is a CmdLet, not a library. Okay, it's a library, but not one that lends itself well to derivation by other assemblies. Oh, and .NET (Core) runs on Linux as well as Windows and something called MacOS.
So we run (in whatever terminal you like):
`dotnet new classlib --name "MyCmdletProject" 

There are far more options to this command, but that's not what this post is about. Then you'll need to reference the System.Management.Automation NuGet package:
`dotnet add package System.Management.Automation 

You'll need a tasks.json file as well as a launch.json file that are placed in that .vscode folder that VSCode places in certain project folders.

The task.json file can look like this:
```json
{
	// See https://go.microsoft.com/fwlink/?LinkId=733558
	// for the documentation about the tasks.json format
	"version": "2.0.0",
	"tasks": [
		{
			"label": "build",
			"type": "process",
			"command": "dotnet",
			"args": [
				"build",
				"${workspaceFolder}/MyCmdletProject.csproj",
				"/property:GenerateFullPaths=true",
				"/consoleloggerparameters:NoSummary"
					],
			"group": "build",
			"presentation": {
				// Reveal the output only if unrecognized errors occur.
				"reveal": "silent"
			},
			// Use the standard MS compiler pattern to detect errors, warnings and infos
			"problemMatcher": "$msCompile"
		}
	]
}
```
And the launch.json should look like:
```json
{
    // Use IntelliSense to learn about possible attributes.
    // Hover to view descriptions of existing attributes.
    // For more information, visit: https://go.microsoft.com/fwlink/?linkid=830387
    "version": "0.2.0",
    "configurations": [
        {
            "name": "PowerShell cmdlets: pwsh",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "pwsh",
            "args": [
                "-NoExit",
                "-NoProfile",
                "-Command",
                "Import-Module ${workspaceFolder}/bin/Debug/net6.0/<output>.dll",
            ],
            "cwd": "${workspaceFolder}",
            "stopAtEntry": false,
            "console": "integratedTerminal"
        }
    ]
}
```
Keep in mind that just like in Visual Studio, we need to debug by launching the PowerShell application and immediately importing the cmdlet that we just created when we called the build `task` in the `preLaunchTask` of our `launch.json` file. And the arguments are where the magic happens: `NoExit` so we effectively don't close the terminal, `NoProfile` so we don't inadvertently cause any harm to our system, and then `Command` telling pwsh to run the arguments for that option: `Import-Module ${workspaceFolder}/bin/Debug/net6.0/MyCmdletProject.dll`. So we're not only launching our own instance of PowerShell, but we're telling it to import our resulting library file so we can then call it in the terminal. And the process for doing this in Visual Studio is similar. It's just that the same information (command and arguments) are entered into the Debug section of the properties of the cmdlet project.
So enjoy the freedom of not only running PowerShell scripts with custom cmdlets in any of the major operating systems, enjoy the freedom of writing and debugging them too.

### References:
[Using Visual Studio Code to debug compiled cmdlets](https://docs.microsoft.com/en-us/powershell/scripting/dev-cross-plat/vscode/using-vscode-for-debugging-compiled-cmdlets?view=powershell-7.2)