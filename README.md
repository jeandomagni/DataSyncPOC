# How to run

OfflineSyncPwaDemoApp.RemoteMicroService and OfflineSyncPwaDemoApp.RemoteMicroService2 were deployed to Azure App Service. They pointed to own PostgreSql database

1. So to run locally, you need to start OfflineSyncPwaDemoApp.LocalMicroService from Visual Studio 2022.
2. Move to AngularUI folder, open cmd and run ng build then ng serve. This will run UI that will connect to LocalMicroService and get data from it.
3. Open http://localhost:4200 in browser
