// import { KeycloakConfig } from 'keycloak-angular';

// // Add here your keycloak setup infos
// const keycloakConfig: KeycloakConfig = {
//   url: 'http://18.188.26.186:8080/auth',
//   realm: 'OER',
//   clientId: 'OER-Client',
//   credentials: {
//     secret: '75b12542-57d3-4ff8-842c-a86e15140ec9'
//   }
// };

// export const environment = {
//   production: true,
//   keycloak: keycloakConfig,
//   clientUrl: 'http://localhost:4200',
//   adminUrl: 'http://18.188.26.186:8091/',
//   apiUrl: 'http://18.188.26.186:8081/api/',
//   serverUrl: 'http://18.188.26.186:8081',
//   encKey: 'rNyuFzkutcZL6kmFqWBEtGzJksun1ijU'
// };


// // This file can be replaced during build by using the `fileReplacements` array.
// // `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// // The list of file replacements can be found in `angular.json`.

import { KeycloakConfig } from 'keycloak-angular';

// Add here your keycloak setup infos
const keycloakConfig: KeycloakConfig = {
  url: 'http://182.72.164.238:8080/auth',
  realm: 'OER',
  clientId: 'OER-SIT-Client',
  credentials: {
    secret: '7e19fa2d-bc21-4c87-b7c6-6dde4472ca95'
  }
};

export const environment = {
  production: true,
  keycloak: keycloakConfig,
  clientUrl: 'http://localhost:4200/',
  adminUrl: 'http://182.72.164.238:7001/',
  apiUrl: 'http://182.72.164.238:7040/api/',
  serverUrl: 'http://182.72.164.238:7040',
  encKey: 'rNyuFzkutcZL6kmFqWBEtGzJksun1ijU'
};
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
