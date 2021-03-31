import {KeycloakConfig} from 'keycloak-angular';

// Add here your keycloak setup infos
const keycloakConfig: KeycloakConfig = {
  url: 'https://loginuat-manara.moe.gov.ae/auth',
  realm: 'OER',
  clientId: 'OER-Client',
  credentials: {
    secret: '68ab147e-4847-48e6-b4e3-70fa8ad6bef4'
  }
};

export const environment = {
  production: true,
  keycloak: keycloakConfig,
  clientUrl: 'https://uat-manara.moe.gov.ae',
  adminUrl: 'https://adminuat-manara.moe.gov.ae/',
  apiUrl: 'https://apiuat-manara.moe.gov.ae/api/',
  serverUrl: 'https://apiuat-manara.moe.gov.ae',
  encKey: 'rNyuFzkutcZL6kmFqWBEtGzJksun1ijU'
};
