import {Injectable} from '@angular/core';
import * as CryptoJS from 'crypto-js';
import {environment} from '../../environments/environment';

/**
 * Encryption Service
 */
@Injectable({
  providedIn: 'root'
})
export class EncService {

  constructor() {
  }

  /**
   * returns encrypted string
   *
   * @param value contain value details
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
   * returns decrypted string
   *
   * @param value contain value details
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
