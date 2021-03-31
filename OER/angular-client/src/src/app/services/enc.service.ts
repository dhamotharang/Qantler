import {Injectable} from '@angular/core';
import * as CryptoJS from 'crypto-js';
import {environment} from '../../environments/environment';

/**
 * Encryption and type validation service
 */
@Injectable({
  providedIn: 'root'
})
export class EncService {
  acceptedImageExtensions: any[];
  acceptedVideoExtensions: any[];
  acceptedAudioExtensions: any[];
  acceptedDocumentExtensions: any[];
  acceptedInvalidExtensions: any[];
  imageSizeLimit: number;
  docSizeLimit: number;
  mediaSizeLimit: number;

  /**
   * initialize types
   */
  constructor() {
    this.imageSizeLimit = 10485760;
    this.docSizeLimit = 104857600;
    this.mediaSizeLimit = 1610612736;
    this.acceptedImageExtensions = [];
    ['jpg', 'jpeg', 'png', 'bmp', 'tiff'].forEach((item) => {
      this.acceptedImageExtensions.push(item);
    });
    this.acceptedVideoExtensions = [];
    ['mp4', 'webm'].forEach((item) => {
      this.acceptedVideoExtensions.push(item);
    });
    this.acceptedAudioExtensions = [];
    ['wav', 'mp3', 'acc', 'ogg'].forEach((item) => {
      this.acceptedAudioExtensions.push(item);
    });
    this.acceptedDocumentExtensions = [];
    ['ppt', 'pptx', 'doc', 'docx', 'xls', 'xlsx', 'pdf'].forEach((item) => {
      this.acceptedDocumentExtensions.push(item);
    });
    this.acceptedInvalidExtensions = [];
    ['wmv', 'mpeg', 'mpg', 'tiff', 'mov', 'avi'].forEach((item) => {
      this.acceptedInvalidExtensions.push(item);
    });
  }

  /**
   * returns encrypted value
   *
   * @param value
   */
  set(value) {
    const key = CryptoJS.enc.Utf8.parse(environment.encKey);
    const iv = CryptoJS.enc.Utf8.parse(environment.encKey);
    const encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(value.toString()), key,
      {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      });
    const words = CryptoJS.enc.Utf8.parse(encrypted.toString());
    return CryptoJS.enc.Hex.stringify(words);
  }

  /**
   * returns decrypted value
   *
   * @param value
   */
  get(value) {
    const words = CryptoJS.enc.Hex.parse(value.toString(CryptoJS.enc.Utf8));
    const key = CryptoJS.enc.Utf8.parse(environment.encKey);
    const iv = CryptoJS.enc.Utf8.parse(environment.encKey);
    const decrypted = CryptoJS.AES.decrypt(CryptoJS.enc.Utf8.stringify(words), key, {
      keySize: 128 / 8,
      iv: iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    });
    return decrypted.toString(CryptoJS.enc.Utf8);
  }

}
