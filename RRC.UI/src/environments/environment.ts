// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,

  Api:'http',
  //apiHostingURL : "http://qtsp2016.centralus.cloudapp.azure.com/rrc/api",
  //DownloadUrl: 'http://qtsp2016.centralus.cloudapp.azure.com/rrc/Downloads/',
  //AttachmentDownloadUrl: 'http://qtsp2016.centralus.cloudapp.azure.com/rrc/api/attachment/download',
  //imageUrl: 'http://qtsp2016.centralus.cloudapp.azure.com/rrc/Uploads/',  
  apiHostingURL : "http://qtk2dev.westus.cloudapp.azure.com/rrcapi/api",
  DownloadUrl: 'http://qtk2dev.westus.cloudapp.azure.com/rrcapi/Downloads/',
  AttachmentDownloadUrl: 'http://qtk2dev.westus.cloudapp.azure.com/rrcapi/api/attachment/download',
  imageUrl: 'http://qtk2dev.westus.cloudapp.azure.com/rrcapi/Uploads',
  calendar_id:10,
  schedulerLicenseKey: 'GPL-My-Project-Is-Open-Source'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
