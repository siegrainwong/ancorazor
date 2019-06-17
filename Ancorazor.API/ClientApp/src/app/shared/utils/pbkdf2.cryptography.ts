/**
 * MARK: Angular PBKDF2
 * https://stackoverflow.com/questions/40459020/angular-js-cryptography-pbkdf2-and-iteration/40468218#40468218
 *
 * 这里特别需要注意的是，在Chrome中没有SSL时会禁用WebCrypto.
 * @param password
 * @param salt
 * @param iterations
 * @param hash
 */
export function deriveAKey(
  password: string,
  salt: string,
  iterations: number,
  hash: string | Algorithm
): Promise<string> {
  return new Promise<string>((res, rej) => {
    // First, create a PBKDF2 "key" containing the password
    window.crypto.subtle
      .importKey("raw", stringToArrayBuffer(password), "PBKDF2", false, [
        "deriveKey"
      ])
      .then(function(baseKey) {
        // Derive a key from the password
        return window.crypto.subtle.deriveKey(
          {
            name: "PBKDF2",
            salt: stringToArrayBuffer(salt),
            iterations: iterations,
            hash: hash
          },
          baseKey,
          { name: "AES-CBC", length: 128 }, // Key we want.Can be any AES algorithm ("AES-CTR", "AES-CBC", "AES-CMAC", "AES-GCM", "AES-CFB", "AES-KW", "ECDH", "DH", or "HMAC")
          true, // Extractable
          ["encrypt", "decrypt"] // For new key
        );
      })
      .then(function(aesKey) {
        // Export it so we can display it
        return window.crypto.subtle.exportKey("raw", aesKey);
      })
      .then(function(keyBytes) {
        // Display key in Base64 format
        let keyS = arrayBufferToString(keyBytes);
        let keyB64 = btoa(keyS);
        res(keyB64);
      });
  });
}

//Utility functions

function stringToArrayBuffer(byteString: string) {
  var byteArray = new Uint8Array(byteString.length);
  for (var i = 0; i < byteString.length; i++) {
    byteArray[i] = byteString.codePointAt(i);
  }
  return byteArray;
}

function arrayBufferToString(buffer: ArrayBuffer) {
  var byteArray = new Uint8Array(buffer);
  var byteString = "";
  for (var i = 0; i < byteArray.byteLength; i++) {
    byteString += String.fromCodePoint(byteArray[i]);
  }
  return byteString;
}
