@SET PATH=%PATH%;C:\Program Files (x86)\Nemerle\net-4.0
@SET PATH=%PATH%;C:\Windows\Microsoft.NET\Framework64\v4.0.30319

msbuild /t:Build /p:Configuration=Debug ZenSharp.Core.nproj
