
import {KeycloakConfig} from 'keycloak-angular';

// Add here your keycloak setup infos
const keycloakConfig: KeycloakConfig = {
  url: 'https://loginuat-manara.moe.gov.ae/auth',
  realm: 'OER',
  clientId: 'OER-Admin',
  credentials: {
    secret: 'c1c57e85-df13-4ac3-8db1-4ca732a7955d'
  }
};

export const environment = {
  production: true,
  keycloak: keycloakConfig,
  clientUrl: 'https://adminuat-manara.moe.gov.ae',
  apiUrl: 'https://apiuat-manara.moe.gov.ae/api/',
  encKey: 'rNyuFzkutcZL6kmFqWBEtGzJksun1ijU',
  userClientUrl: 'https://uat-manara.moe.gov.ae/'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
