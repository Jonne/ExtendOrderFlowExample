# escape=`
FROM avivasolutionsnl.azurecr.io/sitecore-xc-commerce:9.0.3-ltsc2019

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ADD ./WatchDirectoryMultiple.ps1 /Scripts

# Install VS 2017 remote debugging
ADD ./RemoteDebug.ps1 /Scripts

RUN choco install -y --params="Quiet" visualstudio2017-remotetools

ENTRYPOINT /Scripts/RemoteDebug.ps1; `
    /Scripts/WatchDirectoryMultiple.ps1 -Path C:\Workspace `
            -Destinations @('C:\\inetpub\\wwwroot\\CommerceAuthoring_Sc9', `
                            'C:\\inetpub\\wwwroot\\CommerceMinions_Sc9', `
                            'C:\\inetpub\\wwwroot\\CommerceOps_Sc9', `
                            'C:\\inetpub\\wwwroot\\CommerceShops_Sc9')
