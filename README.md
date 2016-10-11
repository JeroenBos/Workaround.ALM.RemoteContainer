# Workaround.ALM.RemoteContainer

This is my workaround to https://connect.microsoft.com/VisualStudio/feedback/details/807273/microsoft-alm-shared-remoting-remotecontainer-high-cpu

It assigns one logical core to each process with the name "Microsoft.Alm.Shared.Remoting.RemoteContainer.dll" such that is cannot use up more than 100%/CoreCount of the CPU, as to make Visual Studio responsive again.
