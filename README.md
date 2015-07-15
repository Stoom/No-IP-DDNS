## No-IP.DDNS
A class library for accessing No-IP DNS hosts.

[![Build status](https://img.shields.io/appveyor/ci/stoom/no-ip-ddns/master.svg?style=flat-square)](https://ci.appveyor.com/project/Stoom/no-ip-ddns)
[![Release](https://img.shields.io/github/release/Stoom/No-IP-DDNS.svg?style=flat-square)](https://github.com/Stoom/No-IP-DDNS/releases/latest)
[![Downloads](https://img.shields.io/nuget/dt/NoIP.DDNS.svg?style=flat-square)](https://www.nuget.org/packages/NoIP.DDNS/)
[![Issues](https://img.shields.io/github/issues/Stoom/No-IP-DDNS.svg?style=flat-square)](https://github.com/Stoom/No-IP-DDNS/issues)

### Documentation
MSDN style documentation can be found [Here](http://docs.stumme.net/NoIp.DDNS/) or can be built via the RebuildClientDocs.bat script.

### Ran into an bug?
Before reporting it to us, please check out the [FAQ](https://github.com/Stoom/No-IP-DDNS/wiki/FAQ) to see if it is actually a bug. If you can not find anything related to your issue, feel free to report it to us in the issue tracker.

#### Bug Reports
Please read [this page](https://github.com/Stoom/No-IP-DDNS/wiki/About-Bug-Reports) before submitting an issue.

### Breaking Changes
None

### A short How To
Generate UserAgent
```csharp
var userAgent = new UserAgent("ProgramName");
```

Register client (Only do this once and store the client.Id and client.Key encrypted)
```csharp
var client = new Client(userAgent);
client.Register("username", "password");
```

Get zones
```csharp
var zones = client.GetZones();
```

Get hosts in specified zone
```csharp
var hosts = client.GetHosts(zone);
```

Update host IP address
```csharp
var host = hosts.where(x => x.Name = "HostToChange");
host.Address = IPAddress.Parse("127.0.0.1");
client.UpdateHost(host);
```

### Contributions

If you've improved MahApps.Metro and think that other people would enjoy it, submit a pull request.