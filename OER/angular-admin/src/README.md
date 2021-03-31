# oeradmin

##Development Setup Instructions 
* Install the Nodejs 

      https://nodejs.org

* Install the Angular CLI 

      https://angular.io/guide/setup-local#step-1-install-the-angular-cli

* Run the command below in the root folder of the app via terminal to install dependencies
    
      $ npm install

* Update the environment ﬁle, path : src/environments/environment.ts 
```typescript
    const keycloakConfig: KeycloakConfig = {
              url: 'http://{keycloak_server_address}}/auth',
              realm: '{keycloak_realm_name}',
              clientId: '{keycloak_client_id}',
              credentials: {
                secret: '{keycloak_client_secret}'
              }
            };
            // Add here your environment setup infos
            export const environment = {
              production: false,
              keycloak: keycloakConfig,
              clientUrl: 'http://localhost:4200',
              apiUrl: 'http://{dotnet_api_address}/api/',
              encKey: 'rNyuFzkutcZL6kmFqWBEtGzJksun1ijU',
              userClientUrl: 'http://{client_app_URL}/'
              encKey: '{random encryption key of length 32(should be same as added in Angular Admin App environment file settings)}'
            };
```
 

* Run the command below in the root folder of the app via terminal to build the app for Deployment
  
      ng serve --watch
